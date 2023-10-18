using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//should be attached to a blank GameObject in the main environment

/// <summary>
/// This class is used to determine which walls and which naviagtion objects need
/// to be set to active. It pulls from the two letter code to figure out what should 
/// be set. 
/// </summary>
public class setPath : MonoBehaviour {
    //gets the two letter code from promptID
    string pathCode = promptID.pathCode;

	/// <summary>
    /// Ensures that everything is set once, on the creation of this script.
    /// This script will be run on the creation of the main environment, so 
    /// everything is properly activated/deactivated before the user can 
    /// interact with anything.
    /// </summary>
	void Start () {
        if (pathCode[1] == 'L') //if the path involves randomization
        {
            //set the fixed path walls to be active
            GameObject walls = GameObject.Find("Wall Composite - Single");
            walls.SetActive(true);
            //set the fixed path navigation to be active
            GameObject nav = GameObject.Find("Navigation - Single");
            nav.SetActive(true);

            //set the choice path walls to be inactive
            GameObject nOwalls = GameObject.Find("Wall Composite - Option");
            nOwalls.SetActive(false);
            //set the choice path navigation to be inactive
            GameObject nOnav = GameObject.Find("Navigation - Option");
            nOnav.SetActive(false);

            //Change the name in quotes based on the grouping of landmarks
            //it'll throw errors otherwise
            //GameObject rand = GameObject.Find("Randomized Landmarks");
            //rand.SetActive(true);
            /* *** NOTE ***
             * I wrote this on the assumption that the randomized landmarks would 
             * be functioning out of the main environment. As they're not, it's going 
             * to mess with the way I wrote some of the landmark code. I'll leave this
             * code here just in case, but you probably won't need it. You should just
             * always have your landmarks be active in the second scene.
             */ 
        }
        else if (pathCode[1] == 'F') //if the path is fixed
        {
            //set the fixed path walls to be active
            GameObject walls = GameObject.Find("Wall Composite - Single");
            walls.SetActive(true);
            //set the fixed path navigation to be active
            GameObject nav = GameObject.Find("Navigation - Single");
            nav.SetActive(true);

            //set the choice path walls to be inactive
            GameObject nOwalls = GameObject.Find("Wall Composite - Option");
            nOwalls.SetActive(false);
            //set the choice path navigation to be inactive
            GameObject nOnav = GameObject.Find("Navigation - Option");
            nOnav.SetActive(false);

            //GameObject rand = GameObject.Find("Randomized Landmarks");
            //rand.SetActive(false);
        }
        else if (pathCode[1] == 'C') //if the path has the choice
        {
            //set the choice path walls to be active
            GameObject walls = GameObject.Find("Wall Composite - Option");
            walls.SetActive(true);
            //set the choice path navigation to be active
            GameObject nav = GameObject.Find("Navigation - Option");
            nav.SetActive(true);

            //set the fixed path walls to be inactive
            GameObject nOwalls = GameObject.Find("Wall Composite - Single");
            nOwalls.SetActive(false);
            //set the fixed path navigation to be inactive
            GameObject nOnav = GameObject.Find("Navigation - Single");
            nOnav.SetActive(false);

            //GameObject rand = GameObject.Find("Randomized Landmarks");
            //rand.SetActive(false);
        }
	}

}
