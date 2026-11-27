using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Persistent audio controller managing looping background music, non-overlapping SFX playback,
/// UI button click hooks, and PlayerPrefs volume/mute persistence.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    private const string MusicMutedKey = "MusicMuted";
    private const string SfxMutedKey = "SfxMuted";
    private const string FullscreenKey = "IsFullscreen";

    public static AudioManager Instance { get; private set; }

    private AudioSource musicSource;
    private AudioSource stepSource;
    private AudioSource boxSource;
    private AudioSource sfxSource;
    private AudioSource uiSource;

    private AudioClip musicClip;
    private AudioClip buttonClickClip;
    private AudioClip stepClip;
    private AudioClip boxClip;
    private AudioClip doorClip;
    private AudioClip portalClip;
    private AudioClip holeClip;
    private AudioClip milkClip;
    private AudioClip pauseClip;
    private AudioClip restartClip;
    private AudioClip levelUpClip;
    private AudioClip meowClip;

    private readonly Dictionary<string, float> lastPlayedAt = new Dictionary<string, float>();
    private Coroutine stepStopCoroutine;

    public float MusicVolume { get; private set; } = 0.7f;
    public float SfxVolume { get; private set; } = 0.9f;
    public bool MusicMuted { get; private set; }
    public bool SfxMuted { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        EnsureExists();
    }

    public static AudioManager EnsureExists()
    {
        if (Instance != null) return Instance;

        var existing = FindFirstObjectByType<AudioManager>();
        if (existing != null)
        {
            Instance = existing;
            return existing;
        }

        var go = new GameObject("AudioManager");
        Instance = go.AddComponent<AudioManager>();
        return Instance;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateSources();
        LoadClips();
        LoadAndApplySettings();
        StartMusicIfNeeded();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void CreateSources()
    {
        musicSource = CreateChannel("MusicChannel", loop: true, ignorePause: true);
        stepSource = CreateChannel("StepChannel", loop: false, ignorePause: false);
        boxSource = CreateChannel("BoxChannel", loop: false, ignorePause: false);
        sfxSource = CreateChannel("SfxChannel", loop: false, ignorePause: false);
        uiSource = CreateChannel("UiChannel", loop: false, ignorePause: true);
    }

    private AudioSource CreateChannel(string childName, bool loop, bool ignorePause)
    {
        var child = new GameObject(childName);
        child.transform.SetParent(transform, false);
        var src = child.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = loop;
        src.spatialBlend = 0f;
        src.ignoreListenerPause = ignorePause;
        return src;
    }

    private void LoadClips()
    {
        musicClip = Resources.Load<AudioClip>("Audio/main-theme");
        buttonClickClip = Resources.Load<AudioClip>("Audio/button-click");
        stepClip = Resources.Load<AudioClip>("Audio/steps");
        boxClip = Resources.Load<AudioClip>("Audio/box");
        doorClip = Resources.Load<AudioClip>("Audio/door");
        portalClip = Resources.Load<AudioClip>("Audio/portal");
        holeClip = Resources.Load<AudioClip>("Audio/hole");
        milkClip = Resources.Load<AudioClip>("Audio/milk");
        pauseClip = Resources.Load<AudioClip>("Audio/pause");
        restartClip = Resources.Load<AudioClip>("Audio/restart");
        levelUpClip = Resources.Load<AudioClip>("Audio/levelup");
        meowClip = Resources.Load<AudioClip>("Audio/meow");
    }

    public void LoadAndApplySettings()
    {
        // Preserve legacy "Volume" key as fallback if new keys haven't been saved yet.
        float legacyVol = PlayerPrefs.GetFloat("Volume", 0.8f);
        MusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MusicVolumeKey, legacyVol * 0.85f));
        SfxVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(SfxVolumeKey, legacyVol));
        MusicMuted = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;
        SfxMuted = PlayerPrefs.GetInt(SfxMutedKey, 0) == 1;

        AudioListener.volume = 1f;
        ApplyVolumes();

        if (PlayerPrefs.HasKey(FullscreenKey))
        {
            Screen.fullScreen = PlayerPrefs.GetInt(FullscreenKey, 0) == 1;
        }
    }

    private void StartMusicIfNeeded()
    {
        if (musicClip == null || musicSource == null) return;
        if (musicSource.clip != musicClip)
        {
            musicSource.clip = musicClip;
        }
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop lingering gameplay step/box sounds across scene loads; keep music & restart sound alive.
        StopStepImmediate();
        if (boxSource != null) boxSource.Stop();
        StartMusicIfNeeded();
        AttachButtonSoundsInLoadedScene();
    }

    public void AttachButtonSoundsInLoadedScene()
    {
        var buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var btn in buttons)
        {
            if (btn == null) continue;
            if (btn.GetComponent<UIButtonSoundMarker>() != null) continue;
            btn.gameObject.AddComponent<UIButtonSoundMarker>();
            btn.onClick.AddListener(PlayButtonClick);
        }
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetMusicMuted(bool muted)
    {
        MusicMuted = muted;
        PlayerPrefs.SetInt(MusicMutedKey, muted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetSfxMuted(bool muted)
    {
        SfxMuted = muted;
        PlayerPrefs.SetInt(SfxMutedKey, muted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyVolumes()
    {
        float effectiveMusic = MusicMuted ? 0f : MusicVolume;
        float effectiveSfx = SfxMuted ? 0f : SfxVolume;

        if (musicSource != null) musicSource.volume = effectiveMusic;
        if (stepSource != null) stepSource.volume = effectiveSfx * 0.75f;
        if (boxSource != null) boxSource.volume = effectiveSfx;
        if (sfxSource != null) sfxSource.volume = effectiveSfx;
        if (uiSource != null) uiSource.volume = effectiveSfx;
    }

    private bool CanPlay(string key, float cooldown)
    {
        if (SfxMuted || SfxVolume <= 0.001f) return false;
        float now = Time.unscaledTime;
        if (lastPlayedAt.TryGetValue(key, out float prev) && now - prev < cooldown)
            return false;
        lastPlayedAt[key] = now;
        return true;
    }

    public void PlayButtonClick()
    {
        if (buttonClickClip == null || uiSource == null) return;
        if (!CanPlay("btn", 0.06f)) return;
        uiSource.PlayOneShot(buttonClickClip, 1f);
    }

    public void PlayStep()
    {
        if (stepClip == null || stepSource == null) return;
        if (!CanPlay("step", 0.14f)) return;

        if (stepStopCoroutine != null)
        {
            StopCoroutine(stepStopCoroutine);
            stepStopCoroutine = null;
        }

        // steps.mp3 is ~3.76s long; play cleanly from start and cap per-step tail to avoid ghost footsteps.
        stepSource.Stop();
        stepSource.clip = stepClip;
        stepSource.volume = (SfxMuted ? 0f : SfxVolume) * 0.75f;
        stepSource.Play();
        stepStopCoroutine = StartCoroutine(StopStepAfterDelay(0.24f));
    }

    public void StopStep()
    {
        // Let the current brief step finish naturally within StopStepAfterDelay.
    }

    private void StopStepImmediate()
    {
        if (stepStopCoroutine != null)
        {
            StopCoroutine(stepStopCoroutine);
            stepStopCoroutine = null;
        }
        if (stepSource != null) stepSource.Stop();
    }

    private System.Collections.IEnumerator StopStepAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stepSource != null && stepSource.isPlaying)
        {
            float startVol = stepSource.volume;
            const float fade = 0.06f;
            float t = 0f;
            while (t < fade && stepSource.isPlaying)
            {
                t += Time.deltaTime;
                stepSource.volume = Mathf.Lerp(startVol, 0f, t / fade);
                yield return null;
            }
            stepSource.Stop();
            stepSource.volume = (SfxMuted ? 0f : SfxVolume) * 0.75f;
        }
        stepStopCoroutine = null;
    }

    public void PlayBoxPush()
    {
        if (boxClip == null || boxSource == null) return;
        if (!CanPlay("box", 0.14f)) return;
        boxSource.Stop();
        boxSource.clip = boxClip;
        boxSource.Play();
    }

    public void PlayDoor()
    {
        if (doorClip == null || sfxSource == null) return;
        if (!CanPlay("door", 0.25f)) return;
        sfxSource.PlayOneShot(doorClip);
    }

    public void PlayPortal()
    {
        if (portalClip == null || sfxSource == null) return;
        if (!CanPlay("portal", 0.2f)) return;
        sfxSource.PlayOneShot(portalClip);
    }

    public void PlayHole()
    {
        if (holeClip == null || sfxSource == null) return;
        if (!CanPlay("hole", 0.25f)) return;
        sfxSource.PlayOneShot(holeClip);
    }

    public void PlayMilk()
    {
        if (milkClip == null || sfxSource == null) return;
        if (!CanPlay("milk", 0.3f)) return;
        sfxSource.PlayOneShot(milkClip);
    }

    public void PlayPause()
    {
        if (pauseClip == null || uiSource == null) return;
        if (!CanPlay("pause", 0.15f)) return;
        uiSource.PlayOneShot(pauseClip);
    }

    public void PlayRestart()
    {
        if (restartClip == null || uiSource == null) return;
        if (!CanPlay("restart", 0.25f)) return;
        StopStepImmediate();
        uiSource.Stop();
        uiSource.clip = restartClip;
        uiSource.Play();
    }

    public void PlayLevelComplete()
    {
        if (levelUpClip == null || uiSource == null) return;
        if (!CanPlay("levelup", 0.5f)) return;
        StopStepImmediate();
        if (boxSource != null) boxSource.Stop();
        uiSource.Stop();
        uiSource.clip = levelUpClip;
        uiSource.Play();
    }

    public void PlayMeow()
    {
        if (meowClip == null || sfxSource == null) return;
        if (!CanPlay("meow", 0.4f)) return;
        sfxSource.PlayOneShot(meowClip);
    }
}

/// <summary>Marker component preventing duplicate click listeners on a UI Button.</summary>
public class UIButtonSoundMarker : MonoBehaviour { }
