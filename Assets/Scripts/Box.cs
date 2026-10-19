using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    private Vector2 targetPos;
    private bool isMoving = false;

    void Start()
    {
        targetPos = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }

    // Вызывается из скрипта игрока: попробовать сдвинуть коробку
    public bool TryPush(Vector2 direction)
    {
        if (isMoving) return false; // коробка ещё едет — не трогаем

        // Проверяем, свободна ли клетка ЗА коробкой
        Vector2 checkStart = (Vector2)transform.position + direction * 0.6f;
        RaycastHit2D hit = Physics2D.Raycast(checkStart, direction, gridSize * 0.5f);

        // Если там что-то есть (стена или другая коробка) — не двигаем
        if (hit.collider != null) return false;

        // Свободно — двигаем коробку на клетку
        targetPos = new Vector2(
            transform.position.x + direction.x * gridSize,
            transform.position.y + direction.y * gridSize
        );
        isMoving = true;
        return true;
    }
}