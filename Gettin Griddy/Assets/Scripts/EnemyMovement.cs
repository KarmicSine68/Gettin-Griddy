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
    //finds the next tile that the enemy should move to
    public void MoveTowardsPlayer(Vector2 startingTile)
    {
        int DistanceToPlayerX = (int)gm.playerTile.gridLocation.x - (int)startingTile.x;
        int DistanceToPlayerY = (int)gm.playerTile.gridLocation.y - (int)startingTile.y;
        Vector2 newTileDirection;
        if (Mathf.Abs(DistanceToPlayerX) > Mathf.Abs(DistanceToPlayerY))
        {
            newTileDirection = new Vector2(Mathf.Sign(DistanceToPlayerX), 0);
        } else {
            newTileDirection = new Vector2(0, Mathf.Sign(DistanceToPlayerY));
        }
        Vector2 newTile = new Vector2(startingTile.x + newTileDirection.x, startingTile.y + newTileDirection.y);
        if (!gm.grid[(int)newTile.x, (int)newTile.y].hasObject) {
            gameObject.transform.position = gm.grid[(int)newTile.x, (int)newTile.y].gameObject.transform.position;
        }
    }
}
