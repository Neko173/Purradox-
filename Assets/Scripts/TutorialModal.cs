using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет окном обучения (справочником механик), доступным из главного меню, настроек и уровня.
/// </summary>
public class TutorialModal : MonoBehaviour
{
    public Button closeButton;
    public Button resetHintsButton;

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(Close);
            closeButton.onClick.AddListener(Close);
        }

        if (resetHintsButton != null)
        {
            resetHintsButton.onClick.RemoveListener(ShowLevelHintsAgain);
            resetHintsButton.onClick.AddListener(ShowLevelHintsAgain);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ShowLevelHintsAgain()
    {
        PlayerPrefs.SetInt("TutorialHintsHidden", 0);
        PlayerPrefs.Save();
    }
}
