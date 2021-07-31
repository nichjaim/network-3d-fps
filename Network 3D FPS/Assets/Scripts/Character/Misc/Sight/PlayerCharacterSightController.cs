using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSightController : CharacterSightController
{
    #region Class Variables

    [SerializeField]
    private Transform cameraDeathPoint = null;

    #endregion




    #region Override Functions

    protected override void StartAllEventListening()
    {
        base.StartAllEventListening();

        _playerCharacterMasterController.CharHealth.OnOutOfHealthAction += ProcessPlayerDeath;
    }

    protected override void StopAllEventListening()
    {
        base.StopAllEventListening();

        _playerCharacterMasterController.CharHealth.OnOutOfHealthAction -= ProcessPlayerDeath;
    }

    #endregion




    #region Player Sight Functions

    /// <summary>
    /// Performs all the neccessary processes for the player death.
    /// </summary>
    private void ProcessPlayerDeath()
    {
        // call internal function as coroutine
        StartCoroutine(ProcessPlayerDeathInternal());
    }

    private IEnumerator ProcessPlayerDeathInternal()
    {
        // positions the camera to the appropriate death position
        PutCameraInDeathPosition();

        // wait the appropriate amount of time till death dialogue should kick in
        yield return new WaitForSeconds(2f);

        // trigger event to denote that party has wiped
        PartyWipeEvent.Trigger();
    }

    /// <summary>
    /// Positions the camera to the appropriate death position.
    /// </summary>
    private void PutCameraInDeathPosition()
    {
        _cameraCoordr.transform.SetPositionAndRotation(cameraDeathPoint.position, 
            cameraDeathPoint.rotation);
    }

    #endregion


}
