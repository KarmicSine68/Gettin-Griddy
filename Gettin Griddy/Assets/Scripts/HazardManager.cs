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
        int randOne = Random.Range(0, 8);
        int randTwo = Random.Range(0, 8);
        if(randOne == randTwo)
        {
            if(randTwo == 0)
            {
                randTwo = randTwo + 1;
            }
            if(randTwo == 8)
            {
                randTwo = randTwo - 1;
            }
        }

        int randChoice = Random.Range(0, 1);
        if(randChoice == 0)//spawns on rows
        {
            for (int i = 0; i < 8; i++)
            {
                Vector3 spawnPos = gameManager.grid[randOne, i].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                gameManager.grid[randOne, i].objectOnTile = Instantiate(hazard, spawnPos, Quaternion.identity);
            }
            for (int i = 0; i < 8; i++)
            {
                Vector3 spawnPos = gameManager.grid[randTwo, i].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                gameManager.grid[randTwo, i].objectOnTile = Instantiate(hazard, spawnPos, Quaternion.identity);
            }
        }
        if(randChoice == 1)//spawn on columns
        {
            for (int i = 0; i < 8; i++)
            {
                Vector3 spawnPos = gameManager.grid[i, randOne].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                gameManager.grid[i, randOne].objectOnTile = Instantiate(hazard, spawnPos, Quaternion.identity);
            }
            for (int i = 0; i < 8; i++)
            {
                Vector3 spawnPos = gameManager.grid[i, randTwo].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                gameManager.grid[i, randTwo].objectOnTile = Instantiate(hazard, spawnPos, Quaternion.identity);
            }
        }

    }
}
