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
        transform.position = Camera.main.transform.position;    
        transform.rotation = Quaternion.LookRotation(Dir, Camera.main.transform.up);
    }
}
