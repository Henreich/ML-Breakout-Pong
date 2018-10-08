using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float speed = 5f;
    public int lastHitBy = 0;
    public bool isBall1;

	// Use this for initialization
	void Start () {
        // Always starts by moving the ball towards the middle.
        float sx = isBall1 ? 1 : -1;
        float sy = Random.Range(0, 2) == 0 ? -1 : 1;

        GetComponent<Rigidbody>().velocity = new Vector3(speed * sx, speed * sy, 0f);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}


    private void OnCollisionEnter(Collision collision)
    {   
        // TODO: Make a switch case instead?

        // Who hit the ball last, specificlly only whenever a collision
        // with a paddle happens
        if(collision.gameObject.name == "Paddle1")
        {
            lastHitBy = 1;
        }
        else if(collision.gameObject.name == "Paddle2")
        {
            lastHitBy = 2;
        }
        // Delete obstacles if they are hit
        else if (collision.gameObject.name == "Obstacle")
        { 
            Destroy(collision.gameObject);
        }
    }
}
