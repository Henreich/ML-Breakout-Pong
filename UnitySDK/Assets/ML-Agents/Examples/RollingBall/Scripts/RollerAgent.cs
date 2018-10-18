using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent {

    public Transform Target;
    public float speed = 10;

    private Rigidbody rBody;
    private float previousDistance = float.MaxValue;

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
	}

    // When resetting the agent. Also used to reset the target
    // in this case, which could also be done by the Academy.
    // (preferred with bigger projects?)
    public override void AgentReset()
    {
        if (this.transform.position.y < -1.0f)
        {
            // Agent fell of the platform, resetting position and velocity to 0.
            this.transform.position = Vector3.zero;
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        }
        else
        {
            Target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        }
    }

    // Collects observations for the brain, which it uses to make decisions
    public override void CollectObservations()
    {
        // Calculate relative position
        Vector3 relativePosition = Target.position - this.transform.position;

        // Divided by 5 to normalize the inputs to the neural network in the
        // range -1 to 1. 5 is used here because the platform is 10 units across.

        // Relative position
        AddVectorObs(relativePosition.x / 5);
        AddVectorObs(relativePosition.z / 5);

        // Distance to the edges of the platform. 4 edges, 4 observations
        AddVectorObs((this.transform.position.x + 5) / 5);
        AddVectorObs((this.transform.position.x - 5) / 5);
        AddVectorObs((this.transform.position.z + 5) / 5);
        AddVectorObs((this.transform.position.z - 5) / 5);

        // Velocity of the Agent
        AddVectorObs(rBody.velocity.x / 5);
        AddVectorObs(rBody.velocity.z / 5);
    }

    // Receives the decision from the brain and selects an action accordingly
    // vectorAction[] = Size depends on "Vector Action Size" and "Vector Action Space Size"
    //                  Set to 2 in this case since we are moving in 2 axis, not 3.
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Move the ball according to the decision made by the brain
        Vector3 controllSignal = Vector3.zero;
        controllSignal.x = vectorAction[0];
        controllSignal.z = vectorAction[1];

        rBody.AddForce(controllSignal * speed);

        // Reached target?
        float distanceToTarget = Vector3.Distance(this.transform.position, Target.position);
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            Done();
        }

        // Time penalty per step it takes to complete the task
        AddReward(-0.05f);

        // Fell off the platform
        if (this.transform.position.y < -1.0)
        {
            AddReward(-1.0f);
            Done();
        }
    }
}
