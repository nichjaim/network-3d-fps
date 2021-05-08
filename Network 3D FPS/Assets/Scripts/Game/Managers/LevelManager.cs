using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, MMEventListener<GameStateModeTransitionEvent>
{
    #region Class Variables

    [SerializeField]
    private LevelArenaCoordinator arenaCoordinator = null;

    #endregion




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




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<GameStateModeTransitionEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<GameStateModeTransitionEvent>();
    }

    public void OnMMEvent(GameStateModeTransitionEvent eventType)
    {
        switch (eventType.gameMode)
        {
            case GameStateMode.Shooter:

                switch (eventType.transitionProgress)
                {
                    case ActionProgressType.Started:
                        // begin a fresh new arena run
                        arenaCoordinator.StartNewRun();
                        break;

                    case ActionProgressType.Finished:
                        // destroy the currenly instantiated level arena
                        arenaCoordinator.DestoryArena();
                        break;
                }

                break;

            case GameStateMode.VisualNovel:

                // destroy the currenly instantiated level arena
                arenaCoordinator.DestoryArena();

                break;
        }
    }

    #endregion


}
