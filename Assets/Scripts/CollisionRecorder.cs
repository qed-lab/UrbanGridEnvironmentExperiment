using System.Collections;
using UnityEngine;
using System;
using System.IO;

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

// This script records collisions between objects and the GazeRay and is attached to the latter

public class CollisionRecorder : MonoBehaviour
{
    [SerializeField] private string includeTag; // Use this to include only specifically tagged objects (Do not put the same tag on exclude and include - the exclude will override and it will not be included)
    [SerializeField] private string excludeTag; // Use this to include everything but this tag in the gaze data collection
    private ArrayList outList = new ArrayList();
    private string FULL_PATH = Directory.GetCurrentDirectory();
    private string dataScriptPath = @"\Assets\Scripts\PythonAnalyzer\GazeDataCruncher.py";
    private string file_path = @"Output\rayPosTest.csv";
    private int pID = 000;
    private string outputPath = "";
    private string phpURL = @"http://bioreg-9020.bluezone.usu.edu/spatial/gazeresult.php";

    private void Start()
    {
        // lots of directory finangling
        FULL_PATH = Directory.GetCurrentDirectory();
        Debug.Log(FULL_PATH);
        dataScriptPath = String.Concat(FULL_PATH, @"\Assets\Scripts\PythonAnalyzer\GazeDataCruncher.py");
        file_path = String.Concat(FULL_PATH, @"\defaultGazeData.csv");
        try
        {
            // this grabs the participant ID from promptID (see trackInfo.cs in "Transfer Stuff") - won't compile if no promptID script
            pID = promptID.idNum;
            outputPath = promptID.filePath;
        }
        catch (Exception ex)
        {
            Debug.Log("Error while getting participant ID or file path from promptID.cs\nException caught: " + ex.ToString());
        }
        Debug.Log("Participant ID: " + pID);
        file_path = String.Concat(FULL_PATH, @"\Output\" + pID + "GazeData.csv");

        try
        {
            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file_path));
            }
            var f = File.CreateText(file_path);
            f.WriteLine("time,exit,obj,angle,dist,HMDrot");
            f.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Tried to create output directory.\nException caught: " + ex.Message);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // timestamp it
        var now = System.DateTime.Now;
        //Debug.Log(other.tag + " " + includeTag); // uncomment for better debugging
        Debug.Log(other.gameObject.name);
        if (includeTag != null || includeTag != "") // checks if there is a tag to be including
        {
            if (other.tag == null || other.tag == "")
            {
                return; // untagged -> exlcude
            }
            if (other.tag != includeTag)
            {
                return; // not the right tag -> exclude
            }
        }
        if (excludeTag != null || excludeTag != "") // checks if this has a tag to be excluded
        {
            if (other.tag == excludeTag)
            {
                return; // has the exclude tag -> exclude
            }
        }
        // The angle is between the camera center (also the center of the screen) and the gaze ray
        string angle = Quaternion.Angle(Camera.main.transform.rotation, this.gameObject.transform.rotation).ToString();
        string dist = ((Camera.main.transform.position - other.transform.position).magnitude).ToString();
        Output(now, 0.ToString(), other.gameObject.name, angle, dist, Camera.main.transform.rotation.ToString());
    }

    private void OnTriggerExit(Collider other)
    { // much like OnTriggerEnter(); see the notes in there
        //Debug.Log(other.tag + " " + includeTag); // uncomment for better debugging
        if (includeTag != null || includeTag != "")
        {
            if (other.tag == null || other.tag == "")
            {
                return;
            }
            if (other.tag != includeTag)
            {
                return;
            }
        }
        var now = System.DateTime.Now;
        string angle = Quaternion.Angle(Camera.main.transform.rotation, this.gameObject.transform.rotation).ToString();
        string dist = ((Camera.main.transform.position - other.transform.position).magnitude).ToString();
        Output(now, 1.ToString(), other.gameObject.name, angle, dist, Camera.main.transform.rotation.ToString());
    }


    private void Output(DateTime now, string exit, string obj, string angle, string dist, string camRot)
    {
        string msg = now.Hour.ToString() + ":" + now.Minute.ToString() + ":" + now.Second.ToString() + ":" + now.Millisecond.ToString() + "," + //time: partial time stamp (HH:MM:SS:mmm), enough for millisecond precision, assuming not playing at midnight
                     exit + "," +                                                                                                               //exit: bool value, 1 for exit, 0 for enter
                     obj + "," +                                                                                                                //obj: the name of the object looked/looking at
                     angle.Replace(",", "") + "," +                                                                                             //angle: degrees from center of HMD that the gazeRay is
                     dist + "," +                                                                                                               //dist: distance between the center of the object and the position of the camera
                     camRot.Replace(",", "");                                                                                                   //HMDrot: quaternion of HMD rotation in world
        Debug.Log("Adding to outList: " + msg);
        outList.Add(msg);
    }

    private void OnDestroy()
    {
        Debug.Log("Adding game-end timestamp...");
        var now = System.DateTime.Now;
        Output(now, "-1", "", "", "", "");

        Debug.Log("Attempting disk write to " + file_path);
        try
        {
            writeCSV(file_path, outList);
            // only continue if the CSV writing was successful, either to the participant specific location or the back-up; otherwise if there was an error, the corresponding catch block will print the details
            try
            {
                Debug.Log("Looking for Python path.");
                // call the Python script in headless mode from the commandline
                System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
                // grab python3 path - exception thrown if no 3 installed or if version could not be parsed, warning if no 3.6+ installed
                string PyPath = getPython3Path();   //pro-tip: depending on your IDE, ctrl-clicking or ctrl-b on something will take you to its definition
                if (PyPath == "") throw new FileNotFoundException("No Python 3 installed in Program Files.GazeDataCruncher requires v3.6+. Data still available at " + file_path);

                // use python console (at PyPath) to run script with the headless tag to get formatted output
                string args = string.Format("\"{0}\" -hl \"{1}\"", dataScriptPath, file_path);
                start.Arguments = args;
                start.FileName = PyPath;
                Debug.Log("Calling " + dataScriptPath + " for " + file_path + " with arguments " + args);
                //start.CreateNoWindow = true;  // NOTE: uncomment this line if you don't want the console window to pop up momentarily
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;

                // get the output of GazeDataCruncher.py and stick it in result
                string result = "";
                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
                {
                    process.EnableRaisingEvents = true;
                    process.Start();
                    process.WaitForExit();
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                    }
                    Debug.Log("Finished with exit code " + process.ExitCode);   // 0 is the exit code you're looking for; otherwise, it encountered an error while running
                }
                Debug.Log(result);

                // generate the url and send it out; but only if no errors were caught in calling and getting the output from GazeDataCruncher.py (hence the nesting try-catch blocks)
                try
                {
                    sendURL(result);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Error while generating URL. Possibly bad Python output.\nException caught: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Error while calling Python script.\nException caught: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Error in writing to disk at default location.\nException caught: " + ex.Message);
        }
    }


    private void writeCSV(string path, ArrayList list)
    {
        try
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path, true);
            foreach (string line in list) writer.WriteLine(line);
            writer.Close();
            Debug.Log("Writing done.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Error writing to " + file_path + "\n(Exception caught: " + ex.Message + ")\n\nAttempting write to backup location...");
            System.IO.StreamWriter writer = new System.IO.StreamWriter(FULL_PATH, true);
            foreach (string line in list) writer.WriteLine(line);
            writer.Close();
            Debug.Log("Backup write successful.");
        }
    }


    private string getPython3Path()
    {
        string PyPath = "";
        foreach (string dir in Directory.GetDirectories(@"C:\Program Files\"))
        {
            if (dir.Contains("Python3"))
            {
                int v = 0;  // v for Python version
                if (Int32.TryParse(dir.Split('3')[1], out v))
                {
                    if (v < 6)
                    {
                        Debug.LogWarning("Warning: Python version less than 3.6.");
                    }
                }
                else
                {
                    throw new FileNotFoundException("Found Python installation folder \"" + dir + "\" but could not parse Python version.");
                }
                PyPath = dir + @"\python.exe";
                Debug.Log("Python3 installation found at " + PyPath);
                break;
            }
        }
        return PyPath;
    }


    private void sendURL(string result)
    {
        // wrap the results into a nice url which calls a php script that throws it all into the database
        string[] results = result.Split('\n');
        int tot_index = results.Length - 2;
        string[] totals = results[tot_index].Split(':');
        string tlt = totals[1].Split(',')[0];
        string tl = totals[1].Split(',')[1];
        string tmi = totals[2].Split(',')[0];
        string tma = totals[2].Split(',')[1];
        string ta = totals[3].Split(',')[0];
        string al = totals[3].Split(',')[1];
        string ts = totals[4];
        string tv = totals[5];

        string url = phpURL +
            "?pid=" + pID +  //participant ID
            "&tlt=" + tlt +  //total look time
            "&tl=" + tl +    //total looks
            "&tmi=" + tmi +  //total min 
            "&tma=" + tma +  //total max
            "&ta=" + ta +    //total average look
            "&al=" + al +    //total average look by landmarks
            "&ts=" + ts +    //total standard deviation
            "&tv=" + tv +    //total variance
            "&lmks=";        //list of landmark data
        foreach (string line in results)
        {
            // just send the raw output strings for each landmark - gazeresult.php will handle the parsing of landmarks
            if (line == result[tot_index].ToString() || line.Contains("Total")) continue;   // skip the totals line
            if (line == "" || line is null) continue;   // skip blank lines
            if (line == results[tot_index - 1]) url += line;    // don't add a delimiter to the last line
            else url += line + "__"; // dunder ("__") delimiter to separate the landmarks in the url
        }
        Debug.Log("Opening URL " + url);
        try
        {
            Application.OpenURL(url);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Error while opening php url\nException caught: " + ex.Message);
        }
    }
}
