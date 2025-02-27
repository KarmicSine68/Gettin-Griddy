using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int hazardClock = 3; // Adjust this in the Inspector

    public void TickDownTimer()
    {
        if (hazardClock > 0)
        {
            hazardClock--;
            Debug.Log(gameObject.name + " Timer: " + hazardClock);

            if (hazardClock <= 0)
            {
                TriggerHazardEffect();
            }
        }
    }

    private void TriggerHazardEffect()
    {
        Debug.Log(gameObject.name + " triggered its effect!");
        // Example: Destroy itself or deal damage
        Destroy(gameObject);
    }
}
