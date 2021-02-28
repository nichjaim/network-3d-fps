﻿using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event fired when an player performs a primary attack.
/// </summary>
public struct PlayerAttackEvent
{
	public int playerNumber;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="playerNumberArg"></param>
	public PlayerAttackEvent(int playerNumberArg)
	{
		playerNumber = playerNumberArg;
	}

	static PlayerAttackEvent e;
	public static void Trigger(int playerNumberArg)
	{
		e.playerNumber = playerNumberArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when starting a fresh new game.
/// </summary>
public struct StartNewGameEvent
{
	static StartNewGameEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a new save file finished being created.
/// </summary>
public struct NewSaveCreatedEvent
{
	public GameSaveData saveData;
	public int saveFileNumber;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="saveDataArg"></param>
	/// <param name="saveFileNumberArg"></param>
	public NewSaveCreatedEvent(GameSaveData saveDataArg, int saveFileNumberArg)
	{
		saveData = saveDataArg;
		saveFileNumber = saveFileNumberArg;
	}

	static NewSaveCreatedEvent e;
	public static void Trigger(GameSaveData saveDataArg, int saveFileNumberArg)
	{
		e.saveData = saveDataArg;
		e.saveFileNumber = saveFileNumberArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when attempting a game pause/unpause.
/// </summary>
public struct GamePausingActionEvent
{
	static GamePausingActionEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when leavign the current game session.
/// </summary>
public struct ExitGameSessionEvent
{
	static ExitGameSessionEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when the asset bundle system is initialized.
/// </summary>
public struct BundleSystemSetupEvent
{
	static BundleSystemSetupEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

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
/// An event fired when user joins a multiplayer game.
/// </summary>
public struct NetworkGameJoinedEvent
{
	static NetworkGameJoinedEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a game load slot is pressed.
/// </summary>
public struct LoadSlotPressedEvent
{
	public GameSaveData saveData;
	public int saveFileNumber;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="saveDataArg"></param>
	/// <param name="saveFileNumberArg"></param>
	public LoadSlotPressedEvent(GameSaveData saveDataArg, int saveFileNumberArg)
	{
		saveData = saveDataArg;
		saveFileNumber = saveFileNumberArg;
	}

	static LoadSlotPressedEvent e;
	public static void Trigger(GameSaveData saveDataArg, int saveFileNumberArg)
	{
		e.saveData = saveDataArg;
		e.saveFileNumber = saveFileNumberArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when entering the haven game state.
/// </summary>
public struct StartHavenGameStateEvent
{
	static StartHavenGameStateEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

