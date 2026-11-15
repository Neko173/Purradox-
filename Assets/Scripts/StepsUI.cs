using UnityEngine;
using TMPro;

public class StepsUI : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        if (GameManager.Instance != null && textComponent != null)
        {
            textComponent.text = "Шаги: " + GameManager.Instance.stepCount;
        }
    }
}