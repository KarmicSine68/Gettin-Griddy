/******************************************************************************
 * Author: Brad Dixon
 * File Name: GameManager.cs
 * Creation Date: 2/25/2025
 * Last Changes: Jake Gorski, 2/26/2025
 * Brief: Keeps track of the status of the grid and where everything is. Handles turn order Player/Enemy/Hazards and can end battles.
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
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject[] worldHazards;
    [SerializeField] private string hazardTag = "Hazard";

    private bool PlayedTurn = false;
    private bool isBattleActive = true; // Battle control flag
    private GameObject[,] grid;

    [Header("Grid Parameters")]
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [SerializeField] private int TurnState = 1;

    private void Start()
    {
        grid = new GameObject[columns, rows]; 
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                GameObject tile = Instantiate(gridSpace, new Vector3(j, 0, i), Quaternion.identity);
                grid[j, i] = tile;
            }
        }

        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        worldHazards = GameObject.FindGameObjectsWithTag(hazardTag);

        SpawnPlayer();
        SpawnBoulder();
        
        StartCoroutine(GameLoop()); // Start the turn system
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

    /// <summary>
    /// Spawns the player on a random grid tile.
    /// </summary>
    private void SpawnPlayer()
    {
        int x = Random.Range(0, columns);
        int y = Random.Range(0, rows);

        Vector3 spawnPos = grid[x, y].transform.position;
        spawnPos += new Vector3(.5f, 1.5f, .5f);

        playerTracker = grid[x, y]; // Track player position
        Instantiate(player, spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// Spawns a boulder that obstructs movement.
    /// Ensures it does not spawn on the player's position.
    /// </summary>
    private void SpawnBoulder()
    {
        int x, y;
        do
        {
            x = Random.Range(0, columns);
            y = Random.Range(0, rows);
        } 
        while (grid[x, y] == playerTracker); // Ensure it doesn't spawn on the player

        Vector3 spawnPos = grid[x, y].transform.position;
        spawnPos += new Vector3(.5f, 1.5f, .5f);

        Instantiate(boulder, spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// Updates where in the grid the player is located.
    /// </summary>
    /// <param name="tile">The new tile the player is on</param>
    public void TrackPlayer(GameObject tile)
    {
        playerTracker = tile;

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                if (grid[j, i] == playerTracker)
                {
                    grid[j, i] = playerTracker;
                }
            }
        }
    }

    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player's Turn");

        PlayedTurn = false;
        yield return new WaitUntil(() => PlayedTurn);

        TurnState = 2;
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's Turn");
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        PlayedTurn = false;

        foreach (GameObject obj in enemies)
        {
            MovingObject movingScript = obj.GetComponent<MovingObject>();
            if (movingScript != null)
            {
                yield return StartCoroutine(movingScript.MoveObject());
            }
        }

        PlayedTurn = true;
        yield return new WaitUntil(() => PlayedTurn);

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
        worldHazards = GameObject.FindGameObjectsWithTag(hazardTag);
        PlayedTurn = false;

        foreach (GameObject obj in worldHazards)
        {
            Hazard hazardScript = obj.GetComponent<Hazard>();
            if (hazardScript != null)
            {
                hazardScript.TickDownTimer();
            }
        }

        PlayedTurn = true;
        yield return new WaitUntil(() => PlayedTurn);

        TurnState = 1;
    }
}
