using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakeningProjectileController : ProjectileController
{
    #region Override Functions

    protected override void OnContactWithEnemy(HitboxController enemyHitboxArg)
    {
        base.OnContactWithEnemy(enemyHitboxArg);

        Debug.Log("NEED IMPL: put weakening debuff on hit target."); // NEED IMPL
    }

    #endregion


}
