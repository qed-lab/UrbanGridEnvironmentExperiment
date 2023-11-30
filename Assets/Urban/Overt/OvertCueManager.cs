using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.Essence.Eye;

public class OvertCueManager : MonoBehaviour
{
    public static OvertCueManager Instance;
    public GameObject OvertCuePrefab;
    public float CueSize = 1f;
    private void OnEnable()
    {
        Instance = this;    
    }
}
