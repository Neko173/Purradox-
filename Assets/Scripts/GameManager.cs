using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasMilkBowl = false;
    public int stepCount;
    public float levelTime = 0f;       // остаток времени на финише (для инфо)
    public bool isLevelFailed = false; // ← новое: провал по таймеру

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectMilkBowl()
    {
        hasMilkBowl = true;
        Debug.Log("Миска собрана! Дверь открыта.");
    }

    public void AddStep()
    {
        stepCount++;
    }
}