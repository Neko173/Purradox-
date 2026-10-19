using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
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
        // Если уже движемся — плавно идём к цели, новое нажатие не читаем
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
            return;
        }

        // Читаем ввод
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Определяем направление (одна ось за раз)
        Vector2 direction = Vector2.zero;
        if (horizontal != 0)
            direction = new Vector2(horizontal, 0);
        else if (vertical != 0)
            direction = new Vector2(0, vertical);

        // Если направление не выбрано — ничего не делаем
        if (direction == Vector2.zero) return;

        // ПРОВЕРКА: есть ли коллайдер (стена) в целевой клетке
        Vector2 rayStart = (Vector2)transform.position + direction * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, gridSize * 0.5f);

        // Если луч ничего не встретил — путь свободен, начинаем движение
        if (hit.collider == null)
        {
            targetPos = new Vector2(
                transform.position.x + direction.x * gridSize,
                transform.position.y + direction.y * gridSize
            );
            isMoving = true;
        }
        // Если hit.collider != null — там стена, движение не начинаем
    }
}