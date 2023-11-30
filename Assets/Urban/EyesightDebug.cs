using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesightDebug : MonoBehaviour
{
    public static EyesightDebug Instance;
    public Vector3 Dir;
    void Start()
    {
        Dir = Vector3.forward;
    }

    void Update()
    {
        //Get the world position of the eyes
        transform.position = EyeSight.Instance.EyePosition;
        //Get the world rotation of the eyes
        transform.rotation = EyeSight.Instance.EyeDirection;
        Debug.Log("EyeSight: Pos " + EyeSight.Instance.EyePosition);
        Debug.Log("EyeSight: Dir " + EyeSight.Instance.EyePosition);
    }
}
