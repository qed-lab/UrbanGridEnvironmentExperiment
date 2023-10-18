//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
// Some modifications by C. N. Spencer for USU LAEP VIVID Lab Spatial Memory Gist studies
/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class GazeLine : MonoBehaviour
            {
                [SerializeField] private LineRenderer GazeRayRenderer;
                [SerializeField] public int LengthOfRay;
                private static EyeData eyeData = new EyeData();
                private bool eye_callback_registered = false;
                private void Start()
                {
                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    Assert.IsNotNull(GazeRayRenderer);
                }

                private void Update()
                {
                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

                    if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
                    {
                        SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                        eye_callback_registered = true;
                    }
                    else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
                    {
                        SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }

                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;

                    if (eye_callback_registered)
                    {
                        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else return;
                    }
                    else
                    {
                        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else return;
                    }

                    // Spencer's modifications for a box collider & rigidbody
                    BoxCollider gazeCollider = GazeRayRenderer.GetComponent<BoxCollider>();
                    Rigidbody gazeRigidbody = GazeRayRenderer.GetComponent<Rigidbody>();

                    Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
                    Vector3 CamPos = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
                    Vector3 GazePos = Camera.main.transform.position + GazeDirectionCombined * LengthOfRay;
                    //Vector3 CamCenter = Camera.main.transform.position * LengthOfRay;

                    GazeRayRenderer.SetPosition(0, CamPos);
                    GazeRayRenderer.SetPosition(1, GazePos);

                    if (!gazeRigidbody.isKinematic)
                    {
                        Debug.Log("Warning: Rigidbody on GazeRayRenderer is not kinematic. (Script-based movement unadvised)");
                    }
                    gazeCollider.transform.position = CamPos;
                    gazeCollider.transform.LookAt(Camera.main.transform.position + GazeDirectionCombined * LengthOfRay);
                    gazeRigidbody.MovePosition(CamPos);
                    gazeRigidbody.transform.LookAt(Camera.main.transform.position + GazeDirectionCombined * LengthOfRay);
                    // **End modifications***
                }
                private void Release() {
                    if (eye_callback_registered == true)
                    {
                        SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }
                }
                private static void EyeCallback(ref EyeData eye_data)
                {
                    eyeData = eye_data;
                }
            }
        }
    }
}
