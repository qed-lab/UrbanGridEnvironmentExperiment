using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyesight : MonoBehaviour
{
    public static PlayerEyesight Instance;
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
        transform.localRotation = EyeSight.Instance.EyeDirection;
    }
}
