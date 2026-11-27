using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Tooltip("Сюда перетащи парный телепорт (Teleport_B)")]
    public Teleport linkedTeleport;

    [Header("Анимация портала")]
    public Sprite[] portalFrames;
    public float animFps = 6f;

    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private int frameIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = new Color(0.65f, 0.35f, 1f, 0.95f); // фиолетовый по умолчанию
        }
    }

    void Update()
    {
        if (spriteRenderer == null || portalFrames == null || portalFrames.Length <= 1) return;

        timer += Time.deltaTime;
        float step = 1f / Mathf.Max(0.1f, animFps);
        if (timer >= step)
        {
            timer -= step;
            frameIndex = (frameIndex + 1) % portalFrames.Length;
            spriteRenderer.sprite = portalFrames[frameIndex];
        }
    }
}
