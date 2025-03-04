/******************************************************************************
 * Author: Tyler Bouchard
 * File Name: UIButtonsScript.cs
 * Creation Date: 2/27/2025
 * Brief: allows enemies to take damage
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    public int health = 5;
    private GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void TakeDamage() {
        health--;
        if (health <= 0) {
            Die();
        } else {
            print("EnemyHealth: " + health);
        }   
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            print("EnemyHealth: " + health);
        }
    }
    private void Die() {
        gm.RemoveEnemy(gameObject);
        print("removed enemy");
        FindObjectOfType<ScoreManager>().IncreaseScore();

        Destroy(gameObject);
    }
}
