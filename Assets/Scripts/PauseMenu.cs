using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;        // Панель паузы (изначально выключена)
    public GameObject pauseButton;       // Кнопка паузы на HUD (опционально)

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
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

        isPaused = true;
        Time.timeScale = 0f;                 // Останавливаем игровое время
        if (pausePanel != null) pausePanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);
    }

    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
    }

    // Кнопка "Заново" в меню паузы
    public void RestartLevel()
    {
        Time.timeScale = 1f;                 // Обязательно до LoadScene!
        isPaused = false;

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
}