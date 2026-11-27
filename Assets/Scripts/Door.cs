using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("Спрайт открытой двери")]
    public Sprite openDoorSprite;

    private BoxCollider2D doorCollider;
    private SpriteRenderer spriteRenderer;
    private Sprite closedDoorSprite;
    private bool hasTriggered = false;
    private bool isCurrentlyOpen = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var colliders = GetComponents<BoxCollider2D>();
        if (colliders.Length > 0) doorCollider = colliders[0];

        if (spriteRenderer != null)
        {
            closedDoorSprite = spriteRenderer.sprite; // Запоминаем закрытый спрайт
        }
    }

    void Update()
    {
        // Защита: если GameManager ещё не создан — ничего не делаем
        if (GameManager.Instance == null) return;

        bool isOpen = GameManager.Instance.hasMilkBowl;

        if (isOpen != isCurrentlyOpen)
        {
            isCurrentlyOpen = isOpen;

            if (doorCollider != null)
                doorCollider.enabled = !isOpen;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = isOpen ? openDoorSprite : closedDoorSprite;
            }

            if (isOpen)
            {
                AudioManager.Instance?.PlayDoor();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance == null || !GameManager.Instance.hasMilkBowl) return;

        hasTriggered = true;
        AudioManager.Instance?.PlayDoor();
        ResultsPanel.Instance.Show();
    }
}
