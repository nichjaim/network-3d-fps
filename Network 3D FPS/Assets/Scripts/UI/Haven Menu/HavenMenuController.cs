using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavenMenuController : MasterMenuControllerCustom, MMEventListener<GameStateModeTransitionEvent>, 
    MMEventListener<NetworkGameLocalJoinedEvent>
{
    #region Class Variables

    [Header("Menu Specialist Properties")]

    [Tooltip("Button that switches to multiplayer sub-menu")]
    [SerializeField]
    private GameObject multiplayerMenuButton = null;

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




    #region Haven Menu Functions

    /// <summary>
    /// Sets up this menu for new joining players if needed.
    /// </summary>
    /// <param name="joiningCharArg"></param>
    private void SetupMenuForJoinerIfAppropriate(PlayerCharacterMasterController 
        joiningCharArg)
    {
        // call internal function as coroutine
        StartCoroutine(SetupMenuForJoinerIfAppropriateInternal(joiningCharArg));
    }

    private IEnumerator SetupMenuForJoinerIfAppropriateInternal(PlayerCharacterMasterController
        joiningCharArg)
    {
        // wait for network properties to be setup
        yield return new WaitForEndOfFrame();

        // if joining player is NOT the host (who should already be in this menu)
        if (joiningCharArg.GetPlayerNumber() != 1)
        {
            // open haven menu's main menu
            SwitchToMainMenu();
            // turn off multiplayer button to prevent access
            multiplayerMenuButton.SetActive(false);
        }
        else
        {
            // turn on multiplayer button to allow access
            multiplayerMenuButton.SetActive(true);
        }
    }

    #endregion




    #region Menu Switch Controller

    public void SwitchToMainMenu()
    {
        SwitchSubMenu("main");
    }

    public void SwitchToMultiplayerMenu()
    {
        SwitchSubMenu("multiplayer");
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
        this.MMEventStartListening<NetworkGameLocalJoinedEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<GameStateModeTransitionEvent>();
        this.MMEventStopListening<NetworkGameLocalJoinedEvent>();
    }

    public void OnMMEvent(GameStateModeTransitionEvent eventType)
    {
        if (eventType.gameMode == GameStateMode.VisualNovel)
        {
            // switch to haven menu's main menu
            SwitchToMainMenu();
        }
        else
        {
            // turn off haven menu
            DeactivateMenu();
        }
    }

    public void OnMMEvent(NetworkGameLocalJoinedEvent eventType)
    {
        // setup this menu for new joining players if needed
        SetupMenuForJoinerIfAppropriate(eventType.joiningChar);
    }

    #endregion


}
