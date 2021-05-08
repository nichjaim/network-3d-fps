using MoreMountains.Tools;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGameCoordinator : MonoBehaviour, MMEventListener<PartyWipeEvent>,
    MMEventListener<DialogueActivationEvent>, MMEventListener<ReturnToHavenEvent>
{
    #region Class Variables

    private GameManager _gameManager = null;

    [Header("Component References")]

    [SerializeField]
    private DialogueSystemTrigger dialogueTrigger = null;

    [SerializeField]
    private StandardDialogueUI dialogueUiHead = null;

    [Header("Dialogue Conversation References")]

    [ConversationPopup(true)]
    [SerializeField]
    private string partyWipeDialogueConvo = string.Empty;

    [ConversationPopup(true)]
    [SerializeField]
    private string returnToHavenDialogueConvo = string.Empty;

    [ConversationPopup(true)]
    [SerializeField]
    private string goToBedDialogueConvo = string.Empty;

    [ConversationPopup(true)]
    [SerializeField]
    private string restDayDialogueConvo = string.Empty;

    private string currentActivityPartnerCharId = string.Empty;
    public string CurrentActivityPartnerCharId
    {
        set { currentActivityPartnerCharId = value; }
    }

    private string currentActivityId = string.Empty;
    public string CurrentActivityId
    {
        set { currentActivityId = value; }
    }

    /// the phases in which certain actions should attempt to occur after a dialogue ends. 
    /// (dialogues chained together upon each ending)
    private bool inDialogueChainPhaseSocialEvents = false;
    private bool inDialogueChainPhasePassTime = false;

    private bool inDialogueChainPhaseRestDay = false;

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




    #region Dialogue Functions

    /// <summary>
    /// Plays a conversation dialogue.
    /// </summary>
    /// <param name="conversationArg"></param>
    private void PlayDialogue(string conversationArg)
    {
        // set the dialogue trigger's dialogue conversation to the given dialogue conversation
        dialogueTrigger.conversation = conversationArg;

        // trigger the dialogue
        dialogueTrigger.OnUse();
    }

    #endregion




    #region Dialogue Chain Functions

    /// <summary>
    /// Starts the next chained dialogue, if should be doing so.
    /// </summary>
    private void PlayNextChainedDialogueIfAppropriate()
    {
        // if while processing the social event chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseSocialEvents())
        {
            // DONT continue code
            return;
        }

        // if while processing the pass time chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhasePassTime())
        {
            // DONT continue code
            return;
        }

        // if while processing the rest day chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseRestDay())
        {
            // DONT continue code
            return;
        }
    }

    /// <summary>
    /// Plays social event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseSocialEvents()
    {
        // if should do a dialogue event (if any available) after dialogues done
        if (inDialogueChainPhaseSocialEvents)
        {
            // get today's social dialogue event
            DialogueEventData todaySocialDialgEvent = _gameManager.HavenData.
                GetTodaySocialDialogueEvent();

            // initialize var for upcoming conditionals
            bool shouldPlayDialogue = false;

            // if there was a social dialogue event for today
            if (todaySocialDialgEvent != null)
            {
                // if dialogue event requires the MC to be partnered up with a specific character for the activity
                if (todaySocialDialgEvent.RequireBeGroupedWithSpecificCharacter())
                {
                    // if MC is partnered with that character for the activity
                    if (todaySocialDialgEvent.associatedCharId == currentActivityPartnerCharId)
                    {
                        // denote that should play the dialogue event
                        shouldPlayDialogue = true;
                    }
                }
                // else if dialogue event needs to be done during a certain time slot
                else if (todaySocialDialgEvent.RequiresSpecificTimeSlot())
                {
                    // if currently in that time slot
                    if (todaySocialDialgEvent.associatedTimeSlot ==
                        _gameManager.HavenData.calendarSystem.currentTimeSlot)
                    {
                        // denote that should play the dialogue event
                        shouldPlayDialogue = true;
                    }
                }
                // else no additional requirements for dialogue event
                else
                {
                    // denote that should play the dialogue event
                    shouldPlayDialogue = true;
                }

                // if given clear to play the dialogue event
                if (shouldPlayDialogue)
                {
                    // denote dialogue to play as now completed
                    _gameManager.HavenData.havenDialogue.AddDialogueToCompletedDialogues(
                        todaySocialDialgEvent.dialogueName);

                    // play dialogue event
                    PlayDialogue(todaySocialDialgEvent.dialogueName);

                    // return that phase is NOT done
                    return false;
                }
            }

            // denote that done doing social events for this chain
            inDialogueChainPhaseSocialEvents = false;
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Plays pass time event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhasePassTime()
    {
        // if should pass time after dialogues done
        if (inDialogueChainPhasePassTime)
        {
            // denote that done doing time pass events for this chain
            inDialogueChainPhasePassTime = false;

            // go to next time slot and get if should be entering a new day
            bool shouldGoToNextDay = _gameManager.HavenData.calendarSystem.GoToNextTimeSlot();

            // if should be entering a new day
            if (shouldGoToNextDay)
            {
                // play bedtime dialogue
                PlayDialogue(goToBedDialogueConvo);

                // trigger event to denote that new day
                DayAdvanceEvent.Trigger();
            }

            // return that phase is NOT done
            return false;
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Plays rest day event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseRestDay()
    {
        // if should play rest day conversations after dialogues done
        if (inDialogueChainPhaseRestDay)
        {
            // get the next story dialogue events that should be played
            List<DialogueEventData> nextAvailableStoryDialogues = _gameManager.
                GetNextAvailableStoryDialogueEvents();

            // if some dialogues avaiable
            if (nextAvailableStoryDialogues.Count > 0)
            {
                // get the next dialogue event to play
                DialogueEventData nextDialogueEvent = nextAvailableStoryDialogues[0];

                // denote dialogue to play as now completed
                _gameManager.HavenData.havenDialogue.AddDialogueToCompletedDialogues(
                    nextDialogueEvent.dialogueName);

                // play the retreived story conversation
                PlayDialogue(nextDialogueEvent.dialogueName);

                // return that phase is NOT done
                return false;
            }

            // denote that finished doing rest day events for this chain
            inDialogueChainPhaseRestDay = false;

            // play rest day conversation
            PlayDialogue(restDayDialogueConvo);

            // trigger event to denote that new day
            DayAdvanceEvent.Trigger();
        }

        // return that phase IS done
        return true;
    }

    #endregion




    #region Activity Functions

    /// <summary>
    /// Start the activity convo that is currently being targeted.
    /// </summary>
    public void StartTargetedActivityDialogue()
    {
        // enter dialogue chain phases (dialogues will occur after the activity dialogue ends)
        inDialogueChainPhaseSocialEvents = true;
        inDialogueChainPhasePassTime = true;

        // get appropriate activity dialogue
        string activityConvo = GetDialogueActivityConversationPrefix() + currentActivityId;

        // play activity conversation
        PlayDialogue(activityConvo);
    }

    private string GetDialogueActivityConversationPrefix()
    {
        return "Activity/Activity-";
    }

    /// <summary>
    /// Return whether the current activity partner is the given char.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    public bool IsActivityPartner(string charIdArg)
    {
        return currentActivityPartnerCharId == charIdArg;
    }

    #endregion




    #region Haven Functions

    /// <summary>
    /// Starts process to return players to the haven.
    /// </summary>
    private void HavenReturn()
    {
        // play return to haven dialogue conversation
        PlayDialogue(returnToHavenDialogueConvo);

        // prep story events for dialogue chain
        inDialogueChainPhaseRestDay = true;
    }

    /// <summary>
    /// Starts process to perform party wipe dialogue operations.
    /// </summary>
    private void SetupPartyWipeDialogue()
    {
        // play party wipe dialogue
        PlayDialogue(partyWipeDialogueConvo);

        // prep story events for dialogue chain
        inDialogueChainPhaseRestDay = true;
    }

    private string GetDialogueHavenLocationConversationPrefix()
    {
        return "HavenLocation/Location-";
    }

    /// <summary>
    /// Starts dialogue conversation associated with given have location.
    /// </summary>
    public void PlayHavenLocationDialogue(HavenLocation locationArg)
    {
        // get appropriate location conversation
        string locationConvo = GetDialogueHavenLocationConversationPrefix() + locationArg.ToString();

        // play dialogue conversation
        PlayDialogue(locationConvo);
    }

    #endregion




    #region Dialogue Event Functions

    /// <summary>
    /// Should be placed in conversation start action event.
    /// </summary>
    public void OnDialogueConversationStart(Transform actor)
    {
        // trigger event to denote that this dialogue is STARTING
        DialogueActivationEvent.Trigger(dialogueUiHead, true);
    }

    /// <summary>
    /// Should be placed in conversation end action event.
    /// </summary>
    public void OnDialogueConversationEnd(Transform actor)
    {
        // trigger event to denote that this dialogue is ENDING
        DialogueActivationEvent.Trigger(dialogueUiHead, false);
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<PartyWipeEvent>();
        this.MMEventStartListening<DialogueActivationEvent>();
        this.MMEventStartListening<ReturnToHavenEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<PartyWipeEvent>();
        this.MMEventStopListening<DialogueActivationEvent>();
        this.MMEventStopListening<ReturnToHavenEvent>();
    }

    public void OnMMEvent(PartyWipeEvent eventType)
    {
        // starts process to perform party wipe dialogue operations
        SetupPartyWipeDialogue();
    }

    public void OnMMEvent(DialogueActivationEvent eventType)
    {
        // if dialogue closing
        if (!eventType.isOpening)
        {
            // start the next chained dialogue if should be doing so
            PlayNextChainedDialogueIfAppropriate();
        }
    }

    public void OnMMEvent(ReturnToHavenEvent eventType)
    {
        // starts process to return players to the haven
        HavenReturn();
    }

    #endregion


}
