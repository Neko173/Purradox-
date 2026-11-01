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
        
        if (pauseButton != null) pauseButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);

        int steps = GameManager.Instance.stepCount;
        stepsText.text = "" + steps;

        int stars = CalculateStars(steps);
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
        Debug.Log("Загрузка сцены");
    }
}