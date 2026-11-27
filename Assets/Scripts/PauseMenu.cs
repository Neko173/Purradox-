using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;        // Панель паузы (изначально выключена)
    public GameObject pauseButton;       // Кнопка паузы на HUD (опционально)
    public GameObject restartButton;
    public GameObject topNavBar;         // Верхняя панель навигации по уровням
    public GameObject helpButton;
    public GameObject tutorialPanel;

    [Header("Быстрые переключатели звука в паузе")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    private bool isPaused = false;
    private bool isSyncingAudio = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        SyncAudioToggles();
    }

    void Update()
    {
        // Не открываем паузу поверх экрана победы
        if (ResultsPanel.Instance != null && ResultsPanel.Instance.IsVisible)
            return;

        // Если открыто окно обучения — Escape сначала закрывает его
        if (tutorialPanel != null && tutorialPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseTutorial();
            }
            return;
        }

        // Пауза по Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // ---------- Кнопки ----------

    public void Pause()
    {
        if (isPaused) return;
        if (ResultsPanel.Instance != null && ResultsPanel.Instance.IsVisible) return;

        isPaused = true;
        Time.timeScale = 0f;                 // Останавливаем игровое время
        SyncAudioToggles();
        AudioManager.Instance?.PlayPause();

        if (pausePanel != null) pausePanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (topNavBar != null) topNavBar.SetActive(false);
        if (helpButton != null) helpButton.SetActive(false);
    }

    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;
        AudioManager.Instance?.PlayPause();

        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
        if (restartButton != null) restartButton.SetActive(true);
        if (topNavBar != null) topNavBar.SetActive(true);
        if (helpButton != null) helpButton.SetActive(true);
    }

    // Кнопка "Заново" в меню паузы
    public void RestartLevel()
    {
        Time.timeScale = 1f;                 // Обязательно до LoadScene!
        isPaused = false;
        AudioManager.Instance?.PlayRestart();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Кнопка "В главное меню"
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;                 // Обязательно до LoadScene!
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevelSelect()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("lvlSelect1");
    }

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

    public void OnMusicToggleChanged(bool enabled)
    {
        if (isSyncingAudio) return;
        AudioManager.EnsureExists().SetMusicMuted(!enabled);
    }

    public void OnSfxToggleChanged(bool enabled)
    {
        if (isSyncingAudio) return;
        AudioManager.EnsureExists().SetSfxMuted(!enabled);
    }

    private void SyncAudioToggles()
    {
        isSyncingAudio = true;
        var audio = AudioManager.EnsureExists();
        if (musicToggle != null) musicToggle.isOn = !audio.MusicMuted;
        if (sfxToggle != null) sfxToggle.isOn = !audio.SfxMuted;
        isSyncingAudio = false;
    }
}
