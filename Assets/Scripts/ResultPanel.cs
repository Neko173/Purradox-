using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    public static ResultsPanel Instance;

    [Header("UI элементы")]
    public TextMeshProUGUI stepsText;
    public TextMeshProUGUI bestStepsText;
    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite filledStarSprite;
    public Sprite emptyStarSprite;
    public GameObject pauseButton;
    public GameObject restartButton;
    public GameObject StepsCounterUI;
    public GameObject stepsPill;
    public GameObject helpButton;
    public GameObject topNavBar;
    public GameObject hintBanner;
    public GameObject pausePanel;
    public Button nextLevelButton;

    [Header("Пороги звёзд")]
    public int stepsFor3Stars = 20;
    public int stepsFor2Stars = 40;

    private CanvasGroup canvasGroup;
    public bool IsVisible { get; private set; }

    void Awake()
    {
        // Настройка синглтона
        if (Instance == null)
        {
            Instance = this;

            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("На объекте " + gameObject.name + " нет компонента Canvas Group! Добавь его.");
                return;
            }

            // Скрываем панель при старте (не выключая объект)
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            IsVisible = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show()
    {
        IsVisible = true;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        if (pauseButton != null) pauseButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (StepsCounterUI != null) StepsCounterUI.SetActive(false);
        if (stepsPill != null) stepsPill.SetActive(false);
        if (helpButton != null) helpButton.SetActive(false);
        if (topNavBar != null) topNavBar.SetActive(false);
        if (hintBanner != null) hintBanner.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;

        int steps = GameManager.Instance != null ? GameManager.Instance.stepCount : 0;
        if (stepsText != null) stepsText.text = "Шагов: " + steps;

        int stars = CalculateStars(steps);
        if (star1 != null) SetStar(star1, stars >= 1);
        if (star2 != null) SetStar(star2, stars >= 2);
        if (star3 != null) SetStar(star3, stars >= 3);

        // --- СОХРАНЯЕМ ПРОГРЕСС И РЕКОРД ---
        int levelNumber = GameManager.Instance != null
            ? GameManager.Instance.GetCurrentLevelNumber()
            : Mathf.Max(1, SceneManager.GetActiveScene().buildIndex - 2);

        bool isNewRecord = false;
        int bestSteps = steps;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteLevel(levelNumber);
            isNewRecord = GameManager.Instance.RecordLevelResult(levelNumber, stars, steps);
            bestSteps = GameManager.Instance.GetLevelBestSteps(levelNumber);
        }

        if (bestStepsText != null)
        {
            bestStepsText.text = isNewRecord
                ? $"Новый рекорд: {bestSteps} ш."
                : $"Лучший результат: {bestSteps} ш.";
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = levelNumber < GameManager.TotalLevels;
        }

        AudioManager.Instance?.PlayLevelComplete();
    }

    int CalculateStars(int steps)
    {
        if (steps <= stepsFor3Stars) return 3;
        if (steps <= stepsFor2Stars) return 2;
        return 1;
    }

    void SetStar(Image star, bool active)
    {
        if (filledStarSprite != null && emptyStarSprite != null)
        {
            star.sprite = active ? filledStarSprite : emptyStarSprite;
            star.color = Color.white;
        }
        else
        {
            star.color = active ? Color.white : new Color(0.35f, 0.35f, 0.35f, 1f);
        }
    }

    public void OnNextLevelButton()
    {
        Time.timeScale = 1f;
        int levelNumber = GameManager.Instance != null
            ? GameManager.Instance.GetCurrentLevelNumber()
            : Mathf.Max(1, SceneManager.GetActiveScene().buildIndex - 2);

        if (levelNumber < GameManager.TotalLevels)
        {
            SceneManager.LoadScene($"Level{levelNumber + 1}");
        }
        else
        {
            SceneManager.LoadScene("lvlSelect1");
        }
    }

    public void OnLevelSelectButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("lvlSelect1");
    }

    public void OnRestartLevelButton()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.PlayRestart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
