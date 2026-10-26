using UnityEngine;

public class Pit : MonoBehaviour
{
    public bool isFilled = false;

    // Заткнуть лунку (вызывается клубком, когда он падает)
    public void Fill()
    {
        isFilled = true;
        // Визуально: лунка становится «закрытой» — например, полупрозрачной
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.3f;
            sr.color = c;
        }
    }
}