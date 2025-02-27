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
    [SerializeField] public TileBehaviour playerTile;
    [SerializeField] private List<TileBehaviour> EnemyTiles;
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
                tile.GetComponent<TileBehaviour>().gridLocation = new Vector2(j, i);
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
    public void TrackPlayer(TileBehaviour tileScript)
    {
        playerTile = tileScript;
    }
    public void TrackEnemy(TileBehaviour tileScript)
    {
        EnemyTiles.Add(tileScript);
    }
    public void Attack(Vector2 attackDir) {
        List<TileBehaviour> tilesToAttack = new List<TileBehaviour>();
        Vector2 playerPos = playerTile.gridLocation;
        tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y)].GetComponent<TileBehaviour>());
        if (attackDir.x != 0)
        {
            tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y + 1)].GetComponent<TileBehaviour>());
            tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y - 1)].GetComponent<TileBehaviour>());
        }
        else {
            tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x + 1), (int)(playerPos.y + attackDir.y)].GetComponent<TileBehaviour>());
            tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x - 1), (int)(playerPos.y + attackDir.y)].GetComponent<TileBehaviour>());
        }
        foreach (TileBehaviour t in tilesToAttack) {
            t.FlashRed();
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

        if (grid[x, y] == playerTile)
        {
            SpawnBoulder();
        }
        else
        {
            Instantiate(boulder, spawnPos, Quaternion.identity);
        }
    }
}
