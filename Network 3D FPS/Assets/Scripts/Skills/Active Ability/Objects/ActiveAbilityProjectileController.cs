using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(NetworkRigidbody))]
public class ActiveAbilityProjectileController : ActiveAbilityController
{
    #region Class Variables

    private Rigidbody _rigBody = null;

    [Header("Base Projectile Properties")]

    [SerializeField]
    protected float projectileSpeed = 1f;

    protected int targetPenetrationCurrent = 0;
    // How many contacts this projectile can pass through before it is done
    protected int targetPenetrationMax = 1;

    #endregion




    #region MonoBehaviour Functions

    protected virtual void OnEnable()
    {
        // resets the number of targets that this projectile has passed through
        ResetCurrentTargetPenetration();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // get collididng object's hitbox component
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if can harm the hitbox target
        if (GeneralMethods.CanHarmHitboxTarget(_casterCharData, collidingHitbox))
        {
            // call to denote that made contact with an opposing character
            OnContactWithEnemy(collidingHitbox);
        }
    }

    #endregion




    #region Override Functions

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // move the projectile
        MoveProjectile();
    }

    protected override void InitializeComponentReferences()
    {
        base.InitializeComponentReferences();

        _rigBody = GetComponent<Rigidbody>();
    }

    public override void SetupActiveAbility(ActiveAbility castedActiveAbilityArg,
        CharacterMasterController casterCharacterArg, CharacterData casterCharacterDataArg)
    {
        base.SetupActiveAbility(castedActiveAbilityArg, casterCharacterArg, casterCharacterDataArg);

        // setup ability projectile properties
        SetupAbilityTargetPenetrationMax();
    }

    #endregion




    #region Setup Functions

    /// <summary>
    /// Adjusts the ability's projectile max penetration based on the given ability rank argument.
    /// </summary>
    protected virtual void SetupAbilityTargetPenetrationMax()
    {
        // initialize ability damage taken property values
        int baseValue = 1;
        int rankAdditionValue = 1;
        int maxValue = 100;

        // get new calculated value
        int newPentMax = Mathf.Clamp(baseValue + (rankAdditionValue *
            (_appropriateAbilityRank - 1)), baseValue, maxValue);

        // set max pentration to calcualted value
        targetPenetrationMax = newPentMax;
    }

    #endregion




    #region Projectile Functions

    /// <summary>
    /// Called when this projectile makes contact with an enemies hitbox.
    /// </summary>
    /// <param name="enemyHitboxArg"></param>
    protected virtual void OnContactWithEnemy(HitboxController enemyHitboxArg)
    {
        // IMPL in child class
    }

    /// <summary>
    /// Moves the projectile. 
    /// Call in FixedUpdate().
    /// </summary>
    private void MoveProjectile()
    {
        _rigBody.velocity = transform.forward * GetTrueMoveSpeed() * Time.deltaTime;
    }

    /// <summary>
    /// Returns the actual speed value for physics movement.
    /// </summary>
    /// <returns></returns>
    private float GetTrueMoveSpeed()
    {
        // initialize the projectile speed multiplier
        float SPEED_MULTIPLER = 100f;

        // return speed with multiplier
        return projectileSpeed * SPEED_MULTIPLER;
    }

    /// <summary>
    /// Resets the number of targets that this projectile has passed through. 
    /// Call in OnEnable().
    /// </summary>
    private void ResetCurrentTargetPenetration()
    {
        targetPenetrationCurrent = 0;
    }

    /// <summary>
    /// Increments number of targets penetrated and despawns if reached penetration limit. 
    /// Call at end of OnContactWithEnemy() in child class if deemed appropriate.
    /// </summary>
    protected virtual void ProcessTargetPenetration()
    {
        // increment number of targets penetrated
        targetPenetrationCurrent++;

        // if penetrated maxed number of targets
        if (targetPenetrationCurrent >= targetPenetrationMax)
        {
            // despawns this pooled object
            DespawnObject();
        }
    }

    #endregion


}
