/******************************************************************************
 * Author: Brad Dixon
 * File Name: PlayerBehaviour.cs
 * Creation Date 2/25/2025
 * Brief: Controls actions for the player
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : GridMovement
{
    InputActionMap playerMap;
    InputAction playerMove;

    bool canMove = true;

    /// <summary>
    /// Enables the new input system
    /// </summary>
    private void Awake()
    {
        playerMap = GetComponent<PlayerInput>().currentActionMap;
        playerMap.Enable();

        playerMove = playerMap.FindAction("Movement");

        playerMove.started += PlayerMove_started;
    }

    /// <summary>
    /// Reads input from the player and moves them
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerMove_started(InputAction.CallbackContext obj)
    {
        canMove = false;
        Vector2 moveDir = playerMove.ReadValue<Vector2>();

        if(moveDir.x > 0)
        {
            MovePlayer(Vector3.right, "Right");
        }
        else if(moveDir.x < 0)
        {
            MovePlayer(Vector3.left, "Left");
        }
        else if(moveDir.y > 0)
        {
            MovePlayer(Vector3.forward, "Up");
        }
        else if(moveDir.y < 0)
        {
            MovePlayer(Vector3.back, "Down");
        }
    }

    private void OnDisable()
    {
        playerMap.Disable();

        playerMove.started -= PlayerMove_started;
    }
}
