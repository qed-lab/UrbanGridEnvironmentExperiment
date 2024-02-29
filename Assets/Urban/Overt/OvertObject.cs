using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvertObject : MonoBehaviour
{
    /// <summary>
    /// The distance between the object and the cue
    /// </summary>
    [Range(0.0f, 10f)]
    public float Radius = 0.5f;
    /// <summary>
    /// The rotation of the cue
    /// </summary>
    public Quaternion CueRotation = Quaternion.identity;
    GameObject CueObj;

    private void Start()
    {
        #region Init the cue
        CueObj = Instantiate(OvertCueManager.Instance.OvertCuePrefab);
        CueObj.transform.position = transform.position + CueRotation * transform.forward * Radius;
        CueObj.transform.LookAt(transform.position);
        CueObj.transform.localScale *= OvertCueManager.Instance.CueSize;
        #endregion
    }

    void Update()
    {
        /*#region Check to see if we need to show the cue
        Vector3 tmpDir = (transform.position - Camera.main.transform.position).normalized;
        if (Vector3.Angle(tmpDir, EyeSight.Instance.Trans.forward) <= 3f)
        {
            CueObj.SetActive(false);
        }
        else
        {
            CueObj.SetActive(true);
        }
        #endregion*/
    }

    #region UGUI
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.DrawWireSphere(transform.position + CueRotation * transform.forward * Radius, Radius * 0.1f);
    }
    #endregion
}
