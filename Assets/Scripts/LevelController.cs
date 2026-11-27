using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void ResetLevel()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.stepCount = 0;
            GameManager.Instance.hasMilkBowl = false;
        }

        AudioManager.Instance?.PlayRestart();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log($"[LevelController] Уровень '{currentScene.name}' перезагружен.");
    }
}
