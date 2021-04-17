using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenResourceBurstIncrease : HavenResource
{
    #region Class Variables

    // the days left on the planted food seeds
    public List<int> growingResourceBundlesRemainingTime = new List<int>();

    #endregion




    #region Constructors

    public HavenResourceBurstIncrease()
    {
        Setup();
    }

    public HavenResourceBurstIncrease(HavenResourceBurstIncrease templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(HavenResourceBurstIncrease templateArg)
    {
        base.Setup(((HavenResource)templateArg));

        growingResourceBundlesRemainingTime = new List<int>();
        foreach (int iterInt in templateArg.growingResourceBundlesRemainingTime)
        {
            growingResourceBundlesRemainingTime.Add(iterInt);
        }
    }

    #endregion




    #region Override Functions

    public override void Setup()
    {
        base.Setup();

        resourcePercentageIncreaseBase = 35;
        resourcePercentageIncreaseBaseRange = 5;
    }

    public override void ApplyDayPassing()
    {
        base.ApplyDayPassing();

        // decreases current resources based on associated percentage change
        DecreaseResourceByPercentage();

        // age bundle timers by one and reap any grown bundles
        ApplyTimePassOnGrowingResourceBundles();
    }

    public override void AddResourceAction()
    {
        base.AddResourceAction();

        // adds a new bundle to bundle growth list
        PlantBundle();
    }

    #endregion




    #region Specialized Resource Functions

    /// <summary>
    /// Age bundle timers by one and reap any grown bundles.
    /// </summary>
    private void ApplyTimePassOnGrowingResourceBundles()
    {
        // loop through all growing resources
        for (int i = 0; i < growingResourceBundlesRemainingTime.Count; i++)
        {
            // decrement iterating reamaining grow time
            growingResourceBundlesRemainingTime[i]--;

            // if iterating resource is finsihed growing
            if (growingResourceBundlesRemainingTime[i] <= 0)
            {
                // increases current resources based on associated percentage change
                IncreaseResourceByPercentage();
            }
        }

        /// remove all finished grown resources from list. NOTE: have to do it here due 
        /// to it being difficult to remove from a list that is being iterated through.
        growingResourceBundlesRemainingTime.RemoveAll(iterTime => iterTime <= 0);
    }

    /// <summary>
    /// Adds a new bundle to bundle growth list.
    /// </summary>
    private void PlantBundle()
    {
        // initialize how amny days it takes for a bundle to grow
        int GROW_DAYS_TIME = 5;

        // add a bundle with timer to growing list
        growingResourceBundlesRemainingTime.Add(GROW_DAYS_TIME);
    }

    /// <summary>
    /// Return how many bundles are currently planted.
    /// </summary>
    /// <returns></returns>
    public int GetBundlesPlantedCount()
    {
        return growingResourceBundlesRemainingTime.Count;
    }

    #endregion


}
