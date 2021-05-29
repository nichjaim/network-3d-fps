using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void Setup(HavenData templateArg)
    {
        havenProgression = new HavenProgressionData(templateArg.havenProgression);

        calendarSystem = new TimeCalendarSystem(templateArg.calendarSystem);

        havenDialogue = new HavenDialogueData(templateArg.havenDialogue);

        activityPlanning = new HavenActivityPlanningData(templateArg.activityPlanning);
    }

    #endregion




    #region Haven Functions

    /// <summary>
    /// Refreshes all week's factors that require player planning.
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
    /// Advance calendar date by one day.
    /// </summary>
    public void AdvanceOneDay()
    {
        // advance one day
        calendarSystem.AdvanceCalendarDays(1);

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
    public void PlanWeekSocialDialogueEvents(List<DialogueEventData> 
        allSocialDialogueEventsArg, List<string> setFlagsArg)
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
            MAX_SOCIAL_DIALOGUE_EVENTS_PER_WEEK);

        // get weekdays in a random order
        List<DayOfWeek> randomWeekdays = GetWeekdaysInRandomOrder();

        // loop through all social dialogue events that were retreived
        for (int i = 0; i < nextDialogueEvents.Count; i++)
        {
            // add the random day of week and iterating dialogue event to week's plan
            weekPlan.Add(new 
                SerializableDataDayOfWeekAndDialogueEventData(randomWeekdays[i], 
                nextDialogueEvents[i]));
        }

        // set planner's weekly social dialogue plan to setup plan
        activityPlanning.dowToSocialDialogueEvent = weekPlan;
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

    #endregion


}
