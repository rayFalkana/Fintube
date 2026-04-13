using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationInputController : MonoBehaviour
{
   // public event EventHandler OnAnimationLoopedFirstTime;
   // public event EventHandler OnAnimationLooped;

    [SerializeField] private Image display;
    
    [SerializeField] private Text textTimeInterval;
    [SerializeField] private Text textZoom;

    [SerializeField] private Slider sliderTimeInterval;
    [SerializeField] private Slider sliderZoom;

    [SerializeField] private int currentFrame;
    [SerializeField] private bool isPlaying = true;

    private int Mode;
    private int ID;
    private float timer;
    private float framerate = 0.1f;
    private float timerInterval;

    public List<AnimSprite> frameArray;

    public void Flush()
    {
        frameArray.Clear();
    }

    public void StopPlaying()
    {
        isPlaying = false;
        gameObject.SetActive(false);
    }

    public void ContinueAnimation()
    {
        isPlaying = true;
        gameObject.SetActive(true);
    }

    public void PlayAnimation(List<AnimSprite> animSprites)
    {
        frameArray = animSprites;
        InitAnim();
    }

    public void InitAnim()
    {
        gameObject.SetActive(true);
        isPlaying = true;
        currentFrame = 0;
        timer = 0f;
        framerate = frameArray[currentFrame].delayTime;
        display.sprite = frameArray[currentFrame].sprite;
    }

    public bool CheckIdAndMode(int[] idAndMode)
    {
        if (ID.Equals(idAndMode[0]) && Mode.Equals(idAndMode[1]))
        {
            return true;
        }
        else
        {
            ID = idAndMode[0];
            Mode = idAndMode[1];
            return false;
        }
    }

    public void StartAnimationInputController()
    {
        textTimeInterval.text = "Speed : " + 50;
        sliderTimeInterval.minValue = 1;
        sliderTimeInterval.maxValue = 100;
        sliderTimeInterval.wholeNumbers = true;
        sliderTimeInterval.value = 50;
        
        timerInterval = 0.01f;

        sliderTimeInterval.onValueChanged.AddListener(x => {
            textTimeInterval.text = "Speed : " + (int)x;
            timerInterval = 0.0002f * x;
        });

        textZoom.text = "Zoom : " + 5;
        sliderZoom.minValue = 1;
        sliderZoom.maxValue = 10;
        sliderZoom.wholeNumbers = true;
        sliderZoom.value = 5;
        sliderZoom.onValueChanged.AddListener(x => {
            textZoom.text = "Zoom : " + (int)x;
            Vector2 size = new Vector2(0.1f * x, 0.1f * x);
            display.transform.localScale = size;
        });
    }

    void Awake()
    {
        ID = 9999;
        Mode = 9999;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (!isPlaying || frameArray == null)
            {
                return;
            }
            timer += timerInterval;
            if (timer >= framerate)
            {
                timer -= framerate;
                currentFrame += 1;
                if (currentFrame >= frameArray.Count)
                {
                    //if (loop)
                    //{
                    //    if (loopCounter < 1) loopCounter++;

                    //    if (loopCounter == 1)
                    //    {
                    //        if (OnAnimationLoopedFirstTime != null) OnAnimationLoopedFirstTime(this, EventArgs.Empty);
                    //    }

                    //    if (OnAnimationLooped != null) OnAnimationLooped(this, EventArgs.Empty);
                    //}
                    currentFrame = 0;
                }

                display.sprite = frameArray[currentFrame].sprite;
            }
        }
        catch { }
    }


}
