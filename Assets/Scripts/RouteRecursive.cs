using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RouteRecursive : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;

    private Vector3[] cpPos;

    private Vector3 gizmosPosition;

    private void OnDrawGizmos()
    {
        Vector3[] temp = new Vector3[controlPoints.Length];
        for (int x = 0; x < controlPoints.Length; x++)
        {
            temp[x] = controlPoints[x].position;
        }
        cpPos = temp;

        //float step = dist(controlPoints[0].position, controlPoints[3].position);

        for (float t = 0; t <= 1; t += .005f)
        {
            gizmosPosition = Recursive(cpPos, t);

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }


    }
    //public float dist(Vector3 A, Vector3 B)
    //{
    //    return (float)Math.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y) + (A.z - B.z) * (A.z - B.z));
    //}

    public static Vector3 Recursive(Vector3[] p, float t)
    {
        Vector3 bt = p[0];

        if (p.Length > 1)
        {
            Vector3[] p1pn = new Vector3[p.Length - 1];
            Array.Copy(p, 1, p1pn, 0, p.Length - 1);
            // The following should be like this but skipped for optimization
            // Vector2[] p0pnMinus1 = Array.Resize(ref p, p.Length - 1);
            // bt = (1 - t)*Recursive(p0pnMinus1 , t) + t*Recursive(p1pn, t);
            Array.Resize(ref p, p.Length - 1);
            bt = (1 - t) * Recursive(p, t) + t * Recursive(p1pn, t);
        }

        return bt;
    }
}