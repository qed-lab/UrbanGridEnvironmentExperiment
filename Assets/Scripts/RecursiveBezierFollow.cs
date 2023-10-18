using System;
using System.Collections;
using UnityEngine;

public class RecursiveBezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;

    private Vector3 nextPos = Vector3.zero;

    private Vector3[] pointPos;

    private float tParam;

    private Vector3 objectPosition;

    private float speedModifier;

    // Start is called before the first frame update
    void Start()
    {
        print("hello world1");
        Vector3[] temp = new Vector3[points.Length];
        for (int x = 0; x<points.Length; x++)
        {
            temp[x] = points[x].position;
        }
        pointPos = temp;
        tParam = 0f;
        speedModifier = 0.1f;
    }
    public float dist(Vector3 A, Vector3 B)
    {
        return (float)Math.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y) + (A.z - B.z) * (A.z - B.z));
    }
    // Update is called once per frame
    void Update()
    {
        print("blink");
        Move();
    }
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

    private void Move()
    {
        print("here" + tParam);
        float temp;
        
        print("hello world");
        tParam += Time.deltaTime * speedModifier;
        temp = tParam + Time.deltaTime * speedModifier;
        if (tParam < 1) objectPosition = Recursive(pointPos, tParam);
        if (temp < 1) nextPos = Recursive(pointPos, temp);

        transform.position = objectPosition;
        if (tParam < 1) transform.LookAt(nextPos);

        
    }
}
