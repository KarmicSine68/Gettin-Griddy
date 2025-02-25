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
    /// <summary>
    /// Childs whatever is on the tile
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.GetComponent<TileBehaviour>())
        {
            collision.transform.SetParent(gameObject.transform);
        }
    }
}
