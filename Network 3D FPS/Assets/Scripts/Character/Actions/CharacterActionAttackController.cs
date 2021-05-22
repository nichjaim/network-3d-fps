using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CharacterActionAttackController : NetworkBehaviour
{
    #region Class Variables

    private NetworkManagerCustom _networkManagerCustom = null;

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    private NetworkObjectPooler _bulletPooler = null;

    //[SerializeField]
    //private Transform firePoint = null;

    [Header("Detection Properties")]

    [Tooltip("Determines if character will automatic attack enemies in sight.")]
    [SerializeField]
    private bool isAttackingAutomatic = false;

    [SerializeField]
    private float detectRefreshRate = 0.1f;

    // denotes if attacking is on cooldown
    private bool attackOnCooldown = false;

    public Action OnCharacterAttackAction;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    private void OnEnable()
    {
        // ensure attacking is OFF cooldown
        attackOnCooldown = false;

        // starts cycle to automatically attack if target in sights
        AutoAttackCycle();
    }

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // if the current state of menus does NOT allow the player chars to perform actions
        if (!_networkManagerCustom.DoesMenusStateAllowPlayerCharacterAction())
        {
            // DONT continue code
            return;
        }

        // performs the attack action if the appropriate player input was made
        AttackIfPlayerInputted();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _networkManagerCustom = (NetworkManagerCustom)NetworkManager.singleton;
        _bulletPooler = GameManager.Instance.SpawnManager.
            GetNetworkObjectPooler(ObjectPoolerContentType.ProjectileBullet);
    }

    #endregion




    #region Action Functions

    /// <summary>
    /// Perform the act of attacking.
    /// </summary>
    private void AttackAction()
    {
        // if attacking is ON cooldown
        if (attackOnCooldown)
        {
            // DONT continue code
            return;
        }

        // if NO weapon equipped
        if (!_characterMasterController.HaveWeaponEquipped())
        {
            // DONT continue code
            return;
        }

        // if do NOT have enough ammo
        if (!_characterMasterController.HaveEnoughAmmo())
        {
            // TODO: play some empty ammo sound (based on weapon type??)
            // DONT continue code
            return;
        }

        // reduces amount of ammo currently held based on equipped weapon
        _characterMasterController.ReduceEquippedAmmoByEquippedWeapon();

        // put attacking on cooldown for weapon fire rate length
        AttackCooldown(_characterMasterController.GetEquippedWeapon().weaponStats.
            attackRateRarityModified);

        // call attack actions if NOT null
        OnCharacterAttackAction?.Invoke();

        // fires a projectile based on the current weapon's fire method.
        FireProjectile();
    }

    /// <summary>
    /// Put attacking on cooldown then take it off after the given amount of time.
    /// </summary>
    /// <param name="cooldownTimeArg"></param>
    private void AttackCooldown(float cooldownTimeArg)
    {
        // call internal function as coroutine
        StartCoroutine(AttackCooldownInternal(cooldownTimeArg));
    }

    private IEnumerator AttackCooldownInternal(float cooldownTimeArg)
    {
        // put attack ON cooldown
        attackOnCooldown = true;

        // wait given cooldown time
        yield return new WaitForSeconds(cooldownTimeArg);

        // take attack OFF cooldown
        attackOnCooldown = false;
    }

    /// <summary>
    /// Fires a projectile based on the current weapon's fire method.
    /// </summary>
    private void FireProjectile()
    {
        // check cases based on equipped weapon's firing method
        switch (_characterMasterController.GetEquippedWeapon().fireMethod)
        {
            case ProjectileFireMethodType.Raycast:
                FireProjectileRaycast();
                break;

            case ProjectileFireMethodType.Object:
                SpawnProjectileObject();
                break;
        }
    }

    #endregion




    #region Raycast Functions

    /// <summary>
    /// Continuously checks if enemy in sights between time delays and if so then 
    /// fires weapon enemies.
    /// Call in OnEnable().
    /// </summary>
    private void AutoAttackCycle()
    {
        // call internal function as coroutine
        StartCoroutine(AttackEnemyIfInSightsCycleInternal());
    }

    private IEnumerator AttackEnemyIfInSightsCycleInternal()
    {
        // loop infinitely
        while (true)
        {
            // wait refresh rate time
            yield return new WaitForSeconds(detectRefreshRate);

            // if should be attacking automatically AND enemy is within weapon sights
            if (isAttackingAutomatic && IsEnemyInSights())
            {
                AttackAction();
            }
        }
    }

    /// <summary>
    /// Fires raycast whose properties are based on the currently equipped weapon.
    /// </summary>
    /// <returns></returns>
    private RaycastHit[] FireWeaponRaycast()
    {
        // get equipped weapon's raycast range
        float castRange = _characterMasterController.GetEquippedWeapon().weaponStats.
            raycastRange;

        // get character's fire point
        Transform originTrans = _characterMasterController.CharSight.transform;

        // cast raycast and get all hits
        RaycastHit[] raycastHits = GeneralMethods.FireRaycast(true, originTrans.position,
            originTrans.forward, castRange);

        return raycastHits;
    }

    /// <summary>
    /// Returns whether an enemy is currenly within weapon sights.
    /// </summary>
    /// <returns></returns>
    private bool IsEnemyInSights()
    {
        // fire a weapon based raycast and returns any hitboxes that were hit by it
        List<HitboxController> hitHitboxes = FireWeaponRaycastAndGetHitHitboxes();

        // if list retrieved
        if (hitHitboxes != null)
        {
            // return whether list is empty or not
            return hitHitboxes.Count > 0;
        }
        // else NULL value gotten back
        else
        {
            // return default negative bool
            return false;
        }
    }

    /// <summary>
    /// Fires a weapon based raycast and returns any hitboxes that were hit by it.
    /// </summary>
    /// <returns></returns>
    private List<HitboxController> FireWeaponRaycastAndGetHitHitboxes()
    {
        // fire weapon raycast and get the hits
        RaycastHit[] wepRaycastHits = FireWeaponRaycast();

        // if nothing hit by raycast
        if (wepRaycastHits == null)
        {
            // DONT continue code
            return null;
        }

        // get all enemy hitboxes from the raycast
        List<HitboxController> enemyHitboxes = GetAllEnemyHitboxesFromRaycastHits(wepRaycastHits);

        return enemyHitboxes;
    }

    /// <summary>
    /// Returns all enemy hitboxes from given raycast hits.
    /// </summary>
    /// <param name="raycastHitsArg"></param>
    /// <returns></returns>
    private List<HitboxController> GetAllEnemyHitboxesFromRaycastHits(RaycastHit[] raycastHitsArg)
    {
        // initialize return var as fresh empty list
        List<HitboxController> enemyHitboxes = new List<HitboxController>();

        // initialize var for upcoming loop
        HitboxController iterHitbox;

        // loop through all raycast hits
        foreach (RaycastHit iterRayHit in raycastHitsArg)
        {
            // get hitbox component of enemy that was hit
            iterHitbox = iterRayHit.transform.GetComponent<HitboxController>();

            // if object hit was NOT an enemy hitbox
            if (iterHitbox == null)
            {
                // skip to next loop iteration
                continue;
            }

            // if hitbox can be harmed by this character
            if (GeneralMethods.CanHarmHitboxTarget(_characterMasterController.CharData, iterHitbox))
            {
                // add the enemy's hitbox to list
                enemyHitboxes.Add(iterHitbox);
            }
        }

        // return populated list
        return enemyHitboxes;
    }

    /// <summary>
    /// Returns random damage within equipped weapon's boundaries.
    /// </summary>
    /// <returns></returns>
    private int GetEquippedWeaponRandomDamage()
    {
        // get equipped weapon's stats
        WeaponStats equipWepStats = _characterMasterController.GetEquippedWeapon().weaponStats;

        // return random damage value
        return UnityEngine.Random.Range(equipWepStats.damageBoundaryLowRarityModified,
            equipWepStats.damageBoundaryHighRarityModified);
    }

    /// <summary>
    /// Damage a hitbox based on equipped weapon.
    /// </summary>
    /// <param name="hitboxArg"></param>
    private void DamageHitboxWithEquippedWeapon(HitboxController hitboxArg)
    {
        hitboxArg.CharMaster.CharHealth.TakeDamage(_characterMasterController, GetEquippedWeaponRandomDamage());
    }

    #endregion




    #region Input Functions

    /// <summary>
    /// Performs the attack action if the appropriate player input was made. 
    /// Call in Update().
    /// </summary>
    private void AttackIfPlayerInputted()
    {
        //if input given
        if (Input.GetMouseButtonDown(0))
        {
            // performs the act of attacking
            AttackAction();
        }
    }

    #endregion




    #region SpawnProjectileObject Functions

    /// <summary>
    /// Spawns and fires a projectile object.
    /// </summary>
    private void SpawnProjectileObject()
    {
        if (NetworkClient.isConnected)
        {
            CmdSpawnProjectileObject();
        }
        else
        {
            SpawnProjectileObjectInternal();
        }
    }

    [Command]
    private void CmdSpawnProjectileObject()
    {
        SpawnProjectileObjectInternal();
    }

    private void SpawnProjectileObjectInternal()
    {
        // get the fire point
        Transform firePoint = _characterMasterController.CharSight.FirePoint;

        // get object from given pooler
        GameObject projectileObj = _bulletPooler.GetFromPool(firePoint.position,
            firePoint.rotation);

        // spawn pooled object on network
        NetworkServer.Spawn(projectileObj);

        // get projectile from pooled object
        BulletController projectile = projectileObj.GetComponent<BulletController>();
        // if no such component found
        if (projectile == null)
        {
            // print warning to console
            Debug.LogWarning("Projectile object does NOT have ProjectileController component! " +
                $"Object name: {projectileObj.name}");

            // DONT continue code
            return;
        }

        // setup projectile data
        projectile.SetupProjectile(_characterMasterController);
    }

    #endregion




    #region FireProjectileRaycast Functions

    /// <summary>
    /// Fires a raycast for the projectile.
    /// </summary>
    private void FireProjectileRaycast()
    {
        if (NetworkClient.isConnected)
        {
            CmdFireProjectileRaycast();
        }
        else
        {
            FireProjectileRaycastInternal();
        }
    }

    [Command]
    private void CmdFireProjectileRaycast()
    {
        FireProjectileRaycastInternal();
    }

    private void FireProjectileRaycastInternal()
    {
        // fire a weapon based raycast and returns any hitboxes that were hit by it
        List<HitboxController> hitHitboxes = FireWeaponRaycastAndGetHitHitboxes();

        // if retreived actual hitbox list
        if (hitHitboxes != null)
        {
            // if some hitboxes were hit
            if (hitHitboxes.Count > 0)
            {
                // damage the front hitbox with equipped weapon
                DamageHitboxWithEquippedWeapon(hitHitboxes[0]);
            }
        }
    }

    #endregion


}
