using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HavenProgressionData
{
    #region Class Variables

    /// Exp gotten in most recent run of FPS section of game, called 'Bond' in-game. 
    /// Used calculate activity progression points.
    public int externalExp;

    public List<SerializableDataStringAndHavenProgressionStatsSet> 
        charIdToProgressionStats;

    #endregion




    #region Constructors

    public HavenProgressionData()
    {
        Setup();
    }

    public HavenProgressionData(HavenProgressionData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        externalExp = 0;

        charIdToProgressionStats = new 
            List<SerializableDataStringAndHavenProgressionStatsSet>();
    }

    private void Setup(HavenProgressionData templateArg)
    {
        externalExp = templateArg.externalExp;

        charIdToProgressionStats = new 
            List<SerializableDataStringAndHavenProgressionStatsSet>();
        foreach (SerializableDataStringAndHavenProgressionStatsSet iterData 
            in templateArg.charIdToProgressionStats)
        {
            charIdToProgressionStats.Add(new
                SerializableDataStringAndHavenProgressionStatsSet(iterData));
        }
    }

    #endregion




    #region External Exp Functions

    /// <summary>
    /// Sets the extenral exp amount based on the given percentage value.
    /// </summary>
    /// <param name="percentArg"></param>
    private void SetExternalExpFromPercentage(float percentArg)
    {
        // get amount to set to exp amount based on given percentage, as a float
        float reducedAmountUnrounded = ((float)externalExp) * percentArg;

        // round value to nearest integer
        int reducedAmount = Mathf.RoundToInt(reducedAmountUnrounded);

        // set exp amount to calcualted int, while ensuring it doesn't fall below zero
        externalExp = Mathf.Max(reducedAmount, 0);
    }

    /// <summary>
    /// Reduces current amount by the party wipe penalty.
    /// </summary>
    public void InflictPartyWipePenalty()
    {
        // initialize penalty value
        float PERCENTAGE_TO_KEEP = 0.25f;

        // set current amount based on penalty value
        SetExternalExpFromPercentage(PERCENTAGE_TO_KEEP);
    }

    /// <summary>
    /// Resets the external exp amount to zero.
    /// </summary>
    public void ResetExternalExpAmount()
    {
        externalExp = 0;
    }

    #endregion




    #region Progression Functions

    /// <summary>
    /// Returns the progress stats set associated with the given character ID.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private HavenProgressionStatsSet GetCharProgressStatsSet(string charIdArg)
    {
        // get serial data that is asscoiated with given char ID
        SerializableDataStringAndHavenProgressionStatsSet matchingEntry = 
            charIdToProgressionStats.FirstOrDefault(
            iterData => iterData.value1 == charIdArg);

        // if match found
        if (matchingEntry != null)
        {
            // return match's prog stat data
            return matchingEntry.value2;
        }
        // else NO match was found
        else
        {
            // return NULL prog stat data
            return null;
        }
    }

    /// <summary>
    /// Returns the progress stats data associated with the given character ID and stat type.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    private HavenProgressionStatData GetCharProgressStatData(string charIdArg, int statTypeNumArg)
    {
        // get progress stat set associated with given character ID
        HavenProgressionStatsSet progStatsSet = GetCharProgressStatsSet(charIdArg);

        // if associated progress stat set actually existed
        if (progStatsSet != null)
        {
            // return stat type's progress stat data
            return progStatsSet.GetProgressStatData(statTypeNumArg);
        }
        // else data NOT found
        else
        {
            // return NULL data
            return null;
        }
    }

    /// <summary>
    /// Get the current points held used to reach the next level for the given char's given stat.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    public int GetCharProgressStatCurrentPoints(string charIdArg, int statTypeNumArg)
    {
        // get char's stat type's progress stat data
        HavenProgressionStatData progStatData = GetCharProgressStatData(charIdArg, statTypeNumArg);

        // if data successfully retreieved
        if (progStatData != null)
        {
            return progStatData.progressPointsCurrent;
        }
        // else data NOT found
        else
        {
            // return some invalid point amount
            return -1;
        }
    }

    /// <summary>
    /// Get the point requirement for the given char's given stat to reach the next level.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    public int GetCharProgressStatNextLevelPointReq(string charIdArg, int statTypeNumArg)
    {
        // get char's stat type's progress stat data
        HavenProgressionStatData progStatData = GetCharProgressStatData(charIdArg, statTypeNumArg);

        // if data successfully retreieved
        if (progStatData != null)
        {
            return progStatData.GetPointRequirementForNextLevel();
        }
        // else data NOT found
        else
        {
            // return some invalid point amount
            return -1;
        }
    }

    /// <summary>
    /// Get the current stat level for the given char's given stat.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    public int GetCharProgressStatLevel(string charIdArg, int statTypeNumArg)
    {
        // get char's stat type's progress stat data
        HavenProgressionStatData progStatData = GetCharProgressStatData(charIdArg, statTypeNumArg);

        // if data successfully retreieved
        if (progStatData != null)
        {
            return progStatData.statLevel;
        }
        // else data NOT found
        else
        {
            // return some invalid level
            return -1;
        }
    }

    /// <summary>
    /// Applies the external exp to the given char's given stat's progression.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="statTypeNumArg"></param>
    /// <returns></returns>
    public bool ApplyExternalExpToCharProgress(string charIdArg, int statTypeNumArg)
    {
        // get char's stat type's progress stat data
        HavenProgressionStatData progStatData = GetCharProgressStatData(charIdArg, statTypeNumArg);

        // if data successfully retreived
        if (progStatData != null)
        {
            return progStatData.ApplyExpToProgression(externalExp);
        }
        // else data NOT found
        else
        {
            // return some default bool
            return false;
        }
    }

    /// <summary>
    /// Returns average total stat points for all stat types.
    /// </summary>
    /// <returns></returns>
    public int GetAverageForAllCumulativeStatPoints()
    {
        // initialize total points
        int totalPts = 0;

        // loop through all stat sets
        foreach (SerializableDataStringAndHavenProgressionStatsSet iterData in charIdToProgressionStats)
        {
            // add iterating set's cumulative points to total points
            totalPts += iterData.value2.GetCumulativeStatPoints();
        }

        // initialize vars for upcoming conditional
        int averagePts = 0;
        int numOfProgChars = charIdToProgressionStats.Count;

        // if there are some progression characters
        if (numOfProgChars > 0)
        {
            // get the average number of points, as a float
            float averagePtsUnrounded = ((float)totalPts) / ((float)numOfProgChars);
            // round average points
            averagePts = Mathf.RoundToInt(averagePtsUnrounded);
        }

        // return calcualted average number of points
        return averagePts;
    }

    #endregion


}
