using MoreMountains.Tools;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGameCoordinator : MonoBehaviour, MMEventListener<PartyWipeEvent>,
    MMEventListener<DialogueActivationEvent>
{
    #region Class Variables

    private GameManager _gameManager = null;

    [Header("Component References")]

    [SerializeField]
    private DialogueSystemTrigger dialogueTrigger = null;

    [Header("Dialogue Conversation References")]

    [ConversationPopup(true)]
    [SerializeField]
    private string partyWipeDialogueConvo = string.Empty;

    [ConversationPopup(true)]
    [SerializeField]
    private string goToBedDialogueConvo = string.Empty;

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



    [ConversationPopup(true)]
    [SerializeField]
    private string testDialogueConvo = string.Empty; // TESTING VAR!!!

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

    // TEST FUNC!!!!!!!!!!!!!!
    public void PlayDialogueTestingConversation()
    {
        PlayDialogue(testDialogueConvo);
    }

    /// <summary>
    /// Start the activity convo that is currently being targeted.
    /// </summary>
    public void StartTargetedActivityDialogue()
    {
        inDialogueChainPhaseSocialEvents = true;
        inDialogueChainPhasePassTime = true;

        // get appropriate activity dialogue
        string activityConvo = GetDialogueActivityConversationPrefix() + currentActivityId;

        // play activity conversation
        PlayDialogue(activityConvo);
    }

    private void SetupNextChainedDialogue()
    {
        if (inDialogueChainPhaseSocialEvents)
        {
            // get today's social dialogue event
            DialogueEventData todaySocialDialgEvent = _gameManager.HavenData.GetTodaySocialDialogueEvent();

            // if there was a social dialogue event for today
            if (todaySocialDialgEvent != null)
            {
                PlayDialogue(todaySocialDialgEvent.dialogueName);

                // DONT continue code
                return;
            }

            inDialogueChainPhaseSocialEvents = false;
        }

        // if should pass time after dialogues done
        if (inDialogueChainPhasePassTime)
        {
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

            // DONT continue code
            return;
        }
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




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<PartyWipeEvent>();
        this.MMEventStartListening<DialogueActivationEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<PartyWipeEvent>();
        this.MMEventStopListening<DialogueActivationEvent>();
    }

    public void OnMMEvent(PartyWipeEvent eventType)
    {
        // play party wipe dialogue
        PlayDialogue(partyWipeDialogueConvo);
    }

    public void OnMMEvent(DialogueActivationEvent eventType)
    {
        //throw new System.NotImplementedException();
    }

    #endregion


}
