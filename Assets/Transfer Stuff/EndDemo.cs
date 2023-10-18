using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//should be attached to a trigger at the end of the demo world

/// <summary>
/// This class handles when the user completes the demo world. It records the time that the 
/// user finished, as well as displays the text relevant for the user to read pertaining 
/// to the main environment. 
/// </summary>
public class EndDemo : MonoBehaviour {
	public static float endTime; //the time the user completed the demo world
	public GameObject text; //the text to be displayed in world - this is set through the inspector
                            //in case you ever need to change it, just create a text object in the world
                            //and then drag and drop that into the slot in the inspector on this script
    string pathCode = promptID.pathCode; //the two letter code entered at the beginning that determines path

    /// <summary>
    /// Initialization
    /// </summary>
	void Start () {
		endTime = 0; //initializes end time to 0
		text.SetActive (false); //makes sure the text is not visible yet
	}

    /// <summary>
    /// When the user enters the trigger that this script is placed on, display
    /// the text for them to read before continuing on to the main enviroment.
    /// </summary>
    /// <param name="other"> the player entering the trigger </param>
	void OnTriggerEnter (Collider other) {
		text.SetActive (true); //set the text to active, meaning it will become visible so the player 
                               //is able to read it 
	}

    /// <summary>
    /// When the user exits the trigger that this script is placed on by walking through the 
    /// previously activated text, record the time they fully completed the demo world and 
    /// load the main environment.
    /// </summary>
    /// <param name="other"> the player entering the trigger </param>
	void OnTriggerExit (Collider other) {
		endTime = Time.time; //the time the user completely finished the demo world 
        if (pathCode[1] != 'L') //if the randomization world does not need to be run
            //KEEP THIS SCENE UPDATED TO THE NEWEST VERSION
            SceneManager.LoadScene("41"); //load the main environment
        else SceneManager.LoadScene("41_RANDOMIZE"); //otherwise run the randomization environment
	}
}
