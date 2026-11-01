using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void ResetLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.stepCount = 0;
            GameManager.Instance.hasMilkBowl = false;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log($"[LevelController] Уровень '{currentScene.name}' перезагружен.");
    }
}