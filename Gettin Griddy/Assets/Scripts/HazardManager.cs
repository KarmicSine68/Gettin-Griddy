using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public int hazardClock = 3;
    [SerializeField] private GameObject hazard;
    [SerializeField] private int col, row;
    [SerializeField] private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        SpawnDouble();
    }

    // Update is called once per frame
    void Update()
    {
        if (hazardClock <= 0) {
            SpawnDouble();
            hazardClock = 3;
        }
    }


    public void SpawnDouble()
    {
        int randOne = Random.Range(0, 8);
        int randTwo = Random.Range(0, 8);
        
        while (randOne == randTwo) 
        {
            randTwo = Random.Range(0, 8);
        }
        
        int randChoice = Random.Range(0, 2);
        Debug.Log(randChoice);
        if(randChoice == 0)//spawns on rows
        {
            for (int i = 0; i < 9; i++)
            {
                Vector3 spawnPos = gameManager.grid[randOne, i].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                Instantiate(hazard, spawnPos, Quaternion.identity);
                Debug.Log("hitting row:" + randOne);
            }
            for (int i = 0; i < 9; i++)
            {
                Vector3 spawnPos = gameManager.grid[randTwo, i].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                Instantiate(hazard, spawnPos, Quaternion.identity);
                Debug.Log("hitting row:" + randTwo);
            }
        }
        if(randChoice == 1)//spawn on columns
        {
            for (int i = 0; i < 9; i++)
            {
                Vector3 spawnPos = gameManager.grid[i, randOne].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                Instantiate(hazard, spawnPos, Quaternion.identity);
                Debug.Log("hitting column:" + randOne);
            }
            for (int i = 0; i < 9; i++)
            {
                Vector3 spawnPos = gameManager.grid[i, randTwo].transform.position;
                spawnPos += new Vector3(.5f, 1.5f, .5f);
                Instantiate(hazard, spawnPos, Quaternion.identity);
                Debug.Log("hitting column:" + randTwo);
            }
        }
    }
}
