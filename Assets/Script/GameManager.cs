using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public bool isGameOver = false;

    [Header("UI")]
    public TextMeshProUGUI scoreNumber; // 👈 ตัวเลขเท่านั้น

    [Header("Score Effect")]
    public float popScale = 1.3f;
    public float popSpeed = 10f;

    private Vector3 originalScale;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalScale = scoreNumber.transform.localScale;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();

        StopAllCoroutines();
        StartCoroutine(ScorePop());
    }

    void UpdateScoreUI()
    {
        scoreNumber.text = score.ToString();
    }

    IEnumerator ScorePop()
    {
        Vector3 targetScale = originalScale * popScale;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * popSpeed;
            scoreNumber.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        scoreNumber.transform.localScale = originalScale;
    }

    public void GameOver()
    {
        isGameOver = true;
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}