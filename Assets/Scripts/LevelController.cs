using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Сброс текущего уровня: перезагружаем активную сцену
    public void ResetLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.stepCount = 0;
            GameManager.Instance.hasMilkBowl = false;
            GameManager.Instance.levelTime = 0f;
            GameManager.Instance.isLevelFailed = false;   // ← новый сброс
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}