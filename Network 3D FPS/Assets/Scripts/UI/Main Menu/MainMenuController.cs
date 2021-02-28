using MoreMountains.Tools;
using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MasterMenuController, MMEventListener<ExitGameSessionEvent>, 
    MMEventListener<NetworkGameJoinedEvent>, MMEventListener<NewSaveCreatedEvent>
{
    #region MonoBehaviour Functions

    private void Start()
    {
        // start game with main menu's primary menu
        SwitchToPrimaryMenu();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Main Menu Controller

    public void SwitchToPrimaryMenu()
    {
        SwitchSubMenu("primary");
    }

    public void SwitchToPlayMenu()
    {
        SwitchSubMenu("play");
    }

    public void SwitchToNewMenu()
    {
        SwitchSubMenu("new");
    }

    public void SwitchToConnectMenu()
    {
        SwitchSubMenu("connect");
    }

    public void SwitchToLoadMenu()
    {
        SwitchSubMenu("load");
    }

    public void SwitchToLoadSelectedMenu()
    {
        SwitchSubMenu("loadselected");
    }

    public void SwitchToLoadDeleteMenu()
    {
        SwitchSubMenu("loaddelete");
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<ExitGameSessionEvent>();
        this.MMEventStartListening<NetworkGameJoinedEvent>();
        this.MMEventStartListening<NewSaveCreatedEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
        this.MMEventStopListening<NetworkGameJoinedEvent>();
        this.MMEventStopListening<NewSaveCreatedEvent>();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        SwitchToPrimaryMenu();
    }

    public void OnMMEvent(NetworkGameJoinedEvent eventType)
    {
        // turn of this menu
        DeactivateMenu();
    }

    public void OnMMEvent(NewSaveCreatedEvent eventType)
    {
        // turn off this menu
        DeactivateMenu();
    }

    #endregion


}
