using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//should be attached to an empty GameObject in the Instructions scene

/// <summary>
/// This class is the entirety of the Instructions scene. It pops up immediately upon the start
/// of that scene. Its purpose is to display some text to the screen for the user, providing 
/// them with necessary information. In the case of VR, it also tells the user to put on the
/// VR headset.
/// </summary>
public class showText : MonoBehaviour {
    string vr = promptID.pathCode; //gets the two letter code from promptID so it knows if its being run in VR or not
    public static float startTime; //records the time that the demo world is started

    /// <summary>
    /// Initialization
    /// </summary>
    private void Start()
    {
        startTime = 0; //initializes startTime to 0
    }

    /// <summary>
    /// This is a built in Unity function. It means that once the screen has initialized and things 
    /// are ready to display, whatever is in this method is going to happen. It allows this to 
    /// display immediately upon the creation of the scene.
    /// </summary>
    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(); //creates a GUI style so the text style can be altered
        gs.alignment = TextAnchor.MiddleCenter; //set the alignment of the text to be centered
        gs.fontSize = 20; //makes the font size larger 
        GUI.contentColor = Color.black; //set the color of all GUI elements to black
        GUILayout.BeginArea(new Rect(0, 200, Screen.width, Screen.height)); //creates the area where GUI elements will be displayed
                                                                            //the new Rect creates a rectangle at the location of the first two paratmeters
                                                                            //and the size of the second two parameters
        //The label is what allows the text to be diaplayed to the screen. The GUI style is also passed to this function so that
        //all of the style changes set earlier will affect this text
        GUILayout.Label("You will now enter a preliminary virtual environment in order to get comfortable navigating,\n" +
            "using the controller, and becoming accustomed to the structure of the environment.\n" +
            "Ensure you are wearing your headphones. These are intended to block out exterior noise. \n" +
            "You will only need to use the labeled buttons on your controller (directional pad and right stick).\n" +
            "Follow the arrows throughout the demo world and once you have made your way to the end, \n" +
            "you will be directed into the actual novel environment.\n\n", gs);

        if (vr[0] == 'V') //if being run in VR
        {
            GUILayout.Label("Once you press OK, please put on the VR headset.", gs); //tell the user to put on the VR headset also
                                                                                     //also uses the GUI style for consistency and visibility
        }
        GUILayout.EndArea(); //tells the graphical system to stop 
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 50, 500, 100, 100)); //creates a new area for the button to be
        bool button = GUILayout.Button("OK"); //sets a boolean to the result of if the button is pushed
        GUILayout.EndArea(); //end the area again
        if (button) //when the button is pushed
        {
            startTime = Time.time; //set the start time of the demo
            SceneManager.LoadScene("DEMO_ve_01"); //load the demo world
        }
    }
}

