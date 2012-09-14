using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Timer 
{


    public delegate void TimerEventHandler();
    public event TimerEventHandler TimerHandle;

    public float Length;
    public float ElapsedTime = 0.0f;
    public float percentComplete;
    public bool Repeat;

    public bool isRunning = false;

    // Update is called once per frame
    public void Update()
    {
        if (!isRunning) return;

        ElapsedTime += Time.smoothDeltaTime;
        percentComplete = ElapsedTime / Length;
        if (ElapsedTime >= Length)
        {
            if (TimerHandle != null)
            {
                TimerHandle();
            }
            if (!Repeat)
            {
                isRunning = false;
                ElapsedTime = 0.0f;
            }
        }
    }

    public void StartTimer(float length)
    {
        Length = length;
        isRunning = true;
        ElapsedTime = 0.0f;
        percentComplete = 0.0f;
    }

    public void StartTimer()
    {
        isRunning = true;
        ElapsedTime = 0.0f;
        percentComplete = 0.0f;
    }

    //public void StopTimer()
    //{
    //    isRunning = false;
    //}

    public void ResetTimer()
    {
        isRunning = false;
        ElapsedTime = 0;
    }

}