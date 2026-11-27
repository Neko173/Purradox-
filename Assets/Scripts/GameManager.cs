using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int TotalLevels = 9;
    private const string UnlockedLevelsKey = "UnlockedLevels";

    public static GameManager Instance;

    public bool hasMilkBowl = false;
    public int stepCount = 0;

    // Прогресс: сколько уровней открыто (по умолчанию 1)
    public int unlockedLevels = 1;

    public event Action OnProgressChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Загружаем сохранённый прогресс при запуске игры
            unlockedLevels = PlayerPrefs.GetInt(UnlockedLevelsKey, 1);
            AudioManager.EnsureExists();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Сбрасываем данные уровня, но НЕ прогресс
        hasMilkBowl = false;
        stepCount = 0;
    }

    public void CollectMilkBowl()
    {
        hasMilkBowl = true;
    }

    public void AddStep()
    {
        stepCount++;
    }

    public int GetCurrentLevelNumber()
    {
        Scene scene = SceneManager.GetActiveScene();
        int fromBuild = scene.buildIndex - 2;
        if (fromBuild >= 1 && fromBuild <= TotalLevels)
            return fromBuild;

        if (scene.name.StartsWith("Level") && int.TryParse(scene.name.Substring(5), out int parsed))
            return parsed;

        return 1;
    }

    // Вызывать при успешном прохождении уровня
    public void CompleteLevel(int currentLevelIndex)
    {
        // Если текущий уровень равен или больше открытых, открываем следующий
        if (currentLevelIndex >= unlockedLevels)
        {
            unlockedLevels = Mathf.Clamp(currentLevelIndex + 1, 1, TotalLevels);
            PlayerPrefs.SetInt(UnlockedLevelsKey, unlockedLevels);
            PlayerPrefs.Save(); // Сохраняем на диск
            Debug.Log("Уровень пройден! Открыт уровень: " + unlockedLevels);
            OnProgressChanged?.Invoke();
        }
    }

    /// <summary>
    /// Сохраняет лучший результат прохождения уровня (звёзды и минимальное число шагов)
    /// через существующую систему PlayerPrefs, не меняя логику расчёта.
    /// Возвращает true, если установлен новый рекорд по шагам или звёздам.
    /// </summary>
    public bool RecordLevelResult(int levelNumber, int stars, int steps)
    {
        if (levelNumber < 1 || levelNumber > TotalLevels) return false;

        int prevStars = GetLevelStars(levelNumber);
        int prevBestSteps = GetLevelBestSteps(levelNumber);

        bool improved = false;
        if (stars > prevStars)
        {
            PlayerPrefs.SetInt(StarsKey(levelNumber), stars);
            improved = true;
        }

        if (steps > 0 && (prevBestSteps <= 0 || steps < prevBestSteps))
        {
            PlayerPrefs.SetInt(BestStepsKey(levelNumber), steps);
            improved = true;
        }

        if (improved)
        {
            PlayerPrefs.Save();
            OnProgressChanged?.Invoke();
        }

        return improved;
    }

    public int GetLevelStars(int levelNumber)
    {
        return PlayerPrefs.GetInt(StarsKey(levelNumber), 0);
    }

    public int GetLevelBestSteps(int levelNumber)
    {
        return PlayerPrefs.GetInt(BestStepsKey(levelNumber), 0);
    }

    public int GetTotalStars()
    {
        int total = 0;
        for (int i = 1; i <= TotalLevels; i++)
            total += GetLevelStars(i);
        return total;
    }

    public void ResetProgress()
    {
        unlockedLevels = 1;
        PlayerPrefs.SetInt(UnlockedLevelsKey, 1);

        for (int i = 1; i <= TotalLevels; i++)
        {
            PlayerPrefs.DeleteKey(StarsKey(i));
            PlayerPrefs.DeleteKey(BestStepsKey(i));
        }

        PlayerPrefs.Save();
        OnProgressChanged?.Invoke();
    }

    private static string StarsKey(int levelNumber) => $"Level_{levelNumber}_Stars";
    private static string BestStepsKey(int levelNumber) => $"Level_{levelNumber}_BestSteps";
}
