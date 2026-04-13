using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using Fintube.MiniClass.AudioExternalFileInput;
using SFB;

public class AudioExternalFileInput : MonoBehaviour
{
    [Header("Essensial")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [Space(5)]
    [Header("UI - Panel")]
    [SerializeField] private GameObject wholeUIObject;
    [SerializeField] private Button btnInputAudio;
   // [SerializeField] private InputField indexFileAudio;

    [Space(5)]
    [Header("UI - AudioTrack")]
    [SerializeField] private EnhancedSlider audioTrack;
    [SerializeField] private MovingTextController audioName;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnStop;
    //[SerializeField] private Button btnPrev;
    //[SerializeField] private Button btnNext;

    [SerializeField] public List<InputAudio> ListOFInputAudio = new List<InputAudio>();

    private string [] path;
    private ExtensionFilter [] extension = new ExtensionFilter[1];

    private bool isDragging = false;
    private bool isOrderedToPlayAtCurrentTrack = false;

    private const int indexAudio = 0;

    public void GetExternalAudio()
    {
        StopAudio();
        path = StandaloneFileBrowser.OpenFilePanel("Show all audio file", "", extension, true);
        StartCoroutine(ILoadAudio());
    }

    public IEnumerator ILoadAudio()
    {
        string fullpath = "file:///" + path[indexAudio];
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullpath, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            InputAudio inputAudioBaru = new InputAudio();
            inputAudioBaru.fullpath = fullpath;
            inputAudioBaru.name = Path.GetFileName(fullpath);

            if (fullpath.EndsWith("mp3"))
            {
                inputAudioBaru.audioClip = NAudioPlayer.FromMp3Data(www.downloadHandler.data);
            }
            else
            {
                inputAudioBaru.audioClip = ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;
            }

            ListOFInputAudio.Clear();
            ListOFInputAudio.Add(inputAudioBaru);
            audioName.SetText(ListOFInputAudio[indexAudio].name);
        }
    }

    private void StartAudio(int changeAudio = 0)
    {
        if (ListOFInputAudio.Count == 0)
        {
            return;
        }
        //indexAudio += changeAudio;
        //if (indexAudio >= ListOFInputAudio.Count)
        //{
        //    indexAudio = 0;
        //}
        //else if (indexAudio < 0)
        //{
        //    indexAudio = (ListOFInputAudio.Count - 1);
        //}

        if (audioSource.isPlaying && changeAudio == 0)
        {

            if (audioTrack.Value >= audioTrack.MaxValue)
            {
                audioTrack.Value = 0;
            }
            return;
        }

        audioSource.clip = ListOFInputAudio[indexAudio].audioClip;
        
        audioTrack.MaxValue = audioSource.clip.length;
        audioTrack.Value = 0;

        audioSource.time = 0;
        audioSource.Play();
    }

    private void StopAudio()
    {
        audioSource.Stop();
    }

    private void UpdateTrack()
    {
        audioTrack.Interactable = audioSource.isPlaying;
        if (audioSource.isPlaying)
        {
            audioTrack.Value += Time.deltaTime;
            //if (audioTrack.Value >= audioSource.clip.length)
            //{
            //    indexAudio++;
            //    if (indexAudio >= ListOFInputAudio.Count)
            //    {
            //        indexAudio = 0;
            //    }
            //    StartAudio();
            //}
        }
        else
        {
            audioTrack.Value = 0;
        }
    }

    private void ChangePlayTrack()
    {
        if (!isDragging && isOrderedToPlayAtCurrentTrack)
        {
            isOrderedToPlayAtCurrentTrack = false;
            audioSource.time = audioTrack.Value;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (isDragging)
        {
            isOrderedToPlayAtCurrentTrack = true;
        }
    }

    private void OnPointerDown(BaseEventData eventData)
    {
        isDragging = true;
    }

    private void OnPointerUp(BaseEventData eventData)
    {
        isDragging = false;
    }

    private void InitAudioTrack()
    {
     //   btnNext.onClick.AddListener(() => { StartAudio(1); });
        btnPlay.onClick.AddListener(() => {
            //indexAudio = int.Parse(indexFileAudio.text);
            StartAudio();
        });
      //  btnPrev.onClick.AddListener(() => { StartAudio(-1); });
        btnStop.onClick.AddListener(() => { StopAudio(); });

        audioTrack.OnSliderValueChanged((x) => { OnSliderValueChanged(x); });
        audioTrack.AddPointerUp(OnPointerUp);
        audioTrack.AddPointerDown(OnPointerDown);

        StopAudio();
    }

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        string[] myExtention = new string[3] { "mp3", "wav" , "ogg"};
        extension[0] = new ExtensionFilter("extensionAudio", myExtention);

        audioSource.outputAudioMixerGroup = audioMixerGroup;

        btnInputAudio.onClick.AddListener(() => { GetExternalAudio(); });

        InitAudioTrack();
    }

    // Update is called once per frame
    void Update()
    {
        try { UpdateTrack(); } catch { }
        ChangePlayTrack();
    }

    private void OnEnable()
    {
        wholeUIObject.SetActive(true);
        if (ListOFInputAudio.Count != 0)
        {
            audioName.ContinueWalk();
        }

        try { audioSource.outputAudioMixerGroup = audioMixerGroup; } catch { }
    }

    private void OnDisable()
    {
        StopAudio();
        try {
            if (ListOFInputAudio.Count != 0)
            {
                audioName.PauseWalk();
            }
            wholeUIObject.SetActive(false); }
        catch { }
    }
    #endregion
}

namespace Fintube.MiniClass.AudioExternalFileInput
{
    [Serializable]
    public struct InputAudio
    {
        public string fullpath;
        public string name;
        public AudioClip audioClip;
    }

    [Serializable]
    public class EnhancedSlider
    {
        [SerializeField] private Slider slider;
        [SerializeField] private EventTrigger trigger;

        public bool Interactable
        {
            get { return slider.interactable; }
            set { slider.interactable = value; }
        }

        public float Value
        {
            get { return slider.value; }
            set { slider.value = value; }
        }

        public float MaxValue
        {
            get { return slider.maxValue; }
            set { slider.maxValue = value; }
        }

        public void AddPointerUp(UnityAction<BaseEventData> _callback)
        {
            AddEventTriggerListener(EventTriggerType.PointerUp, _callback);
        }

        public void AddPointerDown(UnityAction<BaseEventData> _callback)
        {
            AddEventTriggerListener(EventTriggerType.PointerDown, _callback);
        }

        public void OnSliderValueChanged(UnityAction<float> _action)
        {
            slider.onValueChanged.AddListener(_action);
        }

        private void AddEventTriggerListener(EventTriggerType _eventType, UnityAction<BaseEventData> _callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = _eventType;
            entry.callback.AddListener(_callback);
            trigger.triggers.Add(entry);
        }
    }
}


