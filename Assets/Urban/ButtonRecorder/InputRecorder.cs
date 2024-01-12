using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class InputRecorder : MonoBehaviour
{
    public bool UseGamePad = false;

    public InputActionAsset ActionAsset;
    public InputActionReference Button;

    private void OnEnable()
    {
        if (ActionAsset != null)
        {
            ActionAsset.Enable();
        }
        WriteString();
    }

    void Update()
    {
        if (IsPressed() || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Times.Add(Time.time);
            WriteString();
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
    int FlieNo = -1;
    public void WriteString()
    {
        string path = Application.persistentDataPath + "/Record" + FlieNo + ".txt";
        if (FlieNo < 0)
        {
            FlieNo++;
            path = Application.persistentDataPath + "/Record" + FlieNo + ".txt";
            while (File.Exists(path))
            {
                FlieNo++;
                path = Application.persistentDataPath + "/Record" + FlieNo + ".txt";
            }
        }
        StreamWriter writer = new StreamWriter(path, false);
        for (int i = 0; i < Times.Count; i++)
        {
            writer.WriteLine("Time: " + Times[i]);
        }
        writer.Close();

        StreamReader reader = new StreamReader(path);
        reader.Close();
    }
    #endregion
}
