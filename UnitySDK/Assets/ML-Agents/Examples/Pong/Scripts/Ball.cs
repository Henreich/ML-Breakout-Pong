using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float speed = 5f;
    public int lastHitBy = 0;
    public PaddleAgent agentA;
    public PaddleAgent agentB;


	// Use this for initialization
	void Start () {
        SetBallVelocity();
        agentA = agentA.GetComponent<PaddleAgent>();
        agentB = agentB.GetComponent<PaddleAgent>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);

        switch (collision.gameObject.name)
        {
            case "WallA":
                agentA.AddReward(-0.1f);
                agentB.AddReward(0.5f);
                agentB.score++;
                GameObject.Find("Score2").GetComponent<TextMesh>().text = "Score: " + agentB.score;
                ResetGame();
                break;

            case "WallB":
                agentA.AddReward(0.5f);
                agentB.AddReward(-0.1f);
                agentA.score++;
                GameObject.Find("Score1").GetComponent<TextMesh>().text = "Score: " + agentA.score;
                ResetGame();
                break;

            case "PaddleAgentA":
                agentA.AddReward(0.05f);
                break;

            case "PaddleAgentB":
                agentB.AddReward(0.05f);
                break;
        }
    }

    private void ResetGame()
    {
        // Round done, 
        agentA.Done();
        agentB.Done();

        // Reset ball to (0, 0, 0)
        this.transform.position = Vector3.zero;
        SetBallVelocity();
    }

    private void SetBallVelocity()
    {
        float sx = Random.Range(0, 2) == 0 ? -1 : 1;
        float sy = Random.Range(0, 2) == 0 ? -1 : 1;

        GetComponent<Rigidbody>().velocity = new Vector3(speed * sx, speed * sy, 0f);
    }
}
