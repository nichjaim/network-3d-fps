using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/// An event fired when all players have been defeated.
/// </summary>
public struct PartyWipeEvent
{
	static PartyWipeEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when the players successfully make it back to the 
/// haven while all still alive.
/// </summary>
public struct ReturnToHavenEvent
{
	static ReturnToHavenEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a calendar date advances by one day.
/// </summary>
public struct DayAdvanceEvent
{
	static DayAdvanceEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a teleporter is used.
/// </summary>
public struct TeleportingEvent
{
	public TeleporterController teleporter;
	public GameObject teleportingObject;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="teleporterArg"></param>
	/// <param name="teleportingObjectArg"></param>
	public TeleportingEvent(TeleporterController teleporterArg,
		GameObject teleportingObjectArg)
	{
		teleporter = teleporterArg;
		teleportingObject = teleportingObjectArg;
	}

	static TeleportingEvent e;
	public static void Trigger(TeleporterController teleporterArg,
		GameObject teleportingObjectArg)
	{
		e.teleporter = teleporterArg;
		e.teleportingObject = teleportingObjectArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a spawned character lost all health.
/// </summary>
public struct SpawnedCharacterDeathEvent
{
	public CharacterMasterController charMaster;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="charMasterArg"></param>
	public SpawnedCharacterDeathEvent(CharacterMasterController charMasterArg)
	{
		charMaster = charMasterArg;
	}

	static SpawnedCharacterDeathEvent e;
	public static void Trigger(CharacterMasterController charMasterArg)
	{
		e.charMaster = charMasterArg;

		MMEventManager.TriggerEvent(e);
	}
}

