/* [REMOVE THIS LINE]
 * [REMOVE THIS LINE] To use this template, make a copy and remove the lines that start
 * [REMOVE THIS LINE] with "[REMOVE THIS LINE]". Then add your code where the comments indicate.
 * [REMOVE THIS LINE]
 * [REMOVE THIS LINE] If your code references scripts or assets that are outside of the Plugins
 * [REMOVE THIS LINE] folder, move this script outside of the Plugins folder, too.
 * [REMOVE THIS LINE]



using UnityEngine;
using PixelCrushers.DialogueSystem;

// Rename this class to the same name that you used for the script file.
// Add the script to your Dialogue Manager. You can optionally make this 
// a static class and remove the inheritance from MonoBehaviour, in which
// case you won't add it to the Dialogue Manager.
//
// This class registers two example functions: 
//
// - DebugLog(string) writes a string to the Console using Unity's Debug.Log().
// - AddOne(double) returns the value plus one.
//
// You can use these functions as models and then replace them with your own.
public class TemplateCustomLua : MonoBehaviour // Rename this class.
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction("DebugLog", this, SymbolExtensions.GetMethodInfo(() => DebugLog(string.Empty)));
        Lua.RegisterFunction("AddOne", this, SymbolExtensions.GetMethodInfo(() => AddOne((double)0)));
    }

    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction("DebugLog");
            Lua.UnregisterFunction("AddOne");
        }
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    public double AddOne(double value)
    { // Note: Lua always passes numbers as doubles.
        return value + 1;
    }
}



/**/



using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDialogueLuaCoordinator : MonoBehaviour
{
    #region Class Variables

    private GameManager _gameManager = null;
    private UIManager _uiManager = null;

    private string CHAR_ID_MC = "3";
    private string CHAR_ID_WAIFU_1 = "4";
    private string CHAR_ID_WAIFU_2 = "5";
    private string CHAR_ID_WAIFU_3 = "6";

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup vars that hold reference to singletons
        InitializeSingletonReferences();
    }

    private void OnEnable()
    {
        // registers all custom Lua functions
        RegisterLuaFunctions();
    }

    private void OnDisable()
    {
        // un-registers all custom Lua functions
        UnregisterLuaFunctions();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up vars that hold reference to singletons. 
    /// Call in Start(), to give time for singletons to instantiate.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;
    }

    #endregion




    #region Lua Registeration Functions

    /// <summary>
    /// Registers all custom Lua functions. 
    /// Call in OnEnable().
    /// </summary>
    private void RegisterLuaFunctions()
    {
        // ===CONDITION FUNCTIONS===

        // general:
        Lua.RegisterFunction("HaveFlag", this, SymbolExtensions.GetMethodInfo(() => HaveFlag(string.Empty)));

        Lua.RegisterFunction("CanStartShooterStateMode", this, SymbolExtensions.GetMethodInfo(() => CanStartShooterStateMode()));

        // HContent:
        Lua.RegisterFunction("CanShowHcontent", this, SymbolExtensions.GetMethodInfo(() => CanShowHcontent()));

        // activity partner:
        Lua.RegisterFunction("CanActivityPartnerWaifu1", this, SymbolExtensions.GetMethodInfo(() => CanActivityPartnerWaifu1()));
        Lua.RegisterFunction("CanActivityPartnerWaifu2", this, SymbolExtensions.GetMethodInfo(() => CanActivityPartnerWaifu2()));
        Lua.RegisterFunction("CanActivityPartnerWaifu3", this, SymbolExtensions.GetMethodInfo(() => CanActivityPartnerWaifu3()));

        Lua.RegisterFunction("IsActivityPartnerWaifu1", this, SymbolExtensions.GetMethodInfo(() => IsActivityPartnerWaifu1()));
        Lua.RegisterFunction("IsActivityPartnerWaifu2", this, SymbolExtensions.GetMethodInfo(() => IsActivityPartnerWaifu2()));
        Lua.RegisterFunction("IsActivityPartnerWaifu3", this, SymbolExtensions.GetMethodInfo(() => IsActivityPartnerWaifu3()));

        // activity:
        Lua.RegisterFunction("CanDoActivity", this, SymbolExtensions.GetMethodInfo(() => CanDoActivity(string.Empty)));

        // sexual:
        Lua.RegisterFunction("IsRomancedWaifu1", this, SymbolExtensions.GetMethodInfo(() => IsRomancedWaifu1()));
        Lua.RegisterFunction("IsRomancedWaifu2", this, SymbolExtensions.GetMethodInfo(() => IsRomancedWaifu2()));
        Lua.RegisterFunction("IsRomancedWaifu3", this, SymbolExtensions.GetMethodInfo(() => IsRomancedWaifu3()));

        Lua.RegisterFunction("IsSexuallyComfortableWaifu1", this, SymbolExtensions.GetMethodInfo(() => IsSexuallyComfortableWaifu1()));
        Lua.RegisterFunction("IsSexuallyComfortableWaifu2", this, SymbolExtensions.GetMethodInfo(() => IsSexuallyComfortableWaifu2()));
        Lua.RegisterFunction("IsSexuallyComfortableWaifu3", this, SymbolExtensions.GetMethodInfo(() => IsSexuallyComfortableWaifu3()));



        // ===SCRIPT FUNCTIONS===

        // general:
        Lua.RegisterFunction("SetFlag", this, SymbolExtensions.GetMethodInfo(() => SetFlag(string.Empty)));
        Lua.RegisterFunction("AddExp", this, SymbolExtensions.GetMethodInfo(() => AddExp((double)0)));

        Lua.RegisterFunction("StartShooterStateMode", this, SymbolExtensions.GetMethodInfo(() => StartShooterStateMode()));
        Lua.RegisterFunction("StartVisualNovelStateMode", this, SymbolExtensions.GetMethodInfo(() => StartVisualNovelStateMode()));

        Lua.RegisterFunction("TutorialDone", this, SymbolExtensions.GetMethodInfo(() => TutorialDone()));

        // HContent:
        Lua.RegisterFunction("DialogueUIChangeHBackground", this, SymbolExtensions.GetMethodInfo(() => DialogueUIChangeHBackground(string.Empty)));

        // activity partner:
        Lua.RegisterFunction("SetActivityPartnerWaifu1", this, SymbolExtensions.GetMethodInfo(() => SetActivityPartnerWaifu1()));
        Lua.RegisterFunction("SetActivityPartnerWaifu2", this, SymbolExtensions.GetMethodInfo(() => SetActivityPartnerWaifu2()));
        Lua.RegisterFunction("SetActivityPartnerWaifu3", this, SymbolExtensions.GetMethodInfo(() => SetActivityPartnerWaifu3()));

        // activity:
        Lua.RegisterFunction("SetActivity", this, SymbolExtensions.GetMethodInfo(() => SetActivity(string.Empty)));
        Lua.RegisterFunction("SetActivityStart", this, SymbolExtensions.GetMethodInfo(() => SetActivityStart()));

        // character:
        Lua.RegisterFunction("PartyAddWaifu1", this, SymbolExtensions.GetMethodInfo(() => AddCharacterToPartyWaifu1()));
        Lua.RegisterFunction("PartyAddWaifu2", this, SymbolExtensions.GetMethodInfo(() => AddCharacterToPartyWaifu2()));
        Lua.RegisterFunction("PartyAddWaifu3", this, SymbolExtensions.GetMethodInfo(() => AddCharacterToPartyWaifu3()));

        Lua.RegisterFunction("AddLeechShotAbilityMC", this, SymbolExtensions.GetMethodInfo(() => AddLeechShotAbilityToCharacterMC()));
        Lua.RegisterFunction("AddShieldAbilityWaifu1", this, SymbolExtensions.GetMethodInfo(() => AddShieldAbilityToCharacterWaifu1()));
        Lua.RegisterFunction("AddTrapAbilityWaifu2", this, SymbolExtensions.GetMethodInfo(() => AddTrapAbilityToCharacterWaifu2()));
        Lua.RegisterFunction("AddWeakenAbilityWaifu3", this, SymbolExtensions.GetMethodInfo(() => AddWeakenAbilityToCharacterWaifu3()));

        // dialogue UI:
        Lua.RegisterFunction("DialogueUIChangeBackground", this, SymbolExtensions.GetMethodInfo(() => DialogueUIChangeBackground(string.Empty)));

        Lua.RegisterFunction("DialogueUIPortraitEnterWaifu1", this, SymbolExtensions.GetMethodInfo(() => DialogueUIPortraitEnterWaifu1(string.Empty)));
        Lua.RegisterFunction("DialogueUIPortraitEnterWaifu2", this, SymbolExtensions.GetMethodInfo(() => DialogueUIPortraitEnterWaifu2(string.Empty)));
        Lua.RegisterFunction("DialogueUIPortraitEnterWaifu3", this, SymbolExtensions.GetMethodInfo(() => DialogueUIPortraitEnterWaifu3(string.Empty)));

        Lua.RegisterFunction("DialogueUIPortraitExitWaifu1", this, SymbolExtensions.GetMethodInfo(() => DialogueUIPortraitExitWaifu1()));
        Lua.RegisterFunction("DialogueUIPortraitExitWaifu2", this, SymbolExtensions.GetMethodInfo(() => DialogueUIPortraitExitWaifu2()));
        Lua.RegisterFunction("DialogueUIPortraitExitWaifu3", this, SymbolExtensions.GetMethodInfo(() => DialogueUIPortraitExitWaifu3()));

        Lua.RegisterFunction("DialogueUIChangeExpressionWaifu1", this, SymbolExtensions.GetMethodInfo(() => DialogueUIChangeExpressionWaifu1(string.Empty)));
        Lua.RegisterFunction("DialogueUIChangeExpressionWaifu2", this, SymbolExtensions.GetMethodInfo(() => DialogueUIChangeExpressionWaifu2(string.Empty)));
        Lua.RegisterFunction("DialogueUIChangeExpressionWaifu3", this, SymbolExtensions.GetMethodInfo(() => DialogueUIChangeExpressionWaifu3(string.Empty)));

        Lua.RegisterFunction("DialogueUIAssignSpeakerMC", this, SymbolExtensions.GetMethodInfo(() => DialogueUIAssignSpeakerMC()));
        Lua.RegisterFunction("DialogueUIAssignSpeakerWaifu1", this, SymbolExtensions.GetMethodInfo(() => DialogueUIAssignSpeakerWaifu1()));
        Lua.RegisterFunction("DialogueUIAssignSpeakerWaifu2", this, SymbolExtensions.GetMethodInfo(() => DialogueUIAssignSpeakerWaifu2()));
        Lua.RegisterFunction("DialogueUIAssignSpeakerWaifu3", this, SymbolExtensions.GetMethodInfo(() => DialogueUIAssignSpeakerWaifu3()));

        Lua.RegisterFunction("DialogueUIRemoveSpeaker", this, SymbolExtensions.GetMethodInfo(() => DialogueUIRemoveSpeaker()));



        // ===DIALOGUE TAG FUNCTIONS===

        Lua.RegisterFunction("LevelingInfo", this, SymbolExtensions.GetMethodInfo(() => LevelingInfo()));

        Lua.RegisterFunction("CharNameActPartner", this, SymbolExtensions.GetMethodInfo(() => CharNameActPartner()));
        Lua.RegisterFunction("CharNameWaifu1", this, SymbolExtensions.GetMethodInfo(() => CharNameWaifu1()));
        Lua.RegisterFunction("CharNameWaifu2", this, SymbolExtensions.GetMethodInfo(() => CharNameWaifu2()));
        Lua.RegisterFunction("CharNameWaifu3", this, SymbolExtensions.GetMethodInfo(() => CharNameWaifu3()));
    }

    /// <summary>
    /// Un-registers all custom Lua functions. 
    /// Call in OnDisable().
    /// </summary>
    private void UnregisterLuaFunctions()
    {
        // ===CONDITION FUNCTIONS===

        // general:
        Lua.UnregisterFunction("HaveFlag");

        Lua.UnregisterFunction("CanStartShooterStateMode");

        // HContent:
        Lua.UnregisterFunction("CanShowHcontent");

        // activity partner:
        Lua.UnregisterFunction("CanActivityPartnerWaifu1");
        Lua.UnregisterFunction("CanActivityPartnerWaifu2");
        Lua.UnregisterFunction("CanActivityPartnerWaifu3");

        Lua.UnregisterFunction("IsActivityPartnerWaifu1");
        Lua.UnregisterFunction("IsActivityPartnerWaifu2");
        Lua.UnregisterFunction("IsActivityPartnerWaifu3");

        // activity:
        Lua.UnregisterFunction("CanDoActivity");

        // sexual:
        Lua.UnregisterFunction("CanShowHcontent");

        Lua.UnregisterFunction("IsRomancedWaifu1");
        Lua.UnregisterFunction("IsRomancedWaifu2");
        Lua.UnregisterFunction("IsRomancedWaifu3");

        Lua.UnregisterFunction("IsSexuallyComfortableWaifu1");
        Lua.UnregisterFunction("IsSexuallyComfortableWaifu2");
        Lua.UnregisterFunction("IsSexuallyComfortableWaifu3");



        // ===SCRIPT FUNCTIONS===

        // general:
        Lua.UnregisterFunction("SetFlag");
        Lua.UnregisterFunction("AddExp");

        Lua.UnregisterFunction("StartShooterStateMode");
        Lua.UnregisterFunction("StartVisualNovelStateMode");

        Lua.UnregisterFunction("TutorialDone");

        // HContent:
        Lua.UnregisterFunction("DialogueUIChangeHBackground");

        // activity partner:
        Lua.UnregisterFunction("SetActivityPartnerWaifu1");
        Lua.UnregisterFunction("SetActivityPartnerWaifu2");
        Lua.UnregisterFunction("SetActivityPartnerWaifu3");

        // activity:
        Lua.UnregisterFunction("SetActivity");
        Lua.UnregisterFunction("StartActivity");

        // character:
        Lua.UnregisterFunction("PartyAddWaifu1");
        Lua.UnregisterFunction("PartyAddWaifu2");
        Lua.UnregisterFunction("PartyAddWaifu3");

        Lua.UnregisterFunction("AddLeechShotAbilityMC");
        Lua.UnregisterFunction("AddShieldAbilityWaifu1");
        Lua.UnregisterFunction("AddTrapAbilityWaifu2");
        Lua.UnregisterFunction("AddWeakenAbilityWaifu3");

        // dialogue UI:
        Lua.UnregisterFunction("DialogueUIChangeBackground");

        Lua.UnregisterFunction("DialogueUIPortraitEnterWaifu1");
        Lua.UnregisterFunction("DialogueUIPortraitEnterWaifu2");
        Lua.UnregisterFunction("DialogueUIPortraitEnterWaifu3");

        Lua.UnregisterFunction("DialogueUIPortraitExitWaifu1");
        Lua.UnregisterFunction("DialogueUIPortraitExitWaifu2");
        Lua.UnregisterFunction("DialogueUIPortraitExitWaifu3");

        Lua.UnregisterFunction("DialogueUIChangeExpressionWaifu1");
        Lua.UnregisterFunction("DialogueUIChangeExpressionWaifu2");
        Lua.UnregisterFunction("DialogueUIChangeExpressionWaifu3");

        Lua.UnregisterFunction("DialogueUIAssignSpeakerMC");
        Lua.UnregisterFunction("DialogueUIAssignSpeakerWaifu1");
        Lua.UnregisterFunction("DialogueUIAssignSpeakerWaifu2");
        Lua.UnregisterFunction("DialogueUIAssignSpeakerWaifu3");

        Lua.UnregisterFunction("DialogueUIRemoveSpeaker");



        // ===DIALOGUE TAG FUNCTIONS===

        Lua.UnregisterFunction("LevelingInfo");

        Lua.UnregisterFunction("CharNameActPartner");
        Lua.UnregisterFunction("CharNameWaifu1");
        Lua.UnregisterFunction("CharNameWaifu2");
        Lua.UnregisterFunction("CharNameWaifu3");
    }

    #endregion




    #region Lua General Action Functions

    private bool HaveFlag(string flagArg)
    {
        return _gameManager.GameFlags.IsFlagSet(flagArg);
    }

    private void SetFlag(string flagArg)
    {
        _gameManager.GameFlags.SetFlag(flagArg);
    }

    private void AddExp(double expArg)
    {
        Debug.Log("NEED IMPL: AddExp()"); // NEED IMPL
    }

    private bool CanStartShooterStateMode()
    {
        return _gameManager.HavenData.IsTodayShooterSectionDay();
    }

    private void StartShooterStateMode()
    {
        // trigger event to start shooter state mode
        GameStateModeTransitionEvent.Trigger(GameStateMode.Shooter, ActionProgressType.Started);
    }

    private void StartVisualNovelStateMode()
    {
        // trigger event to start VN state mode
        GameStateModeTransitionEvent.Trigger(GameStateMode.VisualNovel, ActionProgressType.Started);
    }

    private void TutorialDone()
    {
        _uiManager.DialgGameCoordr.CompleteTutorial();
    }

    #endregion




    #region Lua H-Content Functions

    private bool CanShowHcontent()
    {
        Debug.LogWarning("NEED IMPL: implement second half of CanShowHcontent()"); // NEED IMPL
        // if H-content is installed AND is enabled to be shown
        return (AssetRefMethods.IsHcontentInstalled()) && (false);
    }

    private void DialogueUIChangeHBackground(string bgIdArg)
    {
        _uiManager.DialgUICoordr.ChangeBackground(bgIdArg, true);
    }

    #endregion




    #region Lua Activity Partner Functions

    /// <summary>
    /// Returns whether the given char is still available to partner up for activites.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private bool CanActivityPartner(string charIdArg)
    {
        return _gameManager.HavenData.activityPlanning.
            IsActivityPartnerAvailable(charIdArg);
    }

    private bool CanActivityPartnerWaifu1()
    {
        return CanActivityPartner(CHAR_ID_WAIFU_1);
    }

    private bool CanActivityPartnerWaifu2()
    {
        return CanActivityPartner(CHAR_ID_WAIFU_2);
    }

    private bool CanActivityPartnerWaifu3()
    {
        return CanActivityPartner(CHAR_ID_WAIFU_3);
    }

    /// <summary>
    /// Sets the current character who will be joining in the activity.
    /// </summary>
    /// <param name="charIdArg"></param>
    private void SetActivityPartner(string charIdArg)
    {
        _uiManager.DialgGameCoordr.CurrentActivityPartnerCharId = charIdArg;
    }

    private void SetActivityPartnerWaifu1()
    {
        SetActivityPartner(CHAR_ID_WAIFU_1);
    }

    private void SetActivityPartnerWaifu2()
    {
        SetActivityPartner(CHAR_ID_WAIFU_2);
    }

    private void SetActivityPartnerWaifu3()
    {
        SetActivityPartner(CHAR_ID_WAIFU_3);
    }

    /// <summary>
    /// Return whether the current activity partner is the given char.
    /// </summary>
    /// <param name="charIdArg"></param>
    private bool IsActivityPartner(string charIdArg)
    {
        return _uiManager.DialgGameCoordr.IsActivityPartner(charIdArg);
    }

    private bool IsActivityPartnerWaifu1()
    {
        return IsActivityPartner(CHAR_ID_WAIFU_1);
    }

    private bool IsActivityPartnerWaifu2()
    {
        return IsActivityPartner(CHAR_ID_WAIFU_2);
    }

    private bool IsActivityPartnerWaifu3()
    {
        return IsActivityPartner(CHAR_ID_WAIFU_3);
    }

    #endregion




    #region Lua Activity Functions

    /// <summary>
    /// Returns whether the given activity is available for the current day.
    /// </summary>
    /// <param name="activityIdArg"></param>
    /// <returns></returns>
    private bool CanDoActivity(string activityIdArg)
    {
        return _gameManager.HavenData.activityPlanning.
            IsActivityAvailable(activityIdArg);
    }

    /// <summary>
    /// Sets the activity that the player is currently targeted to undergo.
    /// </summary>
    /// <param name="activityIdArg"></param>
    private void SetActivity(string activityIdArg)
    {
        _uiManager.DialgGameCoordr.CurrentActivityId = activityIdArg;
    }

    /// <summary>
    /// Sets the activity convo that is currently being targeted to start at end of current dialogue.
    /// </summary>
    private void SetActivityStart()
    {
        _uiManager.DialgGameCoordr.SetTargetedActivityDialogueStart();
    }

    #endregion




    #region Sexual Functions

    private bool IsRomanced(string charIdArg)
    {
        return _gameManager.GameFlags.IsRomanceFlagSet(charIdArg);
    }

    private bool IsRomancedWaifu1()
    {
        return IsRomanced(CHAR_ID_WAIFU_1);
    }

    private bool IsRomancedWaifu2()
    {
        return IsRomanced(CHAR_ID_WAIFU_2);
    }

    private bool IsRomancedWaifu3()
    {
        return IsRomanced(CHAR_ID_WAIFU_3);
    }

    private bool IsSexuallyComfortable(string charIdArg)
    {
        return _gameManager.GameFlags.IsSexuallyComfortableFlagSet(charIdArg);
    }

    private bool IsSexuallyComfortableWaifu1()
    {
        return IsSexuallyComfortable(CHAR_ID_WAIFU_1);
    }

    private bool IsSexuallyComfortableWaifu2()
    {
        return IsSexuallyComfortable(CHAR_ID_WAIFU_2);
    }

    private bool IsSexuallyComfortableWaifu3()
    {
        return IsSexuallyComfortable(CHAR_ID_WAIFU_3);
    }

    #endregion




    #region Character Party Functions

    private void AddCharacterToParty(string charIdArg)
    {
        _gameManager.AddCharacterToParty(charIdArg);
    }

    private void AddCharacterToPartyWaifu1()
    {
        // add weapon slot for MC so that new party member can be used
        _gameManager.AddNewAppropriateWeaponSlotToMainCharacterInventory();

        _gameManager.AddCharacterToParty(CHAR_ID_WAIFU_1);

        /// add waifu 1's starting weapon to MC's ivnentory, so that player can 
        /// use it when switching to this waifu
        _gameManager.AddWeaponToMainCharacterInventory("2");
    }

    private void AddCharacterToPartyWaifu2()
    {
        // add weapon slot for MC so that new party member can be used
        _gameManager.AddNewAppropriateWeaponSlotToMainCharacterInventory();

        _gameManager.AddCharacterToParty(CHAR_ID_WAIFU_2);

        /// add waifu 2's starting weapon to MC's ivnentory, so that player can 
        /// use it when switching to this waifu
        _gameManager.AddWeaponToMainCharacterInventory("3");
    }

    private void AddCharacterToPartyWaifu3()
    {
        // add weapon slot for MC so that new party member can be used
        _gameManager.AddNewAppropriateWeaponSlotToMainCharacterInventory();

        _gameManager.AddCharacterToParty(CHAR_ID_WAIFU_3);

        /// add waifu 3's starting weapon to MC's ivnentory, so that player can 
        /// use it when switching to this waifu
        _gameManager.AddWeaponToMainCharacterInventory("4");
    }

    #endregion




    #region Character Ability Functions

    private void AddAbilityToCharacter(string charIdArg, string abilityIdArg)
    {
        _gameManager.AddAbilityToCharacter(charIdArg, abilityIdArg);
    }

    private void AddAbilityToCharacterMC(string abilityIdArg)
    {
        AddAbilityToCharacter(CHAR_ID_MC, abilityIdArg);
    }

    private void AddAbilityToCharacterWaifu1(string abilityIdArg)
    {
        AddAbilityToCharacter(CHAR_ID_WAIFU_1, abilityIdArg);
    }

    private void AddAbilityToCharacterWaifu2(string abilityIdArg)
    {
        AddAbilityToCharacter(CHAR_ID_WAIFU_2, abilityIdArg);
    }

    private void AddAbilityToCharacterWaifu3(string abilityIdArg)
    {
        AddAbilityToCharacter(CHAR_ID_WAIFU_3, abilityIdArg);
    }

    /*private void AddHealAbilityToCharacterMC()
    {
        AddAbilityToCharacterMC("1");
    }*/

    private void AddLeechShotAbilityToCharacterMC()
    {
        AddAbilityToCharacterMC("5");
    }

    private void AddShieldAbilityToCharacterWaifu1()
    {
        AddAbilityToCharacterWaifu1("2");
    }

    private void AddTrapAbilityToCharacterWaifu2()
    {
        AddAbilityToCharacterWaifu2("3");
    }

    private void AddWeakenAbilityToCharacterWaifu3()
    {
        AddAbilityToCharacterWaifu3("4");
    }

    #endregion




    #region Dialogue UI Functions

    private void DialogueUIChangeBackground(string bgIdArg)
    {
        _uiManager.DialgUICoordr.ChangeBackground(bgIdArg, false);
    }

    private void DialogueUIPortraitEnter(string charIdArg, string exprAbrvArg)
    {
        // get dialogue expression from string abbraviation
        CharacterDialogueExpressionType expression = GeneralMethods.
            GetExpressionFromStringAbbreviation(exprAbrvArg);

        _uiManager.DialgUICoordr.CharacterPortraitEnterDialogueScene(charIdArg, expression);
    }

    private void DialogueUIPortraitEnterWaifu1(string exprAbrvArg)
    {
        DialogueUIPortraitEnter(CHAR_ID_WAIFU_1, exprAbrvArg);
    }

    private void DialogueUIPortraitEnterWaifu2(string exprAbrvArg)
    {
        DialogueUIPortraitEnter(CHAR_ID_WAIFU_2, exprAbrvArg);
    }

    private void DialogueUIPortraitEnterWaifu3(string exprAbrvArg)
    {
        DialogueUIPortraitEnter(CHAR_ID_WAIFU_3, exprAbrvArg);
    }

    private void DialogueUIPortraitExit(string charIdArg)
    {
        _uiManager.DialgUICoordr.CharacterPortraitExitDialogueScene(charIdArg);
    }

    private void DialogueUIPortraitExitWaifu1()
    {
        DialogueUIPortraitExit(CHAR_ID_WAIFU_1);
    }

    private void DialogueUIPortraitExitWaifu2()
    {
        DialogueUIPortraitExit(CHAR_ID_WAIFU_2);
    }

    private void DialogueUIPortraitExitWaifu3()
    {
        DialogueUIPortraitExit(CHAR_ID_WAIFU_3);
    }

    private void DialogueUIChangeExpression(string charIdArg, string exprAbrvArg)
    {
        // get dialogue expression from string abbraviation
        CharacterDialogueExpressionType expression = GeneralMethods.
            GetExpressionFromStringAbbreviation(exprAbrvArg);

        _uiManager.DialgUICoordr.SetCharacterPortraitExpression(charIdArg, expression);
    }

    private void DialogueUIChangeExpressionWaifu1(string exprAbrvArg)
    {
        DialogueUIChangeExpression(CHAR_ID_WAIFU_1, exprAbrvArg);
    }

    private void DialogueUIChangeExpressionWaifu2(string exprAbrvArg)
    {
        DialogueUIChangeExpression(CHAR_ID_WAIFU_2, exprAbrvArg);
    }

    private void DialogueUIChangeExpressionWaifu3(string exprAbrvArg)
    {
        DialogueUIChangeExpression(CHAR_ID_WAIFU_3, exprAbrvArg);
    }

    private void DialogueUIAssignSpeaker(string charIdArg)
    {
        _uiManager.DialgUICoordr.AssignSpeaker(charIdArg);
    }

    private void DialogueUIAssignSpeakerMC()
    {
        DialogueUIAssignSpeaker(CHAR_ID_MC);
    }

    private void DialogueUIAssignSpeakerWaifu1()
    {
        DialogueUIAssignSpeaker(CHAR_ID_WAIFU_1);
    }

    private void DialogueUIAssignSpeakerWaifu2()
    {
        DialogueUIAssignSpeaker(CHAR_ID_WAIFU_2);
    }

    private void DialogueUIAssignSpeakerWaifu3()
    {
        DialogueUIAssignSpeaker(CHAR_ID_WAIFU_3);
    }

    private void DialogueUIRemoveSpeaker()
    {
        _uiManager.DialgUICoordr.RemoveSpeaker();
    }

    #endregion




    #region Dialogue Tag Functions

    /// <summary>
    /// Returns the dialogue that displays the current level-up info.
    /// </summary>
    /// <returns></returns>
    private string LevelingInfo() //GetCurrentLevelUpInfoDialogue()
    {
        // get current character and level up info
        (string, CharacterProgressionInfoSet) charIdToProgInfo = 
            _uiManager.DialgGameCoordr.GetCurrentCharToLevelUpInfo();

        // initialize return dialogue with character's name and some preamble
        string dialogueText = GetCharacterName(charIdToProgInfo.Item1) + "has gained: ";

        // get list of the progress type and values for the upcoming loop
        List<SerializableDataCharacterProgressionTypeAndFloat> progDataToValue = 
            charIdToProgInfo.Item2.progressionTypeAndProgressionAmount;

        // loop through all power increase entries
        for (int i = 0; i < progDataToValue.Count; i++)
        {
            // if this is NOT the first iteration
            if (i > 0)
            {
                // add some text to denote a new entry in this list
                dialogueText += ", ";
            }

            // add iterating power increase type's actual dialogue text
            dialogueText += GetProgressTypeDialogueText(progDataToValue[i].typeValue);
            // add iterating power increase value to dialogue text
            dialogueText += (" " + "+" + progDataToValue[i].floatValue);
        }

        // add ending punctuation to dialogue text
        dialogueText += ".";

        // return the setup dialogue text
        return dialogueText;
    }

    /// <summary>
    /// Returns the name of the character asscoaited with the given ID.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private string GetCharacterName(string charIdArg)
    {
        // load character template from given ID
        CharacterDataTemplate charTemp = AssetRefMethods.
            LoadBundleAssetCharacterDataTemplate(charIdArg);

        // if a character was found
        if (charTemp != null)
        {
            // return loaded char's name
            return charTemp.template.characterInfo.characterName;
        }
        // else NO character template could be found
        else
        {
            // print warning to log
            Debug.LogWarning("Problem in GetCharacterName(). No character " +
                $"associated with ID: {charIdArg}");

            // return some default value
            return string.Empty;
        }
    }

    /// <summary>
    /// Returns dialogue text associated with the given progress type.
    /// </summary>
    /// <param name="typeArg"></param>
    /// <returns></returns>
    private string GetProgressTypeDialogueText(CharacterProgressionType typeArg)
    {
        switch (typeArg)
        {
            case CharacterProgressionType.Stamina:
                return "Health";

            case CharacterProgressionType.Strength:
                return "Weapon Damage";

            case CharacterProgressionType.Dexterity:
                return "Health";

            case CharacterProgressionType.AbilityProficiency:
                return "Ability Proficiency";

            default:
                // print warning to console
                Debug.LogWarning("Problem in GetProgressTypeDialogueText(), No case " +
                    $"for enum: {typeArg.ToString()}");

                // return some default value
                return string.Empty;
        }
    }

    private string CharNameActPartner()
    {
        return GetCharacterName(
            _uiManager.DialgGameCoordr.CurrentActivityPartnerCharId);
    }

    private string CharNameWaifu1()
    {
        return GetCharacterName(CHAR_ID_WAIFU_1);
    }

    private string CharNameWaifu2()
    {
        return GetCharacterName(CHAR_ID_WAIFU_2);
    }

    private string CharNameWaifu3()
    {
        return GetCharacterName(CHAR_ID_WAIFU_3);
    }

    #endregion


}
