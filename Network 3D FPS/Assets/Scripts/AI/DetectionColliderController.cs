using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionColliderController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _charMaster = null;

    [SerializeField]
    private AIDecisionThreatManager aiThreatManager = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnTriggerEnter(Collider other)
    {
        // get collididng object's hitbox component
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if can harm the hitbox target
        if (GeneralMethods.CanHarmHitboxTarget(_charMaster.CharData, collidingHitbox))
        {
            OnEnemyTargetDetection(collidingHitbox);
        }
    }

    #endregion




    #region Detection Functions

    /// <summary>
    /// Should be called when a enemy target has been target.
    /// </summary>
    private void OnEnemyTargetDetection(HitboxController collidingHitboxArg)
    {
        // if given valid hitbox
        if (collidingHitboxArg != null)
        {
            // make threat manager aware of colliding character but do not increase the actual threat they pose
            aiThreatManager.MakeAwareOfThreat(collidingHitboxArg.CharMaster);
        }
    }

    #endregion


}
