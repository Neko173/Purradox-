using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Элементы UI")]
    public Slider volumeSlider;       // Громкость музыки
    public Slider sfxSlider;          // Громкость звуков
    public Toggle musicMuteToggle;    // Выкл/вкл музыки (isOn = звук включён)
    public Toggle sfxMuteToggle;      // Выкл/вкл звуков (isOn = звук включён)
    public Toggle fullscreenToggle;
    public Button resetProgressBtn;
    public Button tutorialBtn;
    public TextMeshProUGUI musicValueText;
    public TextMeshProUGUI sfxValueText;
    public TextMeshProUGUI feedbackText;

    [Header("Панели")]
    public GameObject tutorialPanel;

    private bool isInitializing = false;

    private void Awake()
    {
        AudioManager.EnsureExists();
        LoadSettings();
    }

    private void OnEnable()
    {
        if (feedbackText != null)
            feedbackText.text = string.Empty;
    }

    // Вызывается из OnValueChanged у слайдера музыки
    public void OnVolumeChanged(float value)
    {
        if (isInitializing) return;
        AudioManager.EnsureExists().SetMusicVolume(value);
        UpdateVolumeLabels();
    }

    // Вызывается из OnValueChanged у слайдера звуковых эффектов
    public void OnSfxVolumeChanged(float value)
    {
        if (isInitializing) return;
        AudioManager.EnsureExists().SetSfxVolume(value);
        UpdateVolumeLabels();
    }

    // Toggle: isOn == true означает "Музыка включена"
    public void OnMusicEnabledChanged(bool isEnabled)
    {
        if (isInitializing) return;
        AudioManager.EnsureExists().SetMusicMuted(!isEnabled);
        UpdateVolumeLabels();
    }

    // Toggle: isOn == true означает "Звуки включены"
    public void OnSfxEnabledChanged(bool isEnabled)
    {
        if (isInitializing) return;
        AudioManager.EnsureExists().SetSfxMuted(!isEnabled);
        UpdateVolumeLabels();
    }

    // Вызывается из OnValueChanged у Toggle полноэкранного режима
    public void OnFullscreenChanged(bool isFullscreen)
    {
        if (isInitializing) return;
        AudioManager.EnsureExists().SetFullscreen(isFullscreen);
    }

    // Кнопка «Сброс прогресса» (сбрасывает только прогресс и рекорды, сохраняя настройки)
    public void ResetProgress()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetProgress();
        }
        else
        {
            PlayerPrefs.SetInt("UnlockedLevels", 1);
            for (int i = 1; i <= GameManager.TotalLevels; i++)
            {
                PlayerPrefs.DeleteKey($"Level_{i}_Stars");
                PlayerPrefs.DeleteKey($"Level_{i}_BestSteps");
            }
            PlayerPrefs.Save();
        }

        if (feedbackText != null)
            feedbackText.text = "Прогресс и рекорды сброшены!";

        Debug.Log("[Settings] Прогресс и рекорды сброшены.");
    }

    // Кнопка «Обучение / Руководство»
    public void OpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void SaveSettings()
    {
        var audio = AudioManager.EnsureExists();
        if (volumeSlider != null) audio.SetMusicVolume(volumeSlider.value);
        if (sfxSlider != null) audio.SetSfxVolume(sfxSlider.value);
        if (musicMuteToggle != null) audio.SetMusicMuted(!musicMuteToggle.isOn);
        if (sfxMuteToggle != null) audio.SetSfxMuted(!sfxMuteToggle.isOn);
        if (fullscreenToggle != null) audio.SetFullscreen(fullscreenToggle.isOn);
    }

    public void CloseSettings()
    {
        SaveSettings();
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadSettings()
    {
        isInitializing = true;
        var audio = AudioManager.EnsureExists();
        audio.LoadAndApplySettings();

        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", Screen.fullScreen ? 1 : 0) == 1;

        if (volumeSlider != null) volumeSlider.value = audio.MusicVolume;
        if (sfxSlider != null) sfxSlider.value = audio.SfxVolume;
        if (musicMuteToggle != null) musicMuteToggle.isOn = !audio.MusicMuted;
        if (sfxMuteToggle != null) sfxMuteToggle.isOn = !audio.SfxMuted;
        if (fullscreenToggle != null) fullscreenToggle.isOn = isFullscreen;

        UpdateVolumeLabels();
        isInitializing = false;
    }

    private void UpdateVolumeLabels()
    {
        var audio = AudioManager.EnsureExists();
        if (musicValueText != null)
            musicValueText.text = audio.MusicMuted ? "Выкл" : $"{Mathf.RoundToInt(audio.MusicVolume * 100f)}%";
        if (sfxValueText != null)
            sfxValueText.text = audio.SfxMuted ? "Выкл" : $"{Mathf.RoundToInt(audio.SfxVolume * 100f)}%";
    }
}
