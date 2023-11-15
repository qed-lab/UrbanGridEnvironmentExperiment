using System.Collections;
using Wave.Essence.Eye;
using UnityEngine;

public class OvertObject : MonoBehaviour
{
    public MeshRenderer Renderer;
    public Material OvertM;
    public AnimationCurve Carve;
    private void OnEnable()
    {
        if (EyeManager.Instance != null) { EyeManager.Instance.EnableEyeTracking = true; }
    }

    Vector3 EyeSight;
    void Update()
    {
        //EyeManager.Instance.GetCombindedEyeDirectionNormalized(out EyeSight);
        //EyesightDebug.Instance.Dir = EyeSight;
        if (Vector3.Angle(Camera.main.transform.rotation * EyeSight, transform.position - Camera.main.transform.position) <= 180 || true)
        {
            if (Renderer.materials[0] != null)
            {
                Renderer.materials[0].SetFloat("_Scale", Carve.Evaluate(Time.time%1));
            }
        }
        else
        {
            Renderer.materials[0].SetFloat("_Scale", 1);
        }
    }

    IEnumerator OvertShining()
    {
        YieldInstruction wait = new WaitForSeconds(0.1f);
        float scale = 1;
        if (Renderer.materials[0] != null)
        {
            while (true)
            {
                Debug.Log("Scale: " + scale);
                Renderer.materials[0].SetFloat("_Scale", scale = scale == 1 ? 1.01f : 1);
                yield return wait;
            }
        }
        Debug.Log("No material!");
    }
}
