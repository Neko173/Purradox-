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

        Collider2D currentTeleport = Physics2D.OverlapCircle(transform.position, 0.3f, LayerMask.GetMask("Default"));
    bool isStandingOnTeleport = (currentTeleport != null && currentTeleport.CompareTag("Teleport"));

    Vector2 targetCell = (Vector2)transform.position + direction * gridSize;

    // 1. Проверяем, нет ли телепорта в ЦЕЛЕВОЙ клетке
    Collider2D teleportCollider = Physics2D.OverlapCircle(targetCell, 0.3f, LayerMask.GetMask("Default"));
    if (teleportCollider != null && teleportCollider.CompareTag("Teleport"))
    {
        Teleport tp = teleportCollider.GetComponent<Teleport>();
        if (tp != null && tp.linkedTeleport != null)
        {
            transform.position = tp.linkedTeleport.transform.position;
            targetPos = transform.position;
            GameManager.Instance.AddStep();
            return;
        }
    }


        // 1. СНАЧАЛА проверяем лунку в целевой клетке
        Vector2 checkPos = (Vector2)transform.position + direction * gridSize;
        Collider2D pitCol = Physics2D.OverlapCircle(checkPos, 0.3f, LayerMask.GetMask("Triggers"));

        if (pitCol != null && pitCol.CompareTag("Pit"))
        {
            Pit pit = pitCol.GetComponent<Pit>();
            if (pit != null && !pit.isFilled)
            {
                // Лунка не заткнута — стоим, даже если там коробка
                return;
            }
        }

        // 2. Только теперь проверяем препятствия через Raycast
        int obstacleMask = LayerMask.GetMask("Obstacles");
        Vector2 rayStart = (Vector2)transform.position + direction * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, gridSize * 0.5f, obstacleMask);

        if (hit.collider == null)
        {
            MoveToCell(direction);
        }
        else if (hit.collider.CompareTag("Box"))
        {
            PushableBox box = hit.collider.GetComponent<PushableBox>();
            if (box != null && box.TryPush(direction)) MoveToCell(direction);
        }
        else if (hit.collider.CompareTag("Ball"))
        {
            RollingBall yarn = hit.collider.GetComponent<RollingBall>();
            if (yarn != null && yarn.TryPush(direction)) MoveToCell(direction);
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
        GameManager.Instance.AddStep(); // считаем шаг
    }
}