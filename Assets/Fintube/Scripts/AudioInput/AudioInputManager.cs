using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioInputManager : MonoBehaviour
{
    [Header("Left UI Panel")]
    public Transform WholeLeftPanel;
    public Transform PointPanel;

    [Space(5)]
    [Header("Audio Input Manager")]
    public AudioSource audioSource;
    public MicrophoneInput MIC;
    public AudioExternalFileInput audioExternal;

    //pakek enumasda
    public enum StateVoice
    {
        State_0,
        State_1,
        State_2,
        State_3,
        State_4
    }

    //State variable
    public StateVoice stateVoice;
    public List<Slider> limitDB;

    //UI
    public Text textDB;
    public Slider sliderAttenuationVolumeOutput;
    public Button btnChooseMic;
    public Button btnChooseExternalFile;

    //https://answers.unity.com/questions/165729/editing-height-relative-to-audio-levels.html?childToView=165737#answer-165737
    int qSamples = 1024;  // array size
    float refValue = 0.9f; // RMS value for 0 dB
    float rmsValue;   // sound level - RMS
    float dbValue;    // sound level - dB
    private float[] samples; // audio samples

    void ChooseExternalFileORMic(int index = 0)
    {
        if (index.Equals(1))
        {
            audioSource.time = 0;
            audioSource.loop = true;
            audioExternal.enabled = false;
            MIC.enabled = true;
        }
        else
        {
            audioSource.loop = false;
            MIC.enabled = false;
            audioExternal.enabled = true;
        }
    }

    void GetVolume()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetOutputData(samples, 0); // fill array with samples
            int i;
            float sum = 0;

            for (i = 0; i < qSamples; i++)
            {
                sum += samples[i] * samples[i]; // sum squared samples
            }

            rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
            dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB

            if (dbValue < limitDB[0].value)
            {
                if (!stateVoice.Equals(StateVoice.State_2)) stateVoice = StateVoice.State_0;
            }
            else if (dbValue < limitDB[1].value)
            {
                if (!stateVoice.Equals(StateVoice.State_3)) stateVoice = StateVoice.State_1;
            }
            else
            {
                stateVoice = StateVoice.State_4;
            }
             
            textDB.text = stateVoice.ToString();

            sliderAttenuationVolumeOutput.value = dbValue;
        }
    }

    // Start is called before the first frame update
    public void _start()
    {
        samples = new float[qSamples];

        btnChooseExternalFile.onClick.AddListener(() => { ChooseExternalFileORMic(); });
        btnChooseMic.onClick.AddListener(() => { ChooseExternalFileORMic(1); });

        ChooseExternalFileORMic(1);

        WholeLeftPanel.position = PointPanel.position;
    }

    // Update is called once per frame
    public void _update()
    {
        if (MIC.enabled)
        {
            MIC.CheckMic();
        }
        else if (audioExternal.enabled)
        {

        }
        GetVolume();
    }
}
