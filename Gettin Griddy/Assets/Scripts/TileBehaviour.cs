/******************************************************************************
 * Author: Brad Dixon, Tyler Bouchard
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
    public bool hasObject;
    public GameObject objectOnTile;
    private GameManager gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        objectOnTile = collision.gameObject;
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            collision.transform.SetParent(gameObject.transform);
            gm.TrackPlayer(GetComponent<TileBehaviour>());
        }
        if (collision.gameObject.GetComponent<EnemyTakeDamage>())
        {
            collision.transform.SetParent(gameObject.transform);
            gm.tilesWithEnemies.Add(GetComponent<TileBehaviour>());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        objectOnTile = null;
    }
    public void FlashRed() {
        StartCoroutine(Flash());
    }
    private IEnumerator Flash()
    {
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Renderer>().material.color = Color.white;
    }
}
