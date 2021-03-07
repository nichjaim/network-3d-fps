using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MoreMountains.Tools;
using BundleSystem;

public class NetworkManagerCustom : NetworkManager, MMEventListener<ExitGameSessionEvent>, 
    MMEventListener<StartNewGameEvent>, MMEventListener<BundleSystemSetupEvent>,
    MMEventListener<MenuActivationEvent>
{
    #region Class Variables

    [Header("Performance Properties")]

    [SerializeField]
    private int targetFramerate = 60;

    [Header("Bundle System Properties")]

    [SerializeField]
    private bool showBundleSystemDebugGUI = false;
    [SerializeField]
    private bool logBundleSystemMessages = false;

    // the players who are connected to the multiplayer session
    internal readonly List<PlayerCharacterMasterController> connectedPlayers = new List<PlayerCharacterMasterController>();

    [Header("Mechanic Properties")]

    // bool that denotes if game should be messing with cursor lock and visbility.
    [SerializeField]
    private bool shouldAlterCursorFreedom = false;

    private List<MasterMenuControllerCustom> openMenus = new List<MasterMenuControllerCustom>();

    #endregion




    #region MonoBehaviour Functions

    public override void Awake()
    {
        base.Awake();

        // limit FPS to the targeted framerate
        SetupTargetFramerate();
        // setup the bundle system for use
        InitializeBundleSystem();
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
    /// Limit FPS to the targeted framerate. 
    /// Call in Awake().
    /// </summary>
    private void SetupTargetFramerate()
    {
        // if trying to target a valid framerate
        if (targetFramerate > 0)
        {
            // limit FPS to the targeted framerate
            Application.targetFrameRate = targetFramerate;
        }
        // else not trying to target a specific framerate
        else
        {
            /// set FPS to special value that indicates that the game 
            /// should render at the platform's default framerate
            Application.targetFrameRate = -1;
        }
    }

    /// <summary>
    /// Sets up the bundle system for use. 
    /// Call in Start().
    /// </summary>
    private void InitializeBundleSystem()
    {
        StartCoroutine(InitializeBundleSystemInternal());
    }

    private IEnumerator InitializeBundleSystemInternal()
    {
        // set whether the bundle system debug UI is shown based on set bool
        BundleManager.ShowDebugGUI = showBundleSystemDebugGUI;
        // set whether the bundle system logs messages based on set bool
        BundleManager.LogMessages = logBundleSystemMessages;

        // initialize the bundle system and wait till finished
        yield return BundleManager.Initialize();

        // trigger event to denote that the bundle system is finished initializing
        BundleSystemSetupEvent.Trigger();
    }

    #endregion




    #region Override Functions

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);

        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        //PlayerCharacterMasterController playerCharMaster = player.GetComponent<PlayerCharacterMasterController>();
        //player.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        //networkCoord.SetIsNetworkConnected(true);

        // assigns the appropriate player number to all connected players
        RefreshPlayerNumbers();
        //Debug.Log($"importantNumber = {importantNumber}"); // TEST LINE

        // trigger event to denote that server added a new player
        ServerAddedPlayerEvent.Trigger(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // trigger event denoting server started
        NetworkServerStartEvent.Trigger();

        // get zombie network spawnable prefab
        GameObject zombieSpawnPrefab = spawnPrefabs.Find(prefab => prefab.name == "Zombie");
        // instanitate zombie into world
        GameObject spawnedZombie = Instantiate(zombieSpawnPrefab, new Vector3(10f, 0f, 10f), Quaternion.identity);
        // spawn the created zombie on the network
        NetworkServer.Spawn(spawnedZombie);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        // assigns the appropriate player number to all connected players
        RefreshPlayerNumbers();
        // trigger event to denote that server disconnected a player
        ServerDisconnectedPlayerEvent.Trigger(conn);
    }

    #endregion




    #region Network Functions

    /// <summary>
    /// Starts an offline network session, used for single player.
    /// </summary>
    private void StartOfflineHost()
    {
        // ensure no actual network connections can be made
        maxConnections = 0;
        // point address to local machine
        networkAddress = GetLocalHostNetworkAddress();
        // start host server
        StartServer();
    }

    /// <summary>
    /// Stops the current game network session based on connection purpose.
    /// </summary>
    private void StopGameSession()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            StopServer();
        }
    }

    /// <summary>
    /// Assigns the appropriate player number to all connected players. 
    /// Call in OnServerAddPlayer() and OnServerDisconnect().
    /// </summary>
    private void RefreshPlayerNumbers()
    {
        // loop through all connected players
        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            // assign the appropriate player number to iterating player
            connectedPlayers[i].SetPlayerNumber(i + 1);
        }
    }

    /// <summary>
    /// Register all relevant prefabs for network spawnability.
    /// </summary>
    private void RegisterSpawnablePrefabs()
    {
        // load all enemy character prefabs
        GameObject[] enemyCharPrefabs = AssetRefMethods.LoadAllBundleAssetCharacterEnemy();
        // loop through all iterating prefabs
        foreach (GameObject iterObj in enemyCharPrefabs)
        {
            // registering iterating prefab to network spawnable objects
            ClientScene.RegisterPrefab(iterObj);
        }
    }

    /// <summary>
    /// Stop any new players from joining session.
    /// </summary>
    public void CloseToNewConnections()
    {
        maxConnections = numPlayers;
    }

    /// <summary>
    /// Allows new players to join session.
    /// </summary>
    /// <param name="totalPotentialConnectionsArg"></param>
    private void OpenToNewConnections(int totalPotentialConnectionsArg)
    {
        maxConnections = totalPotentialConnectionsArg;
    }

    /// <summary>
    /// Allows the standard amount of players to join session.
    /// </summary>
    public void OpenToNewConnectionsDefault()
    {
        OpenToNewConnections(4);
    }

    /// <summary>
    /// Is the user hosting an online game session.
    /// </summary>
    /// <returns></returns>
    public bool IsHostingOnlineSession()
    {
        return networkAddress != GetLocalHostNetworkAddress() && numPlayers >= 1;
    }

    /// <summary>
    /// Returns the network address for a machine's local hosting.
    /// </summary>
    /// <returns></returns>
    private string GetLocalHostNetworkAddress()
    {
        return "localhost";
    }

    /// <summary>
    /// Starts hosting an online game session.
    /// </summary>
    public void StartHostingOnlineSession()
    {
        // stop local offline hosting (not sure if supposed to do this step)
        StopHost();

        // set the network address to the user's local IP
        networkAddress = GeneralMethods.GetLocalIPAddress();
        // allows the standard amount of players to join session
        OpenToNewConnectionsDefault();

        // start host
        StartHost();
    }

    #endregion




    #region Game Functions

    /// <summary>
    /// Creates a brand new save game file. 
    /// Returns the newly created save data and save file number.
    /// </summary>
    private (GameSaveData, int) CreateNewSaveGame()
    {
        // create a new save data
        GameSaveData newSaveData = new GameSaveData();
        // get the next available save file number
        int newSaveFile = SaveLoadSystem.GetNumberOfSaveFiles() + 1;

        // load all starting playable character data
        CharacterDataTemplate[] playableCharDataTemplates = AssetRefMethods.
            LoadAllBundleAssetPlayableCharacterDataTemplate();

        // setup fresh empty char data list
        List<CharacterData> startingPlayableCharData = new List<CharacterData>();
        // loop trhough all loaded templates
        foreach (CharacterDataTemplate iterTemp in playableCharDataTemplates)
        {
            // add a playable char data to list that is derived from iterating template
            startingPlayableCharData.Add(new CharacterData(iterTemp));
        }
        // set the new save file's char data to starting data
        newSaveData.playableCharacterData = startingPlayableCharData;

        // save the new save file
        //SaveLoadSystem.Save(newSaveData, newSaveFile); // TEMP COMMENT OUT FOR TESTING PURPOSES!!!

        // return the new save file properties
        return (newSaveData, newSaveFile);
    }

    /// <summary>
    /// Sets cursor's lock and visible status.
    /// </summary>
    /// <param name="shouldFreeArg"></param>
    private void SetCursorFreedom(bool shouldFreeArg)
    {
        // if game should NOT be messing with cursor lock and visbility.
        if (!shouldAlterCursorFreedom)
        {
            // DONT continue code
            return;
        }

        // if should unlock cursor
        if (shouldFreeArg)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // else should lock cursor
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // set cursor visiblity based on given freedom status
        Cursor.visible = shouldFreeArg;
    }

    #endregion




    #region Menu Functions

    /// <summary>
    /// Sets up list of menus that denote what menus is open.
    /// </summary>
    /// <param name="menuArg"></param>
    /// <param name="isOpeningArg"></param>
    private void RefreshOpenMenus(MasterMenuControllerCustom menuArg, bool isOpeningArg)
    {
        // if given menu is considered open when it is closing OR closed when it is opening
        if (openMenus.Exists(iterMenu => iterMenu == menuArg) != isOpeningArg)
        {
            // if menu opening
            if (isOpeningArg)
            {
                // add menu to list of open menus
                openMenus.Add(menuArg);
            }
            // else menu closing
            else
            {
                // remove menu from list of open menus
                openMenus.Remove(menuArg);
            }
        }
    }

    /// <summary>
    /// Returns bool that denotes if any menu is open.
    /// </summary>
    /// <returns></returns>
    public bool IsAnyMenuOpen()
    {
        return openMenus.Count > 0;
    }

    /// <summary>
    /// Returns bool that denotes if the current state of menu's allow the 
    /// player chars to perform actions
    /// </summary>
    public bool DoesMenusStateAllowPlayerCharacterAction()
    {
        return !IsAnyMenuOpen();
    }

    /// <summary>
    /// Returns bool that denotes if the current state of menu's allow the 
    /// player chars to be harmed.
    /// </summary>
    public bool DoesMenusStateAllowPlayerCharacterHarming()
    {
        // if one of the open menus is the haven menu then char can NOT be harmed
        return !openMenus.Exists(iterMenu => (iterMenu as HavenMenuController) != null);
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<ExitGameSessionEvent>();
        this.MMEventStartListening<StartNewGameEvent>();
        this.MMEventStartListening<BundleSystemSetupEvent>();
        this.MMEventStartListening<MenuActivationEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
        this.MMEventStopListening<StartNewGameEvent>();
        this.MMEventStopListening<BundleSystemSetupEvent>();
        this.MMEventStopListening<MenuActivationEvent>();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        // stops the current game network session based on connection purpose
        StopGameSession();
    }

    public void OnMMEvent(StartNewGameEvent eventType)
    {
        // starts an offline network session
        StartOfflineHost();

        // create a brand new save game file
        (GameSaveData, int) newGameSave = CreateNewSaveGame();
        // trigger event to denote that new save game file was created
        NewSaveCreatedEvent.Trigger(newGameSave.Item1, newGameSave.Item2);

        // create a player (with no parent as network objects should be on root??)
        GameObject createdPlayer = Instantiate(playerPrefab);
        //createdPlayer.GetComponent<PlayerNetworkCoordinator>();
    }

    public void OnMMEvent(BundleSystemSetupEvent eventType)
    {
        // register all relevant prefabs for network spawnability
        //RegisterSpawnablePrefabs();
    }

    public void OnMMEvent(MenuActivationEvent eventType)
    {
        // setup list of menus that denote what menus is open
        RefreshOpenMenus(eventType.menu, eventType.isOpening);
        // sets cursor's lock and visible status based on menu activation
        SetCursorFreedom(IsAnyMenuOpen());
    }

    #endregion


}
