using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLAgents
{

    /// <summary>
    /// Behavioral Cloning Helper script. Attach to teacher agent to enable 
    /// resetting the experience buffer, as well as toggling session recording.
    /// </summary>
    public class BCTeacherHelper : MonoBehaviour
    {

        bool recordExperiences;
        bool resetBuffer;
        bool timedReset;
        Agent myAgent;
        float bufferResetTime;
        float timeToReset = 60;

        public KeyCode recordKey = KeyCode.R;
        public KeyCode resetKey = KeyCode.C;

        // Use this for initialization
        void Start()
        {
            recordExperiences = true;
            resetBuffer = false;
            timedReset = false;
            myAgent = GetComponent<Agent>();
            bufferResetTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {

            timeToReset -= Time.deltaTime;
            if (timeToReset < 0) timedReset = true;

            if (Input.GetKeyDown(recordKey))
            {
                recordExperiences = !recordExperiences;
            }

            if (Input.GetKeyDown(resetKey) || timedReset)
            {
                resetBuffer = true;
                bufferResetTime = Time.time;
                Debug.Log("resetting buffer...");
                timedReset = false;
                timeToReset = 60;
            }
            else
            {
                resetBuffer = false;
            }

            Monitor.Log("Recording experiences " + recordKey, recordExperiences.ToString());
            float timeSinceBufferReset = Time.time - bufferResetTime;
            Monitor.Log("Seconds since buffer reset " + resetKey, 
                Mathf.FloorToInt(timeSinceBufferReset).ToString());
        }

        void FixedUpdate()
        {
            // Convert both bools into single comma separated string. Python makes
            // assumption that this structure is preserved. 
            myAgent.SetTextObs(recordExperiences + "," + resetBuffer);
        }
    }
}
