using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class MainMenuPlayMenuController : StandardSubMenuController
{
    #region Class Variables

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private NetworkManager _networkManager = null;

    [SerializeField]
    private TMP_InputField _inputField = null;

    #endregion




    #region Override Functions

    public override void RefreshSubMenu()
    {
        base.RefreshSubMenu();

        // set input field text to default joining address
        _inputField.text = _networkManager.networkAddress;
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressNew()
    {
        // switch main menu's sub menu to the new menu
        menuMaster.GetComponent<MainMenuController>().SwitchToNewMenu();
    }

    public void ButtonPressContinue()
    {
        Debug.LogWarning("Continue button not implemented!"); // NEED IMPL
    }

    public void ButtonPressJoin()
    {
        // start network client
        _networkManager.StartClient();
        // turn off main menu
        menuMaster.DeactivateMenu();
    }

    public void ButtonPressBack()
    {
        // switch main menu's sub menu to the primary menu
        menuMaster.GetComponent<MainMenuController>().SwitchToPrimaryMenu();
    }

    #endregion


}
