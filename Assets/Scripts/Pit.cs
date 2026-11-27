using UnityEngine;

public class Pit : MonoBehaviour
{
    public bool isFilled = false;

    [Tooltip("Спрайт заполненной ямы (если не задан, яма станет полупрозрачной)")]
    public Sprite filledPitSprite;

    // Заткнуть лунку (вызывается клубком, когда он падает)
    public void Fill()
    {
        isFilled = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (filledPitSprite != null)
            {
                sr.sprite = filledPitSprite;
                sr.color = Color.white;
            }
            else
            {
                Color c = sr.color;
                c.a = 0.3f;
                sr.color = c;
            }
        }
    }
}
