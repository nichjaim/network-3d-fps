using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenDialogueData
{
    #region Class Variables

    public List<string> completedDialogues = new List<string>();

    #endregion




    #region Constructors

    public HavenDialogueData()
    {
        Setup();
    }

    public HavenDialogueData(HavenDialogueData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        completedDialogues = new List<string>();
    }

    private void Setup(HavenDialogueData templateArg)
    {
        completedDialogues = new List<string>();
        foreach (string iterDialogueName in templateArg.completedDialogues)
        {
            completedDialogues.Add(iterDialogueName);
        }
    }

    #endregion




    #region Dialogue Functions

    /// <summary>
    /// Returns the next available dialogue events. 
    /// the max return list size must be specified, set to zero for unlimited size. 
    /// This is so that no more than needed is returned.
    /// </summary>
    /// <param name="allDialogueEventsArg"></param>
    /// <param name="currentGroupTraitProgressionArg"></param>
    /// <param name="setFlagsArg"></param>
    /// <param name="maxReturnCountArg"></param>
    /// <returns></returns>
    public List<DialogueEventData> GetNextAvailableDialogueEvents(
        List<DialogueEventData> allDialogueEventsArg, 
        int currentGroupTraitProgressionArg, 
        List<string> setFlagsArg, int maxReturnCountArg)
    {
        // initialize vars for upcoming loop
        List<DialogueEventData> nextDialgEvents = new List<DialogueEventData>();
        int currentReturnCount = 0;
        int currentProgRequirement = 0;

        // loop through all given dialogue events
        foreach (DialogueEventData iterDialgEvent in allDialogueEventsArg)
        {
            /// if there is a max list return size AND gotten to that max size
            if ((maxReturnCountArg > 0) && (currentReturnCount >= maxReturnCountArg))
            {
                // break out of this loop
                break;
            }

            /// add the current event's additional progression requirement to current progression 
            /// required amount
            currentProgRequirement += iterDialgEvent.requiredAdditionalGroupTraitProgression;

            // if iterating dialogue vent has already been completed
            if (completedDialogues.Exists(
                iterDialg => iterDialg == iterDialgEvent.dialogueName))
            {
                // skip to next loop iteration
                continue;
            }

            // if given progression does NOT meet the currently required progression amount
            if (currentGroupTraitProgressionArg <= currentProgRequirement)
            {
                // skip to next loop iteration
                continue;
            }

            // if iterating dialogue event DOES require a flag BUT NO such set flag given
            if (iterDialgEvent.RequiresFlag() && 
                !setFlagsArg.Contains(iterDialgEvent.requiredFlag))
            {
                // skip to next loop iteration
                continue;
            }

            // add iterating dialogue event to return list
            nextDialgEvents.Add(new DialogueEventData(iterDialgEvent));

            // increment number of items currently being returned
            currentReturnCount++;
        }

        return nextDialgEvents;
    }

    /// <summary>
    /// Adds given dialogue to list of completed dialogue to denote it's completion.
    /// </summary>
    /// <param name="dialogueArg"></param>
    public void AddDialogueToCompletedDialogues(string dialogueArg)
    {
        // if given dialogue NOT already considered completed
        if (!completedDialogues.Contains(dialogueArg))
        {
            // add given dialogue to list to denote it's completion
            completedDialogues.Add(dialogueArg);
        }
    }

    #endregion


}
