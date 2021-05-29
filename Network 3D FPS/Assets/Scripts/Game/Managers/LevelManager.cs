using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, /*MMEventListener<GameStateModeTransitionEvent>,*/
    MMEventListener<LevelArenaExitEvent>
{
    #region Class Variables

    [Header("Component References")]

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




    #region Arena Functions

    public void ResetArenaProgress()
    {
        arenaCoordinator.ResetRunProgress();
    }

    public void CreateAppropriateArena()
    {
        arenaCoordinator.CreateAppropriateArena();
    }

    public int GetArenaNumInSet()
    {
        return arenaCoordinator.ArenaNumInSet;
    }

    /// <summary>
    /// Sets the arena data to the tutorial start.
    /// </summary>
    public void SetupTutorialArenaData()
    {
        arenaCoordinator.SetupTutorialArenaData();
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        //this.MMEventStartListening<GameStateModeTransitionEvent>();
        this.MMEventStartListening<LevelArenaExitEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        //this.MMEventStopListening<GameStateModeTransitionEvent>();
        this.MMEventStopListening<LevelArenaExitEvent>();
    }

    /*public void OnMMEvent(GameStateModeTransitionEvent eventType)
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
    }*/

    public void OnMMEvent(LevelArenaExitEvent eventType)
    {
        // destroy the currenly instantiated level arena
        arenaCoordinator.DestoryArena();

        // increments arena progress to next step
        arenaCoordinator.AdvanceArenaProgress();

        // trigger event to denote that arena progress has been advanced
        ArenaProgressionAdvancedEvent.Trigger(arenaCoordinator);
    }

    #endregion


}
