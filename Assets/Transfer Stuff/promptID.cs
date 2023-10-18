using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System;
//should be attached to a blank GameObject in the InfoEntry scene

/// <summary>
/// This is the script that will be run immediately upon the launch of the game.
/// It handles a prompting the user for their ID code, processes that information, 
/// creates a data file with that code, and then loads the instructions scene. 
/// This scene holds most of the static variables that the other scripts use.
/// </summary>
public class promptID : MonoBehaviour {
	string id; //holds whatever is typed in to the text box
	string path; //holds the file path within the computer where the data files will be saved
	static public string filePath; //holds the file path within the computer to the data file that has been created
    static public string pathCode; //holds the two letter code at the beginning of the user id
    static public int idNum; //the id numbers that go behind the pathCode in the user id
    StreamReader sr; //the StreamReader that will read the text file holding the path to the location of where the 
                     //data files are to be stored
    int idLength; //the number of charcters in the participant id - 27OCT20 for clearer usage below

    /// <summary>
    /// Initialization
    /// </summary>
	void Start()
	{ 
        //basic string initialization 
		id = "";
		path = "";
		filePath = "";
        //initializes the StreamReader to be reading from a file called Path.txt that 
        //exists in the same location as the build, or in the assets folder, depending 
        //on whether you're running in the editor or not
        sr = new StreamReader("Path.txt");
        //id length is 7
        idLength = 7;
    }

    /// <summary>
    /// This is a built in Unity function. It means that once the screen has initialized and things 
    /// are ready to display, whatever is in this method is going to happen. It allows this to 
    /// display immediately upon the creation of the scene.
    /// </summary>
    void OnGUI()
	{
        GUIStyle gs = new GUIStyle(); //creates a GUI style so the text style can be altered
        gs.alignment = TextAnchor.MiddleCenter; //set the alignment of the text to be centered
        gs.fontSize = 20; //makes the font size larger 
        GUI.contentColor = Color.black; //set the color of all GUI elements to black
        GUILayout.BeginArea(new Rect(0, 300, Screen.width, Screen.height)); //creates the area where GUI elements will be displayed
                                                                            //the new Rect creates a rectangle at the location of the first two paratmeters
                                                                            //and the size of the second two parameters
        //The label is what allows the text to be displayed to the screen. The GUI style is also passed to this function so that
        //all of the style changes set earlier will affect this text
        GUILayout.Label("Moderator: Please enter virtual environment Code + Participant ID (XX#####):", gs); 
        GUILayout.EndArea(); //tells the graphical system to stop drawing stuff
        GUILayout.BeginArea(new Rect(Screen.width/2 - 50, 350, 100, 100)); //start a new area for the text box and the button
        id = GUILayout.TextField (id, idLength); //draws the text box and sets id to whatever is written inside
                                          //the idLength limits the amount of characters that can be typed into the box.
        bool button = GUILayout.Button("OK"); //sets a boolean to the result of if the button is pushed
        GUILayout.EndArea(); //ends the drawing area again
        if (button) //when the button is pushed
		{
            //call this method to handle all the information that's been taken in
			CheckID();
		}
	}

    /// <summary>
    /// This method checks to see that the id that has been entered is valid. It makes sure
    /// that the length is correct. It validates that the first two characters are accepted
    /// letters and that the rest are numbers. Otherwise, the method will return, and the user
    /// must fix their id to continue.
    /// </summary>
    void CheckID() 
	{
		id = id.Trim(); //removes any weird whitespace
        string code = ""; //a string to hold the two letter code at the beginning of the id
        if (string.IsNullOrEmpty(id)) //if there's nothing in the box at all
        {
			return; //exit the method
		}

        //This is a try catch block. What it does, is run the code within the try, and if any errors are
        //thrown when that code is run, the error is handled in the catch block. In this case, if the user
        //enters something other than numbers for the last 5 digits, when the Convert line is called, an error
        //will be thrown, and the method will return. This will force the user to fix their mistake.
        try
        {
            code = "" + id[0] + id[1]; //sets code to the two letters
            string temp = ""; //takes a temporary string and sets it to the 
                              //remainder of the user id
            for (int i = 0; i < id.Length; i++)
            {
                if (i > 1)
                {
                    temp += id[i];
                }
            }
            idNum = Convert.ToInt32(temp); //this line converts that temporary string, which should hold only numbers
                                           //into an actual number
        } catch 
        {
            return; //exit the method
        }

        //if the first two characters are anything other than these approved letters...
        if ((id[1] != 'F' && id[1] != 'C' && id[1] != 'L') || (id[0] != 'V' && id[0] != 'M' && id[0] != 'P')) 
        {
            return; //...exit the method
        }

        //if the user id has made it this far, it has passed all of the tests ensuring its validity
        //thus the program can continue to create the data file and switch scenes
        HandlePath();
    }

	void HandlePath()
	{
        //this block reads the line holding the path from the text file
        //MAKE SURE YOU HAVE THE TEXT FILE
        //it's pretty important
        //the file should be in the same directory as the scene/build being run
        try
        {
            path = sr.ReadLine(); //reads the first line of the Path.txt data file
        }
        catch
        {
            return; //if there is any error reading the file, exit the method
                    //so if it can't find the file, this will get called
                    //so if the code is right, but nothing is happening, make sure you have 
                    //that data file
        }

		filePath = path+"\\"+id+".txt"; //sets the filePath variable to the path that has just
                                        //been read in and appends on user id as the name
                                        //of the text file
		File.Create(filePath); //creates a file at the location of filePath
        pathCode = "" + id[0] + id[1]; //sets the pathCode, now that everything has been validated
        SceneManager.LoadScene("Instructions"); //loads and runs the instructions scene
        //for ease of testing, the scene here can be changed to jump right to whatever you need
	
	}
}
