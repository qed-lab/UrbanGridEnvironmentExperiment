using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.XR;

public class FollowSpline : MonoBehaviour
{
    public bool AutoMove = true;
    public SplineContainer Path;
    public float MovingSpeed;
    public float CurrentProgress = 0;

    void Start()
    {
        StartCoroutine(MovingAlongPath(auto:AutoMove));
    }

    IEnumerator MovingAlongPath(bool auto = true)
    {
        YieldInstruction waitAFrame = new WaitForEndOfFrame();

        if (auto)
        {
            Vector3 oldPos, newPos;
            Vector3 forward = Vector3.forward;
            oldPos = newPos = transform.position;

            while (CurrentProgress <= 1)
            {
                transform.position = Path.EvaluatePosition(0, CurrentProgress);
                transform.rotation = Quaternion.LookRotation(Path.EvaluateTangent(0, CurrentProgress), Path.EvaluateUpVector(0, CurrentProgress));
                CurrentProgress += MovingSpeed * Time.deltaTime;
                yield return waitAFrame;
            }
        }
        else
        {
            while (true)
            {
                CurrentProgress = GetControllerValue();
                CurrentProgress %= 1;
                transform.position = Path.EvaluatePosition(0, CurrentProgress);
                transform.rotation = Quaternion.LookRotation(Path.EvaluateTangent(0, CurrentProgress), Path.EvaluateUpVector(0, CurrentProgress));
                yield return waitAFrame;
            }
            
        }
    }



    float GetControllerValue()
    {
        return Input.GetAxis("Vertical");
        float value;
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.GameController);
        Debug.Log("VinController: Valid: " + device.isValid);
        device.TryGetFeatureValue(CommonUsages.grip, out value);
        Debug.Log("VinController: Value: " + value);
        return value;
    }
}

