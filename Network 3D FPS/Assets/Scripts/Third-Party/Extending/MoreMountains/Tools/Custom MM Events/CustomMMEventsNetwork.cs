using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// An event fired when the multiplayer server is started up.
/// </summary>
public struct NetworkServerStartEvent
{
	static NetworkServerStartEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when local user joins a multiplayer game.
/// </summary>
public struct NetworkGameLocalJoinedEvent
{
	public PlayerCharacterMasterController joiningChar;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="joiningCharArg"></param>
	public NetworkGameLocalJoinedEvent(PlayerCharacterMasterController joiningCharArg)
	{
		joiningChar = joiningCharArg;
	}

	static NetworkGameLocalJoinedEvent e;
	public static void Trigger(PlayerCharacterMasterController joiningCharArg)
	{
		e.joiningChar = joiningCharArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a server connected a new player.
/// </summary>
public struct ServerAddedPlayerEvent
{
	public NetworkConnection networkConnection;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="networkConnectionArg"></param>
	public ServerAddedPlayerEvent(NetworkConnection networkConnectionArg)
	{
		networkConnection = networkConnectionArg;
	}

	static ServerAddedPlayerEvent e;
	public static void Trigger(NetworkConnection networkConnectionArg)
	{
		e.networkConnection = networkConnectionArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a server disconnected a player.
/// </summary>
public struct ServerDisconnectedPlayerEvent
{
	public NetworkConnection networkConnection;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="networkConnectionArg"></param>
	public ServerDisconnectedPlayerEvent(NetworkConnection networkConnectionArg)
	{
		networkConnection = networkConnectionArg;
	}

	static ServerDisconnectedPlayerEvent e;
	public static void Trigger(NetworkConnection networkConnectionArg)
	{
		e.networkConnection = networkConnectionArg;

		MMEventManager.TriggerEvent(e);
	}
}
