﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;

public class HavenMenuMainMenuController : SubMenuController
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
        dialgGameCoordr.PlayDialogueTestingConversation(); // TEST LINE!!!
    }

    #endregion


}
