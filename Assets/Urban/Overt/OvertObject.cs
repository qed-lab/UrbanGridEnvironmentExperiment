using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvertObject : MonoBehaviour
{
    [Range(0.0f, 10f)]
    public float Radius = 0.5f;
    public Quaternion CueRotation = Quaternion.identity;

    private void OnEnable()
    {

    }
    private void Start()
    {
        GameObject tmpGObj = Instantiate(OvertCueManager.Instance.OvertCuePrefab);
        tmpGObj.transform.position = transform.position + CueRotation * transform.forward * Radius;
        tmpGObj.transform.LookAt(transform.position);
        tmpGObj.transform.localScale *= OvertCueManager.Instance.CueSize;
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.DrawWireSphere(transform.position + CueRotation * transform.forward * Radius, Radius * 0.1f);
    }
}
