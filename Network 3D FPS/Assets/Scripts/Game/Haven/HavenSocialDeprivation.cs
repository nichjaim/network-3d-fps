using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenSocialDeprivation
{
    #region Class Variables

    /// this keeps track of how many social interaction/hangout time slot activities 
    /// that the player has done that did NOT involve the associated girl.
    /// This is needed to know when a girl has become eligble for a hangout bonus 
    /// (a mechanic to incentivize the player to get to know more than one girl).
    /// List entries are associated with each girl (ex. [0] = waifu #1, [1] = waifu #2, etc.).
    /// Entry content goes like this: int1 = currentDeprivationCount, int2 = deprivationCountGoal.
    /// NOTE: need the current and max ints due to needing them both to calculate the true 
    /// deprivation bonus multiplier.
    public List<SerializableDataIntAndInt> memberDeprivationAndDeprivationThreshold = 
        new List<SerializableDataIntAndInt>();

    // the base for the value that denotes when a member becomes socially deprived
    public int deprivationThresholdBase = 1;
    /// the potential range from the threshold base (ex. if thresholdBase = 5, 
    /// and baseRange = 3, then when actual threshold is calcualted it could 
    /// be: (2,3,4, 5, 6,7,8))
    public int deprivationThresholdBaseRange = 1;

    /// the base for the relationship point value multiplier bonus recieved when interacting 
    /// with a member that is socially deprived
    public float deprivationBonusMultiplierBase = 1f;

    #endregion




    #region Constructors

    public HavenSocialDeprivation()
    {
        Setup();
    }

    public HavenSocialDeprivation(HavenSocialDeprivation templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    public virtual void Setup()
    {
        deprivationThresholdBase = 8;
        deprivationThresholdBaseRange = 2;

        /// NOTE: this code section needs to be done here due to needing threshold values to 
        /// already be set
        memberDeprivationAndDeprivationThreshold = new List<SerializableDataIntAndInt>();
        int NUMBER_OF_MEMBERS = 3;
        for (int i = 0; i < NUMBER_OF_MEMBERS; i++)
        {
            memberDeprivationAndDeprivationThreshold.Add(new SerializableDataIntAndInt(0, 
                deprivationThresholdBase));
        }

        deprivationBonusMultiplierBase = 3f;
    }

    public virtual void Setup(HavenSocialDeprivation templateArg)
    {
        deprivationThresholdBase = templateArg.deprivationThresholdBase;
        deprivationThresholdBaseRange = templateArg.deprivationThresholdBaseRange;

        memberDeprivationAndDeprivationThreshold = new List<SerializableDataIntAndInt>();
        foreach (SerializableDataIntAndInt iterData in templateArg.
            memberDeprivationAndDeprivationThreshold)
        {
            memberDeprivationAndDeprivationThreshold.Add(new SerializableDataIntAndInt(iterData));
        }

        deprivationBonusMultiplierBase = templateArg.deprivationBonusMultiplierBase;
    }

    #endregion




    #region Social Deprivation Functions

    /// <summary>
    /// Returns bool that denotes if given member is social interaction deprived.
    /// </summary>
    /// <param name="memberNumberArg"></param>
    /// <returns></returns>
    public bool isMemberSociallyDeprived(int memberNumberArg)
    {
        // if given invalid member number
        if (memberNumberArg <= 0 || memberNumberArg >
            memberDeprivationAndDeprivationThreshold.Count)
        {
            // print warning to console
            Debug.LogWarning($"Invalid member number given: {memberNumberArg}");

            // return default bool
            return false;
        }

        // get given member's associated deprivation list entry
        SerializableDataIntAndInt deprivationEntry = 
            memberDeprivationAndDeprivationThreshold[memberNumberArg - 1];

        /// return whether currenlty held deprivation hit the threshold that dictates 
        /// whether member is actually deprived
        return deprivationEntry.valueInt1 >= deprivationEntry.valueInt2;
    }

    /// <summary>
    /// Resets/Increments social deprivation depending on who was interacted with.
    /// </summary>
    /// <param name="memberNumberInteracteeArg"></param>
    public void ApplySocialDeprivation(int memberNumberInteracteeArg)
    {
        // loop through all potentially socially deprived party members
        for (int i = 0; i < memberDeprivationAndDeprivationThreshold.Count; i++)
        {
            // if iterating member is the member that was socially interacted with
            if ((i + 1) == memberNumberInteracteeArg)
            {
                /// reset iterating member's social deprivation to denote they are no longer 
                /// have any social deprivation
                memberDeprivationAndDeprivationThreshold[i].valueInt1 = 0;
                // get new random deprivation threshold for iterating member
                memberDeprivationAndDeprivationThreshold[i].valueInt2 = GetRandomDeprivationThreshold();
            }
            // else iterating member is NOT the member that was socially interacted with
            else
            {
                // increment iterating member's social deprivation
                memberDeprivationAndDeprivationThreshold[i].valueInt1++;
            }
        }
    }

    /// <summary>
    /// Returns a random deprivation threshold value within the appropriate 
    /// threshold range.
    /// </summary>
    /// <returns></returns>
    private int GetRandomDeprivationThreshold()
    {
        /// NOTE: have to add 1 to max value due to int random range being exclusive 
        /// to the high limit value
        return Random.Range(deprivationThresholdBase - deprivationThresholdBaseRange, 
            (deprivationThresholdBase + deprivationThresholdBaseRange) + 1);
    }

    /// <summary>
    /// Returns deprivation bonus multiplier value based on how socially deprived the member is.
    /// </summary>
    /// <param name="memberNumberArg"></param>
    /// <returns></returns>
    public float GetTrueDeprivationBonusMultiplier(int memberNumberArg)
    {
        // if given invalid member number
        if (memberNumberArg <= 0 || memberNumberArg >
            memberDeprivationAndDeprivationThreshold.Count)
        {
            // print warning to console
            Debug.LogWarning($"Invalid member number given: {memberNumberArg}");

            // return default float
            return 1f;
        }

        // get given member's associated deprivation list entry
        SerializableDataIntAndInt deprivationEntry =
            memberDeprivationAndDeprivationThreshold[memberNumberArg - 1];

        /// get how many times deprivation has passed the threshold, as unrounded float value. 
        /// NOTE: must cast as floats to do float divison
        float deprivationSeverityUnrounded = ((float)deprivationEntry.valueInt1) / 
            ((float)deprivationEntry.valueInt2);

        // round it DOWN to nearest int
        int deprivationSeverity = Mathf.FloorToInt(deprivationSeverityUnrounded);

        /// get final value while ensuring it's not below zero. NOTE: have to reduce by 
        /// one due to not wanting to count the first threshold pass
        deprivationSeverity = Mathf.Max(deprivationSeverity - 1, 0);

        // return severity value added to base bonus multiplier
        return deprivationBonusMultiplierBase + ((float)deprivationSeverity);
    }

    #endregion


}
