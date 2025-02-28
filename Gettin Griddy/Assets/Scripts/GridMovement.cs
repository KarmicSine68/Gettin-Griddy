/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 2/25/2025
 * File Name: GridMovement.cs
 * Brief: Used for anything that moves on the grid. Checks to make sure it can move
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    //The tile the player is on
    [SerializeField] GameObject currentTile;

    /// <summary>
    /// Moves the player to a new space if elligible
    /// </summary>
    protected void MovePlayer(Vector3 dir, string moveDir)
    {
        if(currentTile == null)
        {
            GetTile();
        }

        if(isTile(dir) && CanMove(dir))
        {
            Vector3 newPos = transform.position;
            switch(moveDir)
            {
                case "Up":
                    newPos.z += currentTile.transform.localScale.x;
                    //newPos.z += 1;
                    break;
                case "Down":
                    newPos.z -= currentTile.transform.localScale.x;
                    //newPos.z -= 1;
                    break;
                case "Right":
                    newPos.x += currentTile.transform.localScale.x;
                    //newPos.x += 1;
                    break;
                case "Left":
                    newPos.x -= currentTile.transform.localScale.x;
                    //newPos.x -= 1;
                    break;
                default:
                    Debug.Log("Unknown direction");
                    break;
            }

            transform.position = newPos;

            GetTile();
        }
    }

    /// <summary>
    /// Returns true if there is a tile in that direction
    /// </summary>
    /// <returns></returns>
    protected bool isTile(Vector3 dir)
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y -= .5f;
        return Physics.Raycast(rayOrigin, dir, currentTile.transform.localScale.x);
    }

    /// <summary>
    /// Gets the current cube the player is on
    /// </summary>
    protected void GetTile()
    {
        currentTile = gameObject.GetComponentInParent<TileBehaviour>().gameObject;
    }

    protected bool CanMove(Vector3 dir)
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y -= .5f;
        RaycastHit hit;
        Physics.Raycast(rayOrigin, dir, out hit, currentTile.transform.localScale.x);

        TileBehaviour tile = hit.collider.gameObject.GetComponent<TileBehaviour>();
        return (tile.objectOnTile == null);
    }
}
