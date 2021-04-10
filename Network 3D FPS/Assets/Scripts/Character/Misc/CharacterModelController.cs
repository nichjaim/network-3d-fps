using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _characterMasterController = null;
    private PlayerCharacterMasterController _playerCharacterMasterController = null;

    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;
    [SerializeField]
    private Animator _animator = null;

    /// list of colors that the character model will temporarily be changed to. 
    /// Used for status effects.
    private List<Color> temporaryModelColors = new List<Color>();

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup all the relevent component references
        InitializeComponentReferences();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // set associated animator's controller to the anim apporpriate for this char object
        RefreshCharacterAnimator();

        // sets the model color based on current temporary model colors
        RefreshModelColor();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all the relevent component references.
    /// Call in Awake().
    /// </summary>
    private void InitializeComponentReferences()
    {
        _playerCharacterMasterController = _characterMasterController as PlayerCharacterMasterController;
    }

    #endregion




    #region Model Functions

    /// <summary>
    /// Sets associated animator's controller to the anim apporpriate for this char object.
    /// </summary>
    private void RefreshCharacterAnimator()
    {
        _animator.runtimeAnimatorController = AssetRefMethods.
            LoadBundleAssetCharacterAnimator(GetAppropriateCharId());
    }

    /// <summary>
    /// Returns the char ID of character associated with this char object.
    /// </summary>
    /// <returns></returns>
    private string GetAppropriateCharId()
    {
        // initialize var for upcoming conditionals
        CharacterData charData;
        // if char is a player
        if (_playerCharacterMasterController != null)
        {
            // get secondary char data
            charData = _playerCharacterMasterController.CharDataSecondary;
            // if no secondary char data is set
            if (charData == null)
            {
                // get primary char data
                charData = _characterMasterController.CharData;
            }
        }
        // else char master is NOT a player
        else
        {
            // get primary char data
            charData = _characterMasterController.CharData;
        }

        // if char data was found
        if (charData != null)
        {
            // return the char's ID
            return charData.characterInfo.characterId;
        }
        // else there was no character data
        else
        {
            // return some default ID
            return string.Empty;
        }
    }

    #endregion




    #region Status Effects Functions

    /// <summary>
    /// Returns a mix of all the temporary model colors. 
    /// Returns the default model color if there are NO temporary colors.
    /// </summary>
    /// <returns></returns>
    private Color GetTemporaryModelColorsMix()
    {
        return GeneralMethods.GetColorsMix(temporaryModelColors, Color.white);
    }

    /// <summary>
    /// Sets the model color based on current temporary model colors.
    /// </summary>
    private void RefreshModelColor()
    {
        _spriteRenderer.color = GetTemporaryModelColorsMix();
    }

    /// <summary>
    /// Adds color to temporary model colors.
    /// </summary>
    /// <param name="colorArg"></param>
    public void AddTemporaryModelColor(Color colorArg)
    {
        // add given color to temporary model colors
        temporaryModelColors.Add(colorArg);

        // sets the model color based on current temporary model colors
        RefreshModelColor();
    }

    /// <summary>
    /// Removes color to temporary model colors.
    /// </summary>
    /// <param name="colorArg"></param>
    public void RemoveTemporaryModelColor(Color colorArg)
    {
        // remove given color to temporary model colors
        temporaryModelColors.Remove(colorArg);

        // sets the model color based on current temporary model colors
        RefreshModelColor();
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        _characterMasterController.OnCharDataChangedAction += RefreshCharacterAnimator;

        // if char is a player
        if (_playerCharacterMasterController != null)
        {
            _playerCharacterMasterController.OnCharDataSecondaryChangedAction += RefreshCharacterAnimator;
        }
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        _characterMasterController.OnCharDataChangedAction -= RefreshCharacterAnimator;

        // if char is a player
        if (_playerCharacterMasterController != null)
        {
            _playerCharacterMasterController.OnCharDataSecondaryChangedAction -= RefreshCharacterAnimator;
        }
    }

    #endregion


}
