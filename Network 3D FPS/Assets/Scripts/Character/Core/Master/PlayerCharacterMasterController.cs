using Mirror;
using MoreMountains.Tools;
using System;
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

    /// the char data used for info/stats/etc. (needed in single player when this char data 
    /// is determined by equipped weapon slot)
    [SyncVar(hook = nameof(OnCharDataSecondaryChanged))]
    private CharacterData charDataSecondary = null;
    public Action OnCharDataSecondaryChangedAction;

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
        //RefreshPlayerCharData();
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

        // trigger event to denote that this player joined a network game
        NetworkGameLocalJoinedEvent.Trigger(this);
    }

    /*public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("OnStartClient Called!"); // DEBUG LINE
        // sets the char master's character data based on the current player number
        RefreshPlayerCharData();
        // trigger event to denote that this player joined a network game
        NetworkGameLocalJoinedEvent.Trigger(this);
    }*/

    public override void OnStopClient()
    {
        base.OnStopClient();

        // if this user's character is leaving multiplayer session
        if (isLocalPlayer)
        {
            // trigger event to exit the game session
            ExitGameSessionEvent.Trigger();
        }
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

    public void OnCharDataSecondaryChanged(CharacterData oldCharArg,
        CharacterData newCharArg)
    {
        // call char data change actions if NOT null
        OnCharDataSecondaryChangedAction?.Invoke();
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

        //GameManager.Instance.OnPartyCharactersChangedAction += RefreshPlayerCharData;
        //GameManager.Instance.OnPartyOrderChangedAction += RefreshPlayerCharData;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
        this.MMEventStopListening<NetworkServerStartEvent>();

        //GameManager.Instance.OnPartyCharactersChangedAction -= RefreshPlayerCharData;
        //GameManager.Instance.OnPartyOrderChangedAction -= RefreshPlayerCharData;
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

    public CharacterData GetCharDataSecondary()
    {
        return charDataSecondary;
    }

    #endregion




    #region Setter Functions

    public void SetPlayerNumber(int playerNumArg)
    {
        playerNumber = playerNumArg;
    }

    public void SetCharDataSecondary(CharacterData charDataArg)
    {
        // call internal function as coroutine
        StartCoroutine(SetCharDataSecondaryInternal(charDataArg));
    }

    private IEnumerator SetCharDataSecondaryInternal(CharacterData charDataArg)
    {
        /// wait till network properties setup (NOTE: not sure why or if should 
        /// do this, but seems to fix some things, idk)
        yield return new WaitForEndOfFrame();

        if (NetworkClient.isConnected)
        {
            CmdSetCharDataSecondary(charDataArg);
        }
        else
        {
            SetCharDataSecondaryMain(charDataArg);
            // call sync hook method, as won't be called in offline mode
            OnCharDataSecondaryChanged(charDataArg, charDataArg);
        }
    }

    //[Command(ignoreAuthority = true)]
    [Command]
    private void CmdSetCharDataSecondary(CharacterData charDataArg)
    {
        SetCharDataSecondaryMain(charDataArg);
    }

    private void SetCharDataSecondaryMain(CharacterData charDataArg)
    {
        charDataSecondary = charDataArg;
    }

    #endregion
}
