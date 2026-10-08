using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu:MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SimpleScene");
        Debug.Log("Игра начата");
    }

    public void Settings()
    {
        Debug.Log("Переходим в настройки");
    }

    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();
    }
}
