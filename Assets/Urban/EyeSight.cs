using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.Essence.Eye;

[RequireComponent(typeof(EyeManager))]
public class EyeSight : MonoBehaviour
{
    public static EyeSight Instance;
    public Quaternion EyeDirection = Quaternion.identity;
    public Vector3 EyePosition = Vector3.zero;
    Vector3 EyeSightForward = Vector3.zero;

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
        EyeManager.Instance.GetCombinedEyeOrigin(out EyePosition);
        EyeDirection = Quaternion.LookRotation(EyeSightForward, Camera.main.transform.up);
    }
}