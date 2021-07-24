using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour, /*MMEventListener<GameStateModeTransitionEvent>,*/
    MMEventListener<LevelArenaExitEvent>, MMEventListener<ReturnToHavenEvent>
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private LevelArenaCoordinator arenaCoordinator = null;

    [SerializeField]
    private NavMeshSurface _navMeshSurface = null;

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

        // destroys the currenly instantiated arena
        arenaCoordinator.DestoryArena();
    }

    #endregion




    #region Arena Functions

    /// <summary>
    /// Resets all arena run's progress data to new run.
    /// </summary>
    public void ResetArenaProgress()
    {
        arenaCoordinator.ResetRunProgress();
    }

    /// <summary>
    /// Creates appropriate level arena and sets up it's navigation surface.
    /// </summary>
    public void BuildArena()
    {
        arenaCoordinator.CreateAppropriateArena();

        // setup the navigation mesh surface. 
        RefreshNavMeshSurfaceBake();
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

    /// <summary>
    /// Returns the progress percentage within the current set.
    /// </summary>
    /// <returns></returns>
    public float GetSetProgressPercentage()
    {
        return arenaCoordinator.ArenaNumInSet / 
            arenaCoordinator.ArenaSetData.totalArenasInSet;
    }

    /// <summary>
    /// Sets up the navigation mesh surface. 
    /// Call when a new arena has been created.
    /// </summary>
    private void RefreshNavMeshSurfaceBake()
    {
        // removes any previously made nav mesh bake (I think, not actually sure???)
        _navMeshSurface.RemoveData();

        // creates a new nav mesh
        _navMeshSurface.BuildNavMesh();
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
        this.MMEventStartListening<ReturnToHavenEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        //this.MMEventStopListening<GameStateModeTransitionEvent>();
        this.MMEventStopListening<LevelArenaExitEvent>();
        this.MMEventStopListening<ReturnToHavenEvent>();
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

    public void OnMMEvent(ReturnToHavenEvent eventType)
    {
        // destroy the currenly instantiated level arena
        arenaCoordinator.DestoryArena();

        // resets all arena run's progress data to new run
        ResetArenaProgress();
    }

    #endregion


}
