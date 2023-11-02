using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentOptimizer : MonoBehaviour
{
    public static EnvironmentOptimizer Instance;
    public GameObject TargetEnvironment;
    public GameObject TargetTarrainParent;
    public List<Terrain> TargetTerrains;
    public List<Vector3> TargetTerrainCenters;
    private void OnEnable()
    {
        Instance = this;
        GetTerrainList();
        AddScriptToAllChriden<OptimizObject>(TargetEnvironment.transform);
    }


    void GetTerrainList()
    {
        Terrain tmp;
        for (int i = 0; i < TargetTarrainParent.transform.childCount; i++)
        {
            tmp = TargetTarrainParent.transform.GetChild(i).GetComponent<Terrain>();
            if (tmp)
            {
                TargetTerrains.Add(tmp);
                TargetTerrainCenters.Add(tmp.transform.position + Vector3.left * 75 + Vector3.forward * 75);
            }
        }
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < TargetTerrains.Count; i++) 
        {
            if (Vector3.Distance(TargetTerrainCenters[i], Camera.main.transform.position) <= 400f &&Vector3.Angle(TargetTerrainCenters[i] - Camera.main.transform.position, Camera.main.transform.forward) < 120f)
            {
                if (!TargetTerrains[i].gameObject.activeSelf)
                {
                    TargetTerrains[i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (TargetTerrains[i].gameObject.activeSelf)
                {
                    TargetTerrains[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void AddScriptToAllChriden<T>(Transform target) where T : Component
    {
        if(target.GetComponent<MeshRenderer>())
        {
            target.gameObject.AddComponent<T>();
        }
        for(int i = 0; i < target.childCount; i++)
        {
            AddScriptToAllChriden<T>(target.GetChild(i));
        }
    }
}
