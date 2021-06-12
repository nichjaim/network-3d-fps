using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionFollow : AIAction
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMovementController _charMovement = null;

    [Tooltip("AiDecision won't get the brain reference if not on same object, so " +
        "set here to allow for seperate objects.")]
    [SerializeField]
    private AIBrain _aiBrain = null;

    #endregion




    #region Override Functions

    public override void PerformAction()
    {
        // set follow target to brain's target
        _charMovement.NavTarget = _aiBrain.Target;
    }

    #endregion


}
