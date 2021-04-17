using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenResourcesCollection
{
    #region Class Variables

    /// NOTE: need to have child resource classes as just keeping parent 
    /// will cause data to not be saved when put into json
    
    // Food resource, must plant seeds and will only give burst of food when finally grown
    public HavenResourceBurstIncrease havenResource1 = null;
    /// Water resource, the resource with completely normal behaviour. Gradual decrease 
    /// and instant increase when player spends time getting some
    public HavenResourceStandard havenResource2 = null;
    /// Barricade resource, denotes integrity of main base's barricade. Will get decrease 
    /// bursts due to passing zombie herds
    public HavenResourceBurstDecrease havenResource3 = null;
    /// Electricity resource, when fixing the solar panel it will start gathering power over 
    /// time but takes awhile
    public HavenResourceGradualIncrease havenResource4 = null;

    #endregion




    #region Constructors

    public HavenResourcesCollection()
    {
        Setup();
    }

    public HavenResourcesCollection(HavenResourcesCollection templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    public virtual void Setup()
    {
        havenResource1 = new HavenResourceBurstIncrease();
        havenResource2 = new HavenResourceStandard();
        havenResource3 = new HavenResourceBurstDecrease();
        havenResource4 = new HavenResourceGradualIncrease();
    }

    public virtual void Setup(HavenResourcesCollection templateArg)
    {
        havenResource1 = new HavenResourceBurstIncrease(templateArg.havenResource1);
        havenResource2 = new HavenResourceStandard(templateArg.havenResource2);
        havenResource3 = new HavenResourceBurstDecrease(templateArg.havenResource3);
        havenResource4 = new HavenResourceGradualIncrease(templateArg.havenResource4);
    }

    #endregion




    #region Resource Functions

    /// <summary>
    /// Perform necessary processes on resources when a day has passed.
    /// </summary>
    public void ApplyDayPassing()
    {
        havenResource1.ApplyDayPassing();
        havenResource2.ApplyDayPassing();
        havenResource3.ApplyDayPassing();
        havenResource4.ApplyDayPassing();
    }

    /// <summary>
    /// Perform necessary processes to add (or begin process of adding) to 
    /// resources associated with given number.
    /// </summary>
    /// <param name="havenResourceNumArg"></param>
    public void AddResourceAction(int havenResourceNumArg)
    {
        // check cases based on given resource num
        switch (havenResourceNumArg)
        {
            case 1:
                havenResource1.AddResourceAction();
                break;
            case 2:
                havenResource2.AddResourceAction();
                break;
            case 3:
                havenResource3.AddResourceAction();
                break;
            case 4:
                havenResource4.AddResourceAction();
                break;
            default:
                // print warning to log
                Debug.LogWarning("Invalid havenResourceNumArg given: " +
                    $"{havenResourceNumArg}");
                break;
        }
    }

    /// <summary>
    /// Returns resource status effect based on current state of haven resources.
    /// </summary>
    /// <returns></returns>
    public StatusEffectHavenResources GetResourceStatusEffect()
    {
        // initialize effect change value
        float effectChangePercent = 0f;

        // add effect change percentages based on current state of resources
        effectChangePercent += GetStatusEffectChangePercentage(havenResource1.
            GetResourceCountPercentage());
        effectChangePercent += GetStatusEffectChangePercentage(havenResource2.
            GetResourceCountPercentage());
        effectChangePercent += GetStatusEffectChangePercentage(havenResource3.
            GetResourceCountPercentage());
        effectChangePercent += GetStatusEffectChangePercentage(havenResource4.
            GetResourceCountPercentage());

        // return resource status effect based on calcualted change value
        return new StatusEffectHavenResources(effectChangePercent);
    }

    /// <summary>
    /// Returns the resource status effects's effect change value based on given 
    /// resource percentage.
    /// </summary>
    /// <param name="resourcePercArg"></param>
    /// <returns></returns>
    private float GetStatusEffectChangePercentage(float resourcePercArg)
    {
        // initialize effect change properties

        float RESOURCE_EFFECT_CHANGE_LOW = -0.5f;
        float RESOURCE_EFFECT_CHANGE_HIGH = 0.3f;

        float returnChange = 0f;

        // if given percentage is LOW, decrease based change value based on how low
        if (resourcePercArg <= 0.15f)
        {
            returnChange += RESOURCE_EFFECT_CHANGE_LOW;
        }
        if (resourcePercArg <= 0.3f)
        {
            returnChange += RESOURCE_EFFECT_CHANGE_LOW;
        }
        if (resourcePercArg <= 0.45f)
        {
            returnChange += RESOURCE_EFFECT_CHANGE_LOW;
        }

        // if given percentage is HIGH, increase based change value based on how high
        if (resourcePercArg >= 0.8f)
        {
            returnChange += RESOURCE_EFFECT_CHANGE_HIGH;
        }
        if (resourcePercArg >= 0.9f)
        {
            returnChange += RESOURCE_EFFECT_CHANGE_HIGH;
        }
        if (resourcePercArg >= 1f)
        {
            returnChange += RESOURCE_EFFECT_CHANGE_HIGH;
        }

        return returnChange;
    }

    /// <summary>
    /// Returns how many food seeds are currently planted.
    /// </summary>
    /// <returns></returns>
    public int GetHowManyFoodSeedsPlanted()
    {
        return havenResource1.GetBundlesPlantedCount();
    }

    /// <summary>
    /// Returns bool that denotes if the electricity is currently charging.
    /// </summary>
    /// <returns></returns>
    public bool IsElectricityCharging()
    {
        return havenResource4.InIncreasePhase();
    }

    #endregion


}
