using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Nichjaim.MasterSubMenu;

public class MainMenuConnectMenuController : SubMenuController
{
    #region Class Variables

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private NetworkManager _networkManager = null;

    [SerializeField]
    private TextMeshProUGUI _connectingAddressText = null;

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        // set connecting address text to the address being targeted
        _connectingAddressText.text = _networkManager.networkAddress;
        // start network client
        _networkManager.StartClient();
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressCancel()
    {
        // stop trying to connect
        _networkManager.StopClient();
        // switch to main menu's play menu
        ((MainMenuController)menuMaster).SwitchToPlayMenu();
    }

    #endregion


}
