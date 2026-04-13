using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MicrophoneInput : MonoBehaviour
{
    public AudioInputManager audioInputManager;
    public AudioMixerGroup audioMixerGroup;
    AudioSource audioSource;

    //List that contain existing mic asda
    public string ChoosenMic;
    public int TrackExistingNumOfMic;
    public List<UIMicName> microphoneNames;

    //UI
    public Button btnMic;
    public Button btnChooseThisMic;
    public GameObject WholeUIObject;
    public GameObject PanelMicrophoneNames;
    public GameObject ContentMicrophoneNames;
    public GameObject prefabToggleMicName;
    ToggleGroup activeMic;

    public void MicrophoneToAudioClip(int id = 0)
    {
        string microphoneName = Microphone.devices[id];
        audioSource.clip = Microphone.Start(microphoneName, true, 1, 44100);
        ChoosenMic = microphoneName;
        while (!(Microphone.GetPosition(microphoneName) > 0)) { }
        audioSource.Play();
    }

    public void CheckMic()
    {
        if (Microphone.devices.Length > 0)
        {
            if (!btnMic.interactable) btnMic.interactable = MicOn();
            else return;
        }
        else
        {
            if (btnMic.interactable) btnMic.interactable = MicOff();
            else return;
        }
    }

    bool MicOn()
    {
        if (microphoneNames.Count > 0)
        {
            foreach (UIMicName child in microphoneNames)
            {
                Destroy(child.gameObject);
            }
        }

        microphoneNames.Clear();
        int DevicesCount = Microphone.devices.Count();
        for (int i = 0; i < DevicesCount; i++)
        {
            GameObject temp = Instantiate(prefabToggleMicName, ContentMicrophoneNames.transform);
            UIMicName uIMicName = temp.GetComponent<UIMicName>();
            uIMicName.SetMicName(Microphone.devices[i], activeMic);
            microphoneNames.Add(uIMicName);
        }
        return true;
    }

    bool MicOff()
    {
    //  WhenMicWasLostSuddenly();
        audioInputManager.sliderAttenuationVolumeOutput.value = -80;
        audioSource.Stop();

        return false;
    }

    //void WhenMicWasLostSuddenly()
    //{
    //    UIMicName temp = null;
    //    Debug.Log("HERE 3");
    //    if (microphoneNames.Count > 0)
    //    {
    //        Debug.Log("HERE 4 ");
    //        for (int i = 0; i < microphoneNames.Count; i++)
    //        {
    //            if (microphoneNames[i].GetName().Equals(ChoosenMic))
    //            {
    //                Debug.Log("HERE 5");
    //                temp = microphoneNames[i];
    //                microphoneNames.RemoveAt(i);
    //                break;
    //            }
    //        }
    //        if (temp != null) Destroy(temp.gameObject);
    //    }
    //}

    void ChooseMic()
    {
        for (int i = 0; i < microphoneNames.Count; i++)
        {
            UIMicName temp = microphoneNames[i];
            if (temp.thisToggle.isOn)
            {
                Microphone.End(ChoosenMic);
                MicrophoneToAudioClip(i);
                PanelMicrophoneNames.SetActive(false);
                return;
            }
        }
    }

    //void SetVolume(float sliderValue)
    //{
    //    audioMixer.SetFloat("AttenuationVolume", Mathf.Log10(sliderValue)*20);
    //}

    void ChooseToUseMic()
    {
        try
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            if (Microphone.devices.Length > 0)
            {
                TrackExistingNumOfMic = Microphone.devices.Length;
                btnMic.interactable = MicOn();
                MicrophoneToAudioClip();
            }
            else btnMic.interactable = MicOff();
        }
        catch { }
    }

    void Start()
    {
        audioSource = audioInputManager.audioSource;
        activeMic = ContentMicrophoneNames.GetComponent<ToggleGroup>();

        btnMic.onClick.AddListener(() => {
            if (PanelMicrophoneNames.activeSelf) PanelMicrophoneNames.SetActive(false);
            else PanelMicrophoneNames.SetActive(true);
        });
        btnChooseThisMic.onClick.AddListener(() => {
            ChooseMic();
        });

        ChooseToUseMic();
    }

    private void OnEnable()
    {
        WholeUIObject.SetActive(true);
        ChooseToUseMic();
    }

    private void OnDisable()
    {
        audioSource.Stop();
        try { WholeUIObject.SetActive(false); }
        catch { }
    }
}
