/******************************************************************************
 * Author: Brad Dixon
 * File Name: GameManager.cs
 * Creation Date: 2/25/2025
 * Brief: Keeps track of the status of the grid and where everything is
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gridSpace;
    [SerializeField] private GameObject playerTracker;
    [SerializeField] private GameObject boulder;

    GameObject[,] grid;

    [Header("Grid Parameters")]
    [Tooltip("How many rows are in the grid")]
    [SerializeField] private int rows;
    [Tooltip("How many columns are in the grid")]
    [SerializeField] private int columns;

    /// <summary>
    /// Creates the grid and spwans things in
    /// </summary>
    private void Start()
    {
        grid = new GameObject[columns, rows];
        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                GameObject tile = Instantiate(gridSpace, new Vector3(j, 0, i), Quaternion.identity);
                grid[j, i] = tile;
            }
        }

        SpawnPlayer();
        SpawnBoulder();
    }

    /// <summary>
    /// Spawns the player on the grid
    /// </summary>
    private void SpawnPlayer()
    {
        int x = Random.Range(0, columns);
        int y = Random.Range(0, rows);

        Vector3 spawnPos = grid[x, y].transform.position;

        spawnPos += new Vector3(.5f, 1.5f, .5f);

        Instantiate(player, spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// Updates where in the grid the player is
    /// </summary>
    /// <param name="tile"></param>
    public void TrackPlayer(GameObject tile)
    {
        playerTracker = tile;
        
        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                if(grid[j,i] == playerTracker)
                {
                    grid[j, i] = playerTracker;
                }
            }
        }
    }

    /// <summary>
    /// Spawns a boulder that obstructs the player's movement
    /// </summary>
    private void SpawnBoulder()
    {
        int x = Random.Range(0, columns);
        int y = Random.Range(0, rows);

        Vector3 spawnPos = grid[x, y].transform.position;

        spawnPos += new Vector3(.5f, 1.5f, .5f);

        if (grid[x, y] == playerTracker)
        {
            SpawnBoulder();
        }
        else
        {
            Instantiate(boulder, spawnPos, Quaternion.identity);
        }
    }
}
