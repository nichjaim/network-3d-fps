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
        Lua.RegisterFunction("CanShowHcontent", this, SymbolExtensions.GetMethodInfo(() => CanShowHcontent()));

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

        // activity partner:
        Lua.RegisterFunction("SetActivityPartnerWaifu1", this, SymbolExtensions.GetMethodInfo(() => SetActivityPartnerWaifu1()));
        Lua.RegisterFunction("SetActivityPartnerWaifu2", this, SymbolExtensions.GetMethodInfo(() => SetActivityPartnerWaifu2()));
        Lua.RegisterFunction("SetActivityPartnerWaifu3", this, SymbolExtensions.GetMethodInfo(() => SetActivityPartnerWaifu3()));

        // activity:
        Lua.RegisterFunction("SetActivity", this, SymbolExtensions.GetMethodInfo(() => SetActivity(string.Empty)));
        Lua.RegisterFunction("StartActivity", this, SymbolExtensions.GetMethodInfo(() => StartActivity()));
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

        // activity:
        Lua.UnregisterFunction("CanActivityPartnerWaifu1");
        Lua.UnregisterFunction("CanActivityPartnerWaifu2");
        Lua.UnregisterFunction("CanActivityPartnerWaifu3");



        // ===SCRIPT FUNCTIONS===

        // general:
        Lua.UnregisterFunction("SetFlag");
        Lua.UnregisterFunction("AddExp");
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
    /// Start the activity convo that is currently being targeted.
    /// </summary>
    private void StartActivity()
    {
        _uiManager.DialgGameCoordr.StartTargetedActivityDialogue();
    }

    #endregion




    #region Sexual Functions

    private bool CanShowHcontent()
    {
        Debug.LogWarning("NEED IMPL: implement second half of CanShowHcontent()"); // NEED IMPL
        // if H-content is installed AND is enabled to be shown
        return (AssetRefMethods.IsHcontentInstalled()) && (false);
    }

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


}
