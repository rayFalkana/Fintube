using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Events;
using DesktopAppLowLevelKeyboardHook;

public class KeyboardController : MonoBehaviour
{
    public UnityEvent<string> KeyboardEvent;

    private IDisposable _internalKeyboardListener;
    private LowLevelKeyboardListener _lowlevelKeyboardListener;

    private void OnKeyDown(RawKey _key)
    {
        string key = _key.ToString();
        KeyboardEvent.Invoke(key);
    }

    private void OnKeyUp(RawKey _key)
    {
        
    }

    private void Performing(InputControl _button)
    {
        if (!_button.device.name.Equals("Keyboard"))
        {
            string temp = "_" + _button.name;

            if (temp.Equals("_rightButton"))
            {

            }

            return;
        }

        string key = "_" + _button.name;
        KeyboardEvent.Invoke(key);
    }

    public void Start()
    {
        _lowlevelKeyboardListener = new LowLevelKeyboardListener();
        _lowlevelKeyboardListener.OnKeyUp += OnKeyUp;
        _lowlevelKeyboardListener.OnKeyDown += OnKeyDown;
        _lowlevelKeyboardListener.StartHookKeyboard();

        _internalKeyboardListener = InputSystem.onAnyButtonPress.Call(Performing);
    }

    private void OnApplicationQuit()
    {
        _internalKeyboardListener.Dispose();
        _lowlevelKeyboardListener.StopHookKeyboard();
    }
}
