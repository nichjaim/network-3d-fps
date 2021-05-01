﻿using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUiCoordinator : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private StandardDialogueUI dialogueUiHead = null;

    #endregion




    #region Event Action Functions

    /// <summary>
    /// Should be called when the dialogue panel is opened.
    /// </summary>
    public void OnDialoguePanelOpen()
    {
        // trigger event to denote that this dialogue is OPENING
        DialogueActivationEvent.Trigger(dialogueUiHead, true);
    }

    /// <summary>
    /// Should be called when the dialogue panel is closed.
    /// </summary>
    public void OnDialoguePanelClose()
    {
        // trigger event to denote that this dialogue is CLOSING
        DialogueActivationEvent.Trigger(dialogueUiHead, false);
    }

    /// <summary>
    /// Should be called when subtitle text's typewriter component 
    /// writes a new text character.
    /// </summary>
    public void OnSubtitleTextCharacterType()
    {
        Debug.LogWarning("NEED IMPL: play randomly pitched sound based on who is speaking."); // NEED IMPL
    }

    #endregion


}