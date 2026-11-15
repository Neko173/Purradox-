using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasMilkBowl = false;
    public int stepCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем менеджер между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetLevelData(); // Сбрасываем данные при старте каждой сцены
    }

    public void ResetLevelData()
    {
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

    void OnEnable()
{
    UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDisable()
{
    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
}

void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
{
    ResetLevelData();
}
}