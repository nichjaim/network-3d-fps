using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPooledObjectEnemyController : NetworkPooledObjectController
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _charMaster = null;

    [SerializeField]
    private ObjectActivationController _objActivation = null;

    #endregion




    #region MonoBehaviour Functions

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




    #region Enemy Spawn Functions

    /// <summary>
    /// Should be called when the asscoiated spawned enemy char runs out of health.
    /// </summary>
    private void OnSpawnedEnemyDeath()
    {
        // trigger event to denote that this spawned char died
        SpawnedCharacterDeathEvent.Trigger(_charMaster);

        Debug.Log("NEED IMPL: Play enemy death effect."); // NEED IMPL!!!
        Debug.Log("NEED IMPL: Play enemy death sound."); // NEED IMPL!!!

        // set obj deactivate of this object
        _objActivation.SetObjectActivation(false);
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        _charMaster.CharHealth.OnOutOfHealthAction += OnSpawnedEnemyDeath;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        _charMaster.CharHealth.OnOutOfHealthAction -= OnSpawnedEnemyDeath;
    }

    #endregion


}
