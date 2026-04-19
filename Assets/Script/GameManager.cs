using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        Debug.Log("Score: " + score);
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER");

        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}