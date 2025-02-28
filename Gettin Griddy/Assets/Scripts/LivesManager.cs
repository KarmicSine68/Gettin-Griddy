/******************************************************************************
 * Author: Tyler Bouchard
 * File Name: UIButtonsScript.cs
 * Creation Date: 2/19/2025
 * Brief: keeps track of the lives
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public int lives = 5;
    public string loseScene = "LoseScene";
    public Text LivesText;

    public void DecreaseLives()
    {
        lives--;
        LivesText.text = "Lives: " + lives;
        CheckLoss();
    }
    private void CheckLoss()
    {
        if (lives <= 0)
        {
            SceneManager.LoadScene(loseScene);
        }
    }
}
