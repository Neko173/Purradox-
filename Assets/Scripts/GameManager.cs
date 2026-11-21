using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasMilkBowl = false;
    public int stepCount = 0;

    // Прогресс: сколько уровней пройдено (по умолчанию 0)
    public int unlockedLevels = 1; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Загружаем сохранённый прогресс при запуске игры
            unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
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

    // Вызывать при успешном прохождении уровня
    public void CompleteLevel(int currentLevelIndex)
    {
        // Если текущий уровень равен или больше открытых, открываем следующий
        if (currentLevelIndex >= unlockedLevels)
        {
            unlockedLevels = currentLevelIndex + 1;
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels);
            PlayerPrefs.Save(); // Сохраняем на диск
            Debug.Log("Уровень пройден! Открыт уровень: " + unlockedLevels);
        }
    }

    public void ResetProgress()
    {
        unlockedLevels = 1;
        PlayerPrefs.SetInt("UnlockedLevels", 1);
        PlayerPrefs.Save();
    }
}