using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Description:
/// Controls interaction of mech and objects?
/// </summary>
public class PlayerInteraction : MonoBehaviour {

    public float messegeRange = 3;
    public GameObject messege;

    private List<GameObject> interactables = new List<GameObject>();
    private GameObject currentMech = null;
    private GameObject closestInteractible = null;

    //Player Scripts
    private Player playerScript;
    private PlayerHealth playerHealth;
    private Rigidbody2D playerRigidBody;
    private Collider2D playerCollider;
    private SpriteRenderer playerRend;

    private void Start()
    {
        playerScript = GetComponent<Player>();
        playerHealth = GetComponent<PlayerHealth>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerRend = GetComponentInChildren<SpriteRenderer>();
        
    }

    /// <summary>
    /// Sets the interactable list for the player.
    /// </summary>
    /// <param name="pInteracts">List of objects to be interacted with player.</param>
    public void SetInteractables(List<GameObject> pInteracts)
    {
        interactables = pInteracts;
    }
	
	// Update is called once per frame
	void Update ()
    {
        FindInteractables();

        if (Input.GetButtonDown("Fire3"))
        {
            if (currentMech)
            {
                ExitMech();
            }
            else
            {
                HandleInteractions();
            }
        }
    }

    void FindInteractables()
    {
        
        if (closestInteractible != null && Vector2.Distance(closestInteractible.transform.position, transform.position) > messegeRange)
        {
            closestInteractible = null;
        }
        foreach (GameObject interactable in interactables)
        {
            if (Vector2.Distance(interactable.transform.position, transform.position) < messegeRange)
            {
                closestInteractible = interactable;
            }
        }
    }

    /// <summary>
    /// Interactables between player are handled here.
    /// </summary>
    void HandleInteractions()
    {
        if (interactables.Count > 0)
        {
            //If something to interact with
            if (closestInteractible != null)
            {
                string interactTag = closestInteractible.tag;
                switch (interactTag)
                {
                    case "Mech":
                        if (!currentMech)
                        {
                            EnterMech(closestInteractible);
                            currentMech = closestInteractible;
                            interactables.Remove(closestInteractible);
                            closestInteractible = null;
                        }
                        break;
                }
            }
        }
    }

    void EnterMech(GameObject mech)
    {
        EnablePlayer(false);
        transform.parent = mech.transform;
        transform.position = mech.transform.position;
        mech.GetComponent<MechController>().enabled = true;
    }

    void ExitMech()
    {
        currentMech.GetComponent<MechController>().enabled = false;
        interactables.Add(currentMech);
        EnablePlayer(true);
        currentMech = null;

    }

    void EnablePlayer(bool checkBox)
    {
        playerScript.enabled = checkBox;
        playerHealth.enabled = checkBox;
        playerRigidBody.simulated = checkBox;
        playerCollider.enabled = checkBox;
        playerRend.enabled = checkBox;
    }
}
