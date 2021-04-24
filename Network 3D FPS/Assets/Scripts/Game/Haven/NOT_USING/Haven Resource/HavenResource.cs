using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenResource
{
    #region Class Variables

    public int resourceCountCurrent = 0;
    public int resourceCountMax = 1;

    // the percentage change properties
    public int resourcePercentageIncreaseBase = 1;
    public int resourcePercentageIncreaseBaseRange = 1;

    public int resourcePercentageDecreaseBase = 1;
    public int resourcePercentageDecreaseBaseRange = 1;

    #endregion




    #region Constructors

    public HavenResource()
    {
        Setup();
    }

    public HavenResource(HavenResource templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    public virtual void Setup()
    {
        resourceCountCurrent = 0;
        resourceCountMax = 100;

        resourcePercentageIncreaseBase = 12;
        resourcePercentageIncreaseBaseRange = 2;

        resourcePercentageDecreaseBase = 3;
        resourcePercentageDecreaseBaseRange = 1;
    }

    public virtual void Setup(HavenResource templateArg)
    {
        resourceCountCurrent = templateArg.resourceCountCurrent;
        resourceCountMax = templateArg.resourceCountMax;

        resourcePercentageIncreaseBase = templateArg.resourcePercentageIncreaseBase;
        resourcePercentageIncreaseBaseRange = templateArg.resourcePercentageIncreaseBaseRange;

        resourcePercentageDecreaseBase = templateArg.resourcePercentageDecreaseBase;
        resourcePercentageDecreaseBaseRange = templateArg.resourcePercentageDecreaseBaseRange;
    }

    #endregion




    #region Resource Functions

    /// <summary>
    /// Perform necessary processes on resources when a day has passed.
    /// </summary>
    public virtual void ApplyDayPassing()
    {
        // IMPL in child class
    }

    /// <summary>
    /// Perform necessary processes to add (or begin process of adding) to 
    /// current resources.
    /// </summary>
    public virtual void AddResourceAction()
    {
        // IMPL in child class
    }

    /// <summary>
    /// Returns percentage of resource count currently have.
    /// </summary>
    /// <returns></returns>
    public float GetResourceCountPercentage()
    {
        // need to cast values as float's to ensure float division is done
        return ((float)resourceCountCurrent) / ((float)resourceCountMax);
    }

    /// <summary>
    /// Returns the resource count from the given total percentage.
    /// </summary>
    /// <param name="percentageArg"></param>
    /// <returns></returns>
    protected int GetResourceCountFromPercentage(float percentageArg)
    {
        // get count as unrounded int. NOTE: must cast as float to do float multiplication
        float returnCountUnrounded = ((float)resourceCountMax) * percentageArg;

        // return rounded count
        return Mathf.RoundToInt(returnCountUnrounded);
    }

    /// <summary>
    /// Increases current resources based on associated percentage change.
    /// </summary>
    protected void IncreaseResourceByPercentage()
    {
        // get random INCREASE change percentage
        int percentageChange = GetRandomValueFromRange(resourcePercentageIncreaseBase, 
            resourcePercentageIncreaseBaseRange);

        /// add current resources by add amount, also ensure that current resource does NOT 
        /// go above the max
        resourceCountCurrent = Mathf.Min(resourceCountCurrent + 
            GetResourceCountFromPercentage(percentageChange), resourceCountMax);
    }

    /// <summary>
    /// Decreases current resources based on associated percentage change.
    /// </summary>
    protected void DecreaseResourceByPercentage()
    {
        // get random DECREASE change percentage
        int percentageChange = GetRandomValueFromRange(resourcePercentageDecreaseBase,
            resourcePercentageDecreaseBaseRange);

        /// remove current resources by decrease amount, also ensure that current resource does NOT 
        /// go below zero
        resourceCountCurrent = Mathf.Max(resourceCountCurrent +
            GetResourceCountFromPercentage(percentageChange), 0);
    }

    #endregion




    #region General Functions

    /// <summary>
    /// Returns a random int when given a range to pick from.
    /// </summary>
    /// <param name="valueBaseArg"></param>
    /// <param name="valueRangeArg"></param>
    /// <returns></returns>
    protected int GetRandomValueFromRange(int valueBaseArg, int valueRangeArg)
    {
        // NOTE: must add one to high range due to exclusivity on max end
        return Random.Range(valueBaseArg - valueRangeArg, (valueBaseArg + valueRangeArg) + 1);
    }

    #endregion


}
