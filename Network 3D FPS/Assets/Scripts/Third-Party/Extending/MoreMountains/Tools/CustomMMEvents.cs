using MoreMountains.Tools;
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

