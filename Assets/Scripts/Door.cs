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
        doorCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponents<BoxCollider2D>()[0];
        
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
    }
}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Дверь: OnTriggerEnter2D сработал с " + other.name);
        
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log("Дверь: игрок обнаружен. Миска собрана? " + GameManager.Instance.hasMilkBowl);

        if (!GameManager.Instance.hasMilkBowl) return;

        hasTriggered = true;
        ResultsPanel.Instance.Show();
    }
}