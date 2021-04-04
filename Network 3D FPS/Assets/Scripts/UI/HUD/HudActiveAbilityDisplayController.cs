using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudActiveAbilityDisplayController : MonoBehaviour, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;
    private CharacterActionActiveAbilityController _charActionAbility = null;

    [SerializeField]
    private Image imageAbilityIcon = null;

    [Tooltip("The radial fill image that denotes ability cooldown time.")]
    [SerializeField]
    private Image imageRadialAbilityCooldown = null;

    private StopTimer cooldownTimer = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // fully sets up the displayed ability properties
        RefreshAbilityFullDisplay();
    }

    private void Update()
    {
        // sets the cooldown radial image's fill based on current cooldown timer
        RefreshCooldownRadialFill();
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




    #region Ability Functions

    /// <summary>
    /// Fully sets up the displayed ability properties.
    /// </summary>
    private void RefreshAbilityFullDisplay()
    {
        RefreshAbilityIcon();
        RefreshCooldownTimer();
    }

    /// <summary>
    /// Sets up the ability icon image.
    /// </summary>
    private void RefreshAbilityIcon()
    {
        // if reference to char's ability action is setup
        if (_charActionAbility != null)
        {
            // get equipped active ability
            ActiveAbility currentAbility = _charActionAbility.GetCurrentActiveAbility();

            // if current ability retrieved
            if (currentAbility != null)
            {
                // load appropriate ability icon sprite
                Sprite abilityIcon = AssetRefMethods.LoadBundleAssetActiveAbilityIcon(
                    currentAbility.abilityId);

                // if ability icon successfully loaded
                if (abilityIcon != null)
                {
                    // set ability icon image to loaded sprite
                    imageAbilityIcon.sprite = abilityIcon;

                    // turn ON ability icon image
                    imageAbilityIcon.gameObject.SetActive(true);

                    // DONT continue code
                    return;
                }
            }
        }

        // getting here means that ability icon image should NOT be used, so turn OFF the image object
        imageAbilityIcon.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the cooldown timer to current ability's cooldown timer.
    /// </summary>
    private void RefreshCooldownTimer()
    {
        // if reference to char's ability action is setup
        if (_charActionAbility != null)
        {
            cooldownTimer = _charActionAbility.GetCurrentAbilityCooldownTime();
        }
        // else reference NOT setup
        else
        {
            // set timer to a NULL timer
            cooldownTimer = null;
        }
    }

    /// <summary>
    /// Sets the cooldown radial image's fill based on current cooldown timer. 
    /// Call in Update().
    /// </summary>
    private void RefreshCooldownRadialFill()
    {
        // if ability ON cooldown
        if (cooldownTimer != null)
        {
            // if cooldown radial image is inactive
            if (!imageRadialAbilityCooldown.gameObject.activeSelf)
            {
                // turn ON radial cooldown image
                imageRadialAbilityCooldown.gameObject.SetActive(true);
            }

            // set fill amount based on cooldown progress
            imageRadialAbilityCooldown.fillAmount = cooldownTimer.GetProgressPercentageRemaining();
        }
        // else ability off cooldown
        else
        {
            // if cooldown radial image is active
            if (imageRadialAbilityCooldown.gameObject.activeSelf)
            {
                // turn OFF radial cooldown image
                imageRadialAbilityCooldown.gameObject.SetActive(false);
            }
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
        playerCharArg.OnCharDataSecondaryChangedAction += RefreshAbilityFullDisplay;

        // find the char's action ability component
        _charActionAbility = playerCharArg.GetComponent<
            CharacterActionActiveAbilityController>();
        // if comp found
        if (_charActionAbility != null)
        {
            _charActionAbility.OnActiveAbilityUseAction += RefreshCooldownTimer;
        }
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // fully sets up the displayed ability properties
        RefreshAbilityFullDisplay();
    }

    #endregion


}
