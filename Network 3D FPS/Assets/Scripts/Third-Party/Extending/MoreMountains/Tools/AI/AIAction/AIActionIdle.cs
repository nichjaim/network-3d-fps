using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionIdle : AIAction
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMovementController _charMovement = null;

    #endregion




    #region Override Functions

    public override void PerformAction()
    {
        // stop following any target
        _charMovement.ResetNavigationTarget();
    }

    #endregion


}
