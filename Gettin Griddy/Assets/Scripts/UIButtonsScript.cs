/******************************************************************************
 * Author: Tyler Bouchard
 * File Name: UIButtonsScript.cs
 * Creation Date: 2/19/2025
 * Brief: script for menu bottons
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonsScript : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public string sceneToLoad = "MovementTesting";

    public void GoToMainMenu() {
        SceneManager.LoadScene(mainMenuScene);
    }
    public void GoToTagetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
