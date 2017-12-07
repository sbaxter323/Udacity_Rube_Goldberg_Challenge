using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Vector3 startingPosition;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < 0)
        {
            transform.position = startingPosition;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
	}
}
