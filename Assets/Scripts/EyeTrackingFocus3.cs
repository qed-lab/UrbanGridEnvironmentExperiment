using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.Essence.Eye;

public class EyeTrackingFocus3 : MonoBehaviour
{

    void Start()
    {
        if (EyeManager.Instance != null)
        {
            EyeManager.Instance.EnableEyeTracking = true;
            EyeManager.Instance.GetEyeTrackingStatus();
        }

    }


    void Update()
    {
        if (EyeManager.Instance != null && EyeManager.Instance.IsEyeTrackingAvailable())
        {

            Vector3 combinedEyeOrigin;
            Vector3 combinedEyeDirection;
            bool hasCombinedEyeOrigin = EyeManager.Instance.GetCombinedEyeOrigin(out combinedEyeOrigin);
            bool hasCombinedEyeDirection = EyeManager.Instance.GetCombindedEyeDirectionNormalized(out combinedEyeDirection);

            if (hasCombinedEyeOrigin && hasCombinedEyeDirection)
            {
                Debug.Log("Combined Eye Origin: " + combinedEyeOrigin);
                Debug.Log("Combined Eye Direction: " + combinedEyeDirection);
            }
        }
    }
}
