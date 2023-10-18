
SET UP INSTRUCTIONS FOR ADDING VIVE EYE GAZE TRACKING TO YOUR UNITY PROJECT

1. Import SteamVR (either through the Unity Asset Store or the package steamvr_2_7_3.unitypackage). At the dialog, choose UnityXR. You can accept all on the SteamVR window (Valve.VR.SteamVR_UnitySettingsWindow).

2. Go to GameObject>XR>Convert Main Camera... or right-click on the main Camera and choose the same option. Your camera must be the only MainCamera (i.e. tagged as such) in the scene and at the base of the hierarchy.

3. Import ViveSR (Vive-SRanipal-Unity-Plugin.unitypackage).

4. Add the eye framework prefab to the scene (ViveSR>Prefabs>SRanipal Eye Framework.prefab).

5. Import GazeTracker (GazeTrackervX.X.unitypackage).

6. Add the Line prefab to the scene (GazeTracker>Prefabs>Line.prefab).


INSTRUCTIONS FOR UPDATING GAZE TRACKER

If you have followed the above instructions for the GazeTracker zipped package v2.1 or later, then follow the step(s) here:
1. Simply import the newer GazeTracker.unitypackage package.

If you are updating from v1.3, v1.4, or v2.0:
1. With the project closed, go into the project files and delete PythonAnalyzer folder, GazeLine.cs, CollisionRecorder.cs in Scripts, Line.prefab, and LandmarkTagCube.prefab in Prefabs, and their respective .metas.

2. Launch the project and import the new package version. (Doing this before following step 1. will update the files where they are rather than the new structure. Any of the affected prefabs (i.e. Line) in the scene will appear as missing, but will be corrected once the new package is imported.)

If you are updating from v1.0, v1.1, or v1.2:
1. Follow step 1. in the instructions for updating from v1.3, v1.4, or v2.0, but only for Line.prefab.

2. Follow step 2. in the instructions for updating from v1.3, v1.4, or v2.0.


COMMON ERRORS & TIPS

The GazeTracker package was designed to work with the existing VIVID Urban VE world game flow. A compiling error arises when there isn't that framework (error CS0103: The name 'promptID' does not exist in the current context). To fix, modify the CollisionRecorder.cs script. The lines 30-31 can be changed to reflect whatever participant ID and output path you need.

Note that data output files generated with previous package versions may or may not have the same format and as such may be incompatible with newer versions of the PythonAnalyzer.

If the same participant ID is entered, the previous data file and certain database fields will be overwritten (no entries in the Landmarks table will be altered due to the UID rules). Proceed with caution when reusing participant IDs.

If you get an error about the window layout, go to Window>Layouts>Revert Factory Settings....

The object overlap implementation assumes that the center of an object that is further away than another object is not the focus. This will not be correct if a smaller object which is tagged for inclusion or is otherwise included whose center is within a larger object, only the unseeable smaller object inside will be recorded because its center is closer than the larger object's.

Do not use dunders ("__") in GameObject names. You would get funky SQL/PHP errors flying left and right if you did.

When launching the game from the Unity Editor, if you get an actions manifest dialog, go to Window>SteamVR Input and say yes to the pop-up.

While testing in the Unity Editor, if the headset fails to display the game once playing, be sure to start SteamVR and SRanipalRuntime (the robot icon) before loading the project.

If you get an error about the actions manifest not being found, go to Window>SteamVR Input and select the option to use the example JSON.

If you start the game in the Unity Editor and it doesn't show up or doesn't say it's loading on the headset, enable 360 Stereo Capture in Edit>Project Settings>Player>XR.

If you get an error that says something like "Assets\SteamVR\WindowsHelper\SteamVR_Windows_Editor_Helper.cs(24,36): error CS1069: The type name 'RegistryKey' could not be found in the namespace 'Microsoft.Win32'. This type has been forwarded to assembly 'Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' Consider adding a reference to that assembly.", then go to Edit>Project Settings>Player>Other Options>Configuration>Api Compatibility Level and change .NET Standard 2.0 to .NET 4.x.

Some issues arise from the Plug-in Manager. Install XR Plugin Management from the Assets>Package Manager. Or alternatively, go to Project Settings>XR Plug-in Management and click the option to install.

If no collisions are being registered or not behaving as expected, go to Edit>Project Settings>Physics>Contacts Pair Mode and set it to "Enable All Contacts".

If it's picking up more objects than you're actually seeing, it's probably due to the length of the gaze ray. Change the field under Gaze Line (Script)>Length of Ray.

For the gaze ray to record only landmarks, tag those objects/prefabs with a unique tag and set the field include Tag on Line>Collision Recorder to that same string. Leaving include Tag blank will count all objects. Note that any object you want tracked will need a collider (these are under Add Component>Physics on the object).

If Python 3.6+ is not installed on the computer for all users (i.e. at C:\Program Files\Python3*\python.exe), install python. You will need administrator permission to do so. This is for the GazeDataCruncher.py script that is called on game end by CollisionRecorder.cs.

Be sure to calibrate for each person before starting the world. (If a participant goes twice in a row, he or she does not need to recalibrate; it's only needed between different users.) NOTE: it is much more accurate without glasses on. I'd advise that, if able, participants remove their glasses before donning the headset.


Detailed descriptions of .zip contents:
Included in GazeTracker.unitypackage:
> Line.prefab (Prefabs folder)
	* A simple invisible LineRenderer with a kinematic Rigidbody, trigger BoxCollider, and 2 major scripts which can be found in the Scripts folder:
	- GazeLine.cs: the driver that moves the Gaze Ray Renderer and its collider and Rigidbody to the gaze position. Modified from the SDK's SRanipal_GazeRaySample.cs script. If the field in the Editor isn't populated, set it to the Line.
	- CollisionRecorder.cs: records each trigger collision on landmarks in outList, which writes to [GameProjectDir]/Output/[partID]GazeData.csv on game end. It records the timestamp, exit or entering value, the collided object, the angle from the camera's center, the camera's position, and the camera's rotation. It then calls GazeDataCruncher.py to analyze that data file. In order to call GazeDataCruncher.py, Python 3.6+ has to be installed for all users (i.e. installed at C:\Program Files\Python3*\python.exe). It will run with a warning for any earlier Python3 version.
> LandmarkTagCube.prefab (Prefabs folder)
	* Just a cube with the Landmark tag for easy tagging of other object (since creating new tags is a pain).
	* To use the tag, you may have to add one of these to the scene. It can then be deleted after assigning the Landmark tag to another object.
> PythonAnalyzer code (Scripts folder)
	* Contains scripts for the analytics cruncher framework:
	* GazeDataCruncher.py
	 > Processes the Gaze Data into time durations and saves the simplified data per object
	 > Provides an interactive menu for analyzing the data from a specified GazeData.csv file or can be run in headless mode (such as when called from CollisionRecorder.cs)
	 > NOTE: This script requires at least Python v3.6 to run properly (mainly due to f-strings)
	* Landmark.py
	 > A class that holds the times of looks on and looks off of an object in lists "looks" and "offs", respectively.
	 > Saves the name of the object in Unity in string data member "name".
	* TotalLook.py
	 > FUNCTION = "Total Looks & Durations"
	 > DESCRIPTION = "Sums the total time spent looking and the number of glances at each landmark"
	 > total_look(lmks, hl): If not headless (hl=False), simply prints the report of each object's total look time and number of looks as well as the total (i.e. sums of all object looks, times) as given from the list lmks. If headless, returns a list of the landmarks' totals (in the form of tuples (name, total)) and a tuple of the total time and looks.
	* MinMax.py
	 > FUNCTION = "Min/Max Looks"
	 > DESCRIPTION = "Gives the shortest and longest looks of all and each landmarks"
	 > min_max(lmks, hl): If not headless (hl=False), simply prints the report of each object's shortest and longest look times as well as the overall shortest and longest look times as given from the list lmks. If headless, returns a list of the landmarks' shortest and longest looks (in the form of tuples (name, min, max)) and a tuple of the overall shortest and longest.
	* AvgLook.py
	 > FUNCTION = "Average Look Durations"
	 > DESCRIPTION = "Averages the time spent looking at each landmark and all landmarks"
	 > avg_look(lmks, hl): If not headless (hl=False), simply prints the report of each object's average look time as well as the overall averages divided by total number of looks and number of landmarks as given from the list lmks. If headless, returns a list of the landmarks' averages (in the form of tuples (name, average)) and a tuple of the average time by total looks and by number of landmarks.
	* StdDev.py
	 > FUNCTION = "Standard Deviations"
	 > DESCRIPTION = "The standard deviations of each landmark's glances and all landmarks"
	 > std_dev(lmks, hl): If not headless (hl=False), simply prints the report of each object's standard deviation of looks as well as the standard deviation of all looks as given from the list lmks. If headless, returns a list of the landmarks' standard deviations (in the form of tuples (name, standard deviation)) and the standard deviation of all looks.
	* Var.py
	 > FUNCTION = "Variances"
	 > DESCRIPTION = "The variance of each landmark's glances and all landmarks"
	 > var(lmks, hl): If not headless (hl=False), simply prints the report of each object's variance of looks as well as the variance of all looks as given from the list lmks. If headless, returns a list of the landmarks' variances (in the form of tuples (name, variance)) and the variance of all looks.

Vive-SRanipal-Unity-Plugin.unitypackage
> HTC's Unity plugin. The full SDK for lip and eye tracking headsets as well as the installer and documentation can be found here: https://hub.vive.com/en-US/download

steamvr_2_7_3.unitypackage
> SteamVR for Vive. Documentation can be found here: https://valvesoftware.github.io/steamvr_unity_plugin/api/index.html


*** BUGS & OTHER ISSUES TO CHARISSE.SPENCER@USU.EDU

v2.4 Changelog
> Fixed a bug with the writing of the game-end timestamp
> Changed output format to consolidate camera and object positions into a single distance between
> Changed GazeRay's default position from 0,0,0 to reduce accidental collisions at start of game

v2.3 Changelog
> Expanded update instructions for more version specific updating
> Added headers that include the package version on all scripts for better identification (for updating, etc.)
> Put a copy of this README.txt into GazeTracker.unitypackage

v2.2 Changelog
> Added an Exclude Tag for big projects that only have a few items that are not to be recorded
> Redid make_objs in GazeDataCruncher.py for object overlap detection & added the relevant object position data collection to CollisionRecorder
> Noted the error in center detection for overlapping/enveloped objects
> Elevated some debug logging to warnings for exceptions and other errors encountered in CollisionRecorder.cs
> Added instructions for updating from older versions of GazeTracker.unitypackage

v2.1 Changelog
> Refactored GazeTracker.unitypackage for a single folder at the root of the project
> Added clarity in set-up instructions

v2.0 Changelog
> Changed package format to GazeTracker.unitypackage
> Rewrote instructions to reflect ^
> Added common errors section
> Consolidated Gaze Ray Length to a single field of the same name
> Parser skipping error eliminated from GazeDataCruncher.py & made other functions more robust
> Simplified output of GazeDataCruncher.py
> CollisionRecorder.cs now sends the data to a php script on the Linux server to be added to the database

v1.4 Changelog
> Added clarity to instructions & tested with a new environment to make sure everything functions and loads as it should
> Added action.json error workaround in 1

v1.3 Changelog
> Improved output recorder
	* Excluded objects without a specific tag, or leave blank for all gameObjects with colliders
	* Analyzer script call on game end
> Added step 7. to add a tag to applicable objects/prefabs for exclusionary collision recording
> Added a rudimentary GazeDataCruncher.py script (some bugs in main parser) along with support files in "PythonAnalyzer" folder
> Added step 8. to install Python and place PythonAnalyzer folder in project directory.
> Included the Line cs scripts & detailed their use if the Line prefab doesn't load them

v1.2 Changelog
> Improved output recorder
	* Added angle for ad-hoc accuracy testing
	* Provided detailed descriptions for .zip contents

v1.1 Changelog
> Improved output recorder
	* Changed format to csv
	* Put off the disk writes until the game end

v1.0 Changelog
> First commit
	* Added Line.prefab with modified GazeRaySample script (GazeLine) and primitive CollisionRecorder script (to be improved) to .zip
	* Added SRanipal SDK Unity plugin to .zip
	* Wrote README with set-up instructions