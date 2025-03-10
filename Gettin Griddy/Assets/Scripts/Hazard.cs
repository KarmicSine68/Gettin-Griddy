using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int hazardClock = 3; // Adjust this in the Inspector
    [SerializeField] private Collider hitBox;
    [SerializeField] private int damage = 1;

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
        hitBox.enabled = true;
        // Example: Destroy itself or deal damage
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            //Deal damage
            other.gameObject.GetComponent<EnemyTakeDamage>().TakeDamage(damage);
        }
        if(other.tag == "Player")
        {
            GameObject lives = GameObject.Find("LivesManager");
            lives.GetComponent<LivesManager>().DecreaseLives();
        }
    }
}
