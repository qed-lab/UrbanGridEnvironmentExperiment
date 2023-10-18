using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//should be attached to the landmarks to be randomized

/// <summary>
/// This class handles the placement of the randomized landmarks. 
/// IT IS A WORK IN PROGRESS.
/// It works, but its going to take some more work, mainly on getting
/// the correct positions and rotations.
/// </summary>

    /* To access the local position of an object, you have a couple of options.
     * The easiest is to right click on the tab called inspector in the editor, and 
     * change it to "Debug." This will show you the localPosition. Or you can add a 
     * Debug.Log(transform.localPosition) in an Update method to the object you 
     * need (it's gotta hold an instance of the script). Then just read the console output.
     * 
     * It doesn't matter where the landmarks start, since they'll all get moved around anyway.
     * They can just be floating around - it doesn't matter. Make sure all the objects 
     * are active too.
     * 
     * Most importantly, LANDMARK OBJECTS CANNOT BE STATIC. YOU MUST DOUBLE CHECK THIS FOR
     * ALL LANDMARKS. There's a little box on the top right of the inspector that says "Static."
     * If it is checked, things will not move. So make sure it's unchecked for each object 
     * and its children.
     */ 
public class placeLandmarks : MonoBehaviour {
    public static List<KeyValuePair<Vector3, bool>> spots; //a list of pairs that hold positions and 
                                                           //a bool saying whether or not they have been filled
    //These are the positions of your landmarks - I used localPosition, so the values
    //you want to put in are small like these, not huge.
    //Create as many as you need and set them accordingly
    Vector3 pos1 = new Vector3(79, 17, 149);
    Vector3 pos2 = new Vector3(198, 17, 30);
    Vector3 pos3 = new Vector3(215, 17, -126);
    Vector3 pos4 = new Vector3(216, 17, -233);
    Vector3 pos5 = new Vector3(463, 17, -234);
    Vector3 pos6 = new Vector3(463, 17, -372);
    Vector3 pos7 = new Vector3(493, 17, -514);
    Vector3 pos8 = new Vector3(634, 17, -514);
    
    public static List<Vector3> pos;
    //a list to hold all of the positions in the order required
                                     //to create the lrID

    /// <summary>
    /// Sets all of the landmarks in their randomized positions as the scene
    /// starts.
    /// </summary>
    void Start () {
        if (spots == null) //each landmark will run this start, so we don't want to 
        {                  //reinitialize things everytime
            Init(); //initialize everything 
        }
        System.Random r = new System.Random(); //creates a variable that can produce random numbers
        while (true) //this loop will run until it is broken out of
        {               
            int index = r.Next(0, 8); //gets a random integer from 0 up to but not including the right number
                        //UPDATE  ^ this number to be how many landmarks you have
            if (!spots[index].Value) //if the spot at that random number has not been taken yet
            {
                //set the localPosition of the object to be the location at that spot
                transform.localPosition.Set(spots[index].Key.x, spots[index].Key.y, spots[index].Key.z);
                transform.localPosition = spots[index].Key;
                //change the spot to show that it has been taken
                spots[index] = new KeyValuePair<Vector3, bool>(transform.localPosition, true);
                //This code here was for testing purposes only. You're going to want to update it with your own.
                //This is the place to handle orientation if you need to. If you know a certain location needs a
                //certain rotation, change the localRotation of the object to be a new Quaternion with the values
                //of rotation you want. You should only need to change the first three, which are x,y, and z.
                //This part might get ugly with lots of if blocks, but I don't know how else to do it

                if (index == 0)
                //transform.localRotation = new Quaternion(0, 90, 0, 0);
                { transform.Rotate(Vector3.up, 90.0f); //rotate this landmark location by 90 degrees
                } else if (index == 1)
                { transform.Rotate(Vector3.up, 0.0f);
                }
                else if (index == 2)
                {
                    transform.Rotate(Vector3.up, 0.0f);
                }
                else if (index == 3)
                {
                    transform.Rotate(Vector3.up, 0.0f);
                }
                else if (index == 4)
                {
                    transform.Rotate(Vector3.up, 90.0f);
                }
                else if (index == 5)
                {
                    transform.Rotate(Vector3.up, 0.0f);
                }
                else if (index == 6)
                {
                    transform.Rotate(Vector3.up, 0.0f);
                }
                else if (index == 7)
                {
                    transform.Rotate(Vector3.up, 90.0f);
                    //{
                    //    transform.localRotation = new Quaternion(0, 45, 0, 0);
                }
                /*if (transform.localPosition.z == -6)
                {
                    transform.localRotation = new Quaternion(0, 90, 0, 0);
                } */
                break; //break out of the loop
            }
        }
    }

    /// <summary>
    /// Initialize all of the lists and add things to them
    /// </summary>
    private void Init()
    {
        //this is just initialization
        spots = new List<KeyValuePair<Vector3, bool>>();
        pos = new List<Vector3>();
        //create all of the spots using the positions and false, since nothing is taken yet
        //you'll need to add more of these if you have more landmarks
        KeyValuePair<Vector3, bool> spot1 = new KeyValuePair<Vector3, bool>(pos1, false);
        KeyValuePair<Vector3, bool> spot2 = new KeyValuePair<Vector3, bool>(pos2, false);
        KeyValuePair<Vector3, bool> spot3 = new KeyValuePair<Vector3, bool>(pos3, false);
        KeyValuePair<Vector3, bool> spot4 = new KeyValuePair<Vector3, bool>(pos4, false);
        KeyValuePair<Vector3, bool> spot5 = new KeyValuePair<Vector3, bool>(pos5, false);
        KeyValuePair<Vector3, bool> spot6 = new KeyValuePair<Vector3, bool>(pos6, false);
        KeyValuePair<Vector3, bool> spot7 = new KeyValuePair<Vector3, bool>(pos7, false);
        KeyValuePair<Vector3, bool> spot8 = new KeyValuePair<Vector3, bool>(pos8, false);
        
        //add all the spots, again, may need to add more
        spots.Add(spot1);
        spots.Add(spot2);
        spots.Add(spot3);
        spots.Add(spot4);
        spots.Add(spot5);
        spots.Add(spot6);
        spots.Add(spot7);
        spots.Add(spot8);
        
        //add all the positions - IN ORDER
        //the order you add them here determines the order of position
        //for the lrID
        pos.Add(pos1);
        pos.Add(pos2);
        pos.Add(pos3);
        pos.Add(pos4);
        pos.Add(pos5);
        pos.Add(pos6);
        pos.Add(pos7);
        pos.Add(pos8);
        
    }
}
