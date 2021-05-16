using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

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
/// An event fired when a menu is opened/closed.
/// </summary>
public struct MenuActivationEvent
{
	public MasterMenuControllerCustom menu;
	public bool isOpening;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="menuArg"></param>
	/// <param name="isOpeningArg"></param>
	public MenuActivationEvent(MasterMenuControllerCustom menuArg, bool isOpeningArg)
	{
		menu = menuArg;
		isOpening = isOpeningArg;
	}

	static MenuActivationEvent e;
	public static void Trigger(MasterMenuControllerCustom menuArg, bool isOpeningArg)
	{
		e.menu = menuArg;
		e.isOpening = isOpeningArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when the player char associated with the UI has been changed.
/// </summary>
public struct UiPlayerCharChangedEvent
{
	public PlayerCharacterMasterController playerChar;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="playerCharArg"></param>
	public UiPlayerCharChangedEvent(PlayerCharacterMasterController playerCharArg)
	{
		playerChar = playerCharArg;
	}

	static UiPlayerCharChangedEvent e;
	public static void Trigger(PlayerCharacterMasterController playerCharArg)
	{
		e.playerChar = playerCharArg;

		MMEventManager.TriggerEvent(e);
	}
}

/// <summary>
/// An event fired when a dialogue UI is opened/closed.
/// </summary>
public struct DialogueActivationEvent
{
	public StandardDialogueUI dialogue;
	public bool isOpening;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="dialogueArg"></param>
	/// <param name="isOpeningArg"></param>
	public DialogueActivationEvent(StandardDialogueUI dialogueArg, bool isOpeningArg)
	{
		dialogue = dialogueArg;
		isOpening = isOpeningArg;
	}

	static DialogueActivationEvent e;
	public static void Trigger(StandardDialogueUI dialogueArg, bool isOpeningArg)
	{
		e.dialogue = dialogueArg;
		e.isOpening = isOpeningArg;

		MMEventManager.TriggerEvent(e);
	}
}
