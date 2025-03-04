using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameManager gm;
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }
    public void TryToAttack()
    {
        TileBehaviour currentTile = GetTile();
        TileBehaviour playerTile = gm.playerTile;
        LivesManager lm = GameObject.FindObjectOfType<LivesManager>();
        print(currentTile.GetNeighbor(Vector3.forward));
        print(currentTile.GetNeighbor(Vector3.back));
        print(currentTile.GetNeighbor(Vector3.left));
        print(currentTile.GetNeighbor(Vector3.right));

        if (currentTile.GetNeighbor(Vector3.forward) == playerTile) 
        {
            currentTile.GetNeighbor(Vector3.forward).FlashColor(Color.red);
            lm.DecreaseLives();
        }
        if (currentTile.GetNeighbor(Vector3.back) == playerTile)
        {
            currentTile.GetNeighbor(Vector3.back).FlashColor(Color.red);
            lm.DecreaseLives();
        }
        if (currentTile.GetNeighbor(Vector3.left) == playerTile)
        {
            currentTile.GetNeighbor(Vector3.left).FlashColor(Color.red);
            lm.DecreaseLives();
        }
        if (currentTile.GetNeighbor(Vector3.right) == playerTile)
        {
            currentTile.GetNeighbor(Vector3.right).FlashColor(Color.red);
            lm.DecreaseLives();
        }
    }
    public TileBehaviour GetTile() {
        float rayDistance = 2f;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, rayDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<TileBehaviour>(out TileBehaviour tile))
            {
                return tile;
            }
        }
        print("No tile found");
        return null;
    }
}
