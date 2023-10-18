using System;
using System.Collections;
using UnityEngine;

public class FollowSimple : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private Vector3 TempPoint = Vector3.zero;

    private float tParam;

    private Vector3 objectPosition;

    private float speedModifier;

    private bool coroutineAllowed;

    private Quaternion holdRotation;

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 7800.0f;
        coroutineAllowed = true;
    }
    public float dist(Vector3 A, Vector3 B)
    {
        return (float)Math.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y) + (A.z - B.z) * (A.z - B.z));
    }
    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }


    private IEnumerator GoByTheRoute(int routeNum)
    {
        Vector3 p0;
        coroutineAllowed = false;
        if (TempPoint == Vector3.zero)
        {
            p0 = routes[routeNum].GetChild(0).position;
        }
        else
        {
            p0 = TempPoint;
        }
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        
        transform.rotation = holdRotation;
        transform.position = p0;
        while (true)
        {
 
            transform.LookAt(p1);
            transform.position += transform.forward * Time.deltaTime*speedModifier/dist(p0,p1);

            //float d = dist(p0, p1);
            //transform.LookAt(p1);
            //objectPosition = tParam * (p1-p0);
            //transform.position = objectPosition;

            //tParam += Time.deltaTime * speedModifier / d;
            //temp = tParam;
            //temp += Time.deltaTime * speedModifier / d;

            ////objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1);
            //Vector3 nextPos = temp * (p1 - p0);

            //////print(objectPosition + "hi");
            //////Derivative of position: Vector3 objectRotation = 3 * Mathf.Pow(1 - tParam, 2) * (p1 - p0) + 6 * (1 - tParam) * tParam * (p2 - p1) + 3 * Mathf.Pow(tParam, 2) * (p3 - p2);

            ////transform.position = objectPosition;
            //////transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextPos.normalized), Time.deltaTime * .01f);
            //transform.LookAt(nextPos);

            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;

    }
}
