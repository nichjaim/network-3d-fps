using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event when a level arena is exited.
/// </summary>
public struct LevelArenaExitEvent
{
	static LevelArenaExitEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}

/*/// <summary>
/// An event when a level arena is exited.
/// </summary>
public struct LevelArenaExitEvent
{
	public int setOfExitedArena;
	public bool wasLastArenaOfSet;
	public bool wasLastSetOfSets;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="setOfExitedArenaArg"></param>
	/// <param name="wasLastArenaOfSetArg"></param>
	/// <param name="wasLastSetOfSetsArg"></param>
	public LevelArenaExitEvent(int setOfExitedArenaArg, bool wasLastArenaOfSetArg, 
		bool wasLastSetOfSetsArg)
	{
		setOfExitedArena = setOfExitedArenaArg;
		wasLastArenaOfSet = wasLastArenaOfSetArg;
		wasLastSetOfSets = wasLastSetOfSetsArg;
	}

	static LevelArenaExitEvent e;
	public static void Trigger(int setOfExitedArenaArg, bool wasLastArenaOfSetArg,
		bool wasLastSetOfSetsArg)
	{
		e.setOfExitedArena = setOfExitedArenaArg;
		e.wasLastArenaOfSet = wasLastArenaOfSetArg;
		e.wasLastSetOfSets = wasLastSetOfSetsArg;

		MMEventManager.TriggerEvent(e);
	}
}*/

/// <summary>
/// An event when arena progression has advanced to the next.
/// </summary>
public struct ArenaProgressionAdvancedEvent
{
	public LevelArenaCoordinator arenaCoordr;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="arenaCoordrArg"></param>
	public ArenaProgressionAdvancedEvent(LevelArenaCoordinator arenaCoordrArg)
	{
		arenaCoordr = arenaCoordrArg;
	}

	static ArenaProgressionAdvancedEvent e;
	public static void Trigger(LevelArenaCoordinator arenaCoordrArg)
	{
		e.arenaCoordr = arenaCoordrArg;

		MMEventManager.TriggerEvent(e);
	}
}
