using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTimeToState0 : MonoBehaviour
{
    public AudioInputManager audioInput;
    public float durTimeState0;
    public float durTimeState2;
    public float durTimeState1;
    public float durTimeState3;
    float waitTime_State1to3;
    float waitTime_State0to2;

    AudioInputManager.StateVoice prevState;
    float timer;
    float timer2;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        timer2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioInput.enabled)
        {
            if (audioInput.stateVoice == AudioInputManager.StateVoice.State_0)
            {
                timer += Time.deltaTime;
                if (timer > waitTime_State0to2)
                {
                    audioInput.stateVoice = AudioInputManager.StateVoice.State_2;
                    timer = timer - waitTime_State0to2;
                    waitTime_State0to2 = durTimeState2;
                }
            }
            else if (audioInput.stateVoice == AudioInputManager.StateVoice.State_2)
            {
                timer += Time.deltaTime;
                if (timer > waitTime_State0to2)
                {
                    audioInput.stateVoice = AudioInputManager.StateVoice.State_0;
                    timer = timer - waitTime_State0to2;
                    waitTime_State0to2 = durTimeState0;
                }
            }
            else if (audioInput.stateVoice == AudioInputManager.StateVoice.State_1)
            {
                timer2 += Time.deltaTime;
                if (timer2 > waitTime_State1to3)
                {
                    audioInput.stateVoice = AudioInputManager.StateVoice.State_3;
                    timer2 = timer2 - waitTime_State1to3;
                    waitTime_State1to3 = durTimeState3;
                }
            }
            else if (audioInput.stateVoice == AudioInputManager.StateVoice.State_3)
            {
                timer2 += Time.deltaTime;
                if (timer2 > waitTime_State1to3)
                {
                    audioInput.stateVoice = AudioInputManager.StateVoice.State_1;
                    timer2 = timer2 - waitTime_State1to3;
                    waitTime_State1to3 = durTimeState1;
                }
            }
            else
            {
                //timer = 0;
                //   timer2 = 0;asdada
            }
            prevState = audioInput.stateVoice;
        }
    }

}
