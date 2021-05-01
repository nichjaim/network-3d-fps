using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The story/event markers that denote certain actions or progression.
/// </summary>
[System.Serializable]
public class GameFlags
{
    #region Class Variables

    public List<string> setFlags;

    #endregion




    #region Constructors

    public GameFlags()
    {
        Setup();
    }

    public GameFlags(GameFlags templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        setFlags = new List<string>();
    }

    private void Setup(GameFlags templateArg)
    {
        setFlags = new List<string>();
        foreach (string iterFlag in templateArg.setFlags)
        {
            setFlags.Add(iterFlag);
        }
    }

    #endregion




    #region Core Flag Functions

    /// <summary>
    /// Returns whether the given flag is set ot not.
    /// </summary>
    /// <param name="flagArg"></param>
    /// <returns></returns>
    public bool IsFlagSet(string flagArg)
    {
        // get flag in lower-case
        string flagLower = flagArg.ToLower();

        return setFlags.Contains(flagLower);
    }

    /// <summary>
    /// Sets the given flag if not already.
    /// </summary>
    /// <param name="flagArg"></param>
    public void SetFlag(string flagArg)
    {
        // get flag in lower-case
        string flagLower = flagArg.ToLower();

        // if flag NOT currently set
        if (!setFlags.Contains(flagLower))
        {
            // set the flag
            setFlags.Add(flagLower);
        }
    }

    /// <summary>
    /// Removes the given flag if is set.
    /// </summary>
    /// <param name="flagArg"></param>
    public void UnsetFlag(string flagArg)
    {
        // get flag in lower-case
        string flagLower = flagArg.ToLower();

        // if the flag IS currently set
        if (setFlags.Contains(flagLower))
        {
            // Un-set the flag
            setFlags.Remove(flagLower);
        }
    }

    #endregion




    #region Romance Flag Functions

    public bool IsRomanceFlagSet(string charIdArg)
    {
        // get appropriate flag name
        string romanceFlag = GetRomanceFlagPrefix() + charIdArg;

        return IsFlagSet(romanceFlag);
    }

    private void SetRomanceFlag(string charIdArg)
    {
        // get appropriate flag name
        string romanceFlag = GetRomanceFlagPrefix() + charIdArg;

        SetFlag(romanceFlag);
    }

    private string GetRomanceFlagPrefix()
    {
        return "romanced_";
    }

    /// <summary>
    /// Have flag that denotes that this romanced character has reaveld her 
    /// sexual kink to MC and is more comfortable being sexual with him.
    /// This opens up a lot more H-scenes and H alternatives to normal scenes.
    /// </summary>
    /// <returns></returns>
    public bool IsSexuallyComfortableFlagSet(string charIdArg)
    {
        // get appropriate flag name
        string comfortFlag = GetSexuallyComfortableFlagPrefix() + charIdArg;

        return IsFlagSet(comfortFlag);
    }

    private string GetSexuallyComfortableFlagPrefix()
    {
        return "sex_comfort_";
    }

    #endregion


}
