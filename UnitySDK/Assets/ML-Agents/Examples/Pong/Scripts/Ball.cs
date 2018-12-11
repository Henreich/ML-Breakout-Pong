using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    private Vector3 startPos;
    private Rigidbody ballRb;
    private float timeToReset = 120;
    private int maxBallBounces = 15;
    private int lives = 3;

    public GameObject prefabObstacle1;
    
    public GameObject gameobjectPaddleA;

    public PaddleAgent agentA;
    //public PaddleAgent agentB;
    public float speed;
    //public int lastHitBy = 0;
    public int goalsLetIn = 0;
    public int ballsTouched = 0;
    public int ballBounces = 0;

    // Flags
    public bool playMode; // Is the game being played by a player or a model?
    public bool breakout; // Breakout or pong?

	// Use this for initialization
	void Start () {
        SetBallVelocity();
        agentA = agentA.GetComponent<PaddleAgent>();
        //agentB = agentB.GetComponent<PaddleAgent>();
        startPos = this.transform.position;
        ballRb = GetComponent<Rigidbody>();
    }


    void Update ()
    {
        if (!playMode)
        {
            timeToReset -= Time.deltaTime;

            if (timeToReset < 0) ResetGame();
              
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ballBounces++;
        if (!playMode && ballBounces > maxBallBounces) ResetGame();
        
        string collisionObjectName = (collision.gameObject.name.StartsWith("Obstacle"))  ? "Obstacle" : collision.gameObject.name;


        switch (collisionObjectName)
        {
            case "WallA":
                if (breakout)
                {
                    lives--;
                    if (lives == 0) ResetGame();
                }
                agentA.AddReward(-0.5f);
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
                if (breakout)
                {
                    // Taken from here:  https://answers.unity.com/questions/658533/setting-the-angle-of-a-ball-in-a-breakout-game.html
                    // Sets the velocity of the ball depending on where it collided with the paddle.
                    Vector3 paddlePosition = gameobjectPaddleA.transform.position;
                    Vector3 ballPosition = gameObject.transform.position;
                    Vector3 delta = ballPosition - paddlePosition;
                    Vector3 direction = delta.normalized;
                    ballRb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0f);
                }

                ballsTouched++;
                ballBounces = 0;
                agentA.AddReward(0.1f);
                //if (ballsTouched % 5 == 0) ResetGame();
                break;

            //case "PaddleAgentB":
            //    agentB.AddReward(0.05f);
            //    break;

            case "Obstacle":
                Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
                //Debug.Log(obstacle.getReward());
                agentA.AddReward(obstacle.getReward());
                Destroy(collision.gameObject);
                break;
        }
        if (playMode)
        {
            GameObject.Find("Score1").GetComponent<TextMesh>().text = "Goals scored on the agent: " + goalsLetIn;
            GameObject.Find("Score2").GetComponent<TextMesh>().text = "Ball hit since last failure: " + ballsTouched;
        }

    }

    private void ResetGame()
    {
        timeToReset = 120;
        lives = 3;
        ballBounces = 0;
        // Round done, 
        agentA.Done();
        //agentB.Done();

        // Reset ball to (0, 0, 0)
        this.transform.position = startPos;
        if (lives == 0) Instantiate(prefabObstacle1, transform.position, Quaternion.identity);
        SetBallVelocity();
    }

    private void SetBallVelocity()
    {
        //float vX = Random.Range(0, 2) == 0 ? -1 : 1;
        //float vY = Random.Range(0, 2) == 0 ? -1 : 1;
        float vX;

        if(breakout)
        {
            vX = 1;
        } else
        {
            vX = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        //float vX = Random.Range(0, 2) == 0 ? -1 : 1;
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