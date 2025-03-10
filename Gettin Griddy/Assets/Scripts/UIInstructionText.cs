using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInstructionText : MonoBehaviour
{
    private GameManager gm;
    public Text turnText;
    public Text playerModeText;
    public Text instructionsText;
    public Text enterText;
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (gm.playerTurn) {
            playerModeText.gameObject.SetActive(true);
            instructionsText.gameObject.SetActive(true);
            enterText.gameObject.SetActive(true);
            turnText.text = "Player's Turn";
            if (GameObject.FindObjectOfType<PlayerBehaviour>().attacking) {
                playerModeText.text = "Attack Mode";
                instructionsText.text = "use [W,A,S,D]\nto pick direction";
                enterText.text = "press [Enter]\nto end turn\nand attack";
                playerModeText.color = Color.red;
            } else {
                playerModeText.text = "Move Mode";
                instructionsText.text = "use [W,A,S,D]\nto move";
                enterText.text = "press [Enter]\nto end turn";
                playerModeText.color = Color.green;
            }
        }
        if (gm.enemyTurn)
        {
            playerModeText.gameObject.SetActive(false);
            instructionsText.gameObject.SetActive(false);
            enterText.gameObject.SetActive(false);
            turnText.text = "Enemy's Turn";
        }
        if (gm.worldTurn)
        {
            playerModeText.gameObject.SetActive(false);
            instructionsText.gameObject.SetActive(false);
            enterText.gameObject.SetActive(false);
            turnText.text = "World's Turn";
        }
    }
}
