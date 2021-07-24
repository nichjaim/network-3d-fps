using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterHealthController : CharacterHealthController
{
    #region Class Variables

    private GameManager _gameManager = null;

    [Header("Player-specific Properties")]

    [SerializeField]
    private ObjectActivationController _objectActivation = null;

    #endregion




    #region Override Functions

    protected override void InitializeSingletonReferences()
    {
        base.InitializeSingletonReferences();

        _gameManager = GameManager.Instance;
    }

    protected override void ProcessDeath()
    {
        // if the tutorial is NOT complete
        if (!_gameManager.IsTutorialComplete())
        {
            // get char data for surface level properties from the char master
            CharacterData charFrontFacingData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
                _characterMasterController);

            // bring character's health back to one
            charFrontFacingData.characterStats.ResuscitateIfCriticallyHurt();

            // call health change actions if NOT null
            OnHealthChangedAction?.Invoke();

            // DONT continue code
            return;
        }

        base.ProcessDeath();

        // trigger event to denote that party has wiped
        PartyWipeEvent.Trigger();

        // turn off this object
        _objectActivation.SetObjectActivation(false);
    }

    #endregion


}
