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
            //collision.transform.SetParent(gameObject.transform);
            gm.TrackPlayer(GetComponent<TileBehaviour>());
        }
        if (collision.gameObject.GetComponent<EnemyTakeDamage>())
        {
            //collision.transform.SetParent(gameObject.transform);
            gm.TrackEnemy(GetComponent<TileBehaviour>());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        objectOnTile = null;
    }
    public TileBehaviour GetNeighbor(Vector3 dir) {
        Vector3 rayOrgin = transform.position;
        rayOrgin += new Vector3(0.5f, 0.5f, 0.5f);
        RaycastHit[] hits = Physics.RaycastAll(rayOrgin, dir, 1);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != gameObject && hit.collider.gameObject.TryGetComponent<TileBehaviour>(out TileBehaviour tile))
            {
                return tile;
            }
        }
        return null;
    }
    public bool HasNeighbor(Vector3 dir)
    {
        return GetNeighbor(dir) != null;
    }
    public void FlashGreen() {
        StartCoroutine(Flash());
    }
    private IEnumerator Flash()
    {
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Renderer>().material.color = Color.white;
    }
}
