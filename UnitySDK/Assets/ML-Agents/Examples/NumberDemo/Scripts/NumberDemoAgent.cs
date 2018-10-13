// Training (from the ML-Pong folder): mlagents-learn "config/trainer_config.yaml" --env="Builds/NumberDemo_0/Unity Environment" --train

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class NumberDemoAgent : Agent {
    [SerializeField]
    private float currentNumber;
    [SerializeField]
    private float targetNumber;
    [SerializeField]
    private Text text;

    public GameObject cube;
    public GameObject sphere;


    private int solved;

    public override void CollectObservations()
    {
        AddVectorObs(currentNumber);
        AddVectorObs(targetNumber);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if(text != null)
        {
            text.text = string.Format("C:{0} \n T:{1} [{2}]", currentNumber, targetNumber, solved);
        }

        switch ((int)vectorAction[0])
        {
            case 0:
                currentNumber -= 0.01f;
                break;
            case 1:
                currentNumber += 0.01f;
                break;
            default:
                return;
        }

        cube.transform.position = new Vector3(currentNumber * 5f, 0f, 0f);

        if(currentNumber < -1.2f || currentNumber > 1.2f)
        {
            AddReward(-1f);
            Done(); // Calls AgentReset()?
            return;
        }

        float difference = Mathf.Abs(targetNumber - currentNumber);
        if(difference <= 0.01f)
        {
            solved++;
            AddReward(1f);
            Done();
            return;
        }
    }

    public override void AgentReset()
    {
        targetNumber = UnityEngine.Random.Range(-1f, 1f);
        currentNumber = 0f;
        sphere.transform.position = new Vector3(targetNumber * 5f, 0f, 0f);
    }
}
