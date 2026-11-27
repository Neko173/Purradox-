using UnityEngine;

public class RollingBall : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float gridSize = 1f;

    private Vector2 targetPos;
    private bool isMoving = false;
    private Vector2 rollDirection;
    private bool isRolling = false;

    void Start()
    {
        targetPos = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // Штука для анимации вращения
            transform.Rotate(0, 0, -rollDirection.x * 360f * Time.deltaTime * 2f);
            transform.Rotate(0, 0, -rollDirection.y * 360f * Time.deltaTime * 2f);

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                isMoving = false;

                // Если катимся — проверяем, можно ли ехать дальше
                if (isRolling)
                {
                    TryContinueRolling();
                }
            }
        }
    }

    // Вызывается из скрипта игрока: начать качение в направлении
    public bool TryPush(Vector2 direction)
    {
        if (isMoving) return false;

        // Проверяем клетку перед клубком
        if (!IsCellFree(direction)) return false;

        rollDirection = direction;
        isRolling = true;
        AudioManager.Instance?.PlayBoxPush();
        MoveOneCell(direction);
        return true;
    }

    // Проверка: свободна ли следующая клетка в направлении
    private bool IsCellFree(Vector2 direction)
    {
        Vector2 checkStart = (Vector2)transform.position + direction * 0.6f;
        RaycastHit2D hit = Physics2D.Raycast(checkStart, direction, gridSize * 0.5f);
        return hit.collider == null;
    }

    // Сдвинуть на одну клетку
    private void MoveOneCell(Vector2 direction)
    {
        targetPos = new Vector2(
            transform.position.x + direction.x * gridSize,
            transform.position.y + direction.y * gridSize
        );
        isMoving = true;
    }

    // После остановки на клетке — попробовать ехать дальше
    private void TryContinueRolling()
    {
        if (!isRolling) return;

        // Проверяем лунку впереди
        Vector2 nextPos = (Vector2)transform.position + rollDirection * gridSize;
        Collider2D pitCol = Physics2D.OverlapCircle(nextPos, 0.3f);

        if (pitCol != null && pitCol.CompareTag("Pit"))
        {
            Pit pit = pitCol.GetComponent<Pit>();
            if (pit != null && !pit.isFilled)
            {
                // Лунка не заткнута — падаем в неё
                pit.Fill();
                AudioManager.Instance?.PlayHole();
                gameObject.SetActive(false);
                return;
            }
            else
            {
                // Лунка уже заткнута — стоим
                isRolling = false;
                return;
            }
        }

        // Обычная проверка: свободна ли следующая клетка
        if (IsCellFree(rollDirection))
        {
            MoveOneCell(rollDirection);
        }
        else
        {
            isRolling = false;
        }
    }
}
