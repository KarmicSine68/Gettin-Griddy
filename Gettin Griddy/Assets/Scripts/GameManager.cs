/******************************************************************************
 * Author: Brad Dixon, Tyler Bouchard
 * File Name: GameManager.cs
 * Creation Date: 2/25/2025
 * Brief: Keeps track of the status of the grid and where everything is, controls 
 * game play
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject gridSpace;
    [SerializeField] public TileBehaviour playerTile;
    [SerializeField] public List<TileBehaviour> tilesWithEnemies;
    [SerializeField] private GameObject boulder;

    public bool playerTurn = true;
    public int moveLimit = 3;

    TileBehaviour[,] grid;

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
        grid = new TileBehaviour[columns, rows];
        for(int i = 0; i < rows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                GameObject tile = Instantiate(gridSpace, new Vector3(j, 0, i), Quaternion.identity);
                tile.name = "Tile(" + j + ", " + i + ")";
                grid[j, i] = tile.GetComponent<TileBehaviour>();
                grid[j, i].gridLocation = new Vector2(j, i);
            }
        }
        Spawn(player);
        Spawn(boulder);
        for (int i = 0; i < 10; i++) {
            Spawn(enemy);
        } 
    }
    /// <summary>
    /// spawn a game object on a random unoccupied tile
    /// </summary>
    /// <param name="obj"></param>
    /// pretty sure that this works fine at runtime but it has errors when it first generates enemies
    private void Spawn(GameObject obj) {
        int x = Random.Range(0, columns);
        int y = Random.Range(0, rows);
        while (grid[x,y].hasObject) {
            x = Random.Range(0, columns);
            y = Random.Range(0, rows);
        }
        grid[x, y].hasObject = true;
        Vector3 spawnPos = grid[x, y].transform.position;
        spawnPos += new Vector3(.5f, 1.5f, .5f);
        Instantiate(obj, spawnPos, Quaternion.identity);
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
        tilesWithEnemies.Add(tileScript);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        foreach (TileBehaviour tile in tilesWithEnemies) {
            if (tile.objectOnTile == enemy) {
                tilesWithEnemies.Remove(tile);
                break;
            }
        }
    }
    public void Attack(Vector2 attackDir) {
        playerTurn = false;
        List<TileBehaviour> tilesToAttack = new List<TileBehaviour>();
        Vector2 playerPos = playerTile.gridLocation;

        //finding the tiles that it needs to attack
        if (gridHasPosition(new Vector2((int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y)))) {
            tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y)].GetComponent<TileBehaviour>());
        }
        if (attackDir.x != 0)
        {
            if (gridHasPosition(new Vector2((int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y + 1))))
            {
                tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y + 1)].GetComponent<TileBehaviour>());
            }
            if (gridHasPosition(new Vector2((int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y - 1))))
            {
                tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x), (int)(playerPos.y + attackDir.y - 1)].GetComponent<TileBehaviour>());
            }
        }
        else {
            if (gridHasPosition(new Vector2((int)(playerPos.x + attackDir.x + 1), (int)(playerPos.y + attackDir.y))))
            {
                tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x + 1), (int)(playerPos.y + attackDir.y)].GetComponent<TileBehaviour>());
            }
            if (gridHasPosition(new Vector2((int)(playerPos.x + attackDir.x - 1), (int)(playerPos.y + attackDir.y))))
            {
                tilesToAttack.Add(grid[(int)(playerPos.x + attackDir.x - 1), (int)(playerPos.y + attackDir.y)].GetComponent<TileBehaviour>());
            }
        }

        //applying damage to enemies on the tiles it found
        if (tilesToAttack.Count <= 0)
        {
            print("There're no tiles that way goofy");
        } else {
            foreach (TileBehaviour tile in tilesToAttack)
            {
                tile.FlashRed();
                if (tile.objectOnTile != null && tile.objectOnTile.TryGetComponent<EnemyTakeDamage>(out EnemyTakeDamage enemy)) {
                    enemy.TakeDamage();
                }
            }
        }

        //this is where you will call the function that does the enemy turn logic
        DoEnemyTurn(); 

        
    }

    public void DoEnemyTurn() {

        foreach (TileBehaviour tile in tilesWithEnemies) {
            tile.objectOnTile.GetComponent<EnemyMovement>().MoveTowardsPlayer();
        }

        //switching back to player turn
        print("Enemies have played");
        PlayerBehaviour player = playerTile.objectOnTile.GetComponent<PlayerBehaviour>();
        player.TurnOrginTile = playerTile.gridLocation;
        player.attacking = false;
        playerTurn = true;
    }
    private bool gridHasPosition(Vector2 pos) {
        if (pos.x < 0 || pos.x >= rows || pos.y < 0 || pos.y >= columns) {
            return false;
        }
        return true;
    }
}
