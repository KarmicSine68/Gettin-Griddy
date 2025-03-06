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
    [SerializeField] private GameObject boulder;
    [SerializeField] private GameObject gridSpace;
    public int playerMoveLimit = 3;
    [SerializeField] private Vector2 playerSpawnLocation;
    [SerializeField] private Vector2[] boulderSpawnLocations;
    [SerializeField] private Vector2[] enemySpawnLocations;
    
    [HideInInspector] public TileBehaviour playerTile;
    [HideInInspector] public List<TileBehaviour> EnemyTiles;
    [HideInInspector] private string enemyTag = "Enemy";
    [HideInInspector] private string hazardTag = "Hazard";
   
    
    [SerializeField] private GameObject[] worldHazards;
    

    [HideInInspector] public bool playerTurn = true;
    [HideInInspector] public bool worldTurn = false;
    [HideInInspector] public bool enemyTurn = false;
    [HideInInspector] public TileBehaviour[,] grid;

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

        Spawn(player,playerSpawnLocation);
        for (int i = 0; i < boulderSpawnLocations.Length; i++)
        {
            Spawn(boulder, boulderSpawnLocations[i]);
        }
        for (int i = 0; i < enemySpawnLocations.Length; i++) {
            Spawn(enemy, enemySpawnLocations[i]);
        }
        
    }
    private void FixedUpdate()
    {
        if (playerTurn)
        {
            PlayerTurn();
        }
        else if (enemyTurn)
        {
            EnemyTurn();
        }
        else if (worldTurn)
        {
            WorldTurn();
        }
        else
        {
            print("how is it no ones turn? wtf dude");
        }
    }
    /// <summary>
    /// spawn a game object on a random unoccupied tile
    /// </summary>
    /// <param name="obj"></param>
    /// pretty sure that this works fine at runtime but it has errors when it first generates enemies
    private void SpawnRandom(GameObject obj) {
        int x = Random.Range(0, columns);
        int y = Random.Range(0, rows);
        while (grid[x,y].objectOnTile != null) {
            x = Random.Range(0, columns);
            y = Random.Range(0, rows);
        }
        Vector3 spawnPos = grid[x, y].transform.position;
        spawnPos += new Vector3(.5f, 1.5f, .5f);
        grid[x, y].objectOnTile = Instantiate(obj, spawnPos, Quaternion.identity);
    }
    private void Spawn(GameObject obj, Vector2 location)
    {
        if (!gridHasPosition(location)) {
            print("Tile doesn't exist, spawning at random location instead");
            SpawnRandom(obj);
        }
        else if (grid[(int)location.x, (int)location.y].objectOnTile != null)
        {
            print("Tile is occupied, spawning at random location instead");
            SpawnRandom(obj);
        } else {
            Vector3 spawnPos = grid[(int)location.x, (int)location.y].transform.position;
            spawnPos += new Vector3(.5f, 1.5f, .5f);
            grid[(int)location.x, (int)location.y].objectOnTile = Instantiate(obj, spawnPos, Quaternion.identity);
        }
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
        //print("called");
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
    private void PlayerTurn()
    {
        TurnState = 1;
    }
    public void HighlightMoveRange() {
        foreach (TileBehaviour tile in grid) {
            int tilesAwayX = Mathf.Abs((int)tile.gridLocation.x - (int)playerTile.objectOnTile.GetComponent<PlayerBehaviour>().TurnOrginTile.x);
            int tilesAwayY = Mathf.Abs((int)tile.gridLocation.y - (int)playerTile.objectOnTile.GetComponent<PlayerBehaviour>().TurnOrginTile.y);
            if ((tilesAwayX + tilesAwayY) <= 3) {
                tile.FlashColor(Color.cyan);
            }
        }
    }
    private void EnemyTurn()
    {
        //Debug.Log("Enemy's Turn");
        EnemyMovement[] moveScripts = GameObject.FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemyScript in moveScripts)
        {
            enemyScript.DoEnemyMovement();
        }

        EnemyAttack[] attackScripts = GameObject.FindObjectsOfType<EnemyAttack>();
        foreach (EnemyAttack enemyScript in attackScripts)
        {
            print(enemyScript.gameObject.name + " attacked");
            enemyScript.TryToAttack();
        }

        EndTurn();
        TurnState = 2;

        // Check if all enemies are defeated
        if (GameObject.FindGameObjectsWithTag(enemyTag).Length == 0)
        {
            EndBattle();
        }
    }

    private void WorldTurn()
    {
        Debug.Log("World's Turn");
        worldHazards = GameObject.FindGameObjectsWithTag(hazardTag);
        foreach (GameObject obj in worldHazards)
        {
            Hazard hazardScript = obj.GetComponent<Hazard>();
            if (hazardScript != null)
            {
                hazardScript.TickDownTimer();
            }
        }

        EndTurn();
        PlayerBehaviour player = playerTile.objectOnTile.GetComponent<PlayerBehaviour>();
        player.TurnOrginTile = playerTile.gridLocation;
        player.attacking = false;
        TurnState = 3;
    }
}


