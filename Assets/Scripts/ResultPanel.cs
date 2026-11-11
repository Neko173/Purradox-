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

    [Header("Пороги звёзд")]
    public int stepsFor3Stars = 20;
    public int stepsFor2Stars = 40;

    void Awake()
{
    Debug.Log("Попытка инициализации ResultsPanel. Instance = " + (Instance == null ? "null" : "уже есть!"));
    
    if (Instance == null)
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    else
    {
        Debug.Log("Уничтожаю дубликат: " + gameObject.name);
        Destroy(gameObject);
    }
}

    public void Show()
    {
        Debug.Log("1. Show() вызван");
    gameObject.SetActive(true);
    Debug.Log("2. Панель включена. activeSelf = " + gameObject.activeSelf);

    int steps = GameManager.Instance.stepCount;
    Debug.Log("3. Шагов: " + steps);

    if (stepsText != null)
    {
        stepsText.text = "Шагов: " + steps;
    }
    else
    {
        Debug.LogError("4. ОШИБКА: stepsText равен null! Перетащи текст в Inspector.");
    }

    int stars = CalculateStars(steps);
    if (star1 != null) SetStar(star1, stars >= 1);
    if (star2 != null) SetStar(star2, stars >= 2);
    if (star3 != null) SetStar(star3, stars >= 3);
    Debug.Log("5. Звёзды настроены");
    }

    int CalculateStars(int steps)
    {
        if (steps <= stepsFor3Stars) return 3;
        if (steps <= stepsFor2Stars) return 2;
        return 1;
    }

    void SetStar(Image star, bool active)
    {
        if (star == null) return;
        star.color = active ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    public void OnNextLevelButton()
    {
        SceneManager.LoadScene("lvlSelect1");
        Debug.Log("Загрузка сцены");
    }
}