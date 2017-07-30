using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Holds health and kills the player if you loose all your points.
/// </summary>
public class PlayerHealth : MonoBehaviour {

    public int health = 6;
    public int maxHealth = 6;

    private UIHearts heartUI;

    private void Start()
    {
        heartUI = GameObject.FindObjectOfType<UIHearts>();
        if (heartUI == null)
        {
            Debug.LogWarning("UI for hearts couldn't be found. please add the GUI to scene.");
        }
        heartUI.InitializeHearts(maxHealth, health);
    }

    public void Hurt(int dmg)
    {
        health -= dmg;

        //Update gui
        heartUI.DeductHealth(dmg);

        if(health <= 0)
        {
            Debug.Log("Player died.");
            GetComponent<Player>().enabled = false;
        }
    }

}
