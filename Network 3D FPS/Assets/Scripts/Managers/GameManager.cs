using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using BundleSystem;
using System;

public class GameManager : NetworkBehaviour, MMEventListener<GamePausingActionEvent>, 
    MMEventListener<NewSaveCreatedEvent>
{
    #region Class Variables

    public static GameManager Instance;

    [SyncVar(hook = nameof(OnPlayableCharactersChanged))]
    private List<CharacterData> playableCharacters = new List<CharacterData>();
    public Action OnPlayableCharactersChangedAction;

    [SyncVar(hook = nameof(OnPlayerNumbersToPlayableCharactersChanged))]
    private List<int> playerNumbersToPlayableCharacters = new List<int>();
    public Action OnPlayerNumbersToPlayableCharactersChangedAction;

    // the character who is chosen when all playable characters being used
    [SerializeField]
    private CharacterDataTemplate defaultPlayableCharTemplate = null;
    [SyncVar(hook = nameof(OnDefaultPlayableCharChanged))]
    private CharacterData defaultPlayableChar = null;

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
    private void SetPlayerCharacterToPlayableCharactersToDefault()
    {
        // initialize empty order list
        List<int> orderList = new List<int>();
        // add standard multiplayer player numbers to list
        orderList.Add(1);
        orderList.Add(2);
        orderList.Add(3);
        orderList.Add(4);

        // set order list to created list
        SetPlayerNumbersToPlayableCharacters(orderList);
    }

    /// <summary>
    /// Returns the playable character associated with the given player number 
    /// based on the party order.
    /// </summary>
    /// <param name="playerNumberArg"></param>
    /// <returns></returns>
    public CharacterData GetAppropriatePlayableCharacter(int playerNumberArg)
    {
        /// find the index for the playable char list that is associated with 
        /// the order list based on the given player num
        int playableCharListMatchingIndex = playerNumbersToPlayableCharacters.
            FindIndex(iterPlayerNum => iterPlayerNum == playerNumberArg);
        // if valid playable char found
        if (playableCharListMatchingIndex != -1)
        {
            // return appropriate playable char
            return playableCharacters[playableCharListMatchingIndex];
        }
        // else no playable char was matched
        else
        {
            // if default playable char not setup
            if (defaultPlayableChar == null)
            {
                // setup the default playable character's data
                SetupDefaultPlayableCharacter();
            }

            // return setup default character
            return defaultPlayableChar;
        }
    }

    /// <summary>
    /// Sets up the default playable character's data.
    /// </summary>
    [Command]
    private void SetupDefaultPlayableCharacter()
    {
        // setup the default character's base properties
        defaultPlayableChar = new CharacterData(defaultPlayableCharTemplate);
        /// scale the character's stats to be around lower than average compared to the 
        /// other playable character's current stats
        Debug.LogWarning("NEEDS IMPL: Default char stat scaling"); // NEED IMPL
    }

    #endregion




    #region Sync Functions

    public void OnPlayableCharactersChanged(List<CharacterData> oldListArg, 
        List<CharacterData> newListArg)
    {
        // call playable char change actions if NOT null
        OnPlayableCharactersChangedAction?.Invoke();
    }

    public void OnPlayerNumbersToPlayableCharactersChanged(List<int> oldListArg, 
        List<int> newListArg)
    {
        // call party order change actions if NOT null
        OnPlayerNumbersToPlayableCharactersChangedAction?.Invoke();
    }

    public void OnDefaultPlayableCharChanged(CharacterData oldCharArg, CharacterData newCharArg)
    {
        // nothing for now
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
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<GamePausingActionEvent>();
        this.MMEventStopListening<NewSaveCreatedEvent>();
    }

    public void OnMMEvent(GamePausingActionEvent eventType)
    {
        // switches the game speed between paused and unpaused, if not in online session
        SwitchGamePausingIfAppropriate();
    }

    public void OnMMEvent(NewSaveCreatedEvent eventType)
    {
        SetPlayableCharacters(eventType.saveData.playableCharacterData);
        // setup player char order list to the default order
        SetPlayerCharacterToPlayableCharactersToDefault();
    }

    #endregion




    #region Setter Functions

    public void SetPlayableCharacters(List<CharacterData> charListArg)
    {
        if (NetworkClient.isConnected)
        {
            SetPlayableCharactersCommand(charListArg);
        }
        else
        {
            SetPlayableCharactersInternal(charListArg);
        }
    }

    [Command]
    public void SetPlayableCharactersCommand(List<CharacterData> charListArg)
    {
        SetPlayableCharactersInternal(charListArg);
    }

    public void SetPlayableCharactersInternal(List<CharacterData> charListArg)
    {
        playableCharacters = charListArg;
    }

    public void SetPlayerNumbersToPlayableCharacters(List<int> orderListArg)
    {
        if (NetworkClient.isConnected)
        {
            SetPlayerNumbersToPlayableCharactersCommand(orderListArg);
        }
        else
        {
            SetPlayerNumbersToPlayableCharactersInternal(orderListArg);
        }
    }

    [Command]
    public void SetPlayerNumbersToPlayableCharactersCommand(List<int> orderListArg)
    {
        SetPlayerNumbersToPlayableCharactersInternal(orderListArg);
    }

    public void SetPlayerNumbersToPlayableCharactersInternal(List<int> orderListArg)
    {
        playerNumbersToPlayableCharacters = orderListArg;
    }

    #endregion


}
