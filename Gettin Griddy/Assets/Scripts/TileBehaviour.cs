/******************************************************************************
 * Author: Brad Dixon, Tyler Bouchard, Skylar Turner
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
    public GameObject objectOnTile;
    private GameManager gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        objectOnTile = collision.gameObject;
        if(objectOnTile.GetComponent<PlayerBehaviour>())
        {
            //collision.transform.SetParent(gameObject.transform);
            gm.TrackPlayer(GetComponent<TileBehaviour>());
        }
        if (objectOnTile.GetComponent<EnemyTakeDamage>())
        {
            //collision.transform.SetParent(gameObject.transform);
            gm.TrackEnemy(GetComponent<TileBehaviour>());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyTakeDamage>()) {
            print(collision.gameObject);
            gm.RemoveEnemy(gameObject.GetComponent<TileBehaviour>().objectOnTile);
        }
        objectOnTile = null;
    }*/
    public void SetObjectOnTile(GameObject obj) {
        objectOnTile = obj;
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
    public void SetColor(Color c) {
        GetComponent<Renderer>().material.color = c;
    }
    public bool HasNeighbor(Vector3 dir)
    {
        return GetNeighbor(dir) != null;
    }
    public void FlashColor(Color c) {
        StartCoroutine(Flash(c));
    }
    private IEnumerator Flash(Color c)
    {
        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;
        Color startColor = material.color;
        float duration = 0.25f;
        float elapsedTime = 0f;
        material.color = c;
        yield return new WaitForSeconds(0.25f);
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            material.color = Color.Lerp(c, Color.white, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        material.color = Color.white;
    }

    public bool IsWalkable()
    {
        return objectOnTile == null || objectOnTile.GetComponent<PlayerBehaviour>() != null;
    }

    public List<TileBehaviour> GetNeighbors()
    {
        List<TileBehaviour> neighbors = new List<TileBehaviour>();
        Vector3[] directions = new Vector3[]
        {
            new Vector3(1,0,0), new Vector3(-1,0,0),
            new Vector3(0,0,1), new Vector3(0,0,-1)
        };

        foreach (Vector3 dir in directions)
        {
            TileBehaviour neighbor = GetNeighbor(dir);
            if(neighbor != null && neighbor.IsWalkable())
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
}
