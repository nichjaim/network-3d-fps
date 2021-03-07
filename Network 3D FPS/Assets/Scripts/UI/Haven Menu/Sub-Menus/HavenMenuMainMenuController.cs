using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;

public class HavenMenuMainMenuController : SubMenuController
{
    #region Button Press Functions

    public void ButtonPressMultiplayer()
    {
        // switch to haven menu's multiplayer menu
        ((HavenMenuController)menuMaster).SwitchToMultiplayerMenu();
    }

    #endregion


}
