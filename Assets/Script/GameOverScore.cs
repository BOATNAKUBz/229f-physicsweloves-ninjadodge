using UnityEngine;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("LastScore", 0);
        scoreText.text = "SCORE : " + score;
    }
}