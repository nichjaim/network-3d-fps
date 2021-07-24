using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Data relevant to the player's party (group) home base of operations.
/// </summary>
[System.Serializable]
public class HavenData
{
    #region Class Variables

    public HavenProgressionData havenProgression;

    public TimeCalendarSystem calendarSystem;

    public HavenDialogueData havenDialogue;

    public HavenActivityPlanningData activityPlanning;

    public HavenLocationEventPlanningData locationEventPlanning;

    #endregion




    #region Constructors

    public HavenData()
    {
        Setup();
    }

    public HavenData(HavenData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        havenProgression = new HavenProgressionData();

        calendarSystem = new TimeCalendarSystem();

        havenDialogue = new HavenDialogueData();

        activityPlanning = new HavenActivityPlanningData();

        locationEventPlanning = new HavenLocationEventPlanningData();
    }

    private void Setup(HavenData templateArg)
    {
        havenProgression = new HavenProgressionData(templateArg.havenProgression);

        calendarSystem = new TimeCalendarSystem(templateArg.calendarSystem);

        havenDialogue = new HavenDialogueData(templateArg.havenDialogue);

        activityPlanning = new HavenActivityPlanningData(templateArg.activityPlanning);

        locationEventPlanning = new HavenLocationEventPlanningData(templateArg.locationEventPlanning);
    }

    #endregion




    #region Haven Functions

    /// <summary>
    /// Refreshes all days' factors that require player planning.
    /// </summary>
    public void ResetAllPlanning(List<CharacterData> allActivityPartnersArg)
    {
        activityPlanning.ResetAllPlanning(calendarSystem, allActivityPartnersArg);
    }

    /*private List<CharacterDataTemplate> GetAllActivityPartners()
    {
        // initialize return list as empty list
        List<CharacterDataTemplate> activityPartners = new List<CharacterDataTemplate>();

        // add all waiofu characters to
        activityPartners.Add(AssetRefMethods.LoadBundleAssetCharacterDataTemplate("4"));
        activityPartners.Add(AssetRefMethods.LoadBundleAssetCharacterDataTemplate("5"));
        activityPartners.Add(AssetRefMethods.LoadBundleAssetCharacterDataTemplate("6"));

        return activityPartners;
    }*/

    /// <summary>
    /// Advance calendar date by given number of days day.
    /// </summary>
    public void AdvanceDays(int daysArg)
    {
        // advance given number of days
        calendarSystem.AdvanceCalendarDays(daysArg);

        // set time to morning
        calendarSystem.currentTimeSlot = TimeSlotType.Morning;
    }

    /// <summary>
    /// Returns whether today is day where shooter state takes place.
    /// </summary>
    /// <returns></returns>
    public bool IsTodayShooterSectionDay()
    {
        return calendarSystem.GetCurrentDayOfWeek() == activityPlanning.
            GetShooterGameplayDayOfWeek();
    }

    #endregion




    #region Dialogue Functions

    /// <summary>
    /// Plans the week's social dialogue events.
    /// </summary>
    /// <param name="allSocialDialogueEventsArg"></param>
    /// <param name="setFlagsArg"></param>
    /// <param name="canHcontentBeViewedArg"></param>
    public void PlanWeekSocialDialogueEvents(List<DialogueEventData> 
        allSocialDialogueEventsArg, List<string> setFlagsArg, bool canHcontentBeViewedArg)
    {
        // initialize week's plan as empty list
        List<SerializableDataDayOfWeekAndDialogueEventData> weekPlan = new 
            List<SerializableDataDayOfWeekAndDialogueEventData>();

        /// initialze max number of social dialogue events that can occur in 
        /// any given week
        int MAX_SOCIAL_DIALOGUE_EVENTS_PER_WEEK = 3;

        /// get the next social dialogue events that should occur based on the 
        /// player's current progression state
        List<DialogueEventData> nextDialogueEvents = havenDialogue.
            GetNextAvailableDialogueEvents(allSocialDialogueEventsArg, 
            havenProgression.GetAverageForAllCumulativeStatPoints(), setFlagsArg, 
            canHcontentBeViewedArg, MAX_SOCIAL_DIALOGUE_EVENTS_PER_WEEK);

        // get random weekdays in the proper weekly order
        List<DayOfWeek> orderedRandomWeekdays = GetRandomWeekdaysInWeeklyOrder(nextDialogueEvents.Count);

        // loop through all social dialogue events that were retreived
        for (int i = 0; i < nextDialogueEvents.Count; i++)
        {
            // add the random day of week and iterating dialogue event to week's plan
            weekPlan.Add(new 
                SerializableDataDayOfWeekAndDialogueEventData(orderedRandomWeekdays[i], 
                nextDialogueEvents[i]));
        }

        // set planner's weekly social dialogue plan to setup plan
        activityPlanning.dowToSocialDialogueEvent = weekPlan;
    }

    /// <summary>
    /// Returns a few randomly chosen weekdays in the proper weekly order.
    /// </summary>
    /// <param name="numOfDaysArg"></param>
    /// <returns></returns>
    private List<DayOfWeek> GetRandomWeekdaysInWeeklyOrder(int numOfDaysArg)
    {
        // get all the weekdays in a random order
        List<DayOfWeek> randomWeekdays = GetWeekdaysInRandomOrder();

        // if given inavlid argument value
        if (numOfDaysArg < 1 || numOfDaysArg > randomWeekdays.Count)
        {
            // print warning to console
            Debug.LogWarning("Problem in GetRandomWeekdaysInWeeklyOrder(), " +
                $"given invalid numOfDaysArg: {numOfDaysArg}");
        }

        // ensure the number of days to get is a valid number
        int clampedNumOfDays = Mathf.Clamp(numOfDaysArg, 1, randomWeekdays.Count);

        // get a select number of the randomized weekdays based on given number of days needed
        List<DayOfWeek> selectedWeekdays = new List<DayOfWeek>();
        for (int i = 0; i < clampedNumOfDays; i++)
        {
            selectedWeekdays.Add(randomWeekdays[i]);
        }

        // return list of selected weekdays in the proper weekly order
        return GetDaysOfWeekInWeeklyOrder(selectedWeekdays);
    }

    /// <summary>
    /// Returns the given days of week in the proper weekly order.
    /// </summary>
    /// <param name="daysOfWeekArg"></param>
    /// <returns></returns>
    private List<DayOfWeek> GetDaysOfWeekInWeeklyOrder(List<DayOfWeek> daysOfWeekArg)
    {
        return daysOfWeekArg.OrderBy(iterDay => (int) iterDay).ToList();
    }

    /// <summary>
    /// Returns list of all day of week weekdays in a random order.
    /// </summary>
    /// <returns></returns>
    private List<DayOfWeek> GetWeekdaysInRandomOrder()
    {
        // initialize the list fo weekdays
        List<DayOfWeek> weekdays = new List<DayOfWeek>();

        // add all weekdays to list
        weekdays.Add(DayOfWeek.Monday);
        weekdays.Add(DayOfWeek.Tuesday);
        weekdays.Add(DayOfWeek.Wednesday);
        weekdays.Add(DayOfWeek.Thursday);
        weekdays.Add(DayOfWeek.Friday);

        // return shuffled weekday list
        return GeneralMethods.Shuffle(weekdays);
    }

    /// <summary>
    /// Returns the social dialogue event data that is associated with today's day of week. 
    /// Returns NULL if no such matching data.
    /// </summary>
    /// <returns></returns>
    public DialogueEventData GetTodaySocialDialogueEvent()
    {
        return activityPlanning.GetDialogueSocialEvent(calendarSystem.GetCurrentDayOfWeek());
    }

    /// <summary>
    /// Plans the days location events. Call when entering a new day.
    /// </summary>
    /// <param name="gameFlagsArg"></param>
    /// <param name="canHcontentBeViewedArg"></param>
    /// <param name="maxNumEventsToPlanArg"></param>
    public void PlanTodaysLocationEvents(GameFlags gameFlagsArg, bool canHcontentBeViewedArg, 
        int maxNumEventsToPlanArg)
    {
        locationEventPlanning.PlanTodaysEvents(gameFlagsArg, calendarSystem.GetCurrentDayOfWeek(), 
            canHcontentBeViewedArg, maxNumEventsToPlanArg);
    }

    #endregion


}
