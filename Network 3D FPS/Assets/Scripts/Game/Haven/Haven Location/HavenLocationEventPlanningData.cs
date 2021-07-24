using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HavenLocationEventPlanningData
{
    #region Class Variables

    public List<HavenLocationEvent> nonRepeatableEventsSeen;

    public List<HavenLocationEvent> eventsUsedInCurrentWeek;

    public List<HavenLocationEvent> todaysEvents;

    #endregion




    #region Constructor Functions

    public HavenLocationEventPlanningData()
    {
        Setup();
    }

    public HavenLocationEventPlanningData(HavenLocationEventPlanningData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        nonRepeatableEventsSeen = new List<HavenLocationEvent>();

        eventsUsedInCurrentWeek = new List<HavenLocationEvent>();

        todaysEvents = new List<HavenLocationEvent>();
    }

    private void Setup(HavenLocationEventPlanningData templateArg)
    {
        nonRepeatableEventsSeen = new List<HavenLocationEvent>();
        foreach (HavenLocationEvent iterEvent in templateArg.nonRepeatableEventsSeen)
        {
            nonRepeatableEventsSeen.Add(new HavenLocationEvent(iterEvent));
        }

        eventsUsedInCurrentWeek = new List<HavenLocationEvent>();
        foreach (HavenLocationEvent iterEvent in templateArg.eventsUsedInCurrentWeek)
        {
            eventsUsedInCurrentWeek.Add(new HavenLocationEvent(iterEvent));
        }

        todaysEvents = new List<HavenLocationEvent>();
        foreach (HavenLocationEvent iterEvent in templateArg.todaysEvents)
        {
            todaysEvents.Add(new HavenLocationEvent(iterEvent));
        }
    }

    #endregion




    #region Planning Functions

    /// <summary>
    /// Resets the events used in the week. Call when entering a new week.
    /// </summary>
    public void ResetCurrentWeekPlan()
    {
        eventsUsedInCurrentWeek = new List<HavenLocationEvent>();
    }

    /// <summary>
    /// Plans the days events. Call when entering a new day.
    /// </summary>
    /// <param name="gameFlagsArg"></param>
    /// <param name="todaysDayArg"></param>
    /// <param name="canHcontentBeViewedArg"></param>
    /// <param name="maxNumEventsToPlanArg"></param>
    public void PlanTodaysEvents(GameFlags gameFlagsArg, DayOfWeek todaysDayArg, 
        bool canHcontentBeViewedArg, int maxNumEventsToPlanArg)
    {
        // set no events as currently active
        todaysEvents = new List<HavenLocationEvent>();

        // load all haven location event templates
        HavenLocationEventTemplate[] allLoadedLocEventTemplates = AssetRefMethods.LoadAllBundleAssetHavenLocationEventTemplates();

        // get all haven locatione events from loaded templates
        List<HavenLocationEvent> allLoadedLocEvents = new List<HavenLocationEvent>();
        foreach (HavenLocationEventTemplate iterTemp in allLoadedLocEventTemplates)
        {
            allLoadedLocEvents.Add(new HavenLocationEvent(iterTemp));
        }

        // get all location events that can potentially be used today
        List<HavenLocationEvent> allPotentialLocEvents = GetAllPotentialEventsForToday(
            allLoadedLocEvents, gameFlagsArg, todaysDayArg, canHcontentBeViewedArg);

        // get a random number of events that should be placed
        int randNumOfEvents = UnityEngine.Random.Range(0, maxNumEventsToPlanArg);

        // initialize vars for upcoming loop
        List<HavenLocationEvent> currentPotentialLocEvents;
        int randomIndex;
        HavenLocationEvent selectedEvent;

        // loop for as many events as should be retrieved
        for (int i = 0; i < randNumOfEvents; i++)
        {
            // reset the list of currently potential events
            currentPotentialLocEvents = new List<HavenLocationEvent>();

            /// loop through all potential events. NOTE: Need to re-screen potential events to 
            /// ensure that they do not colfict with the previously set events
            foreach (HavenLocationEvent iterEvent in allPotentialLocEvents)
            {
                // if iterating event is already active
                if (IsEventActive(iterEvent))
                {
                    // skip to next loop iteration
                    continue;
                }

                // if char associated with iterating event is already used in an active event
                if (ActiveEventsUsingCharacter(iterEvent.GetAppropriateAssociatedCharacterId()))
                {
                    // skip to next loop iteration
                    continue;
                }

                // if location of iterating event is already being used by an active event
                if (ActiveEventsUsingLocation(iterEvent.locatedSpot))
                {
                    // skip to next loop iteration
                    continue;
                }

                // add iterating event to currently potential events
                currentPotentialLocEvents.Add(iterEvent);
            }

            // if there is no available events that could be used
            if (currentPotentialLocEvents.Count == 0)
            {
                // print warning to console
                Debug.LogWarning("Problem in PlanTodaysEvents(). Not any more potential events to use!");

                // DONT continue code
                return;
            }

            // get random index from event list
            randomIndex = UnityEngine.Random.Range(0, currentPotentialLocEvents.Count);

            // get the randomly chosen event
            selectedEvent = new HavenLocationEvent(currentPotentialLocEvents[randomIndex]);

            // add selected event to today's active events
            todaysEvents.Add(selectedEvent);
            // denote that selected event has been used for the week
            eventsUsedInCurrentWeek.Add(selectedEvent);
        }
    }

    /// <summary>
    /// Returns all location events that can potentially be used today.
    /// </summary>
    /// <param name="allLocationEventsArg"></param>
    /// <param name="gameFlagsArg"></param>
    /// <param name="todaysDayArg"></param>
    /// <param name="canHcontentBeViewedArg"></param>
    /// <returns></returns>
    private List<HavenLocationEvent> GetAllPotentialEventsForToday(
        List<HavenLocationEvent> allLocationEventsArg, GameFlags gameFlagsArg, 
        DayOfWeek todaysDayArg, bool canHcontentBeViewedArg)
    {
        // initialize return list as empty list
        List<HavenLocationEvent> potentialLocEvent = new List<HavenLocationEvent>();

        // loop through all given events
        foreach (HavenLocationEvent iterEvent in allLocationEventsArg)
        {
            // if iterating event is already active
            if (IsEventActive(iterEvent))
            {
                // skip to next loop iteration
                continue;
            }

            // has iterating event already been used this week already
            if (HasEventBeenUsedThisWeek(iterEvent))
            {
                // skip to next loop iteration
                continue;
            }

            // if char associated with iterating event is already used in an active event
            if (ActiveEventsUsingCharacter(iterEvent.GetAppropriateAssociatedCharacterId()))
            {
                // skip to next loop iteration
                continue;
            }

            // if location of iterating event is already being used by an active event
            if (ActiveEventsUsingLocation(iterEvent.locatedSpot))
            {
                // skip to next loop iteration
                continue;
            }

            // if iterating event is non-repeatable and has already been seen
            if (HasNonRepeatableEventBeenSeen(iterEvent))
            {
                // skip to next loop iteration
                continue;
            }

            // if do NOT have all the required flags for iterating event
            if (!HaveRequiredFlags(iterEvent, gameFlagsArg))
            {
                // skip to next loop iteration
                continue;
            }

            // if iterating event can NOT be used on given todays day
            if (!CanEventBeUsedOnDayOfWeek(iterEvent, todaysDayArg))
            {
                // skip to next loop iteration
                continue;
            }

            // if the iterating event focuses on H-content but players should NOT view H-content
            if (iterEvent.focusesOnHcontent && !canHcontentBeViewedArg)
            {
                // skip to next loop iteration
                continue;
            }

            // add event
            potentialLocEvent.Add(new HavenLocationEvent(iterEvent));
        }

        // return setup list of events
        return potentialLocEvent;
    }

    /// <summary>
    /// Returns whether the given event is already active.
    /// </summary>
    /// <param name="eventArg"></param>
    /// <returns></returns>
    public bool IsEventActive(HavenLocationEvent eventArg)
    {
        return todaysEvents.Exists(iterEvent => iterEvent.eventId == eventArg.eventId);
    }

    /// <summary>
    /// Returns whether the given event has already been used this week.
    /// </summary>
    /// <param name="eventArg"></param>
    /// <returns></returns>
    private bool HasEventBeenUsedThisWeek(HavenLocationEvent eventArg)
    {
        return eventsUsedInCurrentWeek.Exists(iterEvent => iterEvent.eventId == eventArg.eventId);
    }

    /// <summary>
    /// Returns whether the given character is already part of an today event.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private bool ActiveEventsUsingCharacter(string charIdArg)
    {
        return todaysEvents.Exists(iterEvent => iterEvent.GetAppropriateAssociatedCharacterId() == charIdArg);
    }

    /// <summary>
    /// Returns whether the given location is already used by an active event.
    /// </summary>
    /// <param name="locationArg"></param>
    /// <returns></returns>
    private bool ActiveEventsUsingLocation(HavenLocation locationArg)
    {
        return todaysEvents.Exists(iterEvent => iterEvent.locatedSpot == locationArg);
    }

    /// <summary>
    /// Returns whether the given event is non-repeatable and has already been seen.
    /// </summary>
    /// <param name="eventArg"></param>
    /// <returns></returns>
    private bool HasNonRepeatableEventBeenSeen(HavenLocationEvent eventArg)
    {
        return nonRepeatableEventsSeen.Exists(iterEvent => iterEvent.eventId == eventArg.eventId);
    }

    /// <summary>
    /// Removes event from active list and adds to watched non-repeatable event list if appropriate.
    /// Call after an event's dialogue has been played.
    /// </summary>
    /// <param name="eventArg"></param>
    public void AddEventToSeen(HavenLocationEvent eventArg)
    {
        // remove given event from active events list
        todaysEvents.RemoveAll(iterEvent => iterEvent.eventId == eventArg.eventId);

        // if given event is non-repeatable
        if (!eventArg.isRepeatable)
        {
            // add given event to non-repeatable watched list
            nonRepeatableEventsSeen.Add(new HavenLocationEvent(eventArg));
        }
    }

    /// <summary>
    /// Returns whether the given event passes the flag check based on given set flags.
    /// </summary>
    /// <param name="eventArg"></param>
    /// <param name="gameFlagsArg"></param>
    /// <returns></returns>
    private bool HaveRequiredFlags(HavenLocationEvent eventArg, GameFlags gameFlagsArg)
    {
        // if given event does NOT require flags
        if (!eventArg.RequiresFlag())
        {
            // return that passed flag check
            return true;
        }

        // loop through all given events needed flags
        foreach (string iterFlag in eventArg.GetAppropriateRequiredFlags(gameFlagsArg))
        {
            // if given flags do NOT have the iterating needed flag
            if (!gameFlagsArg.IsFlagSet(iterFlag))
            {
                // return that did NOT pass flag check
                return false;
            }
        }

        // return that passed flag check
        return true;
    }

    /// <summary>
    /// Returns whether the given event can be used on today's day of the week.
    /// </summary>
    private bool CanEventBeUsedOnDayOfWeek(HavenLocationEvent eventArg, DayOfWeek dayOfWeekArg)
    {
        // if given event does NOT require a certain day
        if (!eventArg.RequiresCertainDayOfWeek())
        {
            // return that event can be used on given day
            return true;
        }

        // return whether given day matches one of given events needed days
        return eventArg.associatedDaysOfWeek.Contains(dayOfWeekArg);
    }

    #endregion


}
