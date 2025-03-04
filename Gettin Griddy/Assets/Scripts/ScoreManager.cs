/******************************************************************************
 * Author: Tyler Bouchard
 * File Name: UIButtonsScript.cs
 * Creation Date: 2/19/2025
 * Brief: keeps track of the score
 * ***************************************************************************/
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
        scoreText.text = "Score: " + score + "/" + targetScore;
        CheckWin();
    }
    public void IncreaseScore(int amount) {
        score += amount;
        scoreText.text = "Score: " + score + "/" + targetScore;
        CheckWin();
    }
    private void CheckWin() {
        if (score >= targetScore) {
            SceneManager.LoadScene(winScene);
        }
    }
}
