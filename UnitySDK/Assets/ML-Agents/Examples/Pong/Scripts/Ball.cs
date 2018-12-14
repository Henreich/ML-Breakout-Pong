using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    private Vector3 startPos;
    private Rigidbody ballRb;
    //private GameObject Prefab_1_Obstacle;
    //private GameObject Prefab_2_Obstacle;
    //private GameObject Prefab_3_Obstacle;
    //private GameObject Prefab_4_Obstacle;
    //private Vector3 temp1;
    //private Vector3 temp2;
    //private Vector3 temp3;
    //private Vector3 temp4;
    private GameObject prefabObstaclesLeft;
    private GameObject prefabObstaclesRight;
    private GameObject prefabObstaclesImitator;
    private Vector3 prefabObstaclesLeftPosition;
    private Vector3 prefabObstaclesRightPosition;
    private Vector3 prefabObstaclesImitatorPosition;
    private float timeToReset  = 60;    // How much time until soft-resetting the game
    private int maxBallBounces = 15;    // Amount of times the ball can bounce without touching the paddle, bricks or goal.
    private int lives = 3;              // Number of lives before hard-resetting the game
    private int numberOfBlocks = 0;
    private int maxNumberOfBlocks;
    
    // Public, to watch in the editor.
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
    public bool playerTwo; // Is player two present?
    public bool imitator;

	// Use this for initialization
	void Start () {
        SetBallVelocity();
        agentA = agentA.GetComponent<PaddleAgent>();
        //agentB = agentB.GetComponent<PaddleAgent>();
        startPos = this.transform.position;
        ballRb = GetComponent<Rigidbody>();

        if (breakout)
        {
            //Prefab_1_Obstacle = GameObject.Find("1_Obstacle");
            //Prefab_2_Obstacle = GameObject.Find("2_Obstacle");
            //Prefab_3_Obstacle = GameObject.Find("3_Obstacle");
            //Prefab_4_Obstacle = GameObject.Find("4_Obstacle");
            //temp1 = Prefab_1_Obstacle.transform.position;
            //temp2 = Prefab_2_Obstacle.transform.position;
            //temp3 = Prefab_3_Obstacle.transform.position;
            //temp4 = Prefab_4_Obstacle.transform.position;
            prefabObstaclesLeft = GameObject.Find("Obstacles_Left");
            prefabObstaclesLeftPosition = prefabObstaclesLeft.transform.position;

            if (playerTwo)
            {
                prefabObstaclesRight = GameObject.Find("Obstacles_Right");
                prefabObstaclesRightPosition = prefabObstaclesRight.transform.position;
            }

            if (imitator)
            {
                prefabObstaclesImitator = GameObject.Find("Obstacles_Imitation");
                prefabObstaclesImitatorPosition = prefabObstaclesImitator.transform.position;
            }

            //foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
            //{
            //    if (gameObj.name.Contains("Obstacle")) numberOfBlocks++;
            //}
            numberOfBlocks = 44; // Hardcoded for now. Naming makes it difficult to pin down
                                 // which arean one brick is on. Maybe this should be stored within
                                 // the individual brick... or maybe implement a better game manager
                                 // system.
            maxNumberOfBlocks = numberOfBlocks;
            //Debug.Log("Blocks: " + numberOfBlocks);
        }

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
                agentA.AddReward(-0.1f);
                //agentB.AddReward(0.5f);

                if (breakout)
                {
                    --lives;
                    if (lives == 0) ResetGame();
                }
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
                agentA.AddReward(0.5f);

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
                break;

            //case "PaddleAgentB":
            //    agentB.AddReward(0.05f);
            //    break;

            case "Obstacle":
                Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
                //Debug.Log(obstacle.getReward());
                agentA.AddReward(obstacle.getReward());
                Destroy(collision.gameObject);
                numberOfBlocks--;
                ballBounces = 0;
                //Debug.Log("Blocks:" + numberOfBlocks);
                //if (!imitator && !playerTwo) Debug.Log(numberOfBlocks);
                if (numberOfBlocks == 0) ResetGame();

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

        if (breakout) breakoutReset();

        timeToReset = 120;
        ballBounces = 0;
        // Round done, 
        agentA.Done();
        //agentB.Done();

        // Reset ball to its starting position
        this.transform.position = startPos;
        SetBallVelocity();
        //if (lives == 0) Instantiate(prefabObstacle1, transform.position, Quaternion.identity);
    }

    private void SetBallVelocity()
    {
        //float vX = Random.Range(0, 2) == 0 ? -1 : 1;
        //float vY = Random.Range(0, 2) == 0 ? -1 : 1;
        float vX;

        if(breakout)
        {
            if (!playerTwo)
            {
                vX = 1;
            } else
            {
                vX = -1;
            }
            
        } else
        {
            vX = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        //float vX = Random.Range(0, 2) == 0 ? -1 : 1;
        float vY = Random.Range(-1f, 1f);
        //Debug.Log(vX + ", " + vY);

        GetComponent<Rigidbody>().velocity = new Vector3(vX * speed, vY * speed, 0f);
    }

    /**
     * Method to reset the scene when playing breakout.
     */
    private void breakoutReset()
    {
        if (lives == 0 || numberOfBlocks == 0)
        {
            // No need to go through if no blocks are left.
            if (numberOfBlocks > 0)
            {
                // Destroy ObstacleLeft or the clones.
                // TODO: Refactor.
                foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
                {
                    if (gameObj.name.Contains("Obstacles_Left") && !imitator)
                    {
                        Destroy(gameObj);
                    } else if (playerTwo && gameObj.name.Contains("Obstacles_Right"))
                    {
                        Destroy(gameObj);
                    } else if (imitator && gameObj.name.Contains("Obstacles_Imitation"))
                    {
                        Destroy(gameObj);
                    }
                }

            }
            else
            {
                numberOfBlocks = maxNumberOfBlocks;
            }

            // Re-instansiate and reset lives.
            //Instantiate(Resources.Load("Prefabs/1_Obstacle"), temp1, Quaternion.identity);
            //Instantiate(Resources.Load("Prefabs/2_Obstacle"), temp2, Quaternion.identity);
            //Instantiate(Resources.Load("Prefabs/3_Obstacle"), temp3, Quaternion.identity);
            //Instantiate(Resources.Load("Prefabs/4_Obstacle"), temp4, Quaternion.identity);

            if (playerTwo)
            {
                Instantiate(Resources.Load("Prefabs/Obstacles_Right"), prefabObstaclesRightPosition, Quaternion.identity);
            } else if (imitator)
            {
                Instantiate(Resources.Load("Prefabs/Obstacles_Imitation"), prefabObstaclesImitatorPosition, Quaternion.identity);
            } else
            {
                Instantiate(Resources.Load("Prefabs/Obstacles_Left"), prefabObstaclesLeftPosition, Quaternion.identity);
            }
            lives = 3;
        }
    }
}

//float sx = Random.Range(0, 2) == 0 ? -1 : 1;
//float sy = Random.Range(0, 2) == 0 ? -1 : 1;
//float randomnessDirectionX = Random.Range(-1f, 1f);
//float randomnessDirectionY = Random.Range(-1f, 1f);
//Debug.Log(randomnessDirectionX + ", " + randomnessDirectionY);

//GetComponent<Rigidbody>().velocity = new Vector3((speed* sx) + randomnessDirectionX, (speed* sy) + randomnessDirectionY, 0f);

//         float sy = Random.Range(-1, 2);                     // Move in positive, neutral or negative Y.



