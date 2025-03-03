using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameManager gm;
    public int moveLimit = 2;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void MoveTowardsPlayer() {

    }

}
