/******************************************************************************
 * Author: Skylar Turner
 * File Name: MenuButtons.cs
 * Creation Date 2/27/2025
 * Brief: Contains the functions for the menu buttons
 * ***************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("GameScene"); //will need to be renamed to whatever the actual scene name is called
    }

    public void OnClickCredits()
    {
        SceneManager.LoadScene("Credits");
    }    

    public void OnClickQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
