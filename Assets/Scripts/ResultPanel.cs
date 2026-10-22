using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPanel : MonoBehaviour
{
    public static ResultsPanel Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false); // скрыта по умолчанию
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnNextLevelButton()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}