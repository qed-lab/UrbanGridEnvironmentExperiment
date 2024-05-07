using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovertObject : MonoBehaviour
{
    /// <summary>
    /// The material of the cue
    /// </summary>
    public Material Mat;
    /// <summary>
    /// The desired center of the object
    /// </summary>
    public Transform Pivot;
    /// <summary>
    /// The radious of the covert shader effect
    /// </summary>
    public float Radius;
    public float originalRadius;
    /// <summary>
    /// The frequency of the covert shader effect
    /// </summary>
    public AnimationCurve Carve;
    public bool NeedCue = true;

    private LinkedList<float> eyeSightAngleList = new LinkedList<float>();
    private int capacity = 5;
    private float sum = 0.0f;
    private float runningEyeSightAngleAvg = 0.0f;

    GameObject CueObj;

    void Start()
    {
        if (!Pivot)
        {
            Pivot = transform;
        }
        originalRadius = Radius;
    }


    void Update()
    {
        if (!NeedCue)
        {
            return;
        }
        #region Deliver the parameters to the shader
        Mat.SetVector("_CenterPosition", new Vector4(Pivot.transform.position.x, Pivot.transform.position.y, Pivot.transform.position.z, 0));
        Mat.SetFloat("_Radius", Radius);
        Mat.SetFloat("_Modulation", Carve.Evaluate(Time.time % 0.2f));
        #endregion


        #region Check to see if we need to show the cue

        UpdateLinkedList(EyeSightAngle());
        runningEyeSightAngleAvg = GetRunningAverage();

        if (runningEyeSightAngleAvg <= 15f)
        {
            Radius = 0;
            Debug.Log("Eye sight within range");
        }
        else
        {
            Radius = originalRadius;
        }
        #endregion
    }

    public float EyeSightAngle()
    {
        Vector3 tmpDir = (transform.position - Camera.main.transform.position).normalized;
        return Vector3.Angle(tmpDir, EyeSight.Instance.Trans.forward);
    }

    public void UpdateLinkedList(float value)
    {
        if (eyeSightAngleList.Count == capacity)
        {
            sum -= eyeSightAngleList.Last.Value;
            eyeSightAngleList.RemoveLast();
        }

        eyeSightAngleList.AddFirst(value);
        sum += value;
    }

    public float GetRunningAverage()
    {
        if (eyeSightAngleList.Count == 0)
        {
            Debug.Log("Error: no value in linkedlist!");
        }
        return sum / eyeSightAngleList.Count;
    }

    #region UGUI
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (!Pivot)
        {
            Pivot = transform;
        }
        Gizmos.DrawWireSphere(Pivot.transform.position, Radius);
    }
    #endregion
}
