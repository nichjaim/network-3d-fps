using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHealthChangeScreenController : MonoBehaviour, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    [Header("Component References")]

    [SerializeField]
    private Image changeScreen = null;

    [SerializeField]
    private ImageFaderController changeScreenFader = null;

    [Header("Change Screen Properties")]

    [SerializeField]
    private Color screenColorDamage = Color.red;
    [SerializeField]
    private Color screenColorHeal = Color.green;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // setup character's portrait image
        //RefreshCharacterPortrait();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // immedately fade screen out
        changeScreenFader.StartFade(0f, false);
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




    #region Screen Functions

    /// <summary>
    /// Should be called when UI character takes damage.
    /// </summary>
    private void OnDamageTaken()
    {
        PerformScreenChange(true, GetUICharacterPostDamageInvincibility());
    }

    /// <summary>
    /// Should be called when UI character's health is healed.
    /// </summary>
    private void OnHealthHealed()
    {
        PerformScreenChange(false, GetUICharacterPostDamageInvincibility());
    }

    /// <summary>
    /// Flashes the change screen with the apporpriate visuals for the given amount of time.
    /// </summary>
    /// <param name="isTakingDamageArg"></param>
    /// <param name="screenActiveTimeArg"></param>
    private void PerformScreenChange(bool isTakingDamageArg, float screenActiveTimeArg)
    {
        // call internal function as coroutine
        StartCoroutine(PerformScreenChangeInternal(isTakingDamageArg, screenActiveTimeArg));
    }

    private IEnumerator PerformScreenChangeInternal(bool isTakingDamageArg, 
        float screenActiveTimeArg)
    {
        // if taking damage
        if (isTakingDamageArg)
        {
            // set change screen's color to DAMAGE color
            changeScreen.color = screenColorDamage;
        }
        // else healing
        else
        {
            // set change screen's color to HEAL color
            changeScreen.color = screenColorHeal;
        }

        // initialize fading time
        float FADE_TIME = 0.15f;

        /// get the time to wait while ensuring that still a valid wait time. 
        /// NOTE: must reduce by fade time to ensure that incibility concludes when the fade out does.
        float waitTime = Mathf.Max(0, screenActiveTimeArg - FADE_TIME);

        // quickly fade IN change screen
        changeScreenFader.StartFade(FADE_TIME, true);

        // wait for given time
        yield return new WaitForSeconds(waitTime);

        // quickly fade OUT change screen
        changeScreenFader.StartFade(FADE_TIME, false);
    }

    /// <summary>
    /// Returns the current UI character's front-facing char data
    /// </summary>
    /// <returns></returns>
    private CharacterData GetUICharacterData()
    {
        return GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(_uiManager.UiPlayerCharacter);
    }

    /// <summary>
    /// Returns the post-damage invinciblity time stat from the current UI character.
    /// </summary>
    /// <returns></returns>
    private float GetUICharacterPostDamageInvincibility()
    {
        return GetUICharacterData().characterStats.postDamageInvincibilityTime;
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
        //playerCharArg.OnCharDataSecondaryChangedAction += RefreshCharacterPortrait;
        //playerCharArg.CharHealth.OnHealthChangedAction += RefreshScreen;
        playerCharArg.CharHealth.OnDamageTaken += OnDamageTaken;
        playerCharArg.CharHealth.OnHealthHealed += OnHealthHealed;
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // setup character's portrait image
        //RefreshCharacterPortrait();
    }

    #endregion


}
