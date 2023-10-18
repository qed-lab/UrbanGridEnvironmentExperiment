using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altpathornot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] alt = GameObject.FindGameObjectsWithTag("Alt");
        foreach (GameObject x in alt)
        {
            Destroy(x);
        }
    }
// Update is called once per frame
void Update()
    {
        
    }
}
