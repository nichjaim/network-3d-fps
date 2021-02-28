using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPrimaryMenuController : SubMenuController
{
    #region Button Press Functions

    public void ButtonPressPlay()
    {
        // switch main menu's sub menu to the play menu
        ((MainMenuController)menuMaster).SwitchToPlayMenu();
    }

    public void ButtonPressOptions()
    {
        Debug.LogWarning("Options button not implemented!"); // NEED IMPL
    }

    public void ButtonPressExit()
    {
        Debug.LogWarning("Exit button not implemented!"); // NEED IMPL
    }

    #endregion


}
