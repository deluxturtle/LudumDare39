using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Description: Does sword effects!
/// </summary>
public class SwordInteractions : MonoBehaviour {

    public Player player;
    void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.tag == "weed")
        {
            Destroy(other.gameObject);
            player.ResetAttack();
        }

        if(other.tag == "Enemy")
        {
            Enemy tempEnemy = other.GetComponent<Enemy>();
            if(tempEnemy != null)
            {
                tempEnemy.Hurt(1);
                player.ResetAttack();
            }
            
        }


    }
}
