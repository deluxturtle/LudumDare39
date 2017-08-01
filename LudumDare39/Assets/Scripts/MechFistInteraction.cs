using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechFistInteraction : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "weed")
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "Enemy")
        {
            Enemy tempEnemy = other.GetComponent<Enemy>();
            if (tempEnemy != null)
            {
                other.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position).normalized * 600);
                tempEnemy.Hurt(2);
            }

        }


    }
}
