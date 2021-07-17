using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using MoreMountains.Tools;

public class HavenMenuHudCalendarController : MonoBehaviour, MMEventListener<DayAdvanceEvent>
{
    #region Class Variables

    private GameManager _gameManager = null;

    [Header("Component References")]

    [SerializeField]
    private Image timeOfDayImage = null;
    [SerializeField]
    private TextMeshProUGUI dayOfWeekText = null;

    [Header("Time Of Day References")]

    [SerializeField]
    private Sprite timeOfDayIconMorning = null;
    [SerializeField]
    private Sprite timeOfDayIconAfternoon = null;
    [SerializeField]
    private Sprite timeOfDayIconEvening = null;

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
    }

    #endregion




    #region Calendar Functions

    /// <summary>
    /// Sets all the HUD calendar properties appropriately.
    /// </summary>
    public void RefreshHudCalendar()
    {
        // if singleton references NOT setup yet
        if (_gameManager == null)
        {
            // DONT continue code
            return;
        }

        // get game's current calendar data
        TimeCalendarSystem currentCalendar = _gameManager.HavenData.calendarSystem;

        // set day of week text to current abreviated version of current day of week
        dayOfWeekText.text = GetDayOfWeekAbbreviation(currentCalendar.GetCurrentDayOfWeek());

        // time of day image to current time slot's icon
        timeOfDayImage.sprite = GetTimeOfDayIcon(currentCalendar.currentTimeSlot);
    }

    /// <summary>
    /// Returns abbreviated version of given day of week.
    /// </summary>
    /// <param name="dowArg"></param>
    /// <returns></returns>
    private string GetDayOfWeekAbbreviation(DayOfWeek dowArg)
    {
        return dowArg.ToString().Substring(0, 3);
    }

    /// <summary>
    /// Returns the icon associated with the given time slot.
    /// </summary>
    /// <param name="timeSlotArg"></param>
    /// <returns></returns>
    private Sprite GetTimeOfDayIcon(TimeSlotType timeSlotArg)
    {
        switch (timeSlotArg)
        {
            case TimeSlotType.Morning:
                return timeOfDayIconMorning;

            case TimeSlotType.Afternoon:
                return timeOfDayIconAfternoon;

            case TimeSlotType.Evening:
                return timeOfDayIconEvening;

            default:
                return null;
        }
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<DayAdvanceEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<DayAdvanceEvent>();
    }

    public void OnMMEvent(DayAdvanceEvent eventType)
    {
        // sets all the HUD calendar properties appropriately
        RefreshHudCalendar();
    }

    #endregion


}
