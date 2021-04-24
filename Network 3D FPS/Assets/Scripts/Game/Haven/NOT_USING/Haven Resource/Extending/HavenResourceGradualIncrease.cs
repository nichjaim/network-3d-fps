using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenResourceGradualIncrease : HavenResource
{
    #region Class Variables

    // time till increase phase ends
    public int increasePhaseTimeCurrent = 1;

    // increase state time properties
    public int increasePhaseTimeBase = 1;
    public int increasePhaseTimeBaseRange = 1;

    /// NOTE: this needed so that when first leaving the increase phase, you take 
    /// double the decrease. This is done to ensure that even if they do 
    /// immediately go back into increase, it will take time to recover.
    public bool shouldTakeInitialPhaseLeaveDecrease = false;

    #endregion




    #region Constructors

    public HavenResourceGradualIncrease()
    {
        Setup();
    }

    public HavenResourceGradualIncrease(HavenResourceGradualIncrease templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(HavenResourceGradualIncrease templateArg)
    {
        base.Setup(((HavenResource)templateArg));

        increasePhaseTimeBase = templateArg.increasePhaseTimeBase;
        increasePhaseTimeBaseRange = templateArg.increasePhaseTimeBaseRange;

        increasePhaseTimeCurrent = templateArg.increasePhaseTimeCurrent;
    }

    #endregion




    #region Override Functions

    public override void Setup()
    {
        base.Setup();

        /// set increase to small value as it will only be automtacially done when in 
        /// the charging/increase phase
        resourcePercentageIncreaseBase = 2;
        resourcePercentageIncreaseBaseRange = 1;

        // set decrease to slightly larger size so as to make not charging more hazardous
        resourcePercentageDecreaseBase = 5;
        resourcePercentageDecreaseBaseRange = 1;

        // initialize child class values
        increasePhaseTimeBase = 10;
        increasePhaseTimeBaseRange = 1;

        increasePhaseTimeCurrent = increasePhaseTimeBase;

        shouldTakeInitialPhaseLeaveDecrease = true;
    }

    public override void ApplyDayPassing()
    {
        base.ApplyDayPassing();

        // apply time passage to the current increase phase
        ApplyTimeToIncreasePhase();
    }

    public override void AddResourceAction()
    {
        base.AddResourceAction();

        // starts the increase phase with a random amount of phase time
        StartIncreasePhase();
    }

    #endregion




    #region Specialized Resource Functions

    /// <summary>
    /// Apply time passage to the current increase phase.
    /// </summary>
    private void ApplyTimeToIncreasePhase()
    {
        // if currently increasing
        if (InIncreasePhase())
        {
            // decrease time left in increase phase, while ensuring value doesn't fall below zero
            increasePhaseTimeCurrent = Mathf.Max(increasePhaseTimeCurrent - 1, 0);

            // increases current resources based on associated percentage change
            IncreaseResourceByPercentage();
        }
        // else NOT in the increase phase
        else
        {
            // decreases current resources based on associated percentage change
            DecreaseResourceByPercentage();

            // if should take another decrease as just left the increase phase
            if (shouldTakeInitialPhaseLeaveDecrease)
            {
                // denote that relevant decrease was taken
                shouldTakeInitialPhaseLeaveDecrease = false;

                // decrease again for initial phase leaving decrease chunk
                DecreaseResourceByPercentage();
            }
        }
    }

    /// <summary>
    /// Starts the increase phase with a random amount of phase time.
    /// </summary>
    private void StartIncreasePhase()
    {
        // set time left to random phase time
        increasePhaseTimeCurrent = GetRandomIncreasePhaseTime();

        // denote should take extra decrease upon leaving the phase
        shouldTakeInitialPhaseLeaveDecrease = true;
    }

    /// <summary>
    /// Returns bool that denotes if currently in the increase phase.
    /// </summary>
    /// <returns></returns>
    public bool InIncreasePhase()
    {
        return increasePhaseTimeCurrent > 0;
    }

    /// <summary>
    /// Returns random decrease starting time within appropriate range.
    /// </summary>
    /// <returns></returns>
    private int GetRandomIncreasePhaseTime()
    {
        return GetRandomValueFromRange(increasePhaseTimeBase, increasePhaseTimeBaseRange);
    }

    #endregion


}
