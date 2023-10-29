using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class FollowSimple : MonoBehaviour
{
    public SplineContainer Path;
    public float MovingSpeed;
    public float CurrentProgress = 0;

    void Start()
    {
        StartCoroutine(MovingAlongPath());
    }

    IEnumerator MovingAlongPath()
    {
        YieldInstruction waitAFrame = new WaitForEndOfFrame();
        Vector3 oldPos, newPos;
        Vector3 forward = Vector3.forward;
        oldPos = newPos = transform.position;

        while (CurrentProgress <= 1)
        {
            transform.position = Path.EvaluatePosition(0, CurrentProgress);
            transform.rotation = Quaternion.LookRotation(Path.EvaluateTangent(0, CurrentProgress), Path.EvaluateUpVector(0, CurrentProgress));
            CurrentProgress += MovingSpeed * Time.deltaTime;
            //CurrentProgress = CurrentProgress % 1;
            yield return waitAFrame;
        }
    }
}

