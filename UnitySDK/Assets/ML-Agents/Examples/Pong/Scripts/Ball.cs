using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float speed = 5f;
    public int lastHitBy = 0;

    //private PaddleAgent agentA;
    //private PaddleAgent agentB;
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;

	// Use this for initialization
	void Start () {
        // All start directions are random to make things fair.
        float sx = Random.Range(0, 2) == 0 ? -1 : 1;
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
        } else if(collision.gameObject.name == "Wall2" && lastHitBy == 1)
        {
            scorePlayer1++;
            TextMesh Score1 = GameObject.Find("Score1").GetComponent<TextMesh>();
            Score1.text = "Score: " + scorePlayer1;
        }
        else if (collision.gameObject.name == "Wall1" && lastHitBy == 2)
        {
            scorePlayer2++;
            TextMesh Score2 = GameObject.Find("Score2").GetComponent<TextMesh>();
            Score2.text = "Score: " + scorePlayer2;
        }
        TextMesh textObject = GameObject.Find("LastHitBy").GetComponent<TextMesh>();
        textObject.text = "Last hit by: " + lastHitBy.ToString();
    }
}
