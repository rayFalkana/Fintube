using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class EventToggleGroup : MonoBehaviour
{
    [Serializable] public class ToggleEvent : UnityEvent<Toggle> { }
    [SerializeField] private List<Toggle> _toggles;
    [SerializeField] public ToggleEvent onActiveToggleChanged;

    private ToggleGroup _toggleGroup;

    void OnEnable()
    {
        _OnEnable();
    }

    private void OnDisable()
    {
        _OnDisable();
    }

    public void Add(Toggle togg) { _toggles.Add(togg); }

    public virtual void StartToggleGroup() { _toggleGroup = GetComponent<ToggleGroup>(); }
    public virtual void _OnEnable()
    {
        foreach (var item in _toggles)
        {
            if (item.group != null && item.group != _toggleGroup)
            {
                Debug.LogError($"EventToggleGroup is trying to register a Toggle that is a member of another group.");
            }
            item.group = _toggleGroup;
            item.onValueChanged.AddListener(HandleToggleValueChanged);
        }
    }

    public virtual void _OnDisable()
    {
        foreach (var item in _toggleGroup.ActiveToggles())
        {
            item.onValueChanged.RemoveListener(HandleToggleValueChanged);
            item.group = null;
        }
    }

    public void HandleToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            onActiveToggleChanged?.Invoke(_toggleGroup.ActiveToggles().FirstOrDefault());
        }
    }

    public void ChangeToggleViaName(string name)
    {
        foreach (var item in _toggles)
        {
            if(item.name == name)
            {
                if (!item.isOn) item.isOn = true;
                else item.onValueChanged?.Invoke(true);
                break;
            }
        }
    }
}
