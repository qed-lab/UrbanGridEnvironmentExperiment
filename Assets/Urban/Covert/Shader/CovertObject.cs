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
        #region Deliver the parameters to the shader
        Mat.SetVector("_CenterPosition", new Vector4(Pivot.transform.position.x, Pivot.transform.position.y, Pivot.transform.position.z, 0));
        Mat.SetFloat("_Radius", Radius);
        Mat.SetFloat("_Modulation", Carve.Evaluate(Time.time % 0.2f));
        #endregion


        #region Check to see if we need to show the cue
        
        if (EyeSightAngle() <= 10f)
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
