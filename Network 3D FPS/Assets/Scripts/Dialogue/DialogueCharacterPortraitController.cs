using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCharacterPortraitController : MonoBehaviour
{
    #region Class Variables

    private CharacterData currentCharacter = null;
    private CharacterDialogueExpressionType currentExpression = CharacterDialogueExpressionType.Neutral;
    private CharacterPortraitSet currentOutfitPortraits = null;

    [Tooltip("Animator for the portrait frame not necessarily the portrait image itself")]
    [SerializeField]
    private Animator portraitFramingAnimator = null;

    [SerializeField]
    private Image portraitImage = null;

    #endregion




    #region Portrait Functions

    public void SetupCharacterData(CharacterData charDataArg)
    {
        currentCharacter = charDataArg;

        // if given a valid character AND that character is actually wearing an outfit
        if (charDataArg != null && charDataArg.outfitLoadout.GetEquippedOutfit() != null)
        {
            // load current portrait set based on character and their outfit
            currentOutfitPortraits = AssetRefMethods.LoadBundleAssetCharacterPortraitSet(
                charDataArg.characterInfo.characterId, 
                charDataArg.outfitLoadout.GetEquippedOutfit().outfitId);
        }
        // else don't have valid data to work with
        else
        {
            // set outfit portrait set to a NULL set
            currentOutfitPortraits = null;
        }

        // sets the portrait image based on the current character's outfit portrait set
        RefreshPortraitImage();
    }

    public bool HaveCharacterData(string charIdArg)
    {
        if (currentCharacter != null)
        {
            return currentCharacter.characterInfo.characterId == charIdArg;
        }
        else
        {
            return false;
        }
    }

    public void SetupCharacterExpression(CharacterDialogueExpressionType expressionArg)
    {
        currentExpression = expressionArg;

        // sets the portrait image based on the current character's outfit portrait set
        RefreshPortraitImage();
    }

    /// <summary>
    /// Sets the portrait image based on the current character's outfit portrait set.
    /// </summary>
    private void RefreshPortraitImage()
    {
        // if a current outfit portrait set is being used
        if (currentOutfitPortraits != null)
        {
            // set the portrait image to the portrait set's apporpriate expression sprite
            portraitImage.sprite = currentOutfitPortraits.GetAppropriatePortrait(currentExpression);
        }
        // else NO current outfit portrait set
        else
        {
            // set the portrait image to a NULL sprite
            portraitImage.sprite = null;
        }
    }

    #endregion




    #region Animation Functions

    public void PlayAnimationAppear()
    {
        portraitFramingAnimator.SetTrigger("appear");
    }

    public void PlayAnimationDisappear()
    {
        portraitFramingAnimator.SetTrigger("disappear");
    }

    public void PlayAnimationHighlight()
    {
        portraitFramingAnimator.SetTrigger("highlight");
    }

    #endregion


}
