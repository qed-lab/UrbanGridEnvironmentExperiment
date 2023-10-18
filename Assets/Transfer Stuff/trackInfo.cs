using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
//should be attached to the player camera

/// <summary>
/// This is the file that handles all of the information that we're tracking
/// and writing to various places. It's sort of like the main file, as it pulls
/// data from many of the other files. This is the most connected file, meaning it 
/// has dependencies on almost all of the other files for various variables.
/// If you are looking to handle what is being written in the data file, this is
/// the place to be. This file also handles sending the data gathered to the server
/// once the end has been reached.
/// This file additionally holds all of the calculations for the time and distance
/// taken per block. The final determinations of routeID and lrID happen here also.
/// </summary>

/* A quick summary of data types:
 * int - an integer (whole number only)
 * float - number that can hold up to something like 8 or 16 decimal places tldr - has decimals
 * bool - a boolean value. is either true or false
 * List<type> - a list is a way to store other data types. it resizes itself, so you don't have to give it a size
 * Vector3 - a vector from the origin holding position data in the form <x,y,z>
 * KeyValuePair<type, type> - basically just allows the storage of two (potentially) different data types in one
 * string - basically a collection of characters. basically anything in ""
 * string [] (or any data type followed by []) - an array, this time of strings. works like a list, but cannot
 *                                               be resized, so it's length must be specified
 */

/* Unity specific stuff to know:
 * transform.position - holds the Vector3 that has the position in x,y,z of that object
 * GameObject - pretty much any physical thing in the world is a GameObject
 */
 
//GUI stands for Graphical User Interface

/* A quick explanation of statics:
 * a static variable is one that is shared between all instances of 
 * all the classes. it means that a variable made here can be accessed
 * by other classes as well
 */ 

public class trackInfo : MonoBehaviour {
    string path = promptID.filePath; //the file path for the data file (comes from promptID file)
    StreamWriter sw; //the StreamWriter, which is c#'s file I/O object that allows writing to files
    int timer; //a timer to determine how often data should be written to a file
    float startTime; //the time the player enters the main environment (the numbered scene)
    float finishTime; //the time reaches the finish trigger within the main environment
    bool isFinished; //whether or not the person has reached the end trigger
    float demoStart = showText.startTime; //the time that the player started the demo world (comes from showText file)
    float demoEnd = EndDemo.endTime; //the time that the player finished the demo world (comes from endDemo file)
    float blockTime; //the time taken to complete one block within the main environment
    bool writeBT; //whether or not it's time to write down the blockTime
    List<int> timePerBlock = new List<int>(); //a list to hold the times taken per block
    List<int> distPerBlock = new List<int>(); // a list to hold the distance traveled per block
    int lastBlockTime; //the time the last block time was recorded
    int routeID; //the id of the route that the player chose
    float distance; //the distance traveled by the player per block
    Vector3 lastPos; //the last position that the player was at
    bool last; //an alternate boolean to writeBT which takes on writeBT's value after computation has been done
    int pID = promptID.idNum; //the id number only of the participant's id (comes from promptID file)
    bool choice1 = false; //whether or not the player made the first right turn
    bool choice2 = false; //whether or not the player made the second right turn
    bool done = false; //a bool that ensures that the final URL is only opened once
    string code = promptID.pathCode; //the two letter code first entered (comes from promptID file)
    //these have to do with the landmark randomizing stuff
    List<KeyValuePair<Vector3, bool>> randomizerSpots = placeLandmarks.spots; //holds landmark positions and whether or not a landmark
                                                                    //has been placed there. the bool is not really relevant
                                                                    //here. all this file really cares about is the Vector3
    List<Vector3> randomizerPos = placeLandmarks.pos; //a list of the positions that the randomized landmarks are at.
                                            //the other file enters them into this list in order from 
                                            //"first" to "last", so make sure in the other file you add items to 
                                            //this list in the order you want 
    const int LANDMARK_NUMBER = 8; //this is a constant - it can't be changed at runtime
                                   //if you need to change the number of landmarks, this is the number you change

    /// <summary>
    /// The Start method runs on the scripts creation. It's a method
    /// that will only run once, so it's good for initializing variables
    /// or holding computation you only need done once.
    /// </summary>
    void Start () {
        //so much to initialize
        startTime = Time.time; //sets the start time to the current time of the game, since this is run on the 
                               //start up of the main environment
        sw = new StreamWriter (path, true); //initializes the StreamWriter with the file path and whether or not
                                            //it should append to the file at that path. in this case, yes
        timer = 4; //how many update cycles should pass before information should be written to the data file
                   //also setting it to 4 so that data is collected immediately
        //basic initialization - just setting things to zero so they're good to go later
        lastBlockTime = 0;
        routeID = 0;
        distance = 0;
        lastPos = transform.position; //sets the lastPos of the player to the position they're currently at,
                                      //since they haven't moved yet
        last = false; //another basic initialization 
    }

    /// <summary>
    /// The Update method is called many times a second. I'm not sure what Unity's rate is, but XNA's was around 60 times
    /// a second. So this gets called pretty frequently. It's use is to be able to update values as they are 
    /// changed within the game world, and make adjustments accordinly.
    /// In this case, the update method is for keeping variables up to date, calculating some numbers,
    /// and writing data to the file.
    /// </summary>
	void Update () {
        //these are set here so that every time update is called, it will update their values with the
        //values being taken in by those other classes
        finishTime = handleFinish.finishTime; //keeps grabbing the time since it never knows when the finish is
        isFinished = handleFinish.isFinished; //keeps checking to see if the player has finished
        blockTime = PlayerTrigger.blockTime; //keeps grabbing the time taken on this block
        writeBT = PlayerTrigger.valChanged; //keeps checking to see if the block time needs to be recorded

        //This is a call to another method. Scroll down to find its implementation
        HandleBlockTime();

        //checks to see if the player has hit the finish trigger at the end of the main environment
        if (isFinished)
        {
            Finished(); //if they have, call this method which handles sending the final data to the server
        }
        else
        {
            WriteToFile(); //otherwise, continue writing data to a file
        }
    }

    /// <summary>
    /// This method handles writing all of the constant incoming data in to a file. The time of the writing, the 
    /// player's x and z coordinates, and the viewing angle of the player are all recorded.
    /// This method does account for whether the player is in VR or not.
    /// </summary>
    private void WriteToFile()
    {
        //the purpose of this timer here is basically to ignore the incoming data 4 times out of 5. 
        //because update refreshes so often, we don't really need that much data, so it only will 
        //write it down any time the timer is equal to 4
        if (timer == 4)
        {
            if (code[0] != 'V') //if the first letter of the two letter code is NOT 'V'
            {
                //these three lines are some magic I found on the internet that get you the viewing angle of
                //a normal camera
                Vector3 forward = this.transform.forward;
                forward.y = 0;
                float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
                //this line writes to the file using the StreamWriter. it takes in a string to write to the file
                //the WriteLine means that it will add a newline after everytime it writes all of this.
                //This looks choppy, but its all one big string thanks to the + which allow for concatenation
                sw.WriteLine("Time:" + (Time.time - startTime) + ",X:" + this.transform.localPosition.x + ",Z:" + this.transform.localPosition.z
                 + ",Heading angle:" + headingAngle);
            }
            else //otherwise
            {
                //the previous block accounts for angles when not doing oculus. This one accounts specifically for oculus
                sw.WriteLine("Time:" + (Time.time - startTime) + ",X:" + this.transform.localPosition.x + ",Z:" + this.transform.localPosition.z
                 + ",Heading angle:" + UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye).eulerAngles);
            }
            timer = 0; //reset the timer
        }
        timer++; //increment the timer
    }

    /// <summary>
    /// This method just holds the decision making structure for the routeIDs.
    /// It looks at the two choice bools and assigns a number based on them.
    /// </summary>
    private void GetRouteID()
    {
        if (!choice1 && !choice2) routeID = 1; //1-straight 2-straight
        else if (!choice1 && choice2) routeID = 2; //1-straight 2-right
        else if (choice1 && !choice2) routeID = 3; //1-right 2-straight
        else if (choice1 && choice2) routeID = 4; //1-right 2-right
    }

    /// <summary>
    /// This method handles the computation of the time taken per block,
    /// as well as the distance traveled per block. Sorry, the method name sort
    /// of leaves that second part out. 
    /// </summary>
    private void HandleBlockTime()
    {
        distance += Vector3.Distance(transform.position, lastPos); //adds the distance traveled from the last position
                                                                   //to the current position - basically the distance 
                                                                   //that was traveled between update calls
        lastPos = transform.position; //resets the previous position to the current position

        //when entering a block trigger for the first time, so the writeBT bool has just been set
        //to true, but this method has not finished to set last to writeBT's value
        if (writeBT && !last)
        {
            //so if the list holding the times per block is empty
            if (timePerBlock.Count == 0)
            {
                //on a side note, the (int)value is casting. I'm basically telling the compliler to 
                //change that value from whatever it is to an int. In this case, I'm saying hey
                //I don't care about the decimals of this float so get rid of them
                lastBlockTime = (int)blockTime; //sets the last time that a block time was recorded to the 
                                                //most recent time block time was recorded
                int t = (int)(blockTime - startTime); //accounts for the fact that the player did not start
                                                      //playing through the main environment at time 0
                timePerBlock.Add(t); //adds the calculated time to the list keeping track of block times
            }
            else //otherwise, if the list is not empty
            {
                int t = (int)(blockTime - lastBlockTime); //this time just subtract out the last time that a time was
                                                          //recorded to get the block time
                timePerBlock.Add(t); //add to the list
                lastBlockTime = (int)blockTime; //reset the last block time
            }
            //whenever the block time is being recorded, its also time to record the distance that 
            //was traveled that block
            distPerBlock.Add((int)distance); //add the distance traveled to the list 
            distance = 0; //resets the distance variable
        }
        //when leaving the block trigger for the first time, so essentially the converse
        //of the previous if block
        if (!writeBT && last)
        {
            //sets the first choice based on if they move further right than the right wall at the correct junction
            if (distPerBlock.Count == 2 && transform.localPosition.z < 640) choice1 = true;
            //sets the second choice the same way
            if (distPerBlock.Count == 7 && transform.localPosition.z < 220) choice2 = true;
        }
        last = writeBT; //sets last equal to whatever writeBT is at this point in time
    }

    //*** THIS METHOD HAS NOT BEEN TESTED ***
    /// <summary>
    /// Since I haven't been able to test this method, take it with a grain of salt. I'll
    /// comment my algorithm as to what I think should happen, but I don't know if I'm right.
    /// This method is to determine the landmark ID that tells the order of the landmarks.
    /// </summary>
    /// <returns> a string array holding the 2 character landmark codes in the correct order </returns>
    string[] GetlrID()
    {
        string[] lrID = new string[LANDMARK_NUMBER]; //creates a new string array to hold the correct number of landmarks
        //foreach (KeyValuePair<Vector3, bool> pair in randomizerSpots) //this line reads almost like English
        foreach (KeyValuePair<Vector3, bool> pair in placeLandmarks.spots)
        {                                                   //loop through each pair in spots
            for (int i = 0; i < LANDMARK_NUMBER; i++) //loops through each landmark position
            {
                if (pair.Key == placeLandmarks.pos[i]) //if the position of the pair we're looking at equals the position
                {                       //we're looking at
                    lrID[i] = FindObj(placeLandmarks.pos[i]); //that place in the array equals whatever object is there
                }                              //so it's calling the other method to determine the correct object code
            }
        }
        return lrID; //once everything has been looped through and the array is full, return it
    }

    //*** THIS METHOD HAS NOT BEEN TESTED ***
    /// <summary>
    /// This object uses a bunch of Unity's built in functions to determine what GameObject 
    /// is what at runtime. It's purpose is to return the correct landmark code for 
    /// whatever is at that position.
    /// </summary>
    /// <param name="position"> the position the landmark is at</param>
    /// <returns> the correct code of that landmark </returns>
    private static string FindObj(Vector3 position)
    {
        //so for this call to work, your landmarks will need to have the tag "Random" (see https://docs.unity3d.com/Manual/Tags.html for creating Tags) or whatever
        //you want to set it to be. This is changable in the UnityEditor. This call then grabs
        //all of the GameObjects with that tag and puts them into an array
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Random");
        GameObject myObject = null; //create an empty GameObject to assign to once we find the one we want
        foreach (GameObject go in objs) //for each GameObject in that array we just made
        {
            if (go.transform.localPosition == position) //if the position of the GameObject we're on
            {                                           //matches the position we're looking for
                myObject = go; //set that empty object to this one
                break; //break out of the loop - we don't need to run through the rest of them since we've found it
            }
        }
        //now we have one giant conditional block to figure out what exactly we have
        //at the position we're looking at. each if statement asks if the GameObject
        //at that position is equal to the GameObject of name "whatever is in quotes"
        //you'll want to change the names of the objects to the ones you actually need once
        //you have them implemented. these are the ones I was using to test
        if (myObject == GameObject.Find("Fountain_Pivot"))
        {
            return "FO"; //change these to whatever you need them to be too
        }
        else if (myObject == GameObject.Find("Sign_Pivot"))
        {
            return "SI";
        }
        else if (myObject == GameObject.Find("BusStop_Pivot"))
        {
            return "BS";
        }
        else if (myObject == GameObject.Find("Monument_Pivot"))
        {
            return "MO";
        }
        else if (myObject == GameObject.Find("Sculpture_Pivot"))
        {
            return "SC";
        }
        else if (myObject == GameObject.Find("Billboard_Pivot"))
        {
            return "BB";
        }
        else if (myObject == GameObject.Find("Statue_Pivot"))
        {
            return "ST";
        }
        else if (myObject == GameObject.Find("WaterTower_Pivot"))
        {
            return "WT";
        }
        return ""; //this is just so it will compile. it should never get called
    }

    /// <summary>
    /// This method handles the final accumulation of data and sends it all off to the server.
    /// It also opens the URL of the survey for afterward using the created URL. Finally, it 
    /// closes the Unity application on it's own.
    /// </summary>
    void Finished()
	{
        if (!done) //has this code been executed once already, if not do so
        {
            done = true; //set to true so the code cannot be run again
            GetRouteID(); //calls to set the routeID
            string[] lrID = null; //creates a blank variable beforehand
            //code = "MF"; //Turn this on if debugging for the randomizer
            if (code[1] == 'L')   { //if there was randomization
                lrID = GetlrID(); //get its ID
            } else {
                lrID = new string[0]; //assign and empty array. Otherwise the code won't work
            }
            //these next several lines are all one huge string concatenation
            string url = "http://129.130.82.141/spatial/virtualresult.php?vep=" + code + "&pid=" + 
                pID + "&dt=" + (int)(demoEnd - demoStart) + "&ft=" + (int)(finishTime - startTime) +
                "&rtID=" + routeID + "&TpB=";
            //this is still concatenation, but the data here is held in lists, so 
            //you have to loop through each list and add each data point
            //for loops basically initialize a value (int i = 0), then give them
            //a constraint (i < timePerBlock.Count), and then an incrementation.
            //so each time the loop is run through, i will be increased by one.
            //the constraint will also be checked to make sure it's still true.
            //if it's false, the loop will not be run through. 
            //in a sense, it's a way to safely run through each element in a list
            //without hardcoding
            for (int i = 0; i < timePerBlock.Count; i++)
            {
                //if the data point is not the last one
                if (i < timePerBlock.Count - 1)
                    url += timePerBlock[i] + "|"; //add the time and the vertical bar to the URL
                else url += timePerBlock[i]; //if it is the last one, add the data but not the bar
            }
            url += "&DTpB=";
            //this loop works the same as the one previous
            for (int i = 0; i < distPerBlock.Count; i++)
            {
                if (i < distPerBlock.Count - 1)
                    url += distPerBlock[i] + "|";
                else url += distPerBlock[i];
            }
            url += "&lrID="; //this needs to be last because an empty lrID might exists when the randomizer is not on
            for (int i = 0; i < lrID.Length; i++) {
                if (i < lrID.Length - 1)
                    url += lrID[i] + "|";
                else url += lrID[i];
            }
            //opens the URL that's been created
            Application.OpenURL(url); 
            /* If you need to open a different URL for a different survey at the end, you need to do 
             * a couple things. If you need to send data along with it, you may have to alter some of 
             * the above code to change what is being sent. If its the same data, but a new location, 
             * just change the beginning of line 301 - "string url = "blah blah blah"". Or you can 
             * if you need no data and just a URL, you can erase the url from the line above and 
             * paste in the link you need. 
             * */

            sw.Close(); //closes the StreamWriter - you have to do this or nothing will be
                        //written to the file
            Application.Quit(); //closes the Unity application altogether
        }
	}
}
