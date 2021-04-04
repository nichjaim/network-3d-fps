using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : ProjectileController
{
    #region Class Variables

    private WeaponData firingWeapon = null;

    #endregion




    #region Override Functions

    public override void SetupProjectile(CharacterMasterController charArg)
    {
        base.SetupProjectile(charArg);

        firingWeapon = charArg.GetEquippedWeapon();

        projectileSpeed = firingWeapon.weaponStats.projectileSpeed;
    }

    protected override void OnContactWithEnemy(HitboxController enemyHitboxArg)
    {
        base.OnContactWithEnemy(enemyHitboxArg);

        // deal weapon damage to hitbox target
        enemyHitboxArg.CharHealth.TakeDamage(GetRandomWeaponDamage());

        // despawn this pooled object
        DespawnObject();
    }

    #endregion




    #region Bullet Functions

    /// <summary>
    /// Returns random damage within weapon boundaries.
    /// </summary>
    /// <returns></returns>
    private int GetRandomWeaponDamage()
    {
        return Random.Range(firingWeapon.weaponStats.damageBoundaryLowRarityModified, 
            firingWeapon.weaponStats.damageBoundaryHighRarityModified);
    }

    #endregion


}
