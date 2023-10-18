using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleVR : MonoBehaviour {
    string pathCode = promptID.pathCode;

    void Start () {
        if (pathCode[0] == 'V')
        {
            GameObject cam = GameObject.Find("First Person Controller - Resid Monitor");
            cam.SetActive(false);
            GameObject vrCam = GameObject.Find("OVRPlayerController");
            vrCam.SetActive(true);
        }
        else
        {
            GameObject cam = GameObject.Find("First Person Controller - Resid Monitor");
            cam.SetActive(true);
            GameObject vrCam = GameObject.Find("OVRPlayerController");
            vrCam.SetActive(false);
        }
    }
}
