using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveTime = 2f; // Time to move
    public Vector3 targetPosition; // Destination

    public IEnumerator MoveObject()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure it reaches the exact position
    }
}