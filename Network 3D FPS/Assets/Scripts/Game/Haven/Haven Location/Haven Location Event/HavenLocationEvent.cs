using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenLocationEvent
{
    #region Class Variables

    public string eventId;

    //public List<HavenLocation> potentialLocations;
    public HavenLocation locatedSpot;

    [Tooltip("The character assoicated with this event.")]
    public string associatedCharId;
    [Tooltip("Use this for easy selecing of Waifu IDs. Will overwrite " +
        "associated char ID if selected.")]
    public WaifuCharacterType associatedWaifu;

    [Tooltip("The days of the week that this event can appear on. " +
        "Leave empty for no restrictions.")]
    public List<DayOfWeek> associatedDaysOfWeek;

    [Tooltip("The game flags needed for thsi event to appear.")]
    public List<string> requiredFlags;

    [Tooltip("Does event require the waifu asscoaited with this event " +
        "to have the sexually comftorble tag with MC.")]
    public bool requiresAssociatedWaifuSexuallyComfortable;

    public bool isRepeatable;
    public bool focusesOnHcontent;

    #endregion




    #region Constructor Functions

    public HavenLocationEvent()
    {
        Setup();
    }

    public HavenLocationEvent(HavenLocationEvent templateArg)
    {
        Setup(templateArg);
    }

    public HavenLocationEvent(HavenLocationEventTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        eventId = "0";

        //potentialLocations = new List<HavenLocation>();
        locatedSpot = HavenLocation.None;

        associatedCharId = string.Empty;
        associatedWaifu = WaifuCharacterType.None;

        associatedDaysOfWeek = new List<DayOfWeek>();

        requiredFlags = new List<string>();
        requiresAssociatedWaifuSexuallyComfortable = false;

        isRepeatable = true;
        focusesOnHcontent = false;
    }

    private void Setup(HavenLocationEvent templateArg)
    {
        eventId = templateArg.eventId;

        /*potentialLocations = new List<HavenLocation>();
        foreach (HavenLocation iterLoc in templateArg.potentialLocations)
        {
            potentialLocations.Add(iterLoc);
        }*/
        locatedSpot = templateArg.locatedSpot;

        associatedCharId = templateArg.associatedCharId;
        associatedWaifu = templateArg.associatedWaifu;

        associatedDaysOfWeek = new List<DayOfWeek>();
        foreach (DayOfWeek iterDay in templateArg.associatedDaysOfWeek)
        {
            associatedDaysOfWeek.Add(iterDay);
        }

        requiredFlags = new List<string>();
        foreach (string iterFlag in templateArg.requiredFlags)
        {
            requiredFlags.Add(iterFlag);
        }

        requiresAssociatedWaifuSexuallyComfortable = false;

        isRepeatable = templateArg.isRepeatable;
        focusesOnHcontent = templateArg.focusesOnHcontent;
    }

    #endregion




    #region Location Event Functions

    /// <summary>
    /// Returns the appropriate character ID associated with this event.
    /// </summary>
    /// <returns></returns>
    public string GetAppropriateAssociatedCharacterId()
    {
        // if NO selected associated waifu
        if (associatedWaifu == WaifuCharacterType.None)
        {
            // return the custom associated char ID
            return associatedCharId;
        }
        // else a waifu WAS selected
        else
        {
            // return the waifu char ID
            return GeneralMethods.GetWaifuCharacterId(associatedWaifu);
        }
    }

    /// <summary>
    /// Returns list of all flags needed for this event.
    /// </summary>
    /// <param name="gameFlagsArg"></param>
    /// <returns></returns>
    public List<string> GetAppropriateRequiredFlags(GameFlags gameFlagsArg)
    {
        // initialize return list as empty list
        List<string> appropriateFlags = new List<string>();

        // add all the required flags to return list
        foreach (string iterFlag in requiredFlags)
        {
            appropriateFlags.Add(iterFlag);
        }

        // if need flag that denotes if waifu is sexually comfortable with MC
        if (requiresAssociatedWaifuSexuallyComfortable)
        {
            // get the waifu's char ID
            string waifuId = GeneralMethods.GetWaifuCharacterId(associatedWaifu);

            // add waifu's sex comfort flag
            appropriateFlags.Add(gameFlagsArg.GetSexuallyComfortableFlag(waifuId));
        }

        // return the setup list of flags
        return appropriateFlags;
    }

    /// <summary>
    /// Returns whether the event needs a flag.
    /// </summary>
    /// <returns></returns>
    public bool RequiresFlag()
    {
        return (requiredFlags.Count > 0) || (requiresAssociatedWaifuSexuallyComfortable);
    }

    /// <summary>
    /// Retursn whether the event needs to be on certain days of the week.
    /// </summary>
    /// <returns></returns>
    public bool RequiresCertainDayOfWeek()
    {
        return associatedDaysOfWeek.Count > 0;
    }

    #endregion


}
