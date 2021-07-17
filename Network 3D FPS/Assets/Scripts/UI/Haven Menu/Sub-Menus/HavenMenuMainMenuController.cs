using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;

public class HavenMenuMainMenuController : HavenSubMenuController
{
    [SerializeField]
    private DialogueGameCoordinator dialgGameCoordr = null; // TESTING VAR!!!

    #region Button Press Functions

    public void ButtonPressMultiplayer()
    {
        // switch to haven menu's multiplayer menu
        ((HavenMenuController)menuMaster).SwitchToMultiplayerMenu();
    }

    public void ButtonPressGoToRooftops()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.Rooftops);
    }

    public void ButtonPressGoToLibrary()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.Library);
    }

    public void ButtonPressGoToGym()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.Gym);
    }

    public void ButtonPressGoToField()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.Field);
    }

    public void ButtonPressGoToCafeteria()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.Cafeteria);
    }

    public void ButtonPressGoToPool()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.Pool);
    }

    public void ButtonPressGoToClubroomFloor()
    {
        dialgGameCoordr.PlayHavenLocationDialogue(HavenLocation.ClubroomFloor);
    }

    #endregion


}
