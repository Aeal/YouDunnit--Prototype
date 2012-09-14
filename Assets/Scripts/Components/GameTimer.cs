using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    /// <summary>
    /// Class Name: Timer
    /// Author: TC_FALLOWS
    /// Last Edit: TC_FALLOWS 10/18/2011
    /// Purpose: Timer helper class for event triggering/debugging
    /// </summary>

    #region Members
    private float                   mElapsedTime;        // How much time has passed?
	private float                   mStartTime;          // The start time of the timer
	private bool                    mIsPaused;           // Is the timer paused?
    #endregion

    #region Methods
    //******************************************************************
    public void StartTimer()
	{
		mStartTime = Time.time;
		mIsPaused = false;
	}
    //******************************************************************
	public void UpdateTimer()
	{
		//Debug.Log(elapsedTime);
		if(!mIsPaused)
			mElapsedTime = Time.time - mStartTime;
	}
    //******************************************************************
	public float GetElapsedTime()
	{
		return mElapsedTime;
	}
    //******************************************************************
	public int GetElapsedTimeInMinutes()
	{
		return Mathf.CeilToInt(mElapsedTime)/60;
	}
    //******************************************************************
	public int GetElapsedTimeInSeconds()
	{
		return Mathf.CeilToInt(mElapsedTime) % 60;
	}
    //******************************************************************
	public void PauseTimer()
	{
		mIsPaused = true;
	}
    //******************************************************************
	public void UnpauseTimer()
	{
		mIsPaused = false;
    }
    #endregion
}


