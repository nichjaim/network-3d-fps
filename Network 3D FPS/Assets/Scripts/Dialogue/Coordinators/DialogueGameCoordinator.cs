using MoreMountains.Tools;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueGameCoordinator : MonoBehaviour, MMEventListener<PartyWipeEvent>,
    MMEventListener<DialogueActivationEvent>, MMEventListener<ReturnToHavenEvent>,
    MMEventListener<ExitGameSessionEvent>, MMEventListener<GameStateModeTransitionEvent>,
    MMEventListener<ArenaProgressionAdvancedEvent>
{
    #region Class Variables

    private GameManager _gameManager = null;
    private DialogueUiCoordinator _dialgUiCoord = null;

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
    private string newRunDialogueConvo = string.Empty;

    /*[ConversationPopup(true)]
    [SerializeField]
    private string restDayDialogueConvo = string.Empty;*/

    [ConversationPopup(true)]
    [SerializeField]
    private string levelingStartDialogueConvo = string.Empty;

    /*[ConversationPopup(true)]
    [SerializeField]
    private string levelingEndDialogueConvo = string.Empty;*/

    [ConversationPopup(true)]
    [SerializeField]
    private string levelingInfoDialogueConvo = string.Empty;


    private string currentActivityPartnerCharId = string.Empty;
    public string CurrentActivityPartnerCharId
    {
        get { return currentActivityPartnerCharId; }
        set { currentActivityPartnerCharId = value; }
    }

    private string currentActivityId = string.Empty;
    public string CurrentActivityId
    {
        set { currentActivityId = value; }
    }

    /// the phases in which certain actions should attempt to occur after a dialogue ends. 
    /// (dialogues chained together upon each ending)
    private bool inDialogueChainPhaseActivity = false;
    private bool inDialogueChainPhaseSocialEvents = false;
    private bool inDialogueChainPhasePassTime = false;
    private bool inDialogueChainPhaseNextDay = false;

    //private bool inDialogueChainPhaseRestDay = false;
    private bool inDialogueChainPhasePartyWipe = false;
    private bool inDialogueChainPhaseHavenReturn = false;
    private bool inDialogueChainPhaseArena = false;

    // the arena-related dialogues that have already been used in the current run
    private List<string> arenaDialoguesUsed = new List<string>();

    private bool inDialogueChainPhaseLevelUp = false;
    private List<(string, CharacterProgressionInfoSet)> charIdsToLevelUpInfo = new 
        List<(string, CharacterProgressionInfoSet)>();
    private int currentIndexLevelUpChain = 0;

    // the char associated with the current selected hcontent activity BG stuff
    private string hcontentActivityBackgroundCharId = string.Empty;
    // the current selected hcontent activity BG set
    private int hcontentActivityBackgroundSet = 1;
    // the current num of the currently selected hcontent activity BG set
    private int hcontentActivityBackgroundSetCurrentBgNum = 1;

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
        _dialgUiCoord = (UIManager.Instance).DialgUICoordr;
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

    /// <summary>
    /// Returns a random arena dialogue that has NOT been used in the current run yet
    /// </summary>
    /// <returns></returns>
    private string GetAppropriateRandomArenaDialogue()
    {
        // gets all arena dialogues that have NOT been used yet
        List<Conversation> unusedArenaConvos = GetAllUnusedArenaDialogues();

        // get random entry from the unused conversation list
        int randomIndex = UnityEngine.Random.Range(0, unusedArenaConvos.Count);

        // return random dialogue
        return unusedArenaConvos[randomIndex].Title;
    }

    /// <summary>
    /// Returns all arena dialogues that have NOT been used yet.
    /// </summary>
    /// <returns></returns>
    private List<Conversation> GetAllUnusedArenaDialogues()
    {
        // get all dialogue conversations that potentially play between arenas
        List<Conversation> arenaConvos = GetAllArenaDialogueConversations();

        // initialize return list as empty list
        List<Conversation> unusedArenaConvos = new List<Conversation>();

        // initialize var for upoming loop
        bool isUsed;

        // loop through all arena convos
        foreach (Conversation iterAllConvo in arenaConvos)
        {
            // check whether the iterating convo has already been used
            isUsed = arenaDialoguesUsed.Exists(iterUsedConvo => iterAllConvo.Name == iterUsedConvo);

            // if the iterating convo has NOT been used yet
            if (!isUsed)
            {
                // add iterating convo to list of unused convos
                unusedArenaConvos.Add(iterAllConvo);
            }
        }

        // return populated list
        return unusedArenaConvos;
    }

    /// <summary>
    /// Returns all dialogue conversations that potentially play between arenas.
    /// </summary>
    /// <returns></returns>
    private List<Conversation> GetAllArenaDialogueConversations()
    {
        string convoCategoryPrefix = "Arena/Normal/";

        return GeneralMethods.GetAllDialogueConversationsFromCategory(convoCategoryPrefix);
    }

    /// <summary>
    /// Sets up and plays dialogue for a random unused arena-related dialogue.
    /// </summary>
    private void SetupRandomArenaDialogue()
    {
        // get random unused arena dialogue
        string randomArenaDialogue = GetAppropriateRandomArenaDialogue();

        // denote that arena dialogue has now been used
        arenaDialoguesUsed.Add(randomArenaDialogue);

        // play arena dialogue
        PlayDialogue(randomArenaDialogue);
    }

    /// <summary>
    /// Plays arena story dialogue associated with given arena set number.
    /// </summary>
    /// <param name="setNumArg"></param>
    private void PlayArenaStoryDialogue(int setNumArg)
    {
        // initialize dialogue name prefix
        string ARENA_STORY_DIALOGUE_PREFIX = "Arena/Story/Event-";

        // get dialogue name from given arena set number
        string arenaStoryDialogue = ARENA_STORY_DIALOGUE_PREFIX + setNumArg.ToString();

        // play arena story dialogue
        PlayDialogue(arenaStoryDialogue);
    }

    /// <summary>
    /// Plays arena tutorial dialogue associated with the current arena number of set.
    /// </summary>
    private void PlayCurrentArenaTutorialDialogue()
    {
        PlayArenaTutorialDialogue(_gameManager.LevelManager.GetArenaNumInSet());
    }

    /// <summary>
    /// Plays arena tutorial dialogue associated with given arena set number.
    /// </summary>
    /// <param name="arenaNumInSetArg"></param>
    private void PlayArenaTutorialDialogue(int arenaNumInSetArg)
    {
        // initialize dialogue name prefix
        string ARENA_TUTORIAL_DIALOGUE_PREFIX = "Arena/Tutorial/Event-";

        // get dialogue name from given arena set number
        string arenaTutorialDialogue = ARENA_TUTORIAL_DIALOGUE_PREFIX + 
            arenaNumInSetArg.ToString();

        // play arena tutorial dialogue
        PlayDialogue(arenaTutorialDialogue);
    }

    #endregion




    #region Dialogue Change Background Functions

    /// <summary>
    /// Sets dialogue layer-1 background based on the current activity and 
    /// current time slot.
    /// </summary>
    public void ChangeBackgroundL1FromHavenActivity()
    {
        _dialgUiCoord.ChangeBackgroundL1FromHavenActivity(currentActivityId,
            _gameManager.GetCurrentTimeSlot());
    }

    public void ChangeBackgroundL1FromHavenLocationActivityHub(string locationStringArg)
    {
        // initialize var for upcoming conditional
        HavenLocation havLoc;

        // if given string can be assigned to a havenlocation enum
        if (Enum.TryParse<HavenLocation>(locationStringArg, out havLoc))
        {
            _dialgUiCoord.ChangeBackgroundL1FromHavenLocationActivityHub(havLoc,
                _gameManager.GetCurrentTimeSlot());
        }
        else
        {
            // print warning to console
            Debug.LogWarning("In ChangeBackgroundL1FromHavenLocationActivityHub(), " +
                $"no HavenLocation associated with given locationStringArg: {locationStringArg}");
        }
    }

    public void ChangeBackgroundL1FromHavenLocationEvent(string locationEventIdArg)
    {
        _dialgUiCoord.ChangeBackgroundL1FromHavenLocationEvent(locationEventIdArg,
            _gameManager.GetCurrentTimeSlot());
    }

    #endregion




    #region Hcontent Activity Background Functions

    /// <summary>
    /// Sets up the H-Content activity background information based on the given character.
    /// </summary>
    /// <param name="charIdArg"></param>
    private void SetupHcontentActivityBackgroundSet(string charIdArg)
    {
        /// get the total number of sets related to the Hcontent activity backgrounds for the 
        /// given character. 
        /// IMPORTANT: This return value must be set manually as there is no way I'm aware 
        /// of to reliably check the number of folders in each load method.
        int maxSets = AssetRefMethods.
            GetTotalNumberOfHcontentActivityBackgroundSets(charIdArg);

        // set a random set as the current bg set
        hcontentActivityBackgroundSet = UnityEngine.Random.Range(1, maxSets);

        // set the set's current BG num as the first BG
        hcontentActivityBackgroundSetCurrentBgNum = 1;

        // set char of BGs to the given char
        hcontentActivityBackgroundCharId = charIdArg;
    }

    /// <summary>
    /// Set the Layer-2 background based on the current H-Content activity progress.
    /// </summary>
    private void ChangeBackgroundL2FromCurrentHcontentActivityBackground()
    {
        _dialgUiCoord.ChangeBackgroundL2FromHcontentActivityBackground(
            hcontentActivityBackgroundCharId, hcontentActivityBackgroundSet, 
            hcontentActivityBackgroundSetCurrentBgNum);
    }

    /// <summary>
    /// Start up fresh new progress for the H-content activity progress based on 
    /// current activity partner.
    /// </summary>
    public void StartHcontentActivityBackground()
    {
        // sets up the H-Content activity background information based on the given character
        SetupHcontentActivityBackgroundSet(currentActivityPartnerCharId);

        // set the Layer-2 background based on the current H-Content activity progress
        ChangeBackgroundL2FromCurrentHcontentActivityBackground();
    }

    /// <summary>
    /// Go to the next H-Content activity background within the current activity progress.
    /// </summary>
    public void NextHcontentActivityBackground()
    {
        // increment current activity progress background number
        hcontentActivityBackgroundSetCurrentBgNum++;

        // set the Layer-2 background based on the current H-Content activity progress
        ChangeBackgroundL2FromCurrentHcontentActivityBackground();
    }

    #endregion




    #region Dialogue Chain Functions

    /// <summary>
    /// Starts the next chained dialogue, if should be doing so.
    /// </summary>
    private void PlayNextChainedDialogueIfAppropriate()
    {
        // if while processing the activity event chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseActivity())
        {
            // DONT continue code
            return;
        }

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

        // if while processing the next day chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseNextDay())
        {
            // DONT continue code
            return;
        }

        /*// if while processing the rest day chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseRestDay())
        {
            // DONT continue code
            return;
        }*/

        // if while processing the party wipe chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhasePartyWipe())
        {
            // DONT continue code
            return;
        }

        // if while processing the haven return chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseHavenReturn())
        {
            // DONT continue code
            return;
        }

        // if while processing the arena chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseArena())
        {
            // DONT continue code
            return;
        }

        // if while processing the level up chain phase it's denoted that this phase is NOT done
        if (!ProcessDialogueChainPhaseLevelUp())
        {
            // DONT continue code
            return;
        }
    }

    /// <summary>
    /// Plays activity event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseActivity()
    {
        // if player should be put in arena after dialogue is done
        if (inDialogueChainPhaseActivity)
        {
            // starts haven activity dialogue and all associated procceses
            StartHavenActivity();

            // denote that done doing activity stuff for this chain
            inDialogueChainPhaseActivity = false;

            // return that phase is NOT done
            return false;
        }

        // return that phase IS done
        return true;
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
            // will only ever do one social event per chain, so just stop it here
            inDialogueChainPhaseSocialEvents = false;

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
                    _gameManager.AddDialogueToCompletedDialogues(todaySocialDialgEvent.dialogueName);

                    // play dialogue event
                    PlayDialogue(todaySocialDialgEvent.dialogueName);

                    // return that phase is NOT done
                    return false;
                }
            }
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
            bool shouldGoToNextDay = _gameManager.GoToNextTimeSlot();

            // if should be entering a new day
            if (shouldGoToNextDay)
            {
                // play bedtime dialogue
                PlayDialogue(goToBedDialogueConvo);

                // enable next day dialogue chain phase, so that will go to new day once bed dialogue is done
                inDialogueChainPhaseNextDay = true;
            }

            // denote that calendar time has changed
            HavenCalendarTimeChangedEvent.Trigger();

            // return that phas is NOT done, as do NOT want to continue to other dialogue chain phases
            return false;
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Plays next day event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseNextDay()
    {
        // if should go to next day after dialogues done
        if (inDialogueChainPhaseNextDay)
        {
            // denote that done doing next day events for this chain
            inDialogueChainPhaseNextDay = false;

            // trigger event to denote that new day
            DaysAdvanceEvent.Trigger(1);

            // return that do NOT move onto other chain parts
            return false;
        }

        // return that phase IS done
        return true;
    }

    /*/// <summary>
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
    }*/

    /// <summary>
    /// Does party wipe stuff that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhasePartyWipe()
    {
        // if player should start party wipe stuff after dialogue is done
        if (inDialogueChainPhasePartyWipe)
        {
            // denote that done doing party wipe stuff for this chain
            inDialogueChainPhasePartyWipe = false;

            // trigger event to denote returning to haven
            ReturnToHavenEvent.Trigger();

            // return that should stop chain here
            return false;
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Plays haven return event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseHavenReturn()
    {
        // if player should start level up stuff after dialogue is done
        if (inDialogueChainPhaseHavenReturn)
        {
            // denote that done doing haven return stuff for this chain
            inDialogueChainPhaseHavenReturn = false;

            // start leveling up process
            _gameManager.ExecuteLevelingProcess();

            // resets the haven location events used in the week
            _gameManager.ResetLocationEventCurrentWeekPlan();

            // setup the week's planned social dialogue events
            _gameManager.PlanWeekSocialDialogueEvents();

            // advance two days so that both weekend days are past and player starts on monday of new week
            DaysAdvanceEvent.Trigger(2);

            // return that should stop chain here
            return false;
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Plays arena event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseArena()
    {
        // if player should be put in arena after dialogue is done
        if (inDialogueChainPhaseArena)
        {
            // denote that done doing arena stuff for this chain
            inDialogueChainPhaseArena = false;

            // create appropriate level arena and sets up it's navigation surface
            _gameManager.LevelManager.BuildArena();
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Plays pass time event dialogue that is next in chain. 
    /// Returns whether this phase is complete or not.
    /// </summary>
    /// <returns></returns>
    private bool ProcessDialogueChainPhaseLevelUp()
    {
        // if player should give level up info once dialogue is done
        if (inDialogueChainPhaseLevelUp)
        {
            // if have more level up info to show AND there actually is level-up info
            if ((currentIndexLevelUpChain < charIdsToLevelUpInfo.Count) && (charIdsToLevelUpInfo.Count != 0))
            {
                // increment level up chain's index
                currentIndexLevelUpChain++;

                // get char and level up info of current chain link
                //(string, CharacterProgressionInfoSet) charIdToProgInfo = charIdsToLevelUpInfo[currentIndexLevelUpChain];

                Debug.Log("NEED IMPL: Set level up info's char as only portrait in dialogue scene."); // NEED IMPL!!!

                // play dialogue that tells player the current level-up info
                PlayDialogue(levelingInfoDialogueConvo);

                // increment level up chain's index
                //currentIndexLevelUpChain++;
            }
            // else shown all level up info
            else
            {
                // denote that done doing level up stuff for this chain
                inDialogueChainPhaseLevelUp = false;

                // play ending level up phase dialogue
                //PlayDialogue(levelingEndDialogueConvo);
                // play end of day dialogue
                PlayDialogue(goToBedDialogueConvo);
            }

            // return that phase is NOT done
            return false;
        }

        // return that phase IS done
        return true;
    }

    /// <summary>
    /// Turns off all dialogue chain phases.
    /// </summary>
    private void DeactivateAllDialogueChainPhases()
    {
        inDialogueChainPhaseActivity = false;
        inDialogueChainPhaseSocialEvents = false;
        inDialogueChainPhasePassTime = false;
        inDialogueChainPhaseNextDay = false;

        //inDialogueChainPhaseRestDay = false;
        inDialogueChainPhasePartyWipe = false;
        inDialogueChainPhaseHavenReturn = false;
        inDialogueChainPhaseArena = false;
        inDialogueChainPhaseLevelUp = false;
    }

    #endregion




    #region Activity Functions

    /// <summary>
    /// Set the activity convo that is currently being targeted to start on dialogue end.
    /// </summary>
    public void SetTargetedActivityDialogueStart()
    {
        // enter activitry chain pahse to ensure activity dialogue starts after current dialogue finishes
        inDialogueChainPhaseActivity = true;

        /*// enter dialogue chain phases (dialogues will occur after the activity dialogue ends)
        inDialogueChainPhaseSocialEvents = true;
        inDialogueChainPhasePassTime = true;

        // get appropriate activity dialogue
        string activityConvo = GetDialogueActivityConversationPrefix() + currentActivityId;

        /// adds haven activity points to the current activity partner being targeted based on 
        /// the current activity being targeted.
        AddHavenActivityPoints();

        // play activity conversation
        PlayDialogue(activityConvo);*/
    }

    /// <summary>
    /// Starts haven activity dialogue and all associated procceses.
    /// </summary>
    private void StartHavenActivity()
    {
        // enter dialogue chain phases (dialogues will occur after the activity dialogue ends)
        inDialogueChainPhaseSocialEvents = true;
        inDialogueChainPhasePassTime = true;

        // get appropriate activity dialogue
        string activityConvo = GetDialogueActivityConversationPrefix() + currentActivityId;

        /// adds haven activity points to the current activity partner being targeted based on 
        /// the current activity being targeted.
        AddHavenActivityPoints();

        // remove the availability of used activity and partner
        _gameManager.RemoveActivityAvailability(currentActivityId, currentActivityPartnerCharId);

        // play activity conversation
        PlayDialogue(activityConvo);
    }

    /// <summary>
    /// Adds haven activity points to the current activity partner being targeted based on 
    /// the current activity being targeted.
    /// </summary>
    private void AddHavenActivityPoints()
    {
        // load the non-unique activity based on the current activity being targeted
        HavenActivityDataTemplate loadedActivityTemplate = AssetRefMethods.
            LoadBundleAssetHavenActivityTemplate(currentActivityId);

        // if an activity template was loaded
        if (loadedActivityTemplate != null)
        {
            // get activity data from loaded template
            HavenActivityData loadedActivity = new HavenActivityData(loadedActivityTemplate);

            // add activity points based on targeteted partner and targeted activity
            _gameManager.AddHavenActivityPoints(currentActivityPartnerCharId, loadedActivity);
        }
        // else activity could NOT be found
        else
        {
            // print warning to console
            Debug.LogWarning("Problem in AddHavenActivityPoints(), No activity template could " +
                $"be found from activity ID: {currentActivityId}");
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




    #region Haven Functions

    /// <summary>
    /// Starts process to return players to the haven.
    /// </summary>
    private void HavenReturn()
    {
        // play return to haven dialogue conversation
        PlayDialogue(returnToHavenDialogueConvo);

        // prep story events for dialogue chain
        //inDialogueChainPhaseRestDay = true;

        // prep return haven events for dialogue chain
        inDialogueChainPhaseHavenReturn = true;
    }

    /// <summary>
    /// Starts process to perform party wipe dialogue operations.
    /// </summary>
    private void SetupPartyWipeDialogue()
    {
        // play party wipe dialogue
        PlayDialogue(partyWipeDialogueConvo);

        // enter party wipe dialogue chain
        inDialogueChainPhasePartyWipe = true;

        // prep story events for dialogue chain
        //inDialogueChainPhaseRestDay = true;
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

    /// <summary>
    /// Denote that the tutorial has been completed and performs necessary processes.
    /// </summary>
    public void CompleteTutorial()
    {
        // denote that tutorial is complete
        _gameManager.SetTutorialAsComplete();

        // trigger event to start VN state mode
        GameStateModeTransitionEvent.Trigger(GameStateMode.VisualNovel, ActionProgressType.Started);

        // turns off all dialogue chain phases
        DeactivateAllDialogueChainPhases();

        // refreshes week's activity planning factors
        _gameManager.RefreshHavenActivityPlanning();

        // resets the haven location events used in the week
        _gameManager.ResetLocationEventCurrentWeekPlan();
        // plans the days haven location events
        _gameManager.PlanTodaysLocationEvents();

        // sets up the week's planned social dialogue events
        _gameManager.PlanWeekSocialDialogueEvents();

        // trigger event to save game progress
        SaveGameProgressEvent.Trigger();
    }

    /// <summary>
    /// Starts dialogue conversation associated with given have location event.
    /// </summary>
    /// <param name="eventArg"></param>
    public void PlayHavenLocationEventDialogue(HavenLocationEvent eventArg)
    {
        // get appropriate location conversation
        string locationConvo = GetDialogueHavenLocationEventConversationPrefix() + eventArg.eventId;

        // play dialogue conversation
        PlayDialogue(locationConvo);

        // removes event from active list and adds to watched non-repeatable event list if appropriate
        _gameManager.AddHavenLocationEventToSeen(eventArg);
    }

    private string GetDialogueHavenLocationEventConversationPrefix()
    {
        return "HavenLocationEvent/Event-";
    }

    #endregion




    #region Leveling Functions

    /// <summary>
    /// Begins the leveling up dialogue chain.
    /// </summary>
    /// <param name="charIdsToLevelUpInfoArg"></param>
    public void StartLevelingChain(List<(string, CharacterProgressionInfoSet)> charIdsToLevelUpInfoArg)
    {
        // set leveling info to given info
        charIdsToLevelUpInfo = charIdsToLevelUpInfoArg;

        // reset chain index to zero
        //currentIndexLevelUpChain = 0;
        /// reset chain index to -1, needs to be negative one as index needs to be incremented 
        /// first before doing level up stuff is checked as the correct index is needed shortly 
        /// after that.
        currentIndexLevelUpChain = -1;

        // denote that now in leveling chain
        inDialogueChainPhaseLevelUp = true;

        // play starting level-up chain dialogue
        PlayDialogue(levelingStartDialogueConvo);
    }

    public (string, CharacterProgressionInfoSet) GetCurrentCharToLevelUpInfo()
    {
        return charIdsToLevelUpInfo[currentIndexLevelUpChain];
    }

    #endregion




    #region Environment Functions

    /// <summary>
    /// Resets the list of used arena-related dialogues to a fresh empty list.
    /// </summary>
    private void ResetUsedArenaDialogues()
    {
        arenaDialoguesUsed = new List<string>();
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
        this.MMEventStartListening<ExitGameSessionEvent>();
        this.MMEventStartListening<GameStateModeTransitionEvent>();
        this.MMEventStartListening<ArenaProgressionAdvancedEvent>();
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
        this.MMEventStopListening<ExitGameSessionEvent>();
        this.MMEventStopListening<GameStateModeTransitionEvent>();
        this.MMEventStopListening<ArenaProgressionAdvancedEvent>();
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
        // resets the list of used arena-related dialogues to a fresh empty list
        ResetUsedArenaDialogues();

        // starts process to return players to the haven
        HavenReturn();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        // turns off all dialogue chain phases
        DeactivateAllDialogueChainPhases();
    }

    public void OnMMEvent(GameStateModeTransitionEvent eventType)
    {
        // check cases based one event's game mode state
        switch (eventType.gameMode)
        {
            // case where shooter mode is the focus of event
            case GameStateMode.Shooter:

                // check cases based one progress state of the event's game mode transition
                switch (eventType.transitionProgress)
                {
                    // case where just started shooter game mode
                    case ActionProgressType.Started:

                        // activate the arena dialogue chain
                        inDialogueChainPhaseArena = true;

                        // resets the list of used arena-related dialogues to a fresh empty list
                        ResetUsedArenaDialogues();

                        // reset arena coord progress to fresh run
                        _gameManager.LevelManager.ResetArenaProgress();

                        // if the tutorial HAS been completed
                        if (_gameManager.IsTutorialComplete())
                        {
                            // play dialogue associated with starting a new run
                            PlayDialogue(newRunDialogueConvo);
                        }
                        // else still need to do tutorial
                        else
                        {
                            // sets the arena data to the tutorial start
                            _gameManager.LevelManager.SetupTutorialArenaData();

                            // plays arena tutorial dialogue associated with the current arena number of set
                            PlayCurrentArenaTutorialDialogue();
                        }

                        break;
                }

                break;
        }
    }

    public void OnMMEvent(ArenaProgressionAdvancedEvent eventType)
    {
        // activate the arena dialogue chain
        inDialogueChainPhaseArena = true;

        // if tutorial has NOT been completed yet
        if (!_gameManager.IsTutorialComplete())
        {
            // plays arena tutorial dialogue associated with the current arena number of set
            PlayCurrentArenaTutorialDialogue();
        }
        // else if all arenas have been completed
        else if (eventType.arenaCoordr.AreAllArenasCompleted())
        {
            Debug.Log("NEED IMPL: Play game ending dialogue."); // NEED IMPL
        }
        // else if player starting a new set
        else if (eventType.arenaCoordr.AreStartingNewSet())
        {
            // get current arena's set number
            int currentArenaSetNum = eventType.arenaCoordr.ArenaSetData.setNum;

            // if player has already seen this new set's arena dialogue
            if (_gameManager.IsArenaStoryFlagSet(currentArenaSetNum))
            {
                // setup and play dialogue for a random unused arena-related dialogue
                SetupRandomArenaDialogue();
            }
            // else this is first time player has reached this arena set
            else
            {
                // set flag to denote that this arena story dialogue has now been seen
                _gameManager.SetArenaStoryFlag(currentArenaSetNum);

                // plays arena story dialogue associated with given arena set number
                PlayArenaStoryDialogue(currentArenaSetNum);
            }
        }
        // else player just simply progressed in the same set
        else
        {
            // setup and play dialogue for a random unused arena-related dialogue
            SetupRandomArenaDialogue();
        }
    }

    #endregion


}
