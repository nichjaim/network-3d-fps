using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenuNewMenuController : StandardSubMenuController
{
    #region Class Variables

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private NetworkManager _networkManager = null;

    #endregion




    #region Button Press Functions

    public void ButtonPressYes()
    {
        Instantiate(_networkManager.playerPrefab);
        // turn off main menu
        menuMaster.DeactivateMenu();
    }

    public void ButtonPressNo()
    {
        // switch main menu's sub menu to the play menu
        menuMaster.GetComponent<MainMenuController>().SwitchToPlayMenu();
    }

    #endregion


}
