using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovertObject : MonoBehaviour
{
    public Material Mat;
    public Transform Pivot;
    public float Radius;
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
        Mat.SetVector("_CenterPosition", new Vector4(Pivot.transform.position.x, Pivot.transform.position.y, Pivot.transform.position.z, 0));
        Mat.SetFloat("_Radius", Radius);
        Mat.SetFloat("_Modulation", Carve.Evaluate(Time.time % 0.2f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (!Pivot)
        {
            Pivot = transform;
        }
        Gizmos.DrawWireSphere(Pivot.transform.position, Radius);
    }
}
