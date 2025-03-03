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
    [SerializeField] TileBehaviour currentTile;

    //The amount of times an object can move
    [SerializeField] int maxMoves;

    int currentMovesLeft;
    
    /// <summary>
    /// Moves the player to a new space if elligible
    /// </summary>
    protected void Move(Vector3 dir)
    {
        if(currentTile == null)
        {
            currentTile = GetTile();
        }
        if(HasTile(dir) && TileIsEmpty(dir))
        {
            Vector3 newPos = transform.position;
            newPos.x += dir.x;
            newPos.z += dir.z;
            transform.position = newPos;
            currentTile.hasObject = false;
            currentTile = GetTile();

            currentMovesLeft--;
        }
    }

    /// <summary>
    /// Returns true if there is a tile in that direction
    /// </summary>
    /// <returns></returns>
    protected bool HasTile(Vector3 dir)
    {
        return currentTile.HasNeighbor(dir);
    }

    /// <summary>
    /// Gets the current cube the player is on
    /// </summary>
    protected TileBehaviour GetTile()
    {
        float rayDistance = 2f;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, rayDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<TileBehaviour>(out TileBehaviour tile))
            {
                //print(tile.gameObject.name);
                return tile;
            }
        }
        print(gameObject.name + " could not find current tile");
        return null;
    }

    protected bool TileIsEmpty(Vector3 dir)
    {
        return !currentTile.GetNeighbor(dir).hasObject;
    }

    public void ResetMovesToMax()
    {
        currentMovesLeft = maxMoves;
    }
}
