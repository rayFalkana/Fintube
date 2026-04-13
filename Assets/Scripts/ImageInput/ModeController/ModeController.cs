using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ModeController : MonoBehaviour
{
    public static int IdMode;
    public static int ModeCount;
    private static string currentMode;

    [Header("Display Anim GIF")]
    [SerializeField] private AnimationInputController forID1;
    [SerializeField] private AnimationInputController forID2;
    [SerializeField] private AnimationInputController forID3;
    [SerializeField] private AnimationInputController forID4;
    [SerializeField] private AnimationInputController forID5;

    [Space(5)]
    [Header("Prefab and Point")]
    [SerializeField] private Transform Point;
    [SerializeField] private GameObject PrefabModes;
    [SerializeField] private GameObject PrefabModesButton;

    public List<GameObject> Modes = new List<GameObject>();

    public TypeMode GenerateMode(int _index, Transform _parent, Func<GameObject, Toggle> _addToggle, UnityAction<TypeMode> _hotkeyListener)
    {
        GameObject btnMode = Instantiate(PrefabModesButton);
        btnMode.name = "Mode" + _index;
        btnMode.transform.localPosition = Point.transform.localPosition;

        GameObject Mode = Instantiate(PrefabModes, _parent);
        Mode.name = "Mode" + _index + "Buttons";
        Modes.Add(Mode);

        TypeMode typeMode = Mode.GetComponent<TypeMode>();
        typeMode.AssignDisplayText(btnMode.GetComponentInChildren<TextMeshProUGUI>());
        typeMode.AssignToggle(_addToggle.Invoke(btnMode));
        typeMode.AddListener(()=> { _hotkeyListener.Invoke(typeMode); });
        return typeMode;
    }

    public void SetCurrentMode()
    {
        switch (currentMode)
        {
            default:
            case "Mode0":
                IdMode = 0;
                break;
            case "Mode1":
                IdMode = 1;
                break;
            case "Mode2":
                IdMode = 2;
                break;
            case "Mode3":
                IdMode = 3;
                break;
        }

        for (int i = 0; i < Modes.Count; i++)
        {
            if (i.Equals(IdMode))
            {
                Modes[i].SetActive(true);
            }
            else Modes[i].SetActive(false);
        }
    }

    public void ChangeMode(string _name)
    {
        currentMode = _name;
    }

    public void AnimInputStartPlaying()
    {
        forID1.StartAnimationInputController();
        forID2.StartAnimationInputController();
        forID3.StartAnimationInputController();
        forID4.StartAnimationInputController();
        forID5.StartAnimationInputController();
    }

    public void AnimInputStopPlaying()
    {
        forID1.StopPlaying();
        forID2.StopPlaying();
        forID3.StopPlaying();
        forID4.StopPlaying();
        forID5.StopPlaying();
    }

    public AnimationInputController AnimInputStopPlaying(AudioInputManager.StateVoice _stateVoice)
    {
        switch (_stateVoice)
        {
            case AudioInputManager.StateVoice.State_0:
                forID2.StopPlaying();
                forID3.StopPlaying();
                forID4.StopPlaying();
                forID5.StopPlaying();
                return forID1;
            case AudioInputManager.StateVoice.State_1:
                forID1.StopPlaying();
                forID3.StopPlaying();
                forID4.StopPlaying();
                forID5.StopPlaying();
                return forID2;
            case AudioInputManager.StateVoice.State_2:
                forID1.StopPlaying();
                forID2.StopPlaying();
                forID4.StopPlaying();
                forID5.StopPlaying();
                return forID3;
            case AudioInputManager.StateVoice.State_3:
                forID1.StopPlaying();
                forID2.StopPlaying();
                forID3.StopPlaying();
                forID5.StopPlaying();
                return forID4;
            case AudioInputManager.StateVoice.State_4:
                forID1.StopPlaying();
                forID2.StopPlaying();
                forID3.StopPlaying();
                forID4.StopPlaying();
                return forID5;
        }
        return null;
    }

    public void AnimInputContinuePlaying(AnimationInputController _animInput)
    {
        _animInput.ContinueAnimation();
    }

    public void AnimInputPlayAnimation(List<AnimSprite> _sprite, int [] _id)
    {
        switch (_id[0])
        {
            case 0:
                forID1.Flush();
                forID1.PlayAnimation(_sprite);
                break;
            case 1:
                forID2.Flush();
                forID2.PlayAnimation(_sprite);
                break;
            case 2:
                forID3.Flush();
                forID3.PlayAnimation(_sprite);
                break;
            case 3:
                forID4.Flush();
                forID4.PlayAnimation(_sprite);
                break;
            case 4:
                forID5.Flush();
                forID5.PlayAnimation(_sprite);
                break;
        }
    }

}
