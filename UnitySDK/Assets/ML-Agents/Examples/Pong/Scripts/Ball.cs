using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float speed = 7.5f;
    public int lastHitBy = 0;
    public PaddleAgent agentA;
    //public PaddleAgent agentB;
    public Vector3 startPos;
    public int goalsLetIn = 0;
    public int ballsTouched = 0;
    public int ballBounces = 0;


	// Use this for initialization
	void Start () {
        SetBallVelocity();
        agentA = agentA.GetComponent<PaddleAgent>();
        //agentB = agentB.GetComponent<PaddleAgent>();
        startPos = this.transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        ballBounces++;
        if (ballBounces > 15) ResetGame();

        switch (collision.gameObject.name)
        {
            case "WallA":
                //Debug.Log("wallA collision");
                agentA.AddReward(-0.1f);
                //agentB.AddReward(0.5f);
                //agentB.score++;
                //GameObject.Find("Score2").GetComponent<TextMesh>().text = "Score: " + agentB.score;
                goalsLetIn++;
                ballsTouched = 0;
                ResetGame();
                break;

            //case "WallB":
            //    agentA.AddReward(0.5f);
            //    agentB.AddReward(-0.1f);
            //    agentA.score++;
            //    GameObject.Find("Score1").GetComponent<TextMesh>().text = "Score: " + agentA.score;
            //    ResetGame();
            //    break;

            case "PaddleAgentA":
                //Debug.Log("agentA collision");
                ballsTouched++;
                ballBounces = 0;
                agentA.AddReward(0.1f);
                if (ballsTouched % 5 == 0) ResetGame();
                break;

            //case "PaddleAgentB":
            //    agentB.AddReward(0.05f);
            //    break;
        }

        GameObject.Find("Score1").GetComponent<TextMesh>().text = "Goals scored on the agent: " + goalsLetIn;
        GameObject.Find("Score2").GetComponent<TextMesh>().text = "Ball hit since last failure: " + ballsTouched;
    }

    private void ResetGame()
    {
        ballBounces = 0;
        // Round done, 
        agentA.Done();
        //agentB.Done();

        // Reset ball to (0, 0, 0)
        this.transform.position = startPos;
        SetBallVelocity();
    }

    private void SetBallVelocity()
    {
        //float vX = Random.Range(0, 2) == 0 ? -1 : 1;
        //float vY = Random.Range(0, 2) == 0 ? -1 : 1;

        float vX = Random.Range(0, 2) == 0 ? -1 : 1;
        float vY = Random.Range(-1f, 1f);
        //Debug.Log(vX + ", " + vY);

        GetComponent<Rigidbody>().velocity = new Vector3(vX * speed, vY * speed, 0f);
    }
}

//float sx = Random.Range(0, 2) == 0 ? -1 : 1;
//float sy = Random.Range(0, 2) == 0 ? -1 : 1;
//float randomnessDirectionX = Random.Range(-1f, 1f);
//float randomnessDirectionY = Random.Range(-1f, 1f);
//Debug.Log(randomnessDirectionX + ", " + randomnessDirectionY);

//GetComponent<Rigidbody>().velocity = new Vector3((speed* sx) + randomnessDirectionX, (speed* sy) + randomnessDirectionY, 0f);

//         float sy = Random.Range(-1, 2);                     // Move in positive, neutral or negative Y.