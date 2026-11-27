using UnityEngine;

public class PlayerSpriteSwitcher : MonoBehaviour
{
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    [Header("Анимация ходьбы (по 4 кадра на направление)")]
    public Sprite[] walkDown;
    public Sprite[] walkUp;
    public Sprite[] walkLeft;
    public Sprite[] walkRight;

    [Header("Анимация покоя (по 2 кадра на направление)")]
    public Sprite[] idleDown;
    public Sprite[] idleUp;
    public Sprite[] idleLeft;
    public Sprite[] idleRight;

    [Header("Скорость анимации")]
    public float walkFps = 12f;
    public float idleFps = 2.5f;

    private SpriteRenderer spriteRenderer;
    private Vector2 currentDirection = Vector2.down;
    private bool isMoving = false;
    private float animTimer = 0f;
    private int frameIndex = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyCurrentFrame();
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        Sprite[] frames = GetActiveFrames();
        if (frames == null || frames.Length <= 1) return;

        float fps = isMoving ? walkFps : idleFps;
        if (fps <= 0.01f) return;

        animTimer += Time.deltaTime;
        float frameDuration = 1f / fps;
        if (animTimer >= frameDuration)
        {
            animTimer -= frameDuration;
            frameIndex = (frameIndex + 1) % frames.Length;
            spriteRenderer.sprite = frames[frameIndex];
        }
    }

    // Вызывается из PlayerGridMovement при каждом шаге
    public void SetDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        if (direction != currentDirection)
        {
            currentDirection = direction;
            frameIndex = 0;
            animTimer = 0f;
        }
        ApplyCurrentFrame();
    }

    public void SetMoving(bool moving)
    {
        if (isMoving == moving) return;
        isMoving = moving;
        frameIndex = 0;
        animTimer = 0f;
        ApplyCurrentFrame();
    }

    private Sprite[] GetActiveFrames()
    {
        if (isMoving)
        {
            if (currentDirection == Vector2.up && walkUp != null && walkUp.Length > 0) return walkUp;
            if (currentDirection == Vector2.down && walkDown != null && walkDown.Length > 0) return walkDown;
            if (currentDirection == Vector2.left && walkLeft != null && walkLeft.Length > 0) return walkLeft;
            if (currentDirection == Vector2.right && walkRight != null && walkRight.Length > 0) return walkRight;
        }
        else
        {
            if (currentDirection == Vector2.up && idleUp != null && idleUp.Length > 0) return idleUp;
            if (currentDirection == Vector2.down && idleDown != null && idleDown.Length > 0) return idleDown;
            if (currentDirection == Vector2.left && idleLeft != null && idleLeft.Length > 0) return idleLeft;
            if (currentDirection == Vector2.right && idleRight != null && idleRight.Length > 0) return idleRight;
        }
        return null;
    }

    private void ApplyCurrentFrame()
    {
        if (spriteRenderer == null) return;

        Sprite[] frames = GetActiveFrames();
        if (frames != null && frames.Length > 0)
        {
            spriteRenderer.sprite = frames[frameIndex % frames.Length];
            return;
        }

        if (currentDirection == Vector2.up && spriteUp != null) spriteRenderer.sprite = spriteUp;
        else if (currentDirection == Vector2.down && spriteDown != null) spriteRenderer.sprite = spriteDown;
        else if (currentDirection == Vector2.left && spriteLeft != null) spriteRenderer.sprite = spriteLeft;
        else if (currentDirection == Vector2.right && spriteRight != null) spriteRenderer.sprite = spriteRight;
    }
}
