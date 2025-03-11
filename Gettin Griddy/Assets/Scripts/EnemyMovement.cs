/******************************************************************************
 * Author: ?????, Skylar Turner
 * File Name: EnemyMovement.cs
 * Creation Date: 2/??/2025
 * Brief: Handles enemy movement.
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : GridMovement
{
    private GameManager gm;
    public int numOfMoves = 2;
    private int movesUsed = 0;
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        gm.EnemyTiles.Add(GetTile());
        GetTile().objectOnTile = gameObject;
    }
    public void DoEnemyMovement()
    {
        // Try to find a path to the player
        List<TileBehaviour> path = Pathfinder.FindPath(GetTile(), gm.playerTile);
        if (path == null || path.Count < 2)
        {
            // If no path is found, move to the closest valid tile
            MoveToClosestPoint();
            return;
        }

        // If a path is found, proceed with normal movement
        TileBehaviour nextTile = path[1]; // The next step in the path
        MoveEnemy(nextTile.gridLocation - GetTile().gridLocation);
    }

    private void MoveEnemy(Vector2 direction)
    {
        gm.RemoveEnemy(gameObject);
        GetTile().objectOnTile = null;

        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        transform.rotation = Quaternion.LookRotation(dir);
        Move(dir);

        GetTile().objectOnTile = gameObject;
        gm.TrackEnemy(GetTile());
    }

    private void MoveToClosestPoint()
    {
        // Get all walkable neighbors of the enemy's current position
        List<TileBehaviour> neighbors = GetTile().GetNeighbors();

        TileBehaviour closestTile = null;
        float currentDistance = Vector2.Distance(GetTile().gridLocation, gm.playerTile.gridLocation);
        float minDistance = currentDistance;

        // Iterate through all neighbors to find the closest walkable one
        foreach (TileBehaviour neighbor in neighbors)
        {
            if (!neighbor.IsWalkable()) continue; // Skip non-walkable tiles

            float distance = Vector2.Distance(neighbor.gridLocation, gm.playerTile.gridLocation);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTile = neighbor;
            }
        }

        // If a valid closest tile is found, move to it
        if (closestTile != null)
        {
            MoveEnemy(closestTile.gridLocation - GetTile().gridLocation);
        }
    }
    public bool HasUnusedMoves() {
        if (movesUsed >= numOfMoves) {
            return false;
        }
        return true;
    }
}
