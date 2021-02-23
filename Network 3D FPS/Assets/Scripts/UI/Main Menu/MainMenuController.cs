using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MasterMenuController, MMEventListener<ExitGameSessionEvent>
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




    #region Override Functions

    protected override void Start()
    {
        base.Start();

        SwitchToPrimaryMenu();
    }

    #endregion




    #region Main Menu Controller

    public void SwitchToPrimaryMenu()
    {
        SwitchSubMenu(0);
    }

    public void SwitchToPlayMenu()
    {
        SwitchSubMenu(1);
    }

    public void SwitchToNewMenu()
    {
        SwitchSubMenu(2);
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
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        SwitchToPrimaryMenu();
    }

    #endregion


}
