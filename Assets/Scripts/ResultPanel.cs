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

    [Header("Пороги звёзд")]
    public int stepsFor3Stars = 20;
    public int stepsFor2Stars = 40;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false); // скрываем, но НЕ уничтожаем
        }
        else if (Instance != this)
        {
            // Если это дубликат — просто выключаем его, не уничтожаем
            gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        int steps = GameManager.Instance.stepCount;

        // Текст шагов
        stepsText.text = "" + steps / 2;

        // Звёзды
        int stars = CalculateStars(steps / 2);
        SetStar(star1, stars >= 1);
        SetStar(star2, stars >= 2);
        SetStar(star3, stars >= 3);
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
        Debug.Log("Загрузка сцены");
    }
}