using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // set associated animator's controller to the anim apporpriate for this char object
        RefreshCharacterAnimator();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Model Functions

    /// <summary>
    /// Sets associated animator's controller to the anim apporpriate for this char object.
    /// </summary>
    private void RefreshCharacterAnimator()
    {
        GetComponent<Animator>().runtimeAnimatorController = AssetRefMethods.
            LoadBundleAssetPlayableCharacterAnimator(GetAppropriateCharId());
    }

    /// <summary>
    /// Returns the char ID of character associated with this char object.
    /// </summary>
    /// <returns></returns>
    private string GetAppropriateCharId()
    {
        // get char master's assigned character data
        CharacterData charData = _characterMasterController.GetCharData();
        // if char data found
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




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        _characterMasterController.OnCharDataChangedAction += RefreshCharacterAnimator;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        _characterMasterController.OnCharDataChangedAction -= RefreshCharacterAnimator;
    }

    #endregion


}
