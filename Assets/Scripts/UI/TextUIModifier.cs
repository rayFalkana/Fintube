using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUIModifier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Toggle toggle;

    void Start()
    {
        toggle.onValueChanged.AddListener(x => { UpdateTextUI(x); });
        UpdateTextUI(toggle.isOn);
    }

    public void UpdateTextUI(bool isOn)
    {
        if (isOn)
        {
            textMesh.outlineColor = Color.black;
            textMesh.faceColor = Color.white;
        }
        else
        {
            textMesh.outlineColor = Color.white;
            textMesh.faceColor = Color.black;
        }
    }
}
