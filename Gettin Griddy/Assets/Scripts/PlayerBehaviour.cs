/******************************************************************************
 * Author: Brad Dixon
 * File Name: PlayerBehaviour.cs
 * Creation Date 2/25/2025
 * Brief: Controls actions for the player
 * ***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : GridMovement
{
    InputActionMap playerMap;
    InputAction playerMove;
    InputAction playerAttack;
    InputAction playerAttackToggle;

    private GameManager gm;
    public bool attacking = false;
    bool canMove = true;

    /// <summary>
    /// Enables the new input system
    /// </summary>
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        playerMap = GetComponent<PlayerInput>().currentActionMap;
        playerMap.Enable();

        playerMove = playerMap.FindAction("Movement");
        playerAttack = playerMap.FindAction("Attack"); ;
        playerAttackToggle = playerMap.FindAction("AttackToggle"); ;

        playerMove.started += PlayerMove_started;
        playerAttack.started += PlayerAttack;
        playerAttackToggle.started += ToggleAttackMode;
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (attacking) {
            Vector2 attackDir = playerMove.ReadValue<Vector2>();
            gm.Attack(attackDir);
        }
    }

    private void ToggleAttackMode(InputAction.CallbackContext obj)
    {
        attacking = !attacking;
    }

    /// <summary>
    /// Reads input from the player and moves them
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerMove_started(InputAction.CallbackContext obj)
    {
        if (!attacking) {
            canMove = false;
            Vector2 moveDir = playerMove.ReadValue<Vector2>();

            if (moveDir.x > 0)
            {
                MovePlayer(Vector3.right, "Right");
            }
            else if (moveDir.x < 0)
            {
                MovePlayer(Vector3.left, "Left");
            }
            else if (moveDir.y > 0)
            {
                MovePlayer(Vector3.forward, "Up");
            }
            else if (moveDir.y < 0)
            {
                MovePlayer(Vector3.back, "Down");
            }
        }
    }

    private void OnDisable()
    {
        playerMap.Disable();
        
        playerMove.started -= PlayerMove_started;
        playerAttack.started -= ToggleAttackMode;
        playerAttackToggle.started -= PlayerAttack;
    }
}
