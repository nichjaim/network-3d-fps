using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenResourceBurstDecrease : HavenResource
{
    #region Class Variables

    // time till decrease burst
    public int timeTillDecreaseCurrent = 1;

    // decrease starting time properties
    public int timeTillDecreaseBase = 1;
    public int timeTillDecreaseBaseRange = 1;

    #endregion




    #region Constructors

    public HavenResourceBurstDecrease()
    {
        Setup();
    }

    public HavenResourceBurstDecrease(HavenResourceBurstDecrease templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(HavenResourceBurstDecrease templateArg)
    {
        base.Setup(((HavenResource)templateArg));

        timeTillDecreaseBase = templateArg.timeTillDecreaseBase;
        timeTillDecreaseBaseRange = templateArg.timeTillDecreaseBaseRange;

        timeTillDecreaseCurrent = templateArg.timeTillDecreaseCurrent;
    }

    #endregion




    #region Override Functions

    public override void Setup()
    {
        base.Setup();

        /// set decreasing to large value as it will only be done in 
        /// spaced-out bursts
        resourcePercentageDecreaseBase = 35;
        resourcePercentageDecreaseBaseRange = 5;

        // initialize child class values
        timeTillDecreaseBase = 6;
        timeTillDecreaseBaseRange = 2;

        timeTillDecreaseCurrent = timeTillDecreaseBase;
    }

    public override void ApplyDayPassing()
    {
        base.ApplyDayPassing();

        // apply time slot to the pent-up resource decrease buildup
        ApplyTimeToDecreaseBuildup();
    }

    public override void AddResourceAction()
    {
        base.AddResourceAction();

        // increases current resources based on associated percentage change
        IncreaseResourceByPercentage();
    }

    #endregion




    #region Specialized Resource Functions

    /// <summary>
    /// Apply time slot to the pent-up resource decrease buildup.
    /// </summary>
    private void ApplyTimeToDecreaseBuildup()
    {
        // decrement current decrease time
        timeTillDecreaseCurrent--;

        // if current time buildup
        if (timeTillDecreaseCurrent <= 0)
        {
            // decreases current resources based on associated percentage change
            DecreaseResourceByPercentage();

            // set current time to random starting decrease time
            timeTillDecreaseCurrent = GetRandomDecreaseStartTime();
        }
    }

    /// <summary>
    /// Returns random decrease starting time within appropriate range.
    /// </summary>
    /// <returns></returns>
    private int GetRandomDecreaseStartTime()
    {
        return GetRandomValueFromRange(timeTillDecreaseBase, timeTillDecreaseBaseRange);
    }

    #endregion


}
