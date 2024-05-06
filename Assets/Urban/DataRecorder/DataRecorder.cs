using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GeometryUtility;

public class DataRecorder : MonoBehaviour
{
    public bool UseGamePad = false;

    public InputActionAsset ActionAsset;
    public InputActionReference Button;
    public List<CovertObject> CovertObjects;
    public Transform CovertObjectTrans;
    private void OnEnable()
    {
        GetCovertObjectList(CovertObjectTrans);
        if (ActionAsset != null)
        {
            ActionAsset.Enable();
        }
        WriteString(RecordFile.Input);
    }

    private void Start()
    {

        StartCoroutine(RecordingEyeSightAngle());
        
    }

    void Update()
    {
        if (IsPressed() || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Times.Add(Time.time);
            WriteString(RecordFile.Input);
        }
        else
        {
            Debug.Log("VIVEButton NOT Pressed! Value: " + Button.action.ReadValue<float>());
        }
        if (UseGamePad)
        {
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                Debug.Log("A Pressed!");
            }
        }
    }

    IEnumerator RecordingEyeSightAngle()
    {
        YieldInstruction wait = new WaitForSeconds(0.02f);
        yield return wait;
        WriteString(RecordFile.EyeSight);
        while (true)
        {
            WriteString(RecordFile.EyeSight);
            //Debug.Log(Application.persistentDataPath);
            yield return wait;
        }
    }

    float OldButtonValue = 0;
    /// <summary>
    /// True = pressed in this frame, False = not pressed in this frame
    /// </summary>
    /// <returns></returns>
    bool IsPressed()
    {
        bool value = false;
        if(OldButtonValue == 0 && Button.action.ReadValue<float>() == 1)
        {
            value = true;
        }
        else
        {
            value = false;
        }
        OldButtonValue = Button.action.ReadValue<float>();
        return value;
    }

    #region Write to file
    List<float> Times = new List<float>();
    int InputFlieNo = -1;
    int EyeSightFlieNo = -1;
    public void WriteString(RecordFile fileType)
    {
        string path = "";
        StreamWriter writer;
        StreamReader reader;
        switch (fileType)
        {
            case RecordFile.Input:
                path = Application.persistentDataPath + "/" + fileType.ToString() + InputFlieNo + ".csv";
                if (InputFlieNo < 0)
                {
                    InputFlieNo++;
                    path = Application.persistentDataPath + "/" + fileType.ToString() + InputFlieNo + ".csv";
                    while (File.Exists(path))
                    {
                        InputFlieNo++;
                        path = Application.persistentDataPath + "/" + fileType.ToString() + InputFlieNo + ".csv";
                    }
                }
                writer = new StreamWriter(path, false);
                for (int i = 0; i < Times.Count; i++)
                {
                    writer.WriteLine("Time: " + Times[i]);
                }
                writer.Close();

                reader = new StreamReader(path);
                reader.Close();
                break;
            case RecordFile.EyeSight:
                path = Application.persistentDataPath + "/" + fileType.ToString() + EyeSightFlieNo + ".csv";
                if (EyeSightFlieNo < 0)
                {
                    EyeSightFlieNo++;
                    path = Application.persistentDataPath + "/" + fileType.ToString() + EyeSightFlieNo + ".csv";
                    while (File.Exists(path))
                    {
                        EyeSightFlieNo++;
                        path = Application.persistentDataPath + "/" + fileType.ToString() + EyeSightFlieNo + ".csv";
                    }
                }

                writer = new StreamWriter(path, true);

                if (new FileInfo(path).Length == 0)
                {
                    writer.WriteLine("Target Object" + "," + "Is Visible to Camera?" + "," + "Player Distance to Target" + "," + "World Time In Seconds" + "," + "Gaze Angular Distance To Target In Degrees of Arc" + "," + "Center Angular Distance To Target In Degrees of Arc");
                }

                (CovertObject closestObject, float distance) = GetClosestObjectInformation();

                Renderer targetRenderer = closestObject.gameObject.GetComponent<Renderer>();
                Plane[] cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
                bool isVisible = GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, targetRenderer.bounds);
                int visibility = isVisible? 1 : 0;

                if (distance <= 200.0f)
                {
                    writer.WriteLine(closestObject.gameObject.name + "," + visibility + "," + distance + "," + Time.time + "," + closestObject.EyeSightAngle() + "," + Vector3.Angle(Camera.main.transform.forward, (closestObject.transform.position - Camera.main.transform.position).normalized));
                    Debug.Log("vIN: " + closestObject.EyeSightAngle() + "<->" + Vector3.Angle(Camera.main.transform.forward, (closestObject.transform.position - Camera.main.transform.position).normalized));
                }
                writer.Close();

                reader = new StreamReader(path);
                reader.Close();
                break;
        }

    }
    #endregion

    void GetCovertObjectList(Transform parent)
    {
        CovertObjects = new List<CovertObject>();
        //Debug.Log("PPPAP1");
        for (int i = 0; i < parent.childCount; i++)
        {
            //Debug.Log("PPPAP2");
            CovertObject tmp = parent.GetChild(i).GetComponent<CovertObject>();
            if (tmp)
            {
                //Debug.Log("PPPAP3");
                CovertObjects.Add(tmp); 
            }
        }
    }

    (CovertObject closestObject, float distance) GetClosestObjectInformation()
    {
        CovertObject tmp = CovertObjects[0];
        float playerDistanceToTarget = Vector3.Distance(Camera.main.transform.position, tmp.transform.position);
        foreach (CovertObject obj in CovertObjects)
        {
            if(obj == tmp)
            {
                continue;
            }
            else
            {
                if(Vector3.Distance(Camera.main.transform.position, obj.transform.position) < Vector3.Distance(Camera.main.transform.position, tmp.transform.position))
                {
                    tmp = obj;
                    playerDistanceToTarget = Vector3.Distance(Camera.main.transform.position, tmp.transform.position);
                }
            }
        }
        return (tmp, playerDistanceToTarget); 
    }

    public enum RecordFile
    {
        None = -1,
        Input = 1,
        EyeSight = 2,
    }
}
