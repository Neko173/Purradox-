using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("Перетащи сюда кнопки уровней по порядку")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;
    public Button level6Button;
    public Button level7Button;
    public Button level8Button;
    public Button level9Button;
    // Добавляй столько, сколько у тебя уровней

    void Start()
    {
        int unlocked = GameManager.Instance.unlockedLevels;

        // Настраиваем кнопки
        SetupButton(level1Button, 1, unlocked);
        SetupButton(level2Button, 2, unlocked);
        SetupButton(level3Button, 3, unlocked);
        SetupButton(level4Button, 4, unlocked);
        SetupButton(level5Button, 5, unlocked);
        SetupButton(level6Button, 6, unlocked);
        SetupButton(level7Button, 7, unlocked);
        SetupButton(level8Button, 8, unlocked);
        SetupButton(level9Button, 9, unlocked);
    }

    void SetupButton(Button btn, int levelNumber, int unlockedLevels)
    {
        if (btn == null) return;

        // Если уровень открыт (например, открыто 2 уровня, значит уровень 1 и 2 доступны)
        if (levelNumber <= unlockedLevels)
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false; // Кнопка неактивна (серая)
            // Опционально: можно менять цвет или добавлять иконку замка
            // btn.GetComponent<Image>().color = Color.gray;
        }
    }
}