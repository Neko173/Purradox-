using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    public static ResultsPanel Instance;

    [Header("UI элементы")]
    public TextMeshProUGUI stepsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI titleText;   // ← заголовок "Победа!" / "Время вышло!"
    public Image star1;
    public Image star2;
    public Image star3;

    [Header("Пороги звёзд")]
    public int stepsFor3Stars = 20;
    public int stepsFor2Stars = 40;

    [Header("Заголовки")]
    public string winTitle = "Победа!";
    public string failTitle = "Время вышло!";
    public Color winTitleColor = Color.white;
    public Color failTitleColor = new Color(1f, 0.4f, 0.4f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else if (Instance != this)
        {
            gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        bool failed = GameManager.Instance.isLevelFailed;
        int steps = GameManager.Instance.stepCount;
        float timeLeft = GameManager.Instance.levelTime;

        // Заголовок
        if (titleText != null)
        {
            titleText.text = failed ? failTitle : winTitle;
            titleText.color = failed ? failTitleColor : winTitleColor;
        }

        // Шаги
        if (stepsText != null)
            stepsText.text = "" + steps;

        // Время (показываем остаток или 00:00 при провале)
        if (timeText != null)
            timeText.text = failed ? "00:00" : LevelTimer.FormatTime(timeLeft);

        // Звёзды: при провале — ни одной
        int stars = failed ? 0 : CalculateStars(steps);
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
        if (star == null) return;
        star.color = active ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    public void OnNextLevelButton()
    {
        SceneManager.LoadScene("lvlSelect1");
    }
}