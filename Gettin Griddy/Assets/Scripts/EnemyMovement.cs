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
    }
    public void DoEnemyMovement() {
        print(currentTile);
        int distanceX = (int)gm.playerTile.gridLocation.x - (int)currentTile.gridLocation.x;
        int distanceY = (int)gm.playerTile.gridLocation.y - (int)currentTile.gridLocation.y;
        bool canMoveX = true;
        bool canMoveY = true;
        if (distanceX == 0 || currentTile.GetNeighbor(new Vector3(Mathf.Sign(distanceX), 0, 0)).hasObject) {
            canMoveX = false;
        }
        if (distanceY == 0 || currentTile.GetNeighbor(new Vector3(0, 0, Mathf.Sign(distanceY))).hasObject)
        {
            canMoveY = false;
        }

        print("distance to player: " + distanceX + ", " + distanceY + "---" + canMoveX + canMoveY);
        //choosing a direction to move in if it even can
        if (canMoveX && canMoveY)
        {
            int r = Random.Range(0, 2);
            if (r < 1)
            {
                Move(new Vector3(Mathf.Sign(distanceX), 0, 0));
            }
            else
            {
                Move(new Vector3(0, 0, Mathf.Sign(distanceX)));
            }
        }
        else if (canMoveX)
        {
            Move(new Vector3(Mathf.Sign(distanceX), 0, 0));
            movesUsed++;
        }
        else if (canMoveY)
        {
            Move(new Vector3(0, 0, Mathf.Sign(distanceX)));
            movesUsed++;
        }
    }
    public bool HasUnusedMoves() {
        if (movesUsed >= numOfMoves) {
            return false;
        }
        return true;
    }
}
