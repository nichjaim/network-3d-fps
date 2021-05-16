using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/// An event fired when a save file is to be loaded.
/// </summary>
public struct LoadSaveEvent
{
	public GameSaveData saveData;
	public int saveFileNumber;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="saveDataArg"></param>
	/// <param name="saveFileNumberArg"></param>
	public LoadSaveEvent(GameSaveData saveDataArg, int saveFileNumberArg)
	{
		saveData = saveDataArg;
		saveFileNumber = saveFileNumberArg;
	}

	static LoadSaveEvent e;
	public static void Trigger(GameSaveData saveDataArg, int saveFileNumberArg)
	{
		e.saveData = saveDataArg;
		e.saveFileNumber = saveFileNumberArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when need to save the current game session progress.
/// </summary>
public struct SaveGameProgressEvent
{
	static SaveGameProgressEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when entering the current game session.
/// </summary>
public struct EnterGameSessionEvent
{
	public GameSaveData saveData;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="saveDataArg"></param>
	public EnterGameSessionEvent(GameSaveData saveDataArg)
	{
		saveData = saveDataArg;
	}

	static EnterGameSessionEvent e;
	public static void Trigger(GameSaveData saveDataArg)
	{
		e.saveData = saveDataArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when leaving the current game session.
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
/// An event fired when the game state mode is being changed.
/// </summary>
public struct GameStateModeTransitionEvent
{
	public GameStateMode gameMode;
	public ActionProgressType transitionProgress;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="gameModeArg"></param>
	/// <param name="transitionProgressArg"></param>
	public GameStateModeTransitionEvent(GameStateMode gameModeArg,
		ActionProgressType transitionProgressArg)
	{
		gameMode = gameModeArg;
		transitionProgress = transitionProgressArg;
	}

	static GameStateModeTransitionEvent e;
	public static void Trigger(GameStateMode gameModeArg,
		ActionProgressType transitionProgressArg)
	{
		e.gameMode = gameModeArg;
		e.transitionProgress = transitionProgressArg;

		MMEventManager.TriggerEvent(e);
	}
}
