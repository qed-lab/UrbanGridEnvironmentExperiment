using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrawGlow : MonoBehaviour
{
    //public Material Mat;
    public AnimationCurve ScaleCarve;
    //public AnimationCurve TransparentCarve;
    Vector3 OriginalScale = Vector3.one;
    private void OnEnable()
    {
        OriginalScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = ScaleCarve.Evaluate(Time.time % 1) * OriginalScale;
        //transform.localScale = ScaleCarve.Evaluate(Time.time % 1) * OriginalScale;
    }
}
