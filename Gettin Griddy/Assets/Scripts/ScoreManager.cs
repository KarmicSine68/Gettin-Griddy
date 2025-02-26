using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int targetScore = 10;
    public string winScene = "WinScene";
    public Text scoreText;

    public void IncreaseScore() {
        score++;
        scoreText.text = "Score: " + score;
        CheckWin();
    }
    public void IncreaseScore(int amount) {
        score += amount;
        scoreText.text = "Score: " + score;
        CheckWin();
    }
    private void CheckWin() {
        if (score >= targetScore) {
            SceneManager.LoadScene(winScene);
        }
    }
}
