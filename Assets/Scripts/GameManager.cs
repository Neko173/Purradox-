using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasMilkBowl = false;
    public int stepCount;

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