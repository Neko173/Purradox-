using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("Кнопки уровней по порядку")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;
    public Button level6Button;
    public Button level7Button;
    public Button level8Button;
    public Button level9Button;

    [Header("Рекорды и звёзды")]
    public Sprite filledStarSprite;
    public Sprite emptyStarSprite;
    public Sprite unlockedCardSprite;
    public Sprite lockedCardSprite;
    public TextMeshProUGUI totalStarsText;
    public TextMeshProUGUI totalCompletedText;

    void Start()
    {
        RefreshAll();
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnProgressChanged += RefreshAll;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnProgressChanged -= RefreshAll;
    }

    public void RefreshAll()
    {
        int unlocked = GameManager.Instance != null
            ? GameManager.Instance.unlockedLevels
            : PlayerPrefs.GetInt("UnlockedLevels", 1);

        Button[] buttons = {
            level1Button, level2Button, level3Button,
            level4Button, level5Button, level6Button,
            level7Button, level8Button, level9Button
        };

        int totalStars = 0;
        int completedCount = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            int levelNum = i + 1;
            SetupButton(buttons[i], levelNum, unlocked);

            int stars = GameManager.Instance != null
                ? GameManager.Instance.GetLevelStars(levelNum)
                : PlayerPrefs.GetInt($"Level_{levelNum}_Stars", 0);
            int bestSteps = GameManager.Instance != null
                ? GameManager.Instance.GetLevelBestSteps(levelNum)
                : PlayerPrefs.GetInt($"Level_{levelNum}_BestSteps", 0);

            totalStars += stars;
            if (bestSteps > 0 || stars > 0) completedCount++;
        }

        if (totalStarsText != null)
            totalStarsText.text = $"Звёзды: {totalStars} / {GameManager.TotalLevels * 3}";

        if (totalCompletedText != null)
            totalCompletedText.text = $"Пройдено: {completedCount} / {GameManager.TotalLevels}";
    }

    void SetupButton(Button btn, int levelNumber, int unlockedLevels)
    {
        if (btn == null) return;

        bool isUnlocked = levelNumber <= unlockedLevels;
        btn.interactable = isUnlocked;

        var img = btn.GetComponent<Image>();
        if (img != null && unlockedCardSprite != null && lockedCardSprite != null)
        {
            img.sprite = isUnlocked ? unlockedCardSprite : lockedCardSprite;
        }

        int stars = GameManager.Instance != null
            ? GameManager.Instance.GetLevelStars(levelNumber)
            : PlayerPrefs.GetInt($"Level_{levelNumber}_Stars", 0);
        int bestSteps = GameManager.Instance != null
            ? GameManager.Instance.GetLevelBestSteps(levelNumber)
            : PlayerPrefs.GetInt($"Level_{levelNumber}_BestSteps", 0);

        Transform starsRow = btn.transform.Find("StarsRow");
        if (starsRow != null)
        {
            starsRow.gameObject.SetActive(isUnlocked);
            for (int s = 1; s <= 3; s++)
            {
                Transform starTr = starsRow.Find($"Star{s}");
                if (starTr == null) continue;
                Image starImg = starTr.GetComponent<Image>();
                if (starImg == null) continue;

                bool active = stars >= s;
                if (filledStarSprite != null && emptyStarSprite != null)
                {
                    starImg.sprite = active ? filledStarSprite : emptyStarSprite;
                    starImg.color = Color.white;
                }
                else
                {
                    starImg.color = active ? Color.white : new Color(0.35f, 0.35f, 0.35f, 1f);
                }
            }
        }

        Transform recordTr = btn.transform.Find("RecordText");
        if (recordTr != null)
        {
            var tmp = recordTr.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = !isUnlocked
                    ? "Закрыто"
                    : (bestSteps > 0 ? $"Рекорд: {bestSteps}" : "Не пройден");
            }
        }

        Transform lockTr = btn.transform.Find("LockIcon");
        if (lockTr != null)
        {
            lockTr.gameObject.SetActive(!isUnlocked);
        }
    }
}
