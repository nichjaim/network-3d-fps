using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;

public class MainMenuLoadDeleteMenuController : SubMenuController
{
    #region Class Variables

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private MainMenuLoadSelectedMenuController _mainMenuLoadSelectedMenuController = null;

    #endregion




    #region Button Press Functions

    public void ButtonPressYes()
    {
        // delete selected save file
        SaveLoadSystem.Delete(_mainMenuLoadSelectedMenuController.GetSaveFileNumber());
        // switch to main menu's load menu
        ((MainMenuController)menuMaster).SwitchToLoadMenu();
    }

    public void ButtonPressNo()
    {
        // switch to main menu's load selected menu
        ((MainMenuController)menuMaster).SwitchToLoadSelectedMenu();
    }

    #endregion


}
