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
    protected TileBehaviour currentTile;
    
    //moves the player to a new space
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
        }
    }

    //figures out if a tile has a neighboring tile in that direction
    protected bool HasTile(Vector3 dir)
    {
        return currentTile.HasNeighbor(dir);
    }

    //returns the script of the tile that the player is on
    protected TileBehaviour GetTile()
    {
        float rayDistance = 2f;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, rayDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<TileBehaviour>(out TileBehaviour tile))
            {
                return tile;
            }
        }
        print(gameObject.name + " could not find current tile");
        return null;
    }
    //tells you if the tile you are trying to move to is occupied or not
    protected bool TileIsEmpty(Vector3 dir)
    {
        return !currentTile.GetNeighbor(dir).hasObject;
    }
}
