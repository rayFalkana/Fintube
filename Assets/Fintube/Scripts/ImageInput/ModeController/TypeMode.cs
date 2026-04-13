using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TranslateKey = DesktopAppLowLevelKeyboardHook.TranslateKey;
using TMPro;

public class TypeMode : MonoBehaviour
{
    public string Hotkey;
    public Button BtnAssignHotkey;
    public List<ImageData> ImageDatas;

    private string prevHotkey;
    private Toggle cloneToggle;
    private RectTransform rectTransformToggle;
    private TextMeshProUGUI hotkeyUI;

    private const int limitWord = 11;
    private const float sizeDefault = 40.0f;
    private const float additionalSize = 20.0f;

    public void AssignToggle(Toggle _toggle) { 
        cloneToggle = _toggle;
        rectTransformToggle = _toggle.GetComponent<RectTransform>();
    }
    public void AssignDisplayText(TextMeshProUGUI _hotkeyUI) { hotkeyUI = _hotkeyUI; }
    public void TurnOnThisMode() { cloneToggle.isOn = true;}
    public int Count() { return ImageDatas.Count; }
    public void AddListener(UnityAction _delegateTypeMode)
    {
        BtnAssignHotkey.onClick.AddListener(_delegateTypeMode);
    }
    public void UpdateText()
    {
        //try
        //{
        //    if (!prevHotkey.Equals(Hotkey))
        //    {
        //        DisplayText();
        //    }
        //} catch {
        //    prevHotkey = "";
        //    if (!prevHotkey.Equals(Hotkey))
        //    {
        //        DisplayText();
        //    }
        //}
        try
        {
            DisplayText();
        }
        catch { }
    }

    private void DisplayText()
    {
        string hotkey = Hotkey[1..];
        hotkey = TranslateKey.Cut(hotkey);

        int count = hotkey.Length;
        float size = sizeDefault;

        if (count > limitWord)
        {
            size += (limitWord * additionalSize);
            count = limitWord;
        }
        else if (count>1)
        {
            size += (count * additionalSize);
        }

        rectTransformToggle.sizeDelta = new(size, rectTransformToggle.sizeDelta.y);
        hotkeyUI.text = hotkey[0..count];
        prevHotkey = Hotkey;
    }

    private void Update()
    {
        UpdateText();
    }
}
