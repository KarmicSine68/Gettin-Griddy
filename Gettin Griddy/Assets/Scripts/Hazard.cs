using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Adjust this in the Inspector
    [SerializeField] private Collider hitBox;
    [SerializeField] private int damage = 1;
    HazardManager hm;
    [SerializeField] private Animator animator;
    private void Start()
    {
        hm = GameObject.FindObjectOfType<HazardManager>();

        RaycastHit hit;
        //Physics.Raycast(transform.position, Vector3.down, out hit);
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.parent = hit.transform;
        }
    }
    public void CheckHazardTimer()
    {
        //Debug.Log(gameObject.name + " Timer: " + hm.hazardClock);
        if (hm.hazardClock <= 0)
        {
            StartCoroutine(TriggerHazardEffect());
        }
        if(hm.hazardClock == 1)
        {
            StartCoroutine(Warning());
        }
    }

    private IEnumerator Warning()
    {
        TileBehaviour tile = GetComponentInParent<TileBehaviour>();

        while(true)
        {
            if(hm.hazardClock != 1)
            {
                break;
            }
            try
            {
                tile.FlashColor(Color.red);
            }
            catch { };
            yield return new WaitForSeconds(.75f);
        }
    }

    private IEnumerator TriggerHazardEffect()
    {
        Debug.Log(gameObject.name + " triggered its effect!");
        hitBox.enabled = true;
        animator.SetBool("Explodes", true);
        SoundManager.Instance.PlaySFX("FireSound");
        yield return new WaitForSeconds(1.6f);
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
