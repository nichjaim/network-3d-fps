using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;

public class HavenMenuMainMenuController : HavenSubMenuController
{
    /*[SerializeField]
    private DialogueGameCoordinator dialgGameCoordr = null; // TESTING VAR!!!*/

    #region Button Press Functions

    public void ButtonPressMultiplayer()
    {
        // switch to haven menu's multiplayer menu
        ((HavenMenuController)menuMaster).SwitchToMultiplayerMenu();
    }

    public void ButtonPressGoToRooftops()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.Rooftops);
    }

    public void ButtonPressGoToLibrary()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.Library);
    }

    public void ButtonPressGoToGym()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.Gym);
    }

    public void ButtonPressGoToField()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.Field);
    }

    public void ButtonPressGoToCafeteria()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.Cafeteria);
    }

    public void ButtonPressGoToPool()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.Pool);
    }

    public void ButtonPressGoToClubroomFloor()
    {
        _dialgGameCoord.PlayHavenLocationDialogue(HavenLocation.ClubroomFloor);
    }

    #endregion


}
