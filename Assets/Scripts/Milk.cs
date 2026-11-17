using UnityEngine;

public class MilkBowl : MonoBehaviour
{
    [Tooltip("Спрайт пустой миски")]
    public Sprite emptyBowlSprite;

    private bool isCollected = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            GameManager.Instance.CollectMilkBowl();
            if (spriteRenderer != null && emptyBowlSprite != null)
            {
                spriteRenderer.sprite = emptyBowlSprite;
            }
        }
    }
}