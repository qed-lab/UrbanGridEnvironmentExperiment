using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//should be attached to each intersection trigger

/// <summary>
/// This class handles when players enter and exit the triggers
/// that are handling the walls and the navigation. 
/// </summary>
public class PlayerTrigger : MonoBehaviour
{
	public List<GameObject> entryWalls; //the entry walls of the trigger
	public List<GameObject> exitWalls; //the exit walls of the trigger
	public GameObject arrow; //the arrow of the intersection
	private bool shown; //if the arrow has been shown already or not
    public static bool valChanged; //if the blockTime has changed
    public static float blockTime; //the time the player completed one block of distance

    /// <summary>
    /// Initialization - especially important is setting everything to 
    /// inactive at the beginning.
    /// </summary>
	void Start() {
        //things were weird, this sort of fixed something, just ignore it
		entryWalls = this.entryWalls;
		exitWalls = this.exitWalls;
        //for every wall within the exitWalls list
		foreach (GameObject wall in exitWalls) {
			wall.SetActive (false); //set it to inactive 
			wall.GetComponent<BoxCollider>().enabled = false; //again, ignore this. not sure it does anything
		}
        //same as exitWalls, but entryWalls
		foreach (GameObject wall in entryWalls) {
			wall.SetActive (false);
			wall.GetComponent<BoxCollider>().enabled = false;
		}
        //set the arrow to inactive
		arrow.SetActive (false);
        //nothing has been changed yet, so valChanged is false
        valChanged = false;
        blockTime = 0; //just initialization
	}

    /// <summary>
    /// When the user enters the intersections trigger, set the proper walls
    /// to active, the arrow to active, and broadcast that a value has changed.
    /// </summary>
    /// <param name="other"> the player entering the trigger </param>
    private void OnTriggerEnter(Collider other)
	{
		foreach (GameObject wall in entryWalls) {
			wall.SetActive (true); //set all the entryWalls to active so they can't back out
			wall.GetComponent<BoxCollider>().enabled = true;
		}
		if (!shown) { //if the arrow has not been shown yet
			arrow.SetActive (true); //display it
		}
        blockTime = Time.time; //set the blockTime to the current time
        valChanged = true; //say that the block time has changed and needs to be recorded
	}

    /// <summary>
    /// When the user exits the intersections trigger, set the proper walls
    /// to active and the arrow to inactive.
    /// </summary>
    /// <param name="other"> the player exiting the trigger </param>
	private void OnTriggerExit(Collider other)
	{
		foreach (GameObject wall in exitWalls) {
			wall.SetActive (true); //set all the exitWalls to active so they can't back up
			wall.GetComponent<BoxCollider>().enabled = true;
		}
		arrow.SetActive(false); //stop showing the arrow
		shown = true; //tell that the arrow has already been shown, so if they manage to edge into
                      //the trigger again, it won't pop up again
        valChanged = false; //say that no block time needs to be recorded right now
	}

}

