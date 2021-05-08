using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUiCoordinator : MonoBehaviour
{
    #region Event Action Functions

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
