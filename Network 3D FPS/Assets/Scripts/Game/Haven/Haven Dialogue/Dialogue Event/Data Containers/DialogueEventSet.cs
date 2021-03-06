﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

[CreateAssetMenu(fileName = "New Dialogue Event Set", menuName = "Scriptable Objects/Dialogue/Dialogue Event Set")]
public class DialogueEventSet : ScriptableObject
{
    #region Class Variables

    [Tooltip("The flag needed for this set and all further sets to be considered for player availablity. " +
        "Leave empty if no such flag needed")]
    public string requiredFlag;

    [Header("Dialogue Event Properties")]

    //public List<DialogueEventData> dialogueEvents;
    [MMReorderableAttribute(null, "Data", null)]
    public ReorderableListDialogueEventData dialogueEvents;

    #endregion




    #region Set Functions

    /// <summary>
    /// Returns whether a flag is required to access this set.
    /// </summary>
    /// <returns></returns>
    public bool DoesRequireFlag()
    {
        return requiredFlag != string.Empty;
    }

    #endregion


}
