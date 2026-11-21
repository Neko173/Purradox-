using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    public static ResultsPanel Instance;

    [Header("UI элементы")]
    public TextMeshProUGUI stepsText;
    public Image star1;
    public Image star2;
    public Image star3;
    public GameObject pauseButton;
    public GameObject restartButton;
    public GameObject StepsCounterUI;

    [Header("Пороги звёзд")]
    public int stepsFor3Stars = 20;
    public int stepsFor2Stars = 40;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Настройка синглтона
        if (Instance == null)
        {
            Instance = this;
            
            // Получаем CanvasGroup
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
        }
        else
        {
            // Уничтожаем дубликат (если он есть)
            Destroy(gameObject);
        }
    }

    public void Show()
{
    if (canvasGroup != null)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    int steps = GameManager.Instance.stepCount;
    if (stepsText != null) stepsText.text = "Шагов: " + steps;

    int stars = CalculateStars(steps);
    if (star1 != null) SetStar(star1, stars >= 1);
    if (star2 != null) SetStar(star2, stars >= 2);
    if (star3 != null) SetStar(star3, stars >= 3);

    // --- СОХРАНЯЕМ ПРОГРЕСС ---
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int levelNumber = currentSceneIndex - 2;
    GameManager.Instance.CompleteLevel(levelNumber);
}

    int CalculateStars(int steps)
    {
        if (steps <= stepsFor3Stars) return 3;
        if (steps <= stepsFor2Stars) return 2;
        return 1;
    }

    void SetStar(Image star, bool active)
    {
        star.color = active ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    public void OnNextLevelButton()
    {
        SceneManager.LoadScene("lvlSelect1");
    }
}