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
    private GameObject previousMech = null;

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
        playerRend = GetComponent<SpriteRenderer>();
        
    }

    public void SetInteractables(List<GameObject> pInteracts)
    {
        interactables = pInteracts;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if(interactables.Count > 0)
        {
            GameObject closestInteractible = null;
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

            if (closestInteractible != null)
            {
                string interactTag = closestInteractible.tag;

                if (Input.GetButtonDown("Jump"))
                {
                    switch (interactTag)
                    {
                        case "Mech":
                            EnterMech(closestInteractible);
                            previousMech = closestInteractible;
                            interactables.Remove(closestInteractible);
                            closestInteractible = null;
                            break;
                    }
                }
            }
        }

    }

    void EnterMech(GameObject mech)
    {
        Debug.Log("Getting in mech.");
        EnablePlayer(false);
        transform.parent = mech.transform;
    }

    void ExitMech()
    {
        interactables.Add(previousMech);
        previousMech = null;
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
