using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PaddleAgent : Agent {
    public GameObject ball;
    public float speed = 5f;
    public int score = 0;
    public bool isAgentA;
    public Vector3 startPos;

    //public GameObject opponent;

    private Rigidbody agentRb;
    private Rigidbody ballRb;
    private float distanceToEdges = 4.7f;

    public override void InitializeAgent()
    {
        agentRb = GetComponent<Rigidbody>();
        ballRb = GetComponent<Rigidbody>();
        startPos = agentRb.transform.position;
    }

    public override void CollectObservations()
    {
        // Relative position of the ball compared to paddle (?)
        AddVectorObs(this.transform.position.x);
        AddVectorObs(this.transform.position.y);
        AddVectorObs(ball.transform.position.x);
        AddVectorObs(ball.transform.position.y);
        AddVectorObs(this.transform.position.x - ball.transform.position.x);
        AddVectorObs(this.transform.position.y - ball.transform.position.y);



        // Observe the velocity of the agent.
        AddVectorObs(agentRb.velocity.y);

        // Observe the direction and speed of the ball.
        AddVectorObs(ballRb.velocity.x);
        AddVectorObs(ballRb.velocity.y);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //Debug.Log(vectorAction[0]);

        transform.Translate(0f, vectorAction[0] * speed * Time.deltaTime, 0f);
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(transform.position.y, startPos.y - distanceToEdges, startPos.y + distanceToEdges);
        transform.position = clampedPosition;
    }

    public override void AgentReset()
    {
    
        if (isAgentA)
        {
            this.transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
        } 
        else
        {
            this.transform.position = new Vector3(-startPos.x, startPos.y, startPos.z);
        }
    }
}


//clampedPosition.y = Mathf.Clamp(transform.position.y, startPos.y -11f, startPos.y + 11f);