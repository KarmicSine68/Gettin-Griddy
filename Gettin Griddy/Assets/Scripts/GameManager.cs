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
    [SerializeField] private List<TileBehaviour> EnemyTiles;
    [SerializeField] private GameObject boulder;
    
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject[] worldHazards;
    [SerializeField] private string hazardTag = "Hazard";

    public bool playerTurn = true;
    public bool worldTurn = false;
    public bool enemyTurn = false;
    public int moveLimit = 3;

    public TileBehaviour[,] grid;

    [Header("Grid Parameters")]
    [Tooltip("How many rows are in the grid")]
    [SerializeField] private int rows;
    [Tooltip("How many columns are in the grid")]
    [SerializeField] private int columns;

    private bool isBattleActive = true; // Battle control flag

    [SerializeField] private int TurnState = 1;
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
        for (int i = 0; i < 1; i++) {
            Spawn(enemy);
        }
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //worldHazards = GameObject.FindGameObjectsWithTag(hazardTag);

        StartCoroutine(GameLoop()); // Start the turn system
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
    public void EndTurn() {
        if (playerTurn)
        {
            enemyTurn = true;
            playerTurn = false;
            worldTurn = false;
        }
        else if (enemyTurn)
        {
            enemyTurn = false;
            playerTurn = false;
            worldTurn = true;
        }
        else if (worldTurn)
        {
            enemyTurn = false;
            playerTurn = true;
            worldTurn = false;
        }
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
    public void RemoveEnemy(GameObject enemy) {
        print("called");
        foreach (TileBehaviour tile in EnemyTiles) {
            if (tile.objectOnTile == enemy) {
                EnemyTiles.Remove(tile);
                break;
            }
        }
    }
    public List<TileBehaviour> FindAttackTiles(Vector2 attackDir) {
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
        return tilesToAttack;  
    }

    /*public void DoEnemyTurn() {
        print("Enemies have played");
        EndTurn();
    }*/
    private bool gridHasPosition(Vector2 pos) {
        if (pos.x < 0 || pos.x >= rows || pos.y < 0 || pos.y >= columns) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Main turn loop that runs sequentially.
    /// Ends when the battle is no longer active.
    /// </summary>
    private IEnumerator GameLoop()
    {
        while (isBattleActive) // Run while battle is active
        {
            yield return StartCoroutine(PlayerTurn());
            yield return StartCoroutine(EnemyTurn());
            yield return StartCoroutine(WorldTurn());
        }

        Debug.Log("Battle has ended.");
        OnBattleEnd(); // Call any post-battle logic
    }
    /// <summary>
    /// Ends the battle and stops the game loop.
    /// </summary>
    public void EndBattle()
    {
        isBattleActive = false;
    }

    /// <summary>
    /// Handles any post-battle logic (e.g., rewards, returning to menu).
    /// </summary>
    private void OnBattleEnd()
    {
        Debug.Log("Battle Over! Returning to menu or next scene...");
        // Add transition logic here (e.g., load scene, show UI)
    }
    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player's Turn");
        PlayerBehaviour player = playerTile.objectOnTile.GetComponent<PlayerBehaviour>();
        player.TurnOrginTile = playerTile.gridLocation;
        player.attacking = false;
        playerTurn = true;
        EndTurn();
        yield return new WaitUntil(() => enemyTurn);

        TurnState = 2;
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's Turn");
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //PlayedTurn = false;

        foreach (GameObject obj in enemies)
        {
            EnemyMovement enemyMoveScript = obj.GetComponent<EnemyMovement>();
            if (enemyMoveScript != null)
            {
                enemyMoveScript.DoEnemyMovement();
            }
        }

        //PlayedTurn = true;
        EndTurn();
        yield return new WaitUntil(() => worldTurn);

        TurnState = 3;

        // Check if all enemies are defeated
        if (GameObject.FindGameObjectsWithTag(enemyTag).Length == 0)
        {
            EndBattle();
        }
    }

    private IEnumerator WorldTurn()
    {
        Debug.Log("World's Turn");
        //worldHazards = GameObject.FindGameObjectsWithTag(hazardTag);
        //PlayedTurn = false;

        foreach (GameObject obj in worldHazards)
        {
            Hazard hazardScript = obj.GetComponent<Hazard>();
            if (hazardScript != null)
            {
                hazardScript.TickDownTimer();
            }
        }

        //PlayedTurn = true;
        EndTurn();
        yield return new WaitUntil(() => playerTurn);

        TurnState = 1;
    }
}
