using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//should be attached to a trigger at the end of the main environment

/// <summary>
/// This is the code that handles sending off the proper data once the 
/// player has entered the trigger at the end of the main environment. It's
/// purpose is mainly to tell trackInfo that it's time to finish up as well as the 
/// time that the player finished at.
/// </summary>
public class handleFinish : MonoBehaviour {

    static public bool isFinished; //whether or not the player has entered the finish trigger
    static public float finishTime; //the time the player enters the finish trigger, 
                                    //and therefore finishes the game

    /// <summary>
    /// The Start method is used for initialization only, in this case.
    /// </summary>
    void Start () {
        isFinished = false; //when this script is created, the player better not be at the end,
                            //so set this to false 
        finishTime = 0; //it doesn't really matter what we set this to now,
                        //it just needs to be initialized and 0 is standard
    }

    /// <summary>
    /// This is a Unity function that will automatically get called once the player
    /// enters the GameObject this script is attached to. That object has the IsTrigger box checked,
    /// so once it is entered by the player, this will get called.
    /// This script is set to the finishTrigger object.
    /// </summary>
    /// <param name="other"> the player object entering the trigger -- in this case we don't care </param>
    private void OnTriggerEnter(Collider other)
    {
        finishTime = Time.time; //sets the finish time to the current time, 
                                //aka the time the player entered the finish trigger
        isFinished = true; //tells trackInfo that the main environment has been completed
                           //and to run its finish methods
    }
}
