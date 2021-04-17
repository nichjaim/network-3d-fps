using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeCalendarSystem
{
    #region Class Variables

    public DateTime currentDateTime = new DateTime();

    public TimeSlotType currentTimeSlot = TimeSlotType.Morning;

    #endregion




    #region Constructors

    public TimeCalendarSystem()
    {
        Setup();
    }

    public TimeCalendarSystem(TimeCalendarSystem templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        // start some ways into year on a monday
        currentDateTime = new DateTime(2020, 3, 2);

        currentTimeSlot = TimeSlotType.Morning;
    }

    private void Setup(TimeCalendarSystem templateArg)
    {
        currentDateTime = new DateTime(templateArg.currentDateTime.Year, 
            templateArg.currentDateTime.Month, templateArg.currentDateTime.Day);

        currentTimeSlot = templateArg.currentTimeSlot;
    }

    #endregion




    #region Calendar Functions

    /// <summary>
    /// Sets the current time slot to the next time slot. 
    /// Returns whether next time slot is in a new day.
    /// </summary>
    public bool ApplyTimeSlotPassage()
    {
        switch (currentTimeSlot)
        {
            case TimeSlotType.Morning:
                currentTimeSlot = TimeSlotType.Afternoon;
                break;
            case TimeSlotType.Afternoon:
                currentTimeSlot = TimeSlotType.Evening;
                break;
            case TimeSlotType.Evening:
                currentTimeSlot = TimeSlotType.Morning;

                // advances the current date by one day
                AdvanceCalendarDays(1);

                // return fact that next time slot is in a new day
                return true;
            default:
                // print warning to console
                Debug.LogWarning($"Invalid currentTimeSlot: {currentTimeSlot.ToString()}");
                break;
        }

        // getting here means that next time slot is in the same day
        return false;
    }

    /// <summary>
    /// Advances the current date by given number of days.
    /// </summary>
    /// <param name="daysArg"></param>
    public void AdvanceCalendarDays(int daysArg)
    {
        currentDateTime = currentDateTime.AddDays(daysArg);
    }

    /// <summary>
    /// Returns the current day of the week.
    /// </summary>
    /// <returns></returns>
    public DayOfWeek GetCurrentDayOfWeek()
    {
        return currentDateTime.DayOfWeek;
    }

    #endregion


}
