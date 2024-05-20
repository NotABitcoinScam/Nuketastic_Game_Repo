using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZoneScript : MonoBehaviour
{

    public GameObject player;
    public GameObject selfObject;
    public float FinishTimeThreshold;

    private bool playerInZone;
    private float timeInZone;
    private bool hasFinished;
    
    // Start is called before the first frame update
    void Start()
    {
        hasFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasFinished)
        {
            UpdateZoneTimer();
        }
    }

    // OnTriggerStay2D is called every frame the player is touching the finish line
    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    // OnTriggerExit is called when the player exits the finish line
    void OnTriggerExit2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    // Called within the Update() function every frame to count the finish timer
    void UpdateZoneTimer()
    {
        if (playerInZone)
        {
            timeInZone += Time.deltaTime;
        } else
        {
            timeInZone = 0;
        }

        if (timeInZone > FinishTimeThreshold)
        {
            FinishLevel();
        }

    }

    // Called if the player has been within the finish line for a prerequisite amount of time by UpdateZoneTimer()
    void FinishLevel()
    {

        hasFinished = true;
        Debug.Log("FinishLevel CALLED");

        /*
         * TODO:
         * 
         *  LEVEL FINISH HERE
         *  CALL THIS FUNCTION ON timeInZone > some finish threshold
         *  
         *  
         */
    }

}
