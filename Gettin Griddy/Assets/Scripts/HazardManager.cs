using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    [SerializeField] private GameObject hazard;
    [SerializeField] private int col, row;
    [SerializeField] private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnCross()
    {
        for
        for (int i = 0; i < 8; i++)
        {
            Vector3 spawnPos = gameManager.grid[row, i].transform.position;
            spawnPos += new Vector3(.5f, 1.5f, .5f);
            gameManager.grid[row, i].objectOnTile = Instantiate(hazard, spawnPos, Quaternion.identity);
        }
        for (int i = 0; i < 8; i++)
        {
            Vector3 spawnPos = gameManager.grid[i, col].transform.position;
            spawnPos += new Vector3(.5f, 1.5f, .5f);
            gameManager.grid[i, col].objectOnTile = Instantiate(hazard, spawnPos, Quaternion.identity);
        }
    }
}
