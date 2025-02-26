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
