﻿using Mirror;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMasterController : CharacterMasterController, MMEventListener<ExitGameSessionEvent>,
    MMEventListener<NetworkServerStartEvent>, MMEventListener<GameStateModeTransitionEvent>
{
    #region Class Variables

    [Header("Player Component References")]

    [SerializeField]
    private ObjectActivationController _objectActivationController = null;

    [SyncVar(hook = nameof(OnPlayerNumberChange))]
    private int playerNumber = 1;

    /// the char data used for info/stats/etc. (needed in single player when this char data 
    /// is determined by equipped weapon slot)
    [SyncVar(hook = nameof(OnCharDataSecondaryChanged))]
    private CharacterData charDataSecondary = null;
    public Action OnCharDataSecondaryChangedAction;
    public CharacterData CharDataSecondary
    {
        get { return charDataSecondary; }
    }

    #endregion




    #region MonoBehaviour Functions

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(netIdentity))
        {
            // DONT continue code
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            //GameStateModeTransitionEvent.Trigger(GameStateMode.VisualNovel, ActionProgressType.Started);
            ReturnToHavenEvent.Trigger();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PartyWipeEvent.Trigger();
        }

        /*if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("((NetworkManagerCustom)NetworkManager.singleton).numPlayers = " +
                ((NetworkManagerCustom)NetworkManager.singleton).numPlayers);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("((NetworkManagerCustom)NetworkManager.singleton).connectedPlayers.Count = " +
                ((NetworkManagerCustom)NetworkManager.singleton).connectedPlayers.Count);
        }*/
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // attempt to set UI manager's player char to this char
        UIManager.Instance.SetPlayerCharacterIfAppropriate(this);

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
        this.MMEventStartListening<GameStateModeTransitionEvent>();

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
        this.MMEventStopListening<GameStateModeTransitionEvent>();

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

    public void OnMMEvent(GameStateModeTransitionEvent eventType)
    {
        switch (eventType.gameMode)
        {
            case GameStateMode.Shooter:
                switch (eventType.transitionProgress)
                {
                    case ActionProgressType.Started:
                    case ActionProgressType.InProgress:
                        _objectActivationController.SetObjectActivation(true);
                        Debug.Log("Got here"); // DEBUG LINE!!!
                        break;
                    case ActionProgressType.Finished:
                        _objectActivationController.SetObjectActivation(false);
                        break;
                }
                break;
            case GameStateMode.VisualNovel:
                _objectActivationController.SetObjectActivation(false);
                break;
        }
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

    [Command(ignoreAuthority = true)]
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
