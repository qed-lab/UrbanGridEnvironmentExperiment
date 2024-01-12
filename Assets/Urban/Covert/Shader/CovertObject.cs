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
    /// <summary>
    /// The frequency of the covert shader effect
    /// </summary>
    public AnimationCurve Carve;
    void Start()
    {
        if (!Pivot)
        {
            Pivot = transform;
        }
    }


    void Update()
    {
        #region Deliver the parameters to the shader
        Mat.SetVector("_CenterPosition", new Vector4(Pivot.transform.position.x, Pivot.transform.position.y, Pivot.transform.position.z, 0));
        Mat.SetFloat("_Radius", Radius);
        Mat.SetFloat("_Modulation", Carve.Evaluate(Time.time % 0.2f));
        #endregion
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
