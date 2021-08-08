using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavenSubMenuController : SubMenuController
{
    #region Class Variables

    protected GameManager _gameManager = null;
    protected DialogueGameCoordinator _dialgGameCoord = null;

    [Header("Haven Sub-Menu ID Properties")]

    [SerializeField]
    private HavenLocation havenSpot = HavenLocation.None;
    [Tooltip("Sets the sub-menu ID as the haven-location. " +
        "Use for easy switching between sub-menus.")]
    [SerializeField]
    private bool replaceMenuIdWithLocationName = true;

    [Header("Haven Sub-Menu Background References")]

    [SerializeField]
    private Sprite bgSpriteMorning = null;
    [SerializeField]
    private Sprite bgSpriteAfternoon = null;
    [SerializeField]
    private Sprite bgSpriteEvening = null;

    [Header("Haven Sub-Menu Loaction-Event Properties")]

    [SerializeField]
    private List<SerializableDataHavenLocationEventTemplateAndGameObject> 
        havenLocationEventTempToEventObject = 
        new List<SerializableDataHavenLocationEventTemplateAndGameObject>();

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // sets the sub-menu ID as the haven location name, if appropriate.
        ReplaceMenuIdWithLocationIfAppropiate();
    }

    private void Start()
    {
        // setup vars that hold reference to components
        InitializeComponentReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up vars that hold reference to component. 
    /// Call in Start(), to give time for singletons to instantiate.
    /// </summary>
    private void InitializeComponentReferences()
    {
        _gameManager = ((HavenMenuController)menuMaster)._GameManager;

        // if menu master's UIManager reference has been setup
        if (((HavenMenuController)menuMaster)._UiManager != null)
        {
            _dialgGameCoord = ((HavenMenuController)menuMaster)._UiManager.DialgGameCoordr;
        }
    }

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        ((HavenMenuController)menuMaster).BackgroundTransition(
            GetAppropriateBackgroundSprite(), 1f);

        /// activate/deactivate the menu's haven location event objects based 
        /// on if those events are active today.
        RefreshHavenLocationEventObjects();
    }

    #endregion




    #region Haven Menu Functions

    protected void SwitchToHavenLocationMenu(HavenLocation locationArg)
    {
        // switch to haven sub-menu associated with given location
        ((HavenMenuController)menuMaster).SwitchToHavenLocationMenu(locationArg);
    }

    protected void PlayActivitesLocationDialogue(HavenLocation locationArg)
    {
        // if dialogue game coodrinator reference has NOT been setup yet
        if (_dialgGameCoord == null)
        {
            // setup vars that hold reference to component
            InitializeComponentReferences();
        }

        _dialgGameCoord.PlayHavenLocationDialogue(locationArg);
    }

    /// <summary>
    /// Sets the sub-menu ID as the haven location name, if appropriate to do so.
    /// Call in Awake().
    /// </summary>
    private void ReplaceMenuIdWithLocationIfAppropiate()
    {
        // if should replace mnu ID
        if (replaceMenuIdWithLocationName)
        {
            // set menu ID to associated haven location
            menuId = havenSpot.ToString();
        }
    }

    /// <summary>
    /// Activates/Deactivates the menu's haven location event objects based 
    /// on if those events are active today.
    /// </summary>
    private void RefreshHavenLocationEventObjects()
    {
        // initialize var for upcoming loop
        bool isEventActive;

        // loop through all location event entries
        foreach (SerializableDataHavenLocationEventTemplateAndGameObject 
            iterData in havenLocationEventTempToEventObject)
        {
            // get whether the iterating event is currently active
            isEventActive = ((HavenMenuController)menuMaster)._GameManager.
                IsHavenLocationEventActiveToday(iterData.valueTemplate.template);

            //activate/deactivate the iterating event object based on if the event is active or not
            iterData.valueObject.SetActive(isEventActive);
        }
    }

    /// <summary>
    /// Starts dialogue conversation associated with given have location event.
    /// </summary>
    /// <param name="templateArg"></param>
    public void PlayHavenLocationEventDialogue(HavenLocationEventTemplate templateArg)
    {
        // if dialogue game coodrinator reference has NOT been setup yet
        if (_dialgGameCoord == null)
        {
            // setup vars that hold reference to component
            InitializeComponentReferences();
        }

        // starts dialogue conversation associated with given have location event
        _dialgGameCoord.PlayHavenLocationEventDialogue(new HavenLocationEvent(templateArg));

        /// activate/deactivate the menu's haven location event objects based 
        /// on if those events are active today.
        RefreshHavenLocationEventObjects();
    }

    /// <summary>
    /// Returns the appropriate background sprite based on the current time of day.
    /// </summary>
    /// <returns></returns>
    private Sprite GetAppropriateBackgroundSprite()
    {
        // get game's current calendar data
        TimeCalendarSystem currentCalendar = _gameManager.HavenData.calendarSystem;

        // get background spite based on current time of day
        switch (currentCalendar.currentTimeSlot)
        {
            case TimeSlotType.Morning:
                return bgSpriteMorning;

            case TimeSlotType.Afternoon:
                return bgSpriteAfternoon;

            case TimeSlotType.Evening:
                return bgSpriteEvening;

            default:
                // print warning to console
                Debug.LogWarning("In GetAppropriateBackgroundSpite(), got invalid case value: " +
                    $"{currentCalendar.currentTimeSlot.ToString()}");
                // return default invalid sprite
                return null;
        }
    }

    #endregion


}
