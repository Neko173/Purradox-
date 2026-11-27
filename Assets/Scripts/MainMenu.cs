using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Опционально: панель обучения")]
    public GameObject tutorialPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("lvlSelect1");
    }

    public void Settings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void OnCatMascotClicked()
    {
        AudioManager.Instance?.PlayMeow();
    }

    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();
    }
}
