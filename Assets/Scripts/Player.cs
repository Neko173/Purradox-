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
        Physics2D.queriesHitTriggers = false; // лучи игнорируют триггеры
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
            return;
        }

        Vector2 direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            direction = Vector2.right;

        if (direction == Vector2.zero) return;

        // Луч в целевуют клетку
        Vector2 rayStart = (Vector2)transform.position + direction * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, gridSize * 0.5f);

        if (hit.collider == null)
        {
            // Пусто — идём
            MoveToCell(direction);
        }
        else if (hit.collider.CompareTag("Box"))
        {
            // Там коробка — пробуем толкнуть
            PushableBox box = hit.collider.GetComponent<PushableBox>();
            if (box != null && box.TryPush(direction))
            {
                // Коробка сдвинулась — идём на её место
                MoveToCell(direction);
            }
            // Если коробка не сдвинулась (за ней стена) — стоим
        }
        // Иначе там стена — стоим
        if (hit.collider == null)
        {
            MoveToCell(direction);
        }
        else if (hit.collider.CompareTag("Box"))
        {
            PushableBox box = hit.collider.GetComponent<PushableBox>();
            if (box != null && box.TryPush(direction))
            {
                MoveToCell(direction);
            }
        }
        else if (hit.collider.CompareTag("Ball"))
        {
            RollingBall yarn = hit.collider.GetComponent<RollingBall>();
            if (yarn != null && yarn.TryPush(direction))
            {
                MoveToCell(direction);
            }
        }
        // иначе — стена, стоим
    }

    void MoveToCell(Vector2 direction)
    {
        targetPos = new Vector2(
            transform.position.x + direction.x * gridSize,
            transform.position.y + direction.y * gridSize
        );
        isMoving = true;
    }
}