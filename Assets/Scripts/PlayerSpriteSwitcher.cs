using UnityEngine;

public class PlayerSpriteSwitcher : MonoBehaviour
{
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Вызываем этот метод из PlayerGridMovement при каждом шаге
    public void SetDirection(Vector2 direction)
    {
        if (direction == Vector2.up) spriteRenderer.sprite = spriteUp;
        else if (direction == Vector2.down) spriteRenderer.sprite = spriteDown;
        else if (direction == Vector2.left) spriteRenderer.sprite = spriteLeft;
        else if (direction == Vector2.right) spriteRenderer.sprite = spriteRight;
    }
}