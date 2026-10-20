using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasMilkBowl = false;

    void Awake()
    {
        // Простой синглтон — один менеджер на всю игру
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectMilkBowl()
    {
        hasMilkBowl = true;
        Debug.Log("Миска собрана! Дверь открыта.");
    }
}