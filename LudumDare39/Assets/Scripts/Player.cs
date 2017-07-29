using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(Input.GetButtonDown("Fire1"))
            Attack();

    }

    private void Attack()
    {

    }


    private void FixedUpdate()
    {
        float horizontal = Mathf.Round(Input.GetAxis("Horizontal"));
        float vertical = Mathf.Round(Input.GetAxis("Vertical"));


        if (Input.GetKey("right"))
        {
            horizontal = 1;
        }
        if (Input.GetKey("left"))
        {
            horizontal = -1;
        }
        if (Input.GetKey("up") && !Input.GetKey("down"))
        {
            vertical = 1;
        }
        if (Input.GetKey("down") && !Input.GetKey("up"))
        {
            vertical = -1;
        }
        



        transform.position += new Vector3(horizontal, vertical).normalized * Time.deltaTime * speed;
    }
}
