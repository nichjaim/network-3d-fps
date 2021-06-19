using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using BundleSystem;
using System;
using System.Linq;

public class GameManager : NetworkBehaviour, MMEventListener<GamePausingActionEvent>, 
    MMEventListener<NewSaveCreatedEvent>, MMEventListener<LoadSaveEvent>,
    MMEventListener<SaveGameProgressEvent>, MMEventListener<NetworkGameLocalJoinedEvent>, 
    MMEventListener<PartyWipeEvent>, MMEventListener<DayAdvanceEvent>, 
    MMEventListener<EnterGameSessionEvent>
{
    #region Class Variables

    public static GameManager Instance;

    private NetworkManagerCustom _networkManagerCustom = null;

    // the current save file number being used
    private int saveFileNumber = 0;

    [SyncVar(hook = nameof(OnGameStateModeChanged))]
    private GameStateMode gameStateMode = GameStateMode.VisualNovel;
    public Action OnGameStateModeChangedAction;

    [SyncVar(hook = nameof(OnGameFlagsChanged))]
    private GameFlags gameFlags = null;
    public Action OnGameFlagsChangedAction;
    public GameFlags GameFlags
    {
        get { return gameFlags; }
    }

    [SyncVar(hook = nameof(OnPartyCharactersChanged))]
    private List<CharacterData> partyCharacters = new List<CharacterData>();
    public Action OnPartyCharactersChangedAction;
    /*readonly SyncList<CharacterData> partyCharacters = new SyncList<CharacterData>();
    public Action OnPartyCharactersChangedAction;*/

    /// each list entry denotes a party slot and the entry value denotes the associated 
    /// partyCharacters list index key (ex. if partyOrder[0] = 2 then that means that in 
    /// the first party slot is partyCharacters[2] which is the third party character. 
    /// And so player 1 is controlling party character 3)
    [SyncVar(hook = nameof(OnPartyOrderChanged))]
    private List<int> partyOrder = new List<int>();
    public Action OnPartyOrderChangedAction;

    // the character who is chosen when all playable characters being used
    [SerializeField]
    private CharacterDataTemplate defaultPartyCharacterTemplate = null;
    [SyncVar(hook = nameof(OnDefaultPartyCharacterChanged))]
    private CharacterData defaultPartyCharacter = null;
    public Action OnDefaultPartyCharacterChangedAction;

    [SyncVar(hook = nameof(OnPartyInvChanged))]
    private PartyInventory partyInv = null;
    public Action OnPartyInvChangedAction;

    [SyncVar(hook = nameof(OnHavenDataChanged))]
    private HavenData havenData = null;
    public Action OnHavenDataChangedAction;
    public HavenData HavenData
    {
        get { return havenData; }
    }

    [SyncVar(hook = nameof(OnGameSeedChanged))]
    private int gameSeed = 0;
    public Action OnGameSeedChangedAction;

    [Header("Manager References")]

    [SerializeField]
    private SpawnManager spawnManager = null;
    public SpawnManager SpawnManager
    {
        get { return spawnManager; }
    }

    [SerializeField]
    private LevelManager levelManager = null;
    public LevelManager LevelManager
    {
        get { return levelManager; }
    }

    [SerializeField]
    private GameSeedManager gameSeedManager = null;
    public GameSeedManager GameSeedManager
    {
        get { return gameSeedManager; }
    }

    [Header("Dialogue References")]

    [SerializeField]
    private DialogueEventDatabase dialogueEventDatabaseStory = null;
    [SerializeField]
    private DialogueEventDatabase dialogueEventDatabaseSocial = null;

    private string FLAG_TUTORIAL_COMPLETE = "tutorial_done";

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup singelton instance
        InstantiateInstance();

        // deactivate this object if not connected to an online session
        DeactivateIfNotNetworkConnected();
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
    /// Sets up the singleton instance. 
    /// Call in Awake().
    /// </summary>
    private void InstantiateInstance()
    {
        // if an instance of this singeton is already setup
        if (Instance != null)
        {
            // destroy this object
            Destroy(gameObject);
        }
        // else no such singleton object exists yet
        else
        {
            // set the singelton isntance to this entity
            Instance = this;
            // ensure this object is not destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Sets up reference vars to their respective singletons. 
    /// Call in Start() so as to give time for singleton instances to actually instantiate.
    /// </summary>
    private void InitializeSingetonReferences()
    {
        _networkManagerCustom = (NetworkManagerCustom)NetworkManager.singleton;
    }

    /// <summary>
    /// Deactivates this object if not connected to an online session. 
    /// Call in Awake().
    /// </summary>
    private void DeactivateIfNotNetworkConnected()
    {
        // if NOT in online session
        if (!NetworkClient.isConnected)
        {
            // turn off this object
            gameObject.SetActive(false);
        }
    }

    #endregion




    #region Game Functions

    /// <summary>
    /// Switches the game speed between paused and unpaused, if not in online session.
    /// </summary>
    private void SwitchGamePausingIfAppropriate()
    {
        // if NOT in online session
        if (!NetworkClient.isConnected)
        {
            // if game unpaused
            if (Time.timeScale > 0f)
            {
                // pause game
                Time.timeScale = 0f;
            }
            // else game paused
            else
            {
                // unpause game
                Time.timeScale = 1f;
            }
        }
    }

    /// <summary>
    /// Sets up player char order list to the default order.
    /// </summary>
    private void SetPartyOrderToDefault()
    {
        // initialize empty order list
        List<int> orderList = new List<int>();

        // loop through all party characters
        for (int i = 0; i < partyCharacters.Count; i++)
        {
            // add iterating party char list index to order list
            orderList.Add(i);
        }

        // set order list to created list
        SetPartyOrder(orderList);
    }

    /// <summary>
    /// Returns the party character associated with the given party slot 
    /// based on the party order.
    /// </summary>
    /// <param name="partySlotNumberArg"></param>
    /// <returns></returns>
    public CharacterData GetPartyCharacterInPartySlot(int partySlotNumberArg)
    {
        // if given invalid party slot
        if (partySlotNumberArg <= 0)
        {
            // print warning to console
            Debug.LogWarning($"Given invalid party slot <{partySlotNumberArg}>");
            /// return the default party character and makes sure to setup the char 
            /// if not already done
            return RetrieveDefaultPartyCharacter();
        }
        // else if given party slot is unnacounted for by party order
        else if (partySlotNumberArg > partyOrder.Count)
        {
            /// return the default party character and makes sure to setup the char 
            /// if not already done
            return RetrieveDefaultPartyCharacter();
        }
        // else given accounted for party slot
        else
        {
            // get party char index through given slot number
            int partyCharIndex = partyOrder[partySlotNumberArg - 1];
            // if index is invalid
            if (partyCharIndex < 0)
            {
                // print warning to console
                Debug.LogWarning($"Party slot <{partySlotNumberArg}> has an invalid part char index <{partyCharIndex}>");
                /// return the default party character and makes sure to setup the char 
                /// if not already done
                return RetrieveDefaultPartyCharacter();
            }
            // else if index does NOT point to an existing party char
            else if (partyCharIndex >= partyCharacters.Count)
            {
                /// return the default party character and makes sure to setup the char 
                /// if not already done
                return RetrieveDefaultPartyCharacter();
            }
            // else valid index is pointing to an existing party char
            else
            {
                return partyCharacters[partyCharIndex];
            }
        }
    }

    /// <summary>
    /// Returns the default party character and makes sure to setup the char 
    /// if not already done.
    /// </summary>
    /// <returns></returns>
    private CharacterData RetrieveDefaultPartyCharacter()
    {
        // if default party char not setup
        if (defaultPartyCharacter == null)
        {
            // setup the default playable character's data
            SetupDefaultPartyCharacter();
        }

        // return setup default character
        return defaultPartyCharacter;
    }

    /// <summary>
    /// Sets up the default playable character's data.
    /// </summary>
    private void SetupDefaultPartyCharacter()
    {
        // setup the default character's base properties
        CharacterData defaultChar = new CharacterData(defaultPartyCharacterTemplate);
        /// scale the character's stats to be around lower than average compared to the 
        /// other playable character's current stats
        Debug.LogWarning("NEEDS IMPL: Default char stat scaling"); // NEED IMPL
        // set default party char to existing order to ensure all clients get the change
        SetDefaultPartyCharacter(defaultChar);
    }

    /// <summary>
    /// Swaps the paty order between the two given slots.
    /// </summary>
    /// <param name="slotNumA"></param>
    /// <param name="slotNumB"></param>
    public void SwapPartyOrderSlots(int slotNumA, int slotNumB)
    {
        // swap party slots
        GeneralMethods.ListIndexSwap<int>(partyOrder, slotNumA - 1, 
            slotNumB - 1);
        // set party order to existing order to ensure all clients get the change
        SetPartyOrder(partyOrder);
    }

    /// <summary>
    /// Returns all party members that are NOT he Main Character.
    /// </summary>
    /// <returns></returns>
    private List<CharacterData> GetAllPartyMembersExcludingMC()
    {
        // initialize character ID for the MC
        string CHAR_ID_MC = "3";

        // initialize return list as empty list
        List<CharacterData> partyMems = new List<CharacterData>();

        // loop through all party members
        foreach (CharacterData iterData in partyCharacters)
        {
            // if iterating character is NOT the MC
            if (iterData.characterInfo.characterId != CHAR_ID_MC)
            {
                // add iterating party member to return list
                partyMems.Add(iterData);
            }
        }

        // return populated list
        return partyMems;
    }

    #endregion




    #region Party Addition Functions

    /// <summary>
    /// Adds character to party who matches given ID.
    /// </summary>
    /// <param name="charIdArg"></param>
    public void AddCharacterToParty(string charIdArg)
    {
        // if char is already in party
        if (DoesCharacterExistInParty(charIdArg))
        {
            // DONT continue code
            return;
        }

        /*// get all playable character data templates
        CharacterDataTemplate[] charDataTemps = AssetRefMethods.
            LoadAllBundleAssetPlayableCharacterDataTemplate();*/
        CharacterDataTemplate charDataTemp = AssetRefMethods.LoadBundleAssetCharacterDataTemplate(charIdArg);

        /*// get char template whose ID matches given char ID
        CharacterDataTemplate matchingTemplate = charDataTemps.
            FirstOrDefault(iterTemp => iterTemp.template.characterInfo.characterId == charIdArg);*/

        // if data found
        if (charDataTemp != null)
        {
            // add amtching character to party
            partyCharacters.Add(new CharacterData(charDataTemp));
        }
        // else no such data exists
        else
        {
            // if gotten here then print warning to console
            Debug.LogWarning($"No playable character asscoaited with ID: {charIdArg}");
        }
    }

    /// <summary>
    /// Returns whether a character is in party who matches given ID.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private bool DoesCharacterExistInParty(string charIdArg)
    {
        return partyCharacters.Exists(iterChar => iterChar.characterInfo.characterId == charIdArg);
    }

    /// <summary>
    /// Adds associated active ability to associated character.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="abilityIdArg"></param>
    public void AddAbilityToCharacter(string charIdArg, string abilityIdArg)
    {
        // get party char who matches given ID
        CharacterData matchingPartyChar = GetPartyCharacter(charIdArg);

        // if WAS party char found
        if (matchingPartyChar != null)
        {
            // get active ability template assoicated with given ability ID
            ActiveAbilityTemplate abilityTemp = AssetRefMethods.LoadBundleAssetActiveAbilityTemplate(abilityIdArg);

            // if found ability template
            if (abilityTemp != null)
            {
                // add ability to party char
                matchingPartyChar.activeAbilityLoadout.AddActiveAbility(new ActiveAbility(abilityTemp));
            }
            // else NO such ability template found
            else
            {
                // print warning to console
                Debug.LogWarning($"Ability template not found. ID: {abilityIdArg}");
            }
        }
        // else NO party char found
        else
        {
            // print warniong to console
            Debug.LogWarning($"character template not found. ID: {charIdArg}");
        }
    }

    /// <summary>
    /// Returns party character who matches the given ID.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private CharacterData GetPartyCharacter(string charIdArg)
    {
        return partyCharacters.FirstOrDefault(iterChar => iterChar.characterInfo.characterId == charIdArg);
    }

    #endregion




    #region Saving Functions

    /// <summary>
    /// Prepares game for upcoming game session using the given save data.
    /// </summary>
    /// <param name="saveFileNumArg"></param>
    /// <param name="gameSaveArg"></param>
    private void SetupGameSession(int saveFileNumArg, GameSaveData gameSaveArg)
    {
        // set game properties based on event's save data
        SetupGameInfoFromSave(saveFileNumArg, gameSaveArg);

        // setup player char order list to the default order
        SetPartyOrderToDefault();

        // trigger event to enter a game session
        EnterGameSessionEvent.Trigger(gameSaveArg);
    }

    /// <summary>
    /// Sets game properties based on given save data.
    /// </summary>
    /// <param name="saveFileNumArg"></param>
    /// <param name="gameSaveArg"></param>
    private void SetupGameInfoFromSave(int saveFileNumArg, GameSaveData gameSaveArg)
    {
        saveFileNumber = saveFileNumArg;

        SetGameFlags(gameSaveArg.gameFlags);

        SetPartyCharacters(gameSaveArg.playableCharacterData);
        //SetPartyCharacters(new SyncList<CharacterData>(eventType.saveData.playableCharacterData));

        // setup party inventory to save data's party inv
        SetPartyInv(gameSaveArg.partyInventory);

        SetHavenData(gameSaveArg.havenData);
    }

    /// <summary>
    /// Saves the current game's progress.
    /// </summary>
    private void SaveGameProgress()
    {
        // create new save data
        GameSaveData targetSaveData = new GameSaveData();

        // populate save data with current game progress
        targetSaveData.gameFlags = gameFlags;
        targetSaveData.playableCharacterData = partyCharacters;
        targetSaveData.partyInventory = partyInv;
        targetSaveData.havenData = havenData;

        // save the new save file
        SaveLoadSystem.Save(targetSaveData, saveFileNumber);
    }

    #endregion




    #region Seed Functions

    /// <summary>
    /// Set game seed to current synced seed.
    /// </summary>
    private void SetupGameSeed()
    {
        GameSeedManager.SetupSeed(gameSeed);
    }

    /// <summary>
    /// Randomizes the current game seed.
    /// </summary>
    private void RandomizeGameSeed()
    {
        // randomize the game seed
        GameSeedManager.RandomizeSeed();

        // set seed to use to the current seed
        SetGameSeed(GameSeedManager.Seed);
    }

    #endregion




    #region Network Functions

    /*[Command]
    public void SpawnObjectOnNetwork(GameObject objArg)
    {
        NetworkServer.Spawn(objArg);
    }*/

    /*[Command]
    public GameObject CmdSpawnObject(NetworkObjectPooler objectPoolerArg, Vector3 posArg, 
        Quaternion rotArg)
    {
        // get object from given pooler
        GameObject pooledObj = objectPoolerArg.GetFromPool(posArg, rotArg);

        // spawn pooled object on network
        NetworkServer.Spawn(pooledObj);

        // return the pooled object
        return pooledObj;
    }

    [Command]
    public void CmdUnspawnObject(NetworkObjectPooler objectPoolerArg, GameObject pooledObjArg)
    {
        // return object to pool on server
        objectPoolerArg.PutBackInPool(pooledObjArg);

        // tell server to send ObjectDestroyMessage, which will call UnspawnHandler on client
        NetworkServer.UnSpawn(pooledObjArg);
    }*/

    #endregion




    #region Spawn Functions

    /// <summary>
    /// Spawns an enemy.
    /// </summary>
    /// <param name="posArg"></param>
    /// <param name="rotArg"></param>
    public void SpawnEnemy(Vector3 posArg, Quaternion rotArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSpawnEnemy(posArg, rotArg);
        }
        else
        {
            SpawnEnemyMain(posArg, rotArg);
        }
    }

    /// <summary>
    /// Perform the act of attacking.
    /// </summary>
    [Command]
    private void CmdSpawnEnemy(Vector3 posArg, Quaternion rotArg)
    {
        SpawnEnemyMain(posArg, rotArg);
    }

    private void SpawnEnemyMain(Vector3 posArg, Quaternion rotArg)
    {
        // get pooled enemy object
        GameObject enemyObj = SpawnManager.GetNetworkObjectPooler(
            ObjectPoolerContentType.EnemyStandard).GetFromPool(posArg, rotArg);
        // spawn pooled object on network
        NetworkServer.Spawn(enemyObj);
    }

    #endregion




    #region Dialogue Functions

    /// <summary>
    /// Returns the next available story dialogue events.
    /// </summary>
    /// <returns></returns>
    public List<DialogueEventData> GetNextAvailableStoryDialogueEvents()
    {
        // get all STORY dialogue events that can potentially be accessed by player
        List<DialogueEventData> potentialStoryDialogues = 
            GetPotentialDialogueEvents(dialogueEventDatabaseStory);

        // initialize total number of events that can be returned
        int MAX_RETURN_COUNT = 3;

        // get next dialogue events
        List<DialogueEventData> nextStoryDialogues = havenData.havenDialogue.
            GetNextAvailableDialogueEvents(potentialStoryDialogues, 
            havenData.havenProgression.GetAverageForAllCumulativeStatPoints(), 
            gameFlags.setFlags, _networkManagerCustom.CanHcontentBeViewed(), 
            MAX_RETURN_COUNT);

        // return dialogue events
        return nextStoryDialogues;
    }

    /// <summary>
    /// Sets up the week's planned social dialogue events.
    /// </summary>
    private void PlanWeekSocialDialogueEvents()
    {
        // get all SOCIAL dialogue events that can potentially be accessed by player
        List<DialogueEventData> potentialSocialDialogues =
            GetPotentialDialogueEvents(dialogueEventDatabaseSocial);

        // setup the week's social dialogue events
        havenData.PlanWeekSocialDialogueEvents(potentialSocialDialogues, gameFlags.setFlags, 
            _networkManagerCustom.CanHcontentBeViewed());
    }

    /// <summary>
    /// Returns all dialogue events from given database that can potentially be accessed by player.
    /// </summary>
    /// <param name="dialgDatabaseArg"></param>
    /// <returns></returns>
    private List<DialogueEventData> GetPotentialDialogueEvents(DialogueEventDatabase dialgDatabaseArg)
    {
        // initialize return list as empty list
        List<DialogueEventData> potentialDialogueEvents = new List<DialogueEventData>();

        // loop through all given database's sets
        foreach (DialogueEventSet iterSet in dialgDatabaseArg.dialogueSets)
        {
            // if iterating set DOES require a flag to access AND player does NOT have that flag
            if (iterSet.DoesRequireFlag() && !gameFlags.IsFlagSet(iterSet.requiredFlag))
            {
                // DONT continue this loop
                break;
            }
            // else no problem accessing iterating set
            else
            {
                // loop through set's dialogue events
                foreach (DialogueEventData iterData in iterSet.dialogueEvents)
                {
                    // add iterating data to return list
                    potentialDialogueEvents.Add(new DialogueEventData(iterData));
                }
            }
        }

        // return populated list
        return potentialDialogueEvents;
    }

    #endregion




    #region Flag Functions

    /// <summary>
    /// Returns whether the game's tutorial has been completed.
    /// </summary>
    /// <returns></returns>
    public bool IsTutorialComplete()
    {
        return gameFlags.IsFlagSet(FLAG_TUTORIAL_COMPLETE);
    }

    /// <summary>
    /// Marks the game's tutorial as compelted.
    /// </summary>
    public void SetTutorialAsComplete()
    {
        gameFlags.SetFlag(FLAG_TUTORIAL_COMPLETE);
    }

    /// <summary>
    /// Returns whether the flag is set for the given arena set number. 
    /// i.e. Has this arena set been reached by player before?
    /// </summary>
    /// <param name="arenaSetArg"></param>
    /// <returns></returns>
    public bool IsArenaStoryFlagSet(int arenaSetArg)
    {
        return gameFlags.IsFlagSet(GetArenaStoryFlag(arenaSetArg));
    }

    /// <summary>
    /// Sets story arena story flag for given arena set.
    /// i.e. Denotes player has reached this set and seen this dialogue already.
    /// </summary>
    /// <param name="arenaSetArg"></param>
    public void SetArenaStoryFlag(int arenaSetArg)
    {
        gameFlags.SetFlag(GetArenaStoryFlag(arenaSetArg));
    }

    /// <summary>
    /// Returns the arena story flag associated with the given arena set.
    /// </summary>
    /// <param name="arenaSetArg"></param>
    /// <returns></returns>
    private string GetArenaStoryFlag(int arenaSetArg)
    {
        // initialize flag prefix
        string ARENA_STORY_FLAG_PREFIX = "arena_";

        return ARENA_STORY_FLAG_PREFIX + arenaSetArg.ToString();
    }

    #endregion




    #region Sync Functions

    public void OnGameStateModeChanged(GameStateMode oldArg,
        GameStateMode newArg)
    {
        // call game state mode change actions if NOT null
        OnGameStateModeChangedAction?.Invoke();
    }

    public void OnGameFlagsChanged(GameFlags oldArg,
        GameFlags newArg)
    {
        // call game flags change actions if NOT null
        OnGameFlagsChangedAction?.Invoke();
    }

    public void OnPartyCharactersChanged(List<CharacterData> oldListArg, 
        List<CharacterData> newListArg)
    {
        // call party char change actions if NOT null
        OnPartyCharactersChangedAction?.Invoke();
    }

    /*void OnPartyCharactersChanged(SyncList<CharacterData>.Operation op, int index, CharacterData oldItem, CharacterData newItem)
    {
        switch (op)
        {
            case SyncList<CharacterData>.Operation.OP_ADD:
                // index is where it got added in the list
                // item is the new item
                break;
            case SyncList<CharacterData>.Operation.OP_CLEAR:
                // list got cleared
                break;
            case SyncList<CharacterData>.Operation.OP_INSERT:
                // index is where it got added in the list
                // item is the new item
                break;
            case SyncList<CharacterData>.Operation.OP_REMOVEAT:
                // index is where it got removed in the list
                // item is the item that was removed
                break;
            case SyncList<CharacterData>.Operation.OP_SET:
                // index is the index of the item that was updated
                // item is the previous item
                break;
        }

        // call party char change actions if NOT null
        OnPartyCharactersChangedAction?.Invoke();
    }*/

    public void OnPartyOrderChanged(List<int> oldListArg, 
        List<int> newListArg)
    {
        // call party order change actions if NOT null
        OnPartyOrderChangedAction?.Invoke();
    }

    public void OnDefaultPartyCharacterChanged(CharacterData oldCharArg, 
        CharacterData newCharArg)
    {
        // call default party character change actions if NOT null
        OnDefaultPartyCharacterChangedAction?.Invoke();
    }

    public void OnPartyInvChanged(PartyInventory oldArg,
        PartyInventory newArg)
    {
        // call party inv change actions if NOT null
        OnPartyInvChangedAction?.Invoke();
    }

    public void OnHavenDataChanged(HavenData oldArg, HavenData newArg)
    {
        // call haven data change actions if NOT null
        OnHavenDataChangedAction?.Invoke();
    }

    public void OnGameSeedChanged(int oldArg, int newArg)
    {
        // call game seed change actions if NOT null
        OnGameSeedChangedAction?.Invoke();
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<GamePausingActionEvent>();
        this.MMEventStartListening<NewSaveCreatedEvent>();
        this.MMEventStartListening<LoadSaveEvent>();
        this.MMEventStartListening<SaveGameProgressEvent>();
        this.MMEventStartListening<NetworkGameLocalJoinedEvent>();
        this.MMEventStartListening<PartyWipeEvent>();
        this.MMEventStartListening<DayAdvanceEvent>();
        this.MMEventStartListening<EnterGameSessionEvent>();

        //partyCharacters.Callback += OnPartyCharactersChanged;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<GamePausingActionEvent>();
        this.MMEventStopListening<NewSaveCreatedEvent>();
        this.MMEventStopListening<LoadSaveEvent>();
        this.MMEventStopListening<SaveGameProgressEvent>();
        this.MMEventStopListening<NetworkGameLocalJoinedEvent>();
        this.MMEventStopListening<PartyWipeEvent>();
        this.MMEventStopListening<DayAdvanceEvent>();
        this.MMEventStopListening<EnterGameSessionEvent>();

        //partyCharacters.Callback -= OnPartyCharactersChanged;
    }

    public void OnMMEvent(GamePausingActionEvent eventType)
    {
        // switches the game speed between paused and unpaused, if not in online session
        SwitchGamePausingIfAppropriate();
    }

    public void OnMMEvent(NewSaveCreatedEvent eventType)
    {
        LoadSaveEvent.Trigger(eventType.saveData, eventType.saveFileNumber);
    }

    public void OnMMEvent(LoadSaveEvent eventType)
    {
        // prepare game for upcoming game session using the given save data
        SetupGameSession(eventType.saveFileNumber, eventType.saveData);
    }

    public void OnMMEvent(SaveGameProgressEvent eventType)
    {
        // saves the current game's progress
        SaveGameProgress();
    }

    public void OnMMEvent(NetworkGameLocalJoinedEvent eventType)
    {
        // set game seed to current synced seed
        SetupGameSeed();
    }

    public void OnMMEvent(PartyWipeEvent eventType)
    {
        // reduces current amount of external resources by the party wipe penalty.
        havenData.havenProgression.InflictPartyWipePenalty();
    }

    public void OnMMEvent(DayAdvanceEvent eventType)
    {
        // advance calendar date by one day
        havenData.AdvanceOneDay();

        // refreshes week's activity planning factors
        havenData.ResetAllPlanning(GetAllPartyMembersExcludingMC());
        
        Debug.Log("NEED IMPL: ensure switched to visualnovel game mode"); // NEED IMPL!!!
    }

    public void OnMMEvent(EnterGameSessionEvent eventType)
    {
        // create a player (with no parent as network objects should be on root??)
        //GameObject createdPlayer = Instantiate(playerPrefab);

        // if tutorial is already complete
        if (IsTutorialComplete())
        {
            // trigger event to start VN game mode state
            GameStateModeTransitionEvent.Trigger(GameStateMode.VisualNovel, 
                ActionProgressType.Started);
        }
        else
        {
            // trigger event to start shooter game mode state
            GameStateModeTransitionEvent.Trigger(GameStateMode.Shooter,
                ActionProgressType.Started);
        }
    }

    #endregion




    #region Getter Functions

    public List<CharacterData> GetPartyCharacters()
    {
        return partyCharacters;
    }

    /*public SyncList<CharacterData> GetPartyCharacters()
    {
        List<CharacterData> returnList = new List<CharacterData>();
        foreach (CharacterData iterChar in partyCharacters)
        {
            returnList.Add(iterChar);
        }
        return returnList;
        return partyCharacters;
    }*/

    #endregion




    #region gameStateMode Setter Functions

    public void SetGameStateMode(GameStateMode modeArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSetGameStateMode(modeArg);
        }
        else
        {
            SetGameStateModeInternal(modeArg);
            OnGameStateModeChanged(modeArg, modeArg);
        }
    }

    [Command]
    public void CmdSetGameStateMode(GameStateMode modeArg)
    {
        SetGameStateModeInternal(modeArg);
    }

    public void SetGameStateModeInternal(GameStateMode modeArg)
    {
        gameStateMode = modeArg;
    }

    #endregion




    #region gameFlags Setter Functions

    public void SetGameFlags(GameFlags flagsArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSetGameFlags(flagsArg);
        }
        else
        {
            SetGameFlagsInternal(flagsArg);
            OnGameFlagsChanged(flagsArg, flagsArg);
        }
    }

    [Command]
    public void CmdSetGameFlags(GameFlags flagsArg)
    {
        SetGameFlagsInternal(flagsArg);
    }

    public void SetGameFlagsInternal(GameFlags flagsArg)
    {
        gameFlags = flagsArg;
    }

    #endregion




    #region PartyCharacters Setter Functions

    public void SetPartyCharacters(List<CharacterData> charListArg)
    {
        if (NetworkClient.isConnected)
        {
            SetPartyCharactersCommand(charListArg);
        }
        else
        {
            SetPartyCharactersInternal(charListArg);
            OnPartyCharactersChanged(charListArg, charListArg);
        }
    }

    [Command]
    public void SetPartyCharactersCommand(List<CharacterData> charListArg)
    {
        SetPartyCharactersInternal(charListArg);
    }

    public void SetPartyCharactersInternal(List<CharacterData> charListArg)
    {
        partyCharacters = charListArg;
    }

    /*public void SetPartyCharacters(SyncList<CharacterData> charListArg)
    {
        if (NetworkClient.isConnected)
        {
            SetPartyCharactersCommand(charListArg);
        }
        else
        {
            SetPartyCharactersInternal(charListArg);
            //OnPartyCharactersChanged(charListArg, charListArg);
        }
    }

    [Command]
    public void SetPartyCharactersCommand(SyncList<CharacterData> charListArg)
    {
        SetPartyCharactersInternal(charListArg);
    }

    public void SetPartyCharactersInternal(SyncList<CharacterData> charListArg)
    {
        partyCharacters.Clear();
        partyCharacters.AddRange(charListArg);
    }*/

    #endregion




    #region PartyOrder Setter Functions

    public void SetPartyOrder(List<int> orderListArg)
    {
        if (NetworkClient.isConnected)
        {
            SetPartyOrderCommand(orderListArg);
        }
        else
        {
            SetPartyOrderInternal(orderListArg);
            OnPartyOrderChanged(orderListArg, orderListArg);
        }
    }

    [Command(ignoreAuthority = true)]
    public void SetPartyOrderCommand(List<int> orderListArg)
    {
        SetPartyOrderInternal(orderListArg);
    }

    public void SetPartyOrderInternal(List<int> orderListArg)
    {
        partyOrder = orderListArg;
    }

    #endregion




    #region DefaultPartyCharacter Setter Functions

    public void SetDefaultPartyCharacter(CharacterData charArg)
    {
        if (NetworkClient.isConnected)
        {
            SetDefaultPartyCharacterCommand(charArg);
        }
        else
        {
            SetDefaultPartyCharacterInternal(charArg);
        }
    }

    [Command]
    public void SetDefaultPartyCharacterCommand(CharacterData charArg)
    {
        SetDefaultPartyCharacterInternal(charArg);
    }

    public void SetDefaultPartyCharacterInternal(CharacterData charArg)
    {
        defaultPartyCharacter = charArg;
    }

    #endregion




    #region PartyInv Setter Functions

    public void SetPartyInv(PartyInventory invArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSetPartyInv(invArg);
        }
        else
        {
            SetPartyInvInternal(invArg);
        }
    }

    [Command]
    public void CmdSetPartyInv(PartyInventory invArg)
    {
        SetPartyInvInternal(invArg);
    }

    public void SetPartyInvInternal(PartyInventory invArg)
    {
        partyInv = invArg;
    }

    #endregion




    #region havenData Setter Functions

    public void SetHavenData(HavenData dataArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSetHavenData(dataArg);
        }
        else
        {
            SetHavenDataInternal(dataArg);
            OnHavenDataChanged(dataArg, dataArg);
        }
    }

    [Command]
    public void CmdSetHavenData(HavenData dataArg)
    {
        SetHavenDataInternal(dataArg);
    }

    public void SetHavenDataInternal(HavenData dataArg)
    {
        havenData = dataArg;
    }

    #endregion




    #region gameSeed Setter Functions

    public void SetGameSeed(int seedArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSetGameSeed(seedArg);
        }
        else
        {
            SetGameSeedInternal(seedArg);
            OnGameSeedChanged(seedArg, seedArg);
        }
    }

    [Command]
    public void CmdSetGameSeed(int seedArg)
    {
        SetGameSeedInternal(seedArg);
    }

    public void SetGameSeedInternal(int seedArg)
    {
        gameSeed = seedArg;
    }

    #endregion


}
