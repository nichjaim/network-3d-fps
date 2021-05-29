using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueUiCoordinator : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private BackgroundFadeController bgFade = null;

    [SerializeField]
    private Transform characterPortraitsParent = null;
    private List<DialogueCharacterPortraitController> characterPortraits = new 
        List<DialogueCharacterPortraitController>();

    [SerializeField]
    private TextMeshProUGUI textSpeakerName = null;

    [Tooltip("This object must parent the positions sets which parent the actual positions within those sets.")]
    [SerializeField]
    private Transform dialoguePortraitPositionsHolder = null;

    /// the character data loaded for the current dialogue, keep them do that don't have to 
    /// repeatedly loading a bunch of character data
    private List<CharacterData> charactersInDialogueScene = new List<CharacterData>();

    private CharacterData currentSpeaker = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup the variable that holds all the character portraits
        InitializeCharacterPortraits();

        // turns off all character portraits to denote that none are being used
        DeactivateAllCharacterPortraits();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the variable that holds all the character portraits. 
    /// Call in Start().
    /// </summary>
    private void InitializeCharacterPortraits()
    {
        // reset character portraits to fresh empty list
        characterPortraits = new List<DialogueCharacterPortraitController>();

        // initialize var for upcoming loop
        DialogueCharacterPortraitController iterPortrait;
        // loop through all potential portrait objects
        foreach (Transform iterChild in characterPortraitsParent)
        {
            // get portrait comp from iterating child object
            iterPortrait = iterChild.GetComponent<DialogueCharacterPortraitController>();
            // if portrait component found
            if (iterPortrait != null)
            {
                // add portrait to portraits list
                characterPortraits.Add(iterPortrait);
            }
        }
    }

    #endregion




    #region Dialogue Main Functions

    /// <summary>
    /// Sets all dialogue UI elements back to their default state. 
    /// Use when starting a new dialogue
    /// </summary>
    private void ResetDialogueUI()
    {
        // deactivates the backgrounds
        bgFade.RemoveBackgrounds();

        // turns off all character portraits to denote that none are being used
        DeactivateAllCharacterPortraits();

        // set no character as currently speaking
        SetCharSpeaker(null);
    }

    #endregion




    #region Dialogue Portrait Functions

    /// <summary>
    /// Places the associated character portrait in the dialogue scene.
    /// </summary>
    /// <param name="charIdArg"></param>
    public void CharacterPortraitEnterDialogueScene(string charIdArg, CharacterDialogueExpressionType expressionArg)
    {
        // if character portrait is already in scene
        if (IsCharacterPortraitInScene(charIdArg))
        {
            // DONT continue code
            return;
        }

        // get character data for given char ID
        CharacterData charInDialogueScene = GetCharacterInDialogueScene(charIdArg);

        // if char data WAS found
        if (charInDialogueScene != null)
        {
            // get an unused character portrait
            DialogueCharacterPortraitController unusedPort = GetUnusedCharacterPortrait();

            // if an available dialogue portrait was found
            if (unusedPort != null)
            {
                // assign the character data to the dialogue portrait
                unusedPort.SetupCharacterData(charInDialogueScene);
                // asign the character's expression to the dialogue portrait
                unusedPort.SetupCharacterExpression(expressionArg);

                // activate the portrait
                unusedPort.gameObject.SetActive(true);

                // set new portrait's position to their appropriate position
                unusedPort.transform.position = GetAppropriateDialoguePortraitPosition(
                    GetActiveDialoguePortraits().Count, GetDialoguePortaitOrderNumber(charIdArg));

                // play appear animation for the dialogue portrait
                unusedPort.PlayAnimationAppear();

                // set the entering character as the character that is currently speaking
                SetCharSpeaker(charInDialogueScene);
            }
        }

        // moves all active character portraits to their appropriate positions
        RefreshDialoguePortraitPositions();
    }

    private void CharacterPortraitEnterDialogueScene(string charIdArg)
    {
        CharacterPortraitEnterDialogueScene(charIdArg, CharacterDialogueExpressionType.Neutral);
    }

    /// <summary>
    /// Remove the associated character portrait from the dialogue scene.
    /// </summary>
    /// <param name="charIdArg"></param>
    public void CharacterPortraitExitDialogueScene(string charIdArg)
    {
        // call internal function as coroutine
        StartCoroutine(CharacterPortraitExitDialogueSceneInternal(charIdArg));
    }

    private IEnumerator CharacterPortraitExitDialogueSceneInternal(string charIdArg)
    {
        // get the char dialogue portrait associated with the given ID
        DialogueCharacterPortraitController charPort = GetCharacterPortraitInScene(charIdArg);

        // if dialogue portrait actually found
        if (charPort != null)
        {
            // play disappear animation for the dialogue portrait
            charPort.PlayAnimationDisappear();

            // wait till sure that animation is done
            yield return new WaitForSeconds(1f);

            // turn off a character portrait to denote it is no longer being used
            DeactivateCharacterPortrait(charPort);
        }

        // moves all active character portraits to their appropriate positions
        RefreshDialoguePortraitPositions();
    }

    /// <summary>
    /// Moves all active character portraits to their appropriate positions.
    /// </summary>
    private void RefreshDialoguePortraitPositions()
    {
        // get all active dialogue portraits
        List<DialogueCharacterPortraitController> activePortraits = GetActiveDialoguePortraits();

        // loop through all active dialogue portraits
        for (int i = 0; i < activePortraits.Count; i++)
        {
            // set the iterating dialogue's position to the appropriate UI position
            activePortraits[i].transform.position = GetAppropriateDialoguePortraitPosition(
                activePortraits.Count, i + 1);
        }
    }

    /// <summary>
    /// Returns the UI position for a dialogue portrait.
    /// </summary>
    /// <param name="numOfPortraitsInSceneArg"></param>
    /// <param name="portraitOrderNumArg"></param>
    /// <returns></returns>
    private Vector3 GetAppropriateDialoguePortraitPosition(int numOfPortraitsInSceneArg, int portraitOrderNumArg)
    {
        // if the given number of total portraits is invalid
        if (numOfPortraitsInSceneArg <= 0 || 
            numOfPortraitsInSceneArg > dialoguePortraitPositionsHolder.childCount)
        {
            // print warning to console
            Debug.LogWarning($"Invalid numOfPortraitsInSceneArg given: {numOfPortraitsInSceneArg}");

            // return default position
            return Vector3.zero;
        }

        // get the appropriate position set
        Transform posSet = dialoguePortraitPositionsHolder.GetChild(numOfPortraitsInSceneArg - 1);

        // if the given portrait num in the set is invalid
        if (portraitOrderNumArg <= 0 || portraitOrderNumArg > posSet.childCount)
        {
            // print warning to console
            Debug.LogWarning($"Invalid portraitOrderNumArg given: {portraitOrderNumArg}");

            // return default position
            return Vector3.zero;
        }

        // get the appropriate position transform
        Transform pos = posSet.GetChild(portraitOrderNumArg - 1);

        // return the position
        return pos.position;
    }

    /// <summary>
    /// Sets the given character to have the given expression.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="expressionArg"></param>
    public void SetCharacterPortraitExpression(string charIdArg, CharacterDialogueExpressionType expressionArg)
    {
        // get the given character's associated character portrait
        DialogueCharacterPortraitController charPortrait = GetCharacterPortraitInScene(charIdArg);

        // if portrait found
        if (charPortrait != null)
        {
            // set portrait's expression to given expression
            charPortrait.SetupCharacterExpression(expressionArg);
        }
    }

    /// <summary>
    /// Turns off a character portrait to denote it is no longer being used.
    /// </summary>
    /// <param name="charPortraitArg"></param>
    private void DeactivateCharacterPortrait(DialogueCharacterPortraitController charPortraitArg)
    {
        // remove portrait's associated character
        charPortraitArg.SetupCharacterData(null);

        /// deactivate the dialogue portrait to denote character leaving scene and freeing 
        /// up a dialogue portrait for use
        charPortraitArg.gameObject.SetActive(false);
    }

    /// <summary>
    /// Turns off all character portraits to denote that none are being used.
    /// </summary>
    private void DeactivateAllCharacterPortraits()
    {
        characterPortraits.ForEach(iterPort => DeactivateCharacterPortrait(iterPort));
    }

    #endregion




    #region Dialogue Portrait Getter Functions

    private DialogueCharacterPortraitController GetUnusedCharacterPortrait()
    {
        return characterPortraits.FirstOrDefault(iterPort => !iterPort.gameObject.activeSelf);
    }

    private bool IsCharacterPortraitInScene(string charIdArg)
    {
        return characterPortraits.Exists(iterPort => iterPort.HaveCharacterData(charIdArg));
    }

    private DialogueCharacterPortraitController GetCharacterPortraitInScene(string charIdArg)
    {
        return characterPortraits.FirstOrDefault(iterPort => iterPort.HaveCharacterData(charIdArg));
    }

    private List<DialogueCharacterPortraitController> GetActiveDialoguePortraits()
    {
        return characterPortraits.FindAll(iterPort => iterPort.gameObject.activeSelf);
    }

    private int GetDialoguePortaitOrderNumber(string charIdArg)
    {
        return characterPortraits.FindIndex(iterPort => iterPort.HaveCharacterData(charIdArg));
    }

    #endregion




    #region Dialogue Character Functions

    /// <summary>
    /// Returns the character from the characters in the current dialogue scene. 
    /// Adds the character to the current dialogue scene if they're not currently in there.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private CharacterData GetCharacterInDialogueScene(string charIdArg)
    {
        // get the character from the dialogue scene characters
        CharacterData charData = charactersInDialogueScene.
            FirstOrDefault(iterChar => iterChar.characterInfo.characterId == charIdArg);

        // if character NOT in the current dialogue scene
        if (charData == null)
        {
            // get character data from given ID
            charData = GetCharacterDataFromId(charIdArg);

            // if loaded data WAS found
            if (charData != null)
            {
                // add laoded character to dialogue scene for quick later reference
                charactersInDialogueScene.Add(charData);
            }
            // else NO data was found
            else
            {
                // print warning to console
                Debug.LogWarning($"No such character data template for ID: {charIdArg}");
            }
        }

        // return the character data
        return charData;
    }

    /// <summary>
    /// Returns the character data based on the given character ID.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <returns></returns>
    private CharacterData GetCharacterDataFromId(string charIdArg)
    {
        // MAYBE IMPL: MAYBE CHECK PARTY MEMBERS FIRST BEFORE LOADING THE STARTING DATA???

        // load character data template
        CharacterDataTemplate charDataTemp = AssetRefMethods.LoadBundleAssetCharacterDataTemplate(charIdArg);

        // if loaded data WAS found
        if (charDataTemp != null)
        {
            // return data based on loaded template
            return new CharacterData(charDataTemp);
        }
        // else NO data found
        else
        {
            // return NULL data
            return null;
        }
    }

    #endregion




    #region Dialogue Speaker Functions

    /// <summary>
    /// Assigns the current dialogue speaker.
    /// </summary>
    /// <param name="charIdArg"></param>
    public void AssignSpeaker(string charIdArg)
    {
        // get the new character speaker to set
        CharacterData newSpeaker = GetCharacterInDialogueScene(charIdArg);

        // if new speaker is the same speaker as the current one
        if (newSpeaker == currentSpeaker)
        {
            // DONT continue code
            return;
        }

        // get character portrait of speaking character
        DialogueCharacterPortraitController speakerPortrait = GetCharacterPortraitInScene(charIdArg);

        // if speaking character DOES have their portrait in scene
        if (speakerPortrait != null)
        {
            // play portrait's highlight animation
            speakerPortrait.PlayAnimationHighlight();
        }

        // set the new speaker as the character that is currently speaking
        SetCharSpeaker(newSpeaker);
    }

    /// <summary>
    /// Sets the character that is currently speaking.
    /// </summary>
    /// <param name="charArg"></param>
    private void SetCharSpeaker(CharacterData charArg)
    {
        // set the current speaker as the new speaker
        currentSpeaker = charArg;

        // setup the speaker name text based on the current character speaker
        RefreshSpeakerNameText();
    }

    /// <summary>
    /// Sets up the speaker name text based on the current character speaker.
    /// </summary>
    private void RefreshSpeakerNameText()
    {
        // if there IS a speaker
        if (currentSpeaker != null)
        {
            // set speaker name text to the current speaker's name
            textSpeakerName.text = currentSpeaker.characterInfo.characterName;

            // get current speaker's signature color
            Color speakerColor = GeneralMethods.GetColorFromHexColorCode(
                currentSpeaker.characterInfo.characterHexColorCode);

            // set speaker name text color to speaker's signature color
            textSpeakerName.color = speakerColor;
        }

        // activate/deactivate the speaker name text object based on is there is a speaker or not
        textSpeakerName.gameObject.SetActive(currentSpeaker != null);
    }

    /// <summary>
    /// Ensures no actual character is the speaker.
    /// </summary>
    public void RemoveSpeaker()
    {
        SetCharSpeaker(null);
    }

    /// <summary>
    /// Plays the appropriate sound for when a text character is typed into dialogue based 
    /// on who is currently speaking.
    /// </summary>
    private void PlaySpeakerVerbalSound()
    {
        Debug.Log("NEED IMPL: PlaySpeakerVerbalSound()"); // NEED IMPL!!!

        // NEED IMPL: get sound based on the current speaker, save sound byte when saving char data fore quick reuse???

        // NEED IMPL: play sound byte at a random pitch
    }

    #endregion




    #region Dialogue Misc Functions

    /// <summary>
    /// Transitions dialogue background to new BG sprite.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <param name="isHContentArg"></param>
    private void ChangeBackground(string bgIdArg, bool isHContentArg)
    {
        // initialize var for upcoming conditionals
        Sprite bg = null;

        // if bg is H-content
        if (isHContentArg)
        {
            // load ERO background sprite based on given ID
            bg = AssetRefMethods.LoadHcontentBackground(bgIdArg);
        }
        // else bg is all-ages content
        else
        {
            // load NORMAL background sprite based on given ID
            bg = AssetRefMethods.LoadBundleAssetDialogueBackground(bgIdArg);
        }

        // fade transition to new sprite
        bgFade.BackgroundTransition(bg, 1f);
    }

    #endregion




    #region Event Action Functions

    /// <summary>
    /// Should be called when subtitle text's typewriter component 
    /// writes a new text character.
    /// </summary>
    public void OnSubtitleTextCharacterType()
    {
        /// plays the appropriate sound for when a text character is typed into dialogue based 
        /// on who is currently speaking
        PlaySpeakerVerbalSound();
    }

    /// <summary>
    /// Should be called when the dialogue panel closes.
    /// </summary>
    public void OnDialoguePanelClose()
    {
        // sets all dialogue UI elements back to their default state
        ResetDialogueUI();
    }

    #endregion


}
