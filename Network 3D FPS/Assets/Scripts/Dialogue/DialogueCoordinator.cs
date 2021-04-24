using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCoordinator : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private DialogueSystemTrigger dialogueTrigger = null;

    [ConversationPopup(true)]
    [SerializeField]
    private string testDialogueConvo = string.Empty; // TESTING VAR!!!

    #endregion




    #region Dialogue Functions

    /// <summary>
    /// Plays a conversation dialogue.
    /// </summary>
    /// <param name="conversationArg"></param>
    private void PlayDialogue(string conversationArg)
    {
        // set the dialogue trigger's dialogue conversation to the given dialogue conversation
        dialogueTrigger.conversation = conversationArg;

        // trigger the dialogue
        dialogueTrigger.OnUse();
    }

    public void PlayDialogueTestingConversation()
    {
        PlayDialogue(testDialogueConvo);
    }

    #endregion


}
