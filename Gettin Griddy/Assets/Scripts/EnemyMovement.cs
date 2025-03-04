using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : GridMovement
{
    private GameManager gm;
    public int numOfMoves = 2;
    private int movesUsed = 0;
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        gm.EnemyTiles.Add(GetTile());
        GetTile().objectOnTile = gameObject;
    }
    public void DoEnemyMovement() {
        //print(GetTile());
        int distanceX = (int)gm.playerTile.gridLocation.x - (int)GetTile().gridLocation.x;
        int distanceY = (int)gm.playerTile.gridLocation.y - (int)GetTile().gridLocation.y;

        bool canMoveX = true;
        bool canMoveY = true;

        if (distanceX == 0 || GetTile().GetNeighbor(new Vector3(Mathf.Sign(distanceX), 0, 0)).objectOnTile != null) {
            canMoveX = false;
        }
        if (distanceY == 0 || GetTile().GetNeighbor(new Vector3(0, 0, Mathf.Sign(distanceY))).objectOnTile != null)
        {
            canMoveY = false;
        }

        //print("distance to player: " + distanceX + ", " + distanceY + "---" + canMoveX + canMoveY);
        //choosing a direction to move in if it even can
        if (canMoveX && canMoveY)
        {
            int r = Random.Range(0, 2);
            if (r < 1)
            {
                gm.RemoveEnemy(gameObject);
                GetTile().objectOnTile = null;
                
                Move(new Vector3(Mathf.Sign(distanceX), 0, 0));
                GetTile().objectOnTile = gameObject;
                gm.TrackEnemy(GetTile());
            }
            else
            {
                gm.RemoveEnemy(gameObject);
                GetTile().objectOnTile = null;
                
                Move(new Vector3(0, 0, Mathf.Sign(distanceY)));
                GetTile().objectOnTile = gameObject;
                gm.TrackEnemy(GetTile());
            }
        }
        else if (canMoveX)
        {
            gm.RemoveEnemy(gameObject);
            GetTile().objectOnTile = null;
            
            Move(new Vector3(Mathf.Sign(distanceX), 0, 0));
            GetTile().objectOnTile = gameObject;
            gm.TrackEnemy(GetTile());
        }
        else if (canMoveY)
        {
            gm.RemoveEnemy(gameObject);
            GetTile().objectOnTile = null;
            
            Move(new Vector3(0, 0, Mathf.Sign(distanceY)));
            GetTile().objectOnTile = gameObject;
            gm.TrackEnemy(GetTile());
        }
    }
    public bool HasUnusedMoves() {
        if (movesUsed >= numOfMoves) {
            return false;
        }
        return true;
    }
}
