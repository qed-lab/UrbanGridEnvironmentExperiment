using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizObject : MonoBehaviour
{ 

    void Start()
    {
        RegisiterForATerrain();
    }

    void RegisiterForATerrain() 
    {
        int belongingTerrainIndex = 0;

        for (int i = 0; i < EnvironmentOptimizer.Instance.TargetTerrains.Count; i++)
        {
            if(Vector3.Distance(EnvironmentOptimizer.Instance.TargetTerrainCenters[belongingTerrainIndex], transform.position) > Vector3.Distance(EnvironmentOptimizer.Instance.TargetTerrainCenters[i], transform.position))
            {
                belongingTerrainIndex = i;
            }
        }
        transform.parent = EnvironmentOptimizer.Instance.TargetTerrains[belongingTerrainIndex].transform;
    }
}
