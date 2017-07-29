using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject targetFollow;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = new Vector3(targetFollow.transform.position.x, targetFollow.transform.position.y, -10);

    }
}
