/******************************************************************************
 * Author: Brad Dixon
 * File Name: TileBehaviour.cs
 * Creation Date: 2/25/2025
 * Brief: Childs whatever object is on the tile to the tile
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public Vector2 gridLocation;
    private GameManager gm;
    /// <summary>
    /// Childs whatever is on the tile
    /// </summary>
    /// <param name="collision"></param>
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            collision.transform.SetParent(gameObject.transform);
            gm.TrackPlayer(GetComponent<TileBehaviour>());
        }
        /*if (collision.gameObject.GetComponent<EnemyBehavior>())
        {
            collision.transform.SetParent(gameObject.transform);
            gm.TrackEnemy(GetComponent<TileBehaviour>());
        }*/
    }
    public void FlashRed() {
        StartCoroutine(Flash());
    }
    private IEnumerator Flash()
    {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Renderer>().material.color = Color.white;
    }
}
