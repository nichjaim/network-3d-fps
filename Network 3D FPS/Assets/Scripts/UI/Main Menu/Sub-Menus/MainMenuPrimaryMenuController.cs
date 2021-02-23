using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPrimaryMenuController : StandardSubMenuController
{
    #region Button Press Functions

    public void ButtonPressPlay()
    {
        // switch main menu's sub menu to the play menu
        menuMaster.GetComponent<MainMenuController>().SwitchToPlayMenu();
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




    /*#region Network Functions

    private void HostServer()
    {
        // if build platform is WebGL (browser)
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // print warning to console
            Debug.LogWarning("WebGL cannot be a server!");
            // DONT continue code
            return;
        }

        // start server
        _networkManager.StartHost();
    }

    private void ClientJoin()
    {
        _networkManager.StartClient();

        // set address to join to the user inputted address
        _networkManager.networkAddress = _inputField.text;

        ClientScene.Ready(NetworkClient.connection);

        if (ClientScene.localPlayer == null)
        {
            ClientScene.AddPlayer(NetworkClient.connection);
        }
    }

    #endregion*/


}
