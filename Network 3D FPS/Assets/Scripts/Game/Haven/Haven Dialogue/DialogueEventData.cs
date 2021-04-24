using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEventData
{
    #region Class Variables

    // the ID of the dialogue
    public string dialogueName;

    /// the additional amount of total trait progression points earned among every trait for every 
    /// girl needed for this event. This number is meant to add onto the value of the events that came 
    /// before to get actual requirement, it's done this way because events are supposed to be displayed 
    /// in a fixed order. This is value needed to see how far into the social-sim the player is.
    public int requiredAdditionalGroupTraitProgression;
    /// some social events should only be shown if certain flags are set. This 
    /// most often used for story social events.
    public string requiredFlag;
    /// some events should only be played if right after doing an activity with a 
    /// particular girl because the social event directly pertains to that girl 
    /// and largely that girl alone.
    public string associatedCharId;
    /// the time slot that this event should play at. Will be ignored if event asscoaited with char as 
    /// characters can be hanged out with in any time slot.
    public TimeSlotType associatedTimeSlot;

    #endregion




    #region Constructors

    public DialogueEventData()
    {
        Setup();
    }

    public DialogueEventData(DialogueEventData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        dialogueName = string.Empty;

        requiredAdditionalGroupTraitProgression = 0;
        requiredFlag = string.Empty;
        associatedCharId = string.Empty;
        associatedTimeSlot = TimeSlotType.Morning;
    }

    private void Setup(DialogueEventData templateArg)
    {
        dialogueName = templateArg.dialogueName;

        requiredAdditionalGroupTraitProgression = templateArg.
            requiredAdditionalGroupTraitProgression;
        requiredFlag = templateArg.requiredFlag;
        associatedCharId = templateArg.associatedCharId;
        associatedTimeSlot = templateArg.associatedTimeSlot;
    }

    #endregion




    #region Dialogue Functions

    /// <summary>
    /// Returns bool that denotes if event requires a flag.
    /// </summary>
    /// <returns></returns>
    public bool RequiresFlag()
    {
        return requiredFlag != string.Empty;
    }

    /// <summary>
    /// Returns bool that denotes if the event requires the player 
    /// to be grouped with a certain other character.
    /// </summary>
    /// <returns></returns>
    public bool RequireBeGroupedWithSpecificCharacter()
    {
        return associatedCharId != string.Empty;
    }

    #endregion


}
