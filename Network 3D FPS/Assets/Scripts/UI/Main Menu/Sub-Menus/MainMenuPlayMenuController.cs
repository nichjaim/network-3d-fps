using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using Nichjaim.MasterSubMenu;

public class MainMenuPlayMenuController : SubMenuController
{
    #region Class Variables

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private NetworkManager _networkManager = null;

    [SerializeField]
    private TMP_InputField _inputField = null;

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        // set input field text to default joining address
        _inputField.text = _networkManager.networkAddress;
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressNew()
    {
        // switch main menu's sub menu to the new menu
        ((MainMenuController)menuMaster).SwitchToNewMenu();
    }

    public void ButtonPressContinue()
    {
        // switch to main menu's load menu
        ((MainMenuController)menuMaster).SwitchToLoadMenu();
    }

    public void ButtonPressJoin()
    {
        // set network address to inputted address
        _networkManager.networkAddress = _inputField.text;
        // switch to main menu's connect menu
        ((MainMenuController)menuMaster).SwitchToConnectMenu();
    }

    public void ButtonPressBack()
    {
        // switch main menu's sub menu to the primary menu
        menuMaster.GetComponent<MainMenuController>().SwitchToPrimaryMenu();
    }

    #endregion


}
