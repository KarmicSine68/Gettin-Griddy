/******************************************************************************
 * Author: Brad Dixon, Tyler Bouchard
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
    InputAction endTurn;

    private GameManager gm;
    public bool attacking = false;
    public Vector2 TurnOrginTile;

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
        playerAttackToggle = playerMap.FindAction("AttackToggle");
        endTurn = playerMap.FindAction("EndTurn"); ;

        playerMove.started += PlayerMove_started;
        playerAttack.started += PlayerAttack;
        playerAttackToggle.started += ToggleAttackMode;
        endTurn.started += EndTurn;

        FindStartTile();
        TurnOrginTile = gm.playerTile.gridLocation;
        
    }

    private void EndTurn(InputAction.CallbackContext obj)
    {
        print(currentTile);
        gm.EndTurn();
    }

    private void FindStartTile() {
        float rayDistance = 2f;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, rayDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<TileBehaviour>(out TileBehaviour tile))
            {
                gm.TrackPlayer(tile);
            }
        } 
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (attacking) {
            Vector2 attackDir = playerMove.ReadValue<Vector2>();
            List<TileBehaviour> tilesInAttackRange = gm.FindAttackTiles(attackDir);
            if (tilesInAttackRange.Count <= 0) {
                print("There're no tiles that way goofy");
            } else {
                foreach (TileBehaviour tile in tilesInAttackRange) {
                    tile.FlashGreen();
                    if (tile.objectOnTile != null && tile.objectOnTile.TryGetComponent<EnemyTakeDamage>(out EnemyTakeDamage enemy)) {
                        enemy.TakeDamage();
                    }
                }
                gm.EndTurn();
            }
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
        if (!attacking && gm.playerTurn) {
            Vector2 moveDir = playerMove.ReadValue<Vector2>();
            if (moveDir.x > 0)
            {
                if (withinTurnsMoveLimit(moveDir)) {
                    Move(Vector3.right);
                }   
            }
            else if (moveDir.x < 0)
            {
                if (withinTurnsMoveLimit(moveDir))
                {
                    Move(Vector3.left);
                }
            }
            else if (moveDir.y > 0)
            {
                if (withinTurnsMoveLimit(moveDir))
                {
                    Move(Vector3.forward);
                }
            }
            else if (moveDir.y < 0)
            {
                if (withinTurnsMoveLimit(moveDir))
                {
                    Move(Vector3.back);
                }
            }
        }
    }

    private bool withinTurnsMoveLimit(Vector2 moveDir)
    {
        int tilesMovedX = Mathf.Abs(((int)gm.playerTile.gridLocation.x + (int)moveDir.x) - (int)TurnOrginTile.x);
        int tilesMovedY = Mathf.Abs(((int)gm.playerTile.gridLocation.y + (int)moveDir.y) - (int)TurnOrginTile.y);

        if ((tilesMovedX + tilesMovedY) <= gm.moveLimit) {
            //print("You have moved " + (tilesMovedX + tilesMovedY) + " tiles");
        } else {
            print("Tile is to far away to move to it");
        }

        if ((tilesMovedX + tilesMovedY) <= gm.moveLimit)
        {
            return true;
        }
        return false;
    }

    private void OnDisable()
    {
        playerMap.Disable();
        
        playerMove.started -= PlayerMove_started;
        playerAttack.started -= ToggleAttackMode;
        playerAttackToggle.started -= PlayerAttack;
        endTurn.started -= EndTurn;
    }
}
