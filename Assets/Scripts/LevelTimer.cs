using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    [Header("Настройки (задаются на каждом уровне)")]
    [Tooltip("Сколько секунд даётся на уровень")]
    public float timeLimit = 60f;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Цвета текста")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    [Tooltip("За сколько секунд до конца включать предупреждающий цвет")]
    public float warningThreshold = 10f;

    [Header("События")]
    public UnityEvent onTimeOut;   // что вызвать при истечении времени

    private float remainingTime;
    private bool isRunning = false;
    private bool isFinished = false;

    void Start()
    {
        remainingTime = timeLimit;
        isRunning = true;
        isFinished = false;
        UpdateText();
    }

    void Update()
    {
        if (!isRunning || isFinished) return;

        // При Time.timeScale = 0 (пауза) таймер сам замирает
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateText();
            TimeOut();
            return;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (timerText == null) return;
        timerText.text = FormatTime(remainingTime);

        // Подсветка, когда время на исходе
        if (remainingTime <= warningThreshold)
            timerText.color = warningColor;
        else
            timerText.color = normalColor;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void TimeOut()
    {
        if (isFinished) return;
        isFinished = true;
        isRunning = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.levelTime = 0f;
            GameManager.Instance.isLevelFailed = true;   // ← провал
        }

        Debug.Log("[LevelTimer] Время вышло!");

        if (ResultsPanel.Instance != null)
            ResultsPanel.Instance.Show();
    }

    public static string FormatTime(float seconds)
    {
        if (seconds < 0f) seconds = 0f;
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }
}