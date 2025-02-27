/******************************************************************************
 * Author: Brad Dixon
 * File Name: GameManager.cs
 * Creation Date: 2/25/2025
 * Last Modified By Jake Gorski 2/26/2025
 * Brief: Keeps track of the status of the grid and where everything is. It also now tracks the player/enemy/hazard turn order.
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
    private GameObject[,] grid;

    [Header("Grid Parameters")]
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [SerializeField] private int TurnState = 1;

    /// <summary>
    /// Initializes the grid, finds objects, and spawns the player and boulder.
    /// </summary>
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
    /// </summary>
    private IEnumerator GameLoop()
    {
        while (true) // Infinite loop to handle turns
        {
            yield return StartCoroutine(PlayerTurn());
            yield return StartCoroutine(EnemyTurn());
            yield return StartCoroutine(WorldTurn());
        }
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
    /// Handles the player's turn.
    /// </summary>
    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player's Turn");

        PlayedTurn = false;
        yield return new WaitUntil(() => PlayedTurn);

        TurnState = 2;
    }

    /// <summary>
    /// Handles enemy movement and actions during their turn.
    /// </summary>
    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's Turn");

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
    }

    /// <summary>
    /// Handles world events, such as hazard countdowns.
    /// </summary>
    private IEnumerator WorldTurn()
    {
        Debug.Log("World's Turn");

        PlayedTurn = false;

        foreach (GameObject obj in worldHazards)
        {
            Hazard hazardScript = obj.GetComponent<Hazard>();
            if (hazardScript != null)
            {
                hazardScript.TickDownTimer(); // Reduce hazard timer by 1
            }
        }

        PlayedTurn = true;
        yield return new WaitUntil(() => PlayedTurn);

        TurnState = 1; // Back to player turn
    }
}
