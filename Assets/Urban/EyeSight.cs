using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.Essence.Eye;

[RequireComponent(typeof(EyeManager))]
public class EyeSight : MonoBehaviour
{
    public static EyeSight Instance;
    /// <summary>
    /// The world space rotation of the eyesight
    /// </summary>
    public Quaternion EyeDirection = Quaternion.identity;
    /// <summary>
    /// The world space position of the eyesight
    /// </summary>
    public Vector3 EyePosition = Vector3.zero;
    Vector3 EyeSightForward = Vector3.zero;
    public Transform Trans;

    private void OnEnable()
    {
        if (EyeManager.Instance != null)
        {
            EyeManager.Instance.EnableEyeTracking = true;
        }
        Instance = this;
    }

    void Update()
    {
        EyeManager.Instance.GetCombindedEyeDirectionNormalized(out EyeSightForward);
        EyePosition = Camera.main.transform.position;
        EyeDirection = Quaternion.LookRotation(EyeSightForward, Camera.main.transform.up);
        Debug.Log(" Eyesight " + EyeDirection);
        //Trans.rotation = EyeDirection;
    }
}