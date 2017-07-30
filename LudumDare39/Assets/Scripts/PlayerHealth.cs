using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Holds health and kills the player if you loose all your points.
/// </summary>
public class PlayerHealth : MonoBehaviour {

    public int health = 6;


    public void Hurt(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Debug.Log("Player died.");
            GetComponent<Player>().enabled = false;
        }
    }

}
