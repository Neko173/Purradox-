using UnityEngine;

public class MilkBowl : MonoBehaviour
{
    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            GameManager.Instance.CollectMilkBowl();
            gameObject.SetActive(false); // миска исчезает
        }
    }
}