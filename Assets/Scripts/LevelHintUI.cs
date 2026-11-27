using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ненавязчивая пропускаемая подсказка внизу экрана для первых уровней с новыми механиками.
/// Никогда не блокирует управление и может быть скрыта или открыта снова кнопкой «?».
/// </summary>
public class LevelHintUI : MonoBehaviour
{
    private const string HintsHiddenKey = "TutorialHintsHidden";

    public GameObject hintBanner;
    public TextMeshProUGUI hintText;
    public Button skipHintsButton;
    public Button helpButton;
    public GameObject tutorialPanel;

    private int levelNumber = 1;
    private string levelHintMessage;

    private void Start()
    {
        levelNumber = GameManager.Instance != null
            ? GameManager.Instance.GetCurrentLevelNumber()
            : 1;

        levelHintMessage = GetHintForLevel(levelNumber);

        if (skipHintsButton != null)
        {
            skipHintsButton.onClick.RemoveListener(SkipHints);
            skipHintsButton.onClick.AddListener(SkipHints);
        }

        if (helpButton != null)
        {
            helpButton.onClick.RemoveListener(OpenTutorialFromHud);
            helpButton.onClick.AddListener(OpenTutorialFromHud);
        }

        RefreshBannerVisibility();
    }

    public void RefreshBannerVisibility()
    {
        if (hintBanner == null) return;

        bool hidden = PlayerPrefs.GetInt(HintsHiddenKey, 0) == 1;
        bool hasHint = !string.IsNullOrEmpty(levelHintMessage);

        hintBanner.SetActive(hasHint && !hidden);
        if (hasHint && hintText != null)
        {
            hintText.text = levelHintMessage;
        }
    }

    public void SkipHints()
    {
        PlayerPrefs.SetInt(HintsHiddenKey, 1);
        PlayerPrefs.Save();
        if (hintBanner != null) hintBanner.SetActive(false);
    }

    public void OpenTutorialFromHud()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        }
    }

    private static string GetHintForLevel(int level)
    {
        switch (level)
        {
            case 1:
                return "WASD / Стрелки — ходьба. Толкай ящики, собери молоко и иди к двери!";
            case 2:
                return "Совет: следи за шагами — меньше шагов даёт 3 звезды!";
            case 4:
                return "Новая механика: клубок катится до препятствия и закрывает яму, чтобы по ней можно было пройти.";
            case 7:
                return "Новая механика: порталы мгновенно переносят котика к парному порталу!";
            default:
                return string.Empty;
        }
    }
}
