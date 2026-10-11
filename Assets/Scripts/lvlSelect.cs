using UnityEngine;
using UnityEngine.SceneManagement;

public class lvlSelect : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Возвращаемся в Главное Меню");
    }

    public void lvl1()
    {
        SceneManager.LoadScene("Level1");
        Debug.Log("Открытие уровня 1");
    }

    public void lvl2()
    {
        SceneManager.LoadScene("Level2");
        Debug.Log("Открытие уровня 2");
    }

    public void lvl3()
    {
        SceneManager.LoadScene("Level3");
        Debug.Log("Открытие уровня 3");
    }

    public void lvl4()
    {
        SceneManager.LoadScene("Level4");
        Debug.Log("Открытие уровня 4");
    }

    public void lvl5()
    {
        SceneManager.LoadScene("Level5");
        Debug.Log("Открытие уровня 5");
    }

    public void lvl6()
    {
        SceneManager.LoadScene("Level6");
        Debug.Log("Открытие уровня 6");
    }

    public void lvl7()
    {
        SceneManager.LoadScene("Level7");
        Debug.Log("Открытие уровня 7");
    }

    public void lvl8()
    {
        SceneManager.LoadScene("Level8");
        Debug.Log("Открытие уровня 8");
    }

    public void lvl9()
    {
        SceneManager.LoadScene("Level9");
        Debug.Log("Открытие уровня 9");
    }
}
