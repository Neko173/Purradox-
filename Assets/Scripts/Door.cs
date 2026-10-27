using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D doorCollider;
    private SpriteRenderer spriteRenderer;
    private bool hasTriggered = false;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponents<BoxCollider2D>()[0];
    }

    void Update()
    {
        bool isOpen = GameManager.Instance.hasMilkBowl;
        doorCollider.enabled = !isOpen;

        Color c = spriteRenderer.color;
        c.a = isOpen ? 0.3f : 1f;
        spriteRenderer.color = c;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (!GameManager.Instance.hasMilkBowl) return;

        hasTriggered = true;

        LevelTimer timer = FindObjectOfType<LevelTimer>();
        if (timer != null)
        {
            timer.StopTimer();
            GameManager.Instance.levelTime = timer.GetRemainingTime(); // остаток на финише
        }

        GameManager.Instance.isLevelFailed = false;   // победа
        ResultsPanel.Instance.Show();
    }
}