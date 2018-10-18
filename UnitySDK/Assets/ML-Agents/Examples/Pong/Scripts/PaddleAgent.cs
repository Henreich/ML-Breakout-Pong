using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PaddleAgent : Agent {
    public GameObject ball;
    public float speed = 2f;
    public int score = 0;

    //public GameObject opponent;

    private Rigidbody agentRb;
    private Rigidbody ballRb;

    public override void InitializeAgent()
    {
        agentRb = GetComponent<Rigidbody>();
        ballRb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations()
    {
        // Relative position of the ball compared to paddle (?)
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
        Debug.Log(vectorAction[0]);
        //Debug.Log(vectorAction[1]);
        //agentRb.velocity = new Vector3(0f, vectorAction[0], 0f);
        transform.Translate(0f, vectorAction[0] * speed * Time.deltaTime, 0f);
        //switch (vectorAction)
        //{

        //}

    }
}
