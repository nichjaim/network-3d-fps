using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PixelCrushers.DialogueSystem;

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

/*/// <summary>
/// An event fired when entering the haven game state.
/// </summary>
public struct StartHavenGameStateEvent
{
	static StartHavenGameStateEvent e;
	public static void Trigger()
	{
		MMEventManager.TriggerEvent(e);
	}
}*/

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
/// An event fired for the various stages of world level generation.
/// </summary>
public struct GenerateWorldLevelEvent
{
	public ActionProgressType generateProgress;
	public WorldChunkController[][] generatedWorld;

	/// <summary>
	/// Initializes a new instance of the struct.
	/// </summary>
	/// <param name="generateProgressArg"></param>
	/// <param name="generatedWorldArg"></param>
	public GenerateWorldLevelEvent(ActionProgressType generateProgressArg,
		WorldChunkController[][] generatedWorldArg)
	{
		generateProgress = generateProgressArg;
		generatedWorld = generatedWorldArg;
	}

	static GenerateWorldLevelEvent e;
	public static void Trigger(ActionProgressType generateProgressArg,
		WorldChunkController[][] generatedWorldArg)
	{
		e.generateProgress = generateProgressArg;
		e.generatedWorld = generatedWorldArg;

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

