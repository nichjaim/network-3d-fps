using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HavenMenuController : MasterMenuControllerCustom, MMEventListener<GameStateModeTransitionEvent>, 
    MMEventListener<NetworkGameLocalJoinedEvent>
{
    #region Class Variables

    private GameManager _gameManager = null;
    public GameManager _GameManager
    {
        get { return _gameManager; }
    }

    private UIManager _uiManager = null;
    public UIManager _UiManager
    {
        get { return _uiManager; }
    }

    [Header("Menu Specialist Properties")]

    [Tooltip("Button that switches to multiplayer sub-menu")]
    [SerializeField]
    private GameObject multiplayerMenuButton = null;

    [Tooltip("Controller for the haven HUD calendar")]
    [SerializeField]
    private HavenMenuHudCalendarController havenHudCalendar = null;

    [Tooltip("The controller for the popup menu that has the scheduel info.")]
    [SerializeField]
    private HavenMenuSchedulePopupController havenSchedulePopup = null;

    [Header("Background Transition References")]

    [SerializeField]
    private BackgroundFadeController bgFader = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup vars that hold reference to singletons
        InitializeSingletonReferences();
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




    #region Initialization Functions

    /// <summary>
    /// Sets up vars that hold reference to singletons. 
    /// Call in Start(), to give time for singletons to instantiate.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;
    }

    #endregion




    #region Override Functions

    protected override void SetMenuActivation(bool activeArg)
    {
        base.SetMenuActivation(activeArg);

        // if haven menu is being turned on
        if (activeArg)
        {
            // set all the HUD calendar properties appropriately
            havenHudCalendar.RefreshHudCalendar();

            // deactivate schedule popup menu
            havenSchedulePopup.MenuDeactivate();
        }
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
            SwitchToStartingMenu();
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

    public void SwitchToStartingMenu()
    {
        SwitchToHavenLocationMenu(HavenLocation.FrontHallway1F);
    }

    public void SwitchToMultiplayerMenu()
    {
        SwitchSubMenu("multiplayer");
    }

    /// <summary>
    /// Switch to haven sub-menu associated with given location.
    /// </summary>
    /// <param name="locationArg"></param>
    public void SwitchToHavenLocationMenu(HavenLocation locationArg)
    {
        SwitchSubMenu(locationArg.ToString());
    }

    private void PlayActivitesLocationDialogue(HavenLocation locationArg)
    {
        _uiManager.DialgGameCoordr.PlayHavenLocationDialogue(locationArg);
    }

    #endregion




    #region Transition Functions

    /// <summary>
    /// Performs the background transition.
    /// </summary>
    /// <param name="newBgArg"></param>
    /// <param name="transitionTimeArg"></param>
    public void BackgroundTransition(Sprite newBgArg, float transitionTimeArg)
    {
        bgFader.BackgroundTransition(newBgArg, transitionTimeArg);
    }

    #endregion




    #region Switch Location First-Floor Functions

    public void SwitchToHavenLocationMenuFrontHallway1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.FrontHallway1F);
    }

    public void SwitchToHavenLocationMenuBackHallway1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.BackHallway1F);
    }

    public void SwitchToHavenLocationMenuRightHallway1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.RightHallway1F);
    }

    public void SwitchToHavenLocationMenuLeftHallway1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.LeftHallway1F);
    }

    public void SwitchToHavenLocationMenuFrontRightCorner1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.FrontRightCorner1F);
    }

    public void SwitchToHavenLocationMenuFrontLeftCorner1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.FrontLeftCorner1F);
    }

    public void SwitchToHavenLocationMenuBackRightCorner1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.BackRightCorner1F);
    }

    public void SwitchToHavenLocationMenuBackLeftCorner1F()
    {
        SwitchToHavenLocationMenu(HavenLocation.BackLeftCorner1F);
    }

    public void SwitchToHavenLocationMenuCentralCourtyardOuter()
    {
        SwitchToHavenLocationMenu(HavenLocation.CentralCourtyardOuter);
    }

    #endregion




    #region Switch Location Second-Floor Functions

    public void SwitchToHavenLocationMenuFrontHallway2F()
    {
        SwitchToHavenLocationMenu(HavenLocation.FrontHallway2F);
    }

    public void SwitchToHavenLocationMenuBackHallway2F()
    {
        SwitchToHavenLocationMenu(HavenLocation.BackHallway2F);
    }

    public void SwitchToHavenLocationMenuRightHallway2F()
    {
        SwitchToHavenLocationMenu(HavenLocation.RightHallway2F);
    }

    public void SwitchToHavenLocationMenuLeftHallway2F()
    {
        SwitchToHavenLocationMenu(HavenLocation.LeftHallway2F);
    }

    #endregion




    #region Location Dialogue First-Floor Functions

    public void PlayActivitesLocationDialogueMainGate()
    {
        PlayActivitesLocationDialogue(HavenLocation.MainGate);
    }

    public void PlayActivitesLocationDialogueLibrary()
    {
        PlayActivitesLocationDialogue(HavenLocation.Library);
    }

    public void PlayActivitesLocationDialogueArtClubrooms()
    {
        PlayActivitesLocationDialogue(HavenLocation.ArtClubrooms);
    }

    public void PlayActivitesLocationDialogueFacilityRooms()
    {
        PlayActivitesLocationDialogue(HavenLocation.FacilityRooms);
    }

    public void PlayActivitesLocationDialogueCafeteria()
    {
        PlayActivitesLocationDialogue(HavenLocation.Cafeteria);
    }

    public void PlayActivitesLocationDialogueGym()
    {
        PlayActivitesLocationDialogue(HavenLocation.Gym);
    }

    public void PlayActivitesLocationDialoguePool()
    {
        PlayActivitesLocationDialogue(HavenLocation.Pool);
    }

    public void PlayActivitesLocationDialogueField()
    {
        PlayActivitesLocationDialogue(HavenLocation.Field);
    }

    public void PlayActivitesLocationDialogueCourtyardInner()
    {
        PlayActivitesLocationDialogue(HavenLocation.CourtyardInner);
    }

    #endregion




    #region Location Dialogue Second-Floor Functions

    public void PlayActivitesLocationDialogueFrontRooftop()
    {
        PlayActivitesLocationDialogue(HavenLocation.FrontRooftop);
    }

    public void PlayActivitesLocationDialogueBackRooftop()
    {
        PlayActivitesLocationDialogue(HavenLocation.BackRooftop);
    }

    public void PlayActivitesLocationDialogueGameClubrooms()
    {
        PlayActivitesLocationDialogue(HavenLocation.GameClubrooms);
    }

    public void PlayActivitesLocationDialogueTeacherLounge()
    {
        PlayActivitesLocationDialogue(HavenLocation.TeacherLounge);
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
            SwitchToStartingMenu();
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
