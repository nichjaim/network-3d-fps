using Mirror;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMasterController : CharacterMasterController, MMEventListener<ExitGameSessionEvent>,
    MMEventListener<NetworkServerStartEvent>
{
    #region Class Variables

    private NetworkIdentity _networkIdentity;

    [SyncVar(hook = nameof(OnPlayerNumberChange))]
    private int playerNumber = 1;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
    }

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartHavenGameStateEvent.Trigger();
        }
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // sets the char master's character data based on the current player number
        RefreshPlayerCharData();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Override Functions

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Add this to the static Players List
        ((NetworkManagerCustom)NetworkManager.singleton).connectedPlayers.Add(this);
    }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistant storage</para>
    /// </summary>
    public override void OnStopServer()
    {
        CancelInvoke();
        // Remove this to the static Players List
        ((NetworkManagerCustom)NetworkManager.singleton).connectedPlayers.Remove(this);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // trigger event to denote that network game has been joined
        NetworkGameJoinedEvent.Trigger();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        // if this user's character is leacing multiplayer session
        if (isLocalPlayer)
        {
            // trigger event to exit the game session
            ExitGameSessionEvent.Trigger();
        }
    }

    #endregion




    #region Player Character Functions

    /// <summary>
    /// Sets the char master's character data based on the current player number.
    /// </summary>
    private void RefreshPlayerCharData()
    {
        SetCharData(GameManager.Instance.GetAppropriatePlayableCharacter(playerNumber));
    }

    #endregion




    #region Network Functions

    /// <summary>
    /// Destroys this object if it is not connected to a network session.
    /// </summary>
    private void DestroyIfNotConnectedToNetwork()
    {
        // if NOT connected to network
        if (netId == 0)
        {
            // destroy this player object
            Destroy(gameObject);
        }
    }

    #endregion




    #region Sync Functions

    private void OnPlayerNumberChange(int oldNumArg, int newNumArg)
    {
        // nothing right now
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
        this.MMEventStartListening<NetworkServerStartEvent>();

        GameManager.Instance.OnPlayableCharactersChangedAction += RefreshPlayerCharData;
        GameManager.Instance.OnPlayerNumbersToPlayableCharactersChangedAction += RefreshPlayerCharData;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
        this.MMEventStopListening<NetworkServerStartEvent>();

        GameManager.Instance.OnPlayableCharactersChangedAction -= RefreshPlayerCharData;
        GameManager.Instance.OnPlayerNumbersToPlayableCharactersChangedAction -= RefreshPlayerCharData;
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        // destroys this object if it is not connected to a network session
        DestroyIfNotConnectedToNetwork();
    }

    public void OnMMEvent(NetworkServerStartEvent eventType)
    {
        // destroys this object if it is not connected to a network session
        DestroyIfNotConnectedToNetwork();
    }

    #endregion




    #region Getter Functions

    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    #endregion




    #region Setter Functions

    public void SetPlayerNumber(int playerNumArg)
    {
        playerNumber = playerNumArg;
    }

    #endregion
}
