using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MasterMenuController, MMEventListener<GamePausingActionEvent>
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




    #region Main Menu Controller

    public void SwitchToMainMenu()
    {
        SwitchSubMenu(0);
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<GamePausingActionEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<GamePausingActionEvent>();
    }

    public void OnMMEvent(GamePausingActionEvent eventType)
    {
        // if pause menu is already open
        if (IsMenuOpen())
        {
            // close pause menu
            DeactivateMenu();
        }
        // else pause menu is NOT open
        else
        {
            // open pause menu's main menu
            SwitchToMainMenu();
        }
    }

    #endregion


}
