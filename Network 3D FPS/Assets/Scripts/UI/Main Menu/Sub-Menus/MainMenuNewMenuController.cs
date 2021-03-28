using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Nichjaim.MasterSubMenu;

public class MainMenuNewMenuController : SubMenuController
{
    #region Button Press Functions

    public void ButtonPressYes()
    {
        // trigger event to denote that a new game should be started
        StartNewGameEvent.Trigger();
    }

    public void ButtonPressNo()
    {
        // switch main menu's sub menu to the play menu
        ((MainMenuController)menuMaster).SwitchToPlayMenu();
    }

    #endregion


}
