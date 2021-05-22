using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectActivationController : ObjectActivationController, 
    MMEventListener<LevelArenaExitEvent>, MMEventListener<ReturnToHavenEvent>
{
    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // deactivate object
        SetObjectActivation(false);
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<LevelArenaExitEvent>();
        this.MMEventStartListening<ReturnToHavenEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<LevelArenaExitEvent>();
        this.MMEventStopListening<ReturnToHavenEvent>();
    }

    public void OnMMEvent(LevelArenaExitEvent eventType)
    {
        // deactivate object
        SetObjectActivation(false);
    }

    public void OnMMEvent(ReturnToHavenEvent eventType)
    {
        // deactivate object
        SetObjectActivation(false);
    }

    #endregion


}
