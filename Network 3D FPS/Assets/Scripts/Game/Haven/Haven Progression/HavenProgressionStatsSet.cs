using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenProgressionStatsSet
{
    #region Class Variables

    /// the various stat types to their respective total accrued points. 
    /// Stat type names are (in order): Vitality, Proficiency, Athletics, Utility.
    public List<HavenProgressionStatData> statTypesToStatData;

    #endregion




    #region Constructors

    public HavenProgressionStatsSet()
    {
        Setup();
    }

    public HavenProgressionStatsSet(HavenProgressionStatsSet templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        int NUM_OF_STAT_TYPES = 4;

        statTypesToStatData = new List<HavenProgressionStatData>();
        for (int i = 0; i < NUM_OF_STAT_TYPES; i++)
        {
            statTypesToStatData.Add(new HavenProgressionStatData());
        }
    }

    private void Setup(HavenProgressionStatsSet templateArg)
    {
        statTypesToStatData = new List<HavenProgressionStatData>();
        foreach (HavenProgressionStatData iterData in templateArg.statTypesToStatData)
        {
            statTypesToStatData.Add(new HavenProgressionStatData(iterData));
        }
    }

    #endregion




    #region Progression Functions

    /*/// <summary>
    /// Returns the currently held level and the points have for the upcoming level.
    /// </summary>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    public (int, int) GetCurrentStatLevelAndPointsForUpcomingLevel(int statTypeNumArg)
    {
        // get currently held number of points in associated stat type
        int ptsRemaining = statTypesToStatPoints[statTypeNumArg - 1];

        // initialize vars for upcoming loop
        int returnLevel = 1;
        int iterPtsReq;

        // loop as many times as needed
        while (true)
        {
            // get the next level's point requirement
            iterPtsReq = GetPointRequirementForStatLevel(returnLevel + 1);

            // if remaning number of points has not reached point requirement
            if (ptsRemaining < iterPtsReq)
            {
                // return current stat level and points have for upcoming level
                return (returnLevel, ptsRemaining);
            }

            // reduce remaining points by point requirement
            ptsRemaining -= iterPtsReq;
            // increment the current stat level
            returnLevel++;
        }
    }*/

    /*/// <summary>
    /// Returns point requirement to attain the given level.
    /// </summary>
    /// <param name="statLevelArg"></param>
    /// <returns></returns>
    public int GetPointRequirementForStatLevel(int statLevelArg)
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
    }*/

    /// <summary>
    /// Returns progress stat data assoicated with given stat type.
    /// </summary>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    public HavenProgressionStatData GetProgressStatData(int statTypeNumArg)
    {
        return statTypesToStatData[statTypeNumArg - 1];
    }

    public List<HavenProgressionStatData> GetAllProgressStatData()
    {
        return statTypesToStatData;
    }

    /// <summary>
    /// Returns total stat points for all stat types.
    /// </summary>
    /// <returns></returns>
    public int GetCumulativeStatPoints()
    {
        // initialize total points
        int totalPts = 0;

        // loop through all stat types
        foreach (HavenProgressionStatData iterData in statTypesToStatData)
        {
            // add iterating stat type's lifetime stat points to total points
            totalPts += iterData.GetTotalLifetimePoints();
        }

        // return calcualted number of points
        return totalPts;
    }

    #endregion


}
