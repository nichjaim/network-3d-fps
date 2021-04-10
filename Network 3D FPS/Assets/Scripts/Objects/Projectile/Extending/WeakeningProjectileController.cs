using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakeningProjectileController : ProjectileController
{
    #region Class Variables

    WeakeningStatusEffect statusEffect = null;
    private float effectDuration = 1f;

    #endregion




    #region Override Functions

    protected override void OnContactWithEnemy(HitboxController enemyHitboxArg)
    {
        base.OnContactWithEnemy(enemyHitboxArg);

        // apply weakening debuff to hit target
        enemyHitboxArg.CharMaster.CharStatus.AddStatusEffect(statusEffect, effectDuration);

        // increment number of targets penetrated and despawns if reached penetration limit
        ProcessTargetPenetration();
    }

    #endregion




    #region Ability Functions

    public void SetupWeakeningProjectile(float dmgTakeIncrPercArg, float effectDurationArg)
    {
        statusEffect = new WeakeningStatusEffect(dmgTakeIncrPercArg);
        effectDuration = effectDurationArg;
    }

    #endregion


}
