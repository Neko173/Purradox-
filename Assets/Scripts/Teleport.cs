using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Tooltip("Сюда перетащи парный телепорт (Teleport_B)")]
    public Teleport linkedTeleport;

    // Визуально можно сделать мигание или цвет
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(0.5f, 0f, 1f, 0.8f); // фиолетовый
    }
}