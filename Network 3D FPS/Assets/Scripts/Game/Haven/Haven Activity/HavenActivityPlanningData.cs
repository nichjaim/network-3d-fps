using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HavenActivityPlanningData
{
    #region Class Variables

    public List<HavenActivityData> availableActivities;

    // the group members who you have not used on an activity yet
    public List<string> availableActivityPartnerIds;

    public List<SerializableDataDayOfWeekAndDialogueEventData> dowToSocialDialogueEvent;

    #endregion




    #region Constructors

    public HavenActivityPlanningData()
    {
        Setup();
    }

    public HavenActivityPlanningData(HavenActivityPlanningData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        availableActivities = new List<HavenActivityData>();

        availableActivityPartnerIds = new List<string>();

        dowToSocialDialogueEvent = new List<SerializableDataDayOfWeekAndDialogueEventData>();
    }

    private void Setup(HavenActivityPlanningData templateArg)
    {
        availableActivities = new List<HavenActivityData>();
        foreach (HavenActivityData iterData in templateArg.availableActivities)
        {
            availableActivities.Add(new HavenActivityData(iterData));
        }

        availableActivityPartnerIds = new List<string>();
        foreach (string iterId in templateArg.availableActivityPartnerIds)
        {
            availableActivityPartnerIds.Add(iterId);
        }

        dowToSocialDialogueEvent = new List<SerializableDataDayOfWeekAndDialogueEventData>();
        foreach (SerializableDataDayOfWeekAndDialogueEventData iterData in 
            templateArg.dowToSocialDialogueEvent)
        {
            dowToSocialDialogueEvent.Add(iterData);
        }
    }

    #endregion




    #region Activity Planning Functions

    /// <summary>
    /// Refreshes all factors that require player planning.
    /// </summary>
    /// <param name="timeCalendarArg"></param>
    public void ResetAllPlanning(TimeCalendarSystem timeCalendarArg)
    {
        SetupAvailableActivities(timeCalendarArg);

        ResetAvailableActivityPartners();
    }

    /// <summary>
    /// Removes the availability of given activity and partner. 
    /// This used when player does an activity and so don't want them to be able to 
    /// do the activity again or use same person in different activity.
    /// </summary>
    /// <param name="activityArg"></param>
    /// <param name="activityPartnerIdArg"></param>
    public void RemoveActivityAvailability(HavenActivityData activityArg, 
        string activityPartnerIdArg)
    {
        // remove given activity and partner from availability

        availableActivities.RemoveAll(
            iterActivity => iterActivity.activityId == activityArg.activityId);

        availableActivityPartnerIds.Remove(activityPartnerIdArg);
    }

    /// <summary>
    /// Sets the available activity partner list back to default all party members.
    /// </summary>
    private void ResetAvailableActivityPartners()
    {
        // reset partner list to empty list
        availableActivityPartnerIds = new List<string>();

        // get all main character templates
        CharacterDataTemplate[] charTemps = AssetRefMethods.
            LoadAllBundleAssetPlayableCharacterDataTemplate();
        // initialize var for upcoming loop
        string iterId;

        // loop through all character datas
        for (int i = 0; i < charTemps.Length; i++)
        {
            // if NOT the MC's data
            if (i >= 1)
            {
                // get the iterating char's ID
                iterId = charTemps[i].template.characterInfo.characterId;

                // add the iterating main char ID to activity partner list
                availableActivityPartnerIds.Add(iterId);
            }
        }
    }

    /// <summary>
    /// Returns whether the given char is still available to partner up for activites.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    public bool IsActivityPartnerAvailable(string charIdArg)
    {
        return availableActivityPartnerIds.Contains(charIdArg);
    }

    /// <summary>
    /// Returns whether the given activity is available for the current day.
    /// </summary>
    /// <param name="activityIdArg"></param>
    /// <returns></returns>
    public bool IsActivityAvailable(string activityIdArg)
    {
        return availableActivities.Exists(iterActvty => iterActvty.activityId == activityIdArg);
    }

    /// <summary>
    /// Sets up day's activities based on given calendar day.
    /// </summary>
    /// <param name="timeCalendarArg"></param>
    private void SetupAvailableActivities(TimeCalendarSystem timeCalendarArg)
    {
        availableActivities = GetAvailableActivities(timeCalendarArg);
    }

    /// <summary>
    /// Returns random day activities based on given calendar day.
    /// </summary>
    /// <param name="timeCalendarArg"></param>
    /// <returns></returns>
    private List<HavenActivityData> GetAvailableActivities(TimeCalendarSystem timeCalendarArg)
    {
        // initialize day's activites as empty list
        List<HavenActivityData> dayActivities = new List<HavenActivityData>();

        // if today is when the shooter game state mode is supposed to take place
        if (timeCalendarArg.GetCurrentDayOfWeek() == GetShooterGameplayDayOfWeek())
        {
            // add shooter transition acivity??
            //availableActivities.Add();

            // stop code early, return populated activity list
            return dayActivities;
        }

        // add all unique activities based on given time day
        dayActivities.AddRange(GetUniqueActivitiesBasedOnCurrentDay(timeCalendarArg));

        // get remaining activites needed for the given day
        int remainingActivitiesNeeded = GetNumberOfActivityOptionsPerDay() - dayActivities.Count;
        // if still need some activites for day
        if (remainingActivitiesNeeded > 0)
        {
            // add random activites
            dayActivities.AddRange(GetRandomNonUniqueActivities(remainingActivitiesNeeded));
        }

        // return populated activity list
        return dayActivities;
    }

    /// <summary>
    /// Returns list of all unique activites that should appear on given calendar day.
    /// </summary>
    /// <param name="currentTimeCalendarArg"></param>
    /// <returns></returns>
    private List<HavenActivityData> GetUniqueActivitiesBasedOnCurrentDay(
        TimeCalendarSystem currentTimeCalendarArg)
    {
        // initialize return list as empty list
        List<HavenActivityDataTemplate> uniqueActivityTemplates = new 
            List<HavenActivityDataTemplate>();

        // check cases based on current day's day of week
        switch (currentTimeCalendarArg.GetCurrentDayOfWeek())
        {
            case DayOfWeek.Tuesday:
                uniqueActivityTemplates.Add(AssetRefMethods.
                    LoadBundleAssetUniqueHavenActivityTemplate("1"));
                break;
            case DayOfWeek.Thursday:
                uniqueActivityTemplates.Add(AssetRefMethods.
                    LoadBundleAssetUniqueHavenActivityTemplate("1"));
                break;
        }

        // initialize return list as empty list
        List<HavenActivityData> returnList = new List<HavenActivityData>();
        // loop through all loaded templates
        foreach (HavenActivityDataTemplate iterTemplate in uniqueActivityTemplates)
        {
            // add iterating template's activity to return list
            returnList.Add(new HavenActivityData(iterTemplate));
        }

        // return populated list
        return returnList;
    }

    /// <summary>
    /// Returns random non-unique activities.
    /// </summary>
    /// <param name="numOfActivitiesArg"></param>
    /// <returns></returns>
    private List<HavenActivityData> GetRandomNonUniqueActivities(int numOfActivitiesArg)
    {
        // load all non-unique activity templates
        HavenActivityDataTemplate[] loadedActivityTemplates = AssetRefMethods.
            LoadAllBundleAssetHavenActivityTemplates();

        // if NOT enough loaded activites to return
        if (numOfActivitiesArg > loadedActivityTemplates.Length)
        {
            // print warning to console
            Debug.LogWarning("Not enough loaded activites to return!");

            // return some default value
            return null;
        }

        // get shuffled template list
        List<HavenActivityDataTemplate> shuffledTemplateList = GeneralMethods.
            Shuffle<HavenActivityDataTemplate>(loadedActivityTemplates.ToList());

        // initialize return list as empty list
        List<HavenActivityData> returnList = new List<HavenActivityData>();
        // loop for as many activites as should return
        for (int i = 0; i < numOfActivitiesArg; i++)
        {
            // add iterating activity to return list
            returnList.Add(new HavenActivityData(shuffledTemplateList[i]));
        }

        // return populated list
        return returnList;
    }

    /// <summary>
    /// Returns the number of activites that player has options to choose from in a day.
    /// </summary>
    /// <returns></returns>
    private int GetNumberOfActivityOptionsPerDay()
    {
        return 6;
    }

    /// <summary>
    /// Returns the day of week where the shooter game state mode is supposed to take place.
    /// </summary>
    /// <returns></returns>
    private DayOfWeek GetShooterGameplayDayOfWeek()
    {
        return DayOfWeek.Saturday;
    }

    /// <summary>
    /// Returns the social dialogue event data that is associated with the given day of week. 
    /// Returns NULL if no such matching data.
    /// </summary>
    /// <param name="dowArg"></param>
    /// <returns></returns>
    public DialogueEventData GetDialogueSocialEvent(DayOfWeek dowArg)
    {
        // get data that is assoicated with given day of week
        SerializableDataDayOfWeekAndDialogueEventData match = dowToSocialDialogueEvent.
            FirstOrDefault(iterData => iterData.valueDow == dowArg);

        // if data found
        if (match != null)
        {
            return match.valueDialogueEvent;
        }
        // else NO match found
        else
        {
            return null;
        }
    }

    #endregion


}
