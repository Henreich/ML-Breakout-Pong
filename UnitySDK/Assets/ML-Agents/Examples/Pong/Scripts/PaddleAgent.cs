using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PaddleAgent : Agent {
    public GameObject ball;
    public GameObject opponent;

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
        AddVectorObs(transform.position.x - ball.transform.position.x);
        AddVectorObs(transform.position.y - ball.transform.position.y);

        // Observe the direction and speed of the agent.
        AddVectorObs(agentRb.velocity.y);

        // Observe the direction and speed of the ball.
        AddVectorObs(ballRb.velocity.x);
        AddVectorObs(ballRb.velocity.y);
    }
}
