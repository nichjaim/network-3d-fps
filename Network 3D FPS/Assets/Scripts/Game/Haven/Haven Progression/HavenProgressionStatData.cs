using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenProgressionStatData
{
    #region Class Variables

    // the current amount of exp points held, used for leveling up
    public int progressPointsCurrent;
    // the total points that were cashed in for levels
    public int progressPointsUsed;

    /// the current amount of points held for this week's activites, will 
    /// be reset at end of week after calculations.
    public int activityPoints;

    // the currently held stat level
    public int statLevel;

    #endregion




    #region Constructors

    public HavenProgressionStatData()
    {
        Setup();
    }

    public HavenProgressionStatData(HavenProgressionStatData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        progressPointsCurrent = 0;
        progressPointsUsed = 0;

        activityPoints = 0;

        statLevel = 1;
    }

    private void Setup(HavenProgressionStatData templateArg)
    {
        progressPointsCurrent = templateArg.progressPointsCurrent;
        progressPointsUsed = templateArg.progressPointsUsed;

        activityPoints = templateArg.activityPoints;

        statLevel = templateArg.statLevel;
    }

    #endregion




    #region Stat Functions

    /// <summary>
    /// Returns point requirement to attain the given level.
    /// </summary>
    /// <param name="statLevelArg"></param>
    /// <returns></returns>
    private int GetPointRequirementForStatLevel(int statLevelArg)
    {
        // initialize point requirement values
        float BASE_LEVEL_REQ = 100f;
        float LEVEL_MULTIPLIER = 1.5f;

        // the level at which requirement growth stops happening
        int REQ_GROWTH_CEILING = 10;

        // get given stat level while ensuring it doesn't go above the growth ceiling
        int statLevelClamped = Mathf.Min(statLevelArg, REQ_GROWTH_CEILING); ;

        // calculate the point requirement, as a float
        float levelReqUnrounded = BASE_LEVEL_REQ * (((float)statLevelClamped) * LEVEL_MULTIPLIER);

        // return the rounded calcualted value
        return Mathf.RoundToInt(levelReqUnrounded);
    }

    /// <summary>
    /// Returns the points needed for stat to reach the next level.
    /// </summary>
    /// <returns></returns>
    public int GetPointRequirementForNextLevel()
    {
        return GetPointRequirementForStatLevel(statLevel + 1);
    }

    /// <summary>
    /// Add amount to current progress points.
    /// </summary>
    /// <param name="amountArg"></param>
    /// <returns></returns>
    private bool AddToCurrentProgressPoints(int amountArg)
    {
        // add given points to current points
        progressPointsCurrent += amountArg;

        // get points required for the next stat level
        int ptsReq = GetPointRequirementForNextLevel();

        // if have enough points to level up
        if (progressPointsCurrent >= ptsReq)
        {
            // increment stat level
            statLevel++;

            // reduce currently held points by points spent to level up
            progressPointsCurrent -= ptsReq;

            // add the spent points to the used points amount
            progressPointsUsed += ptsReq;

            // return that stat WAS leveled up
            return true;
        }
        // else did NOT have enough points to level up
        else
        {
            // return that stat was NOT leveled up
            return false;
        }
    }

    /// <summary>
    /// Applies the given exp to the stat progression while taking into account the current 
    /// activity points.
    /// </summary>
    /// <param name="expArg"></param>
    /// <returns></returns>
    public bool ApplyExpToProgression(int expArg)
    {
        // get amount to add based on activity (which act as multipliers)
        int amountToAdd = expArg * activityPoints;

        // add calcualted amount to progress points while getting whether this stat leveld up
        bool didLevelUp = AddToCurrentProgressPoints(amountToAdd);

        // reset activity points to zero to denote they have been used
        activityPoints = 0;

        // return whether stat leveled up from this
        return didLevelUp;
    }

    /// <summary>
    /// Returns total points that currenlty have and also previously spent.
    /// </summary>
    /// <returns></returns>
    public int GetTotalLifetimePoints()
    {
        return progressPointsCurrent + progressPointsUsed;
    }

    #endregion


}
