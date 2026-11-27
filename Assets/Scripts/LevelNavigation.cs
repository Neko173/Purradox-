using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Управляет кнопками «Назад» и «Вперёд» на каждом игровом уровне.
/// </summary>
public class LevelNavigation : MonoBehaviour
{
    public Button prevLevelButton;
    public Button nextLevelButton;
    public TextMeshProUGUI levelLabel;

    private int currentLevel = 1;

    private void Start()
    {
        currentLevel = GameManager.Instance != null
            ? GameManager.Instance.GetCurrentLevelNumber()
            : ParseLevelFromSceneName();

        if (levelLabel != null)
        {
            levelLabel.text = $"Ур. {currentLevel}/{GameManager.TotalLevels}";
        }

        if (prevLevelButton != null)
        {
            prevLevelButton.onClick.RemoveListener(GoToPreviousLevel);
            prevLevelButton.onClick.AddListener(GoToPreviousLevel);
            prevLevelButton.interactable = true;
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveListener(GoToNextLevel);
            nextLevelButton.onClick.AddListener(GoToNextLevel);
            // На последнем уровне кнопка «Вперёд» блокируется, чтобы избежать ошибки
            nextLevelButton.interactable = currentLevel < GameManager.TotalLevels;
        }
    }

    public void GoToPreviousLevel()
    {
        Time.timeScale = 1f;
        if (currentLevel <= 1)
        {
            SceneManager.LoadScene("lvlSelect1");
        }
        else
        {
            SceneManager.LoadScene($"Level{currentLevel - 1}");
        }
    }

    public void GoToNextLevel()
    {
        if (currentLevel >= GameManager.TotalLevels) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene($"Level{currentLevel + 1}");
    }

    private static int ParseLevelFromSceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level") && int.TryParse(sceneName.Substring(5), out int n))
            return Mathf.Clamp(n, 1, GameManager.TotalLevels);
        return 1;
    }
}
