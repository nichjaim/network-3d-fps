using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data relevant to the player's party (group) home base of operations.
/// </summary>
[System.Serializable]
public class HavenData
{
    #region Class Variables

    // resources acquired in FPS part of game, this resource called 'Bond' in-game
    public int externalResource;

    public TimeCalendarSystem calendarSystem;

    public HavenDialogueData havenDialogue;

    #endregion




    #region Constructors

    public HavenData()
    {
        Setup();
    }

    public HavenData(HavenData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        externalResource = 0;

        calendarSystem = new TimeCalendarSystem();

        havenDialogue = new HavenDialogueData();
    }

    private void Setup(HavenData templateArg)
    {
        externalResource = templateArg.externalResource;

        calendarSystem = new TimeCalendarSystem(templateArg.calendarSystem);

        havenDialogue = new HavenDialogueData(templateArg.havenDialogue);
    }

    #endregion


}
