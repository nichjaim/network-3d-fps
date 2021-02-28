using MoreMountains.Tools;
using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavenMenuController : MasterMenuController, MMEventListener<StartHavenGameStateEvent>
{
    #region MonoBehaviour Functions

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




    #region Haven Menu Controller

    public void SwitchToMainMenu()
    {
        SwitchSubMenu("main");
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<StartHavenGameStateEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<StartHavenGameStateEvent>();
    }

    public void OnMMEvent(StartHavenGameStateEvent eventType)
    {
        // switch to haven menu's main menu
        SwitchToMainMenu();
    }

    #endregion


}
