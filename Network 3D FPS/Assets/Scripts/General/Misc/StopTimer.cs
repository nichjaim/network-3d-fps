using System;
using UnityEngine;

public class StopTimer
{
    #region Class Variables

    private float currentTime;
    public float CurrentTime
    {
        get { return currentTime; }
    }

    private float goalTime;
    public float GoalTime
    {
        get { return goalTime; }
    }

    private bool timerOn;

    public Action OnTimerFinished;

    #endregion




    #region Constructors

    public StopTimer()
    {
        Setup();
    }

    public StopTimer(float goalTimeArg)
    {
        Setup();

        StartTimer(goalTimeArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        currentTime = 0f;
        goalTime = 0f;

        timerOn = false;
    }

    #endregion




    #region Timer Functions

    /// <summary>
    /// Increase current timer time by game time passed. 
    /// Call in Update().
    /// </summary>
    public void IncreaseCurrentTimeByTimePassed()
    {
        // if timer is OFF
        if (!timerOn)
        {
            // DONT continue code
            return;
        }

        // increase current time by time passed
        currentTime += Time.deltaTime;

        // if timer has finished
        if (IsTimerFinished())
        {
            // turn OFF timer
            timerOn = false;

            // call timer finished actions if NOT null
            OnTimerFinished?.Invoke();
        }
    }

    /// <summary>
    /// Returns bool that denotes if time has finished.
    /// </summary>
    /// <returns></returns>
    public bool IsTimerFinished()
    {
        return currentTime >= goalTime;
    }

    /// <summary>
    /// Starts timer with given goal time.
    /// </summary>
    /// <param name="goalTimeArg"></param>
    public void StartTimer(float goalTimeArg)
    {
        // set current time to zero
        currentTime = 0f;

        // set goal time to given time while ensuring a negative time was NOT given
        goalTime = Mathf.Max(goalTimeArg, 0f);

        // turn ON timer
        timerOn = true;
    }

    /// <summary>
    /// Get percentage of timer that has been gone through.
    /// </summary>
    /// <returns></returns>
    public float GetProgressPercentageCompleted()
    {
        return currentTime / goalTime;
    }

    /// <summary>
    /// Get percentage of timer that still remains.
    /// </summary>
    /// <returns></returns>
    public float GetProgressPercentageRemaining()
    {
        return 1f - GetProgressPercentageCompleted();
    }

    #endregion


}
