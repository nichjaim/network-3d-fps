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
    /// Returns the total points needed for stat to reach the next level.
    /// </summary>
    /// <returns></returns>
    public int GetPointRequirementForNextLevel()
    {
        return GetPointRequirementForStatLevel(statLevel + 1);
    }

    /// <summary>
    /// Add amount to current progress points. 
    /// Returns the levels that were reached, need this info to be able to know what 
    /// stats to improve.
    /// </summary>
    /// <param name="amountArg"></param>
    /// <returns></returns>
    private List<int> AddToCurrentProgressPoints(int amountArg)
    {
        // set new var from given value that will be manipulated throughout func
        int addingExp = amountArg;

        // initialize these vars for upcoming loop
        int ptsReq;
        List<int> levelsReached = new List<int>();
        int expNeededForCurrentLvl;

        // while there is still some given exp left
        while (addingExp > 0)
        {
            // get the total exp needed for next level up
            ptsReq = GetPointRequirementForNextLevel();

            // get the actual exp needed for next level up
            expNeededForCurrentLvl = ptsReq - progressPointsCurrent;

            // if still have enough given enough exp for a level up
            if (addingExp >= expNeededForCurrentLvl)
            {
                // remove the given exp by the amount spent to reach this level
                addingExp -= expNeededForCurrentLvl;

                // increment stat level
                statLevel++;

                // add the new level to the levels that were reached
                levelsReached.Add(statLevel);

                // denote that current prgoress reset to beginning of next level
                progressPointsCurrent = 0;

                // add the spent points to the used points amount
                progressPointsUsed += ptsReq;
            }
            // else do NOT still have enough given exp for a level up
            else
            {
                // add remaining given exp to current level progress
                progressPointsCurrent += addingExp;
            }
        }

        // return the levels that were reached
        return levelsReached;
    }

    /// <summary>
    /// Applies the given exp to the stat progression while taking into account the current 
    /// activity points. 
    /// Returns what levels were reached.
    /// </summary>
    /// <param name="expArg"></param>
    /// <returns></returns>
    public List<int> ApplyExpToProgression(int expArg)
    {
        // get amount to add based on activity (which act as multipliers)
        int amountToAdd = expArg * activityPoints;

        // add calcualted amount to progress points while getting what levels were reached
        List<int> levelsReached = AddToCurrentProgressPoints(amountToAdd);

        // reset activity points to zero to denote they have been used
        activityPoints = 0;

        // return what stat levels were reached due to given exp
        return levelsReached;
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
