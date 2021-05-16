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

    #endregion




    #region Override Functions

    public override void PerformAction()
    {
        // set follow target to brain's target
        _charMovement.NavTarget = _brain.Target;
    }

    #endregion


}
