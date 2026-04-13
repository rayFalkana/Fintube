using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMicName : MonoBehaviour
{
    public Toggle thisToggle;
    public Text micropohoneName;
    public void SetMicName(string name, ToggleGroup toggleGroup)
    {
        micropohoneName.text = name;
        thisToggle.group = toggleGroup;
    }

    public string GetName () { return micropohoneName.text; }
}
