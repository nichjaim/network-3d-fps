using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudCharacterPortraitController : MonoBehaviour, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    [Header("Component References")]

    [SerializeField]
    private Image charPortrait = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // setup character's portrait image
        RefreshCharacterPortrait();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all the vars that hold references to singletons. 
    /// Call in Start(), because need to allow time for singletons to instantiate.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _uiManager = UIManager.Instance;
    }

    #endregion




    #region Portrait Functions

    /// <summary>
    /// Sets up character's portrait image.
    /// </summary>
    private void RefreshCharacterPortrait()
    {
        // get player char the UI is associated with
        PlayerCharacterMasterController playerChar = _uiManager.UiPlayerCharacter;

        // if player char found
        if (playerChar != null)
        {
            // get character data that is responsible for the player's apperance
            CharacterData visualCharData = playerChar.CharDataSecondary;

            // if char data found
            if (visualCharData != null)
            {
                // load character icon
                Sprite charIcon = AssetRefMethods.LoadBundleAssetCharacterIcon(
                    visualCharData.characterInfo.characterId);

                // if char icon successfully loaded
                if (charIcon != null)
                {
                    // turn ON char portrait image
                    charPortrait.gameObject.SetActive(true);
                    // set portrait image to loaded sprite
                    charPortrait.sprite = charIcon;
                }
                // else NO char icon found
                else
                {
                    // turn OFF char portrait image
                    charPortrait.gameObject.SetActive(false);
                }
            }
            // else NO char data retrieved
            else
            {
                // turn OFF char portrait image
                charPortrait.gameObject.SetActive(false);
            }
        }
        // else NO player char retrieved
        else
        {
            // turn OFF char portrait image
            charPortrait.gameObject.SetActive(false);
        }
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<UiPlayerCharChangedEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<UiPlayerCharChangedEvent>();
    }

    /// <summary>
    /// Add functions to be called for given player's action events.
    /// </summary>
    /// <param name="playerCharArg"></param>
    private void AddPlayerEventListening(PlayerCharacterMasterController playerCharArg)
    {
        playerCharArg.OnCharDataSecondaryChangedAction += RefreshCharacterPortrait;
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // setup character's portrait image
        RefreshCharacterPortrait();
    }

    #endregion


}
