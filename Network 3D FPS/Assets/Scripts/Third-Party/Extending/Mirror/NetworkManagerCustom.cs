using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MoreMountains.Tools;

public class NetworkManagerCustom : NetworkManager, MMEventListener<ExitGameSessionEvent>
{
    #region MonoBehaviour Functions

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




    #region Override Functions

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);

        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        PlayerNetworkCoordinator networkCoord = player.GetComponent<PlayerNetworkCoordinator>();
        //networkCoord.SetIsNetworkConnected(true);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // get zombie network spawnable prefab
        GameObject zombieSpawnPrefab = spawnPrefabs.Find(prefab => prefab.name == "Zombie");
        // instanitate zombie into world
        GameObject spawnedZombie = Instantiate(zombieSpawnPrefab, new Vector3(10f, 0f, 10f), Quaternion.identity);
        // spawn the created zombie on the network
        NetworkServer.Spawn(spawnedZombie);
    }

    #endregion




    #region Network Functions

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

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<ExitGameSessionEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        // stops the current game network session based on connection purpose
        StopGameSession();
    }

    #endregion


}
