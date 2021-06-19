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

    [Header("Attack Properties")]

    [SerializeField]
    private LayerMask effectableRaycastLayers = new LayerMask();

    //[SerializeField]
    //private Transform firePoint = null;

    [Header("Detection Properties")]

    [Tooltip("Determines if character will automatic attack enemies in sight.")]
    [SerializeField]
    private bool isAttackingAutomatic = false;

    [SerializeField]
    private float detectRefreshRate = 0.1f;

    [Header("Testing Properties")]

    [SerializeField]
    private LineDrawerController _lineDrawer = null;

    [SerializeField]
    private bool drawRaycasts = false;

    // denotes if attacking is on cooldown
    private bool attackOnCooldown = false;

    public Action OnCharacterAttackAction;

    /// positive for increased, negative for decreased. 
    /// Used for status effects.
    private float temporaryChangePercentageAttackPower = 0f;
    private float temporaryChangePercentageAttackSpeed = 0f;

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

        // put attacking on cooldown for the appropriate length length
        AttackCooldown(GetAttackSpeedCooldown());

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
        // get the currently equipped weapon
        WeaponData equipWep = _characterMasterController.GetEquippedWeapon();

        // get the shot values based on the equipped weapon
        int projPerShot = equipWep.weaponStats.projectilesPerShot;
        float directionalSpread = GetDirectionSpreadFromShotSpreadStat(equipWep.weaponStats.shotSpread);

        // initialize these vars for upcoming loop
        Vector3 baseDir = GetStraightFiringDirection();
        Vector3 iterDir;

        // loop for as many projectiles are in a single firing
        for (int i = 0; i < projPerShot; i++)
        {
            // if first projectile in shot
            if (i == 0)
            {
                // get a straight direction
                iterDir = GetStraightFiringDirection();
            }
            // else this an additonal projectile in shot
            else
            {
                // get direction randomly offset by weapon spread
                iterDir = GeneralMethods.GetRandomRotationDirection(
                    baseDir, directionalSpread, true);
            }

            // check cases based on equipped weapon's firing method
            switch (_characterMasterController.GetEquippedWeapon().fireMethod)
            {
                case ProjectileFireMethodType.Raycast:
                    FireProjectileRaycast(iterDir);
                    break;

                case ProjectileFireMethodType.Object:
                    SpawnProjectileObject();
                    break;
            }
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
    private RaycastHit[] FireWeaponRaycast(Vector3 fireDirectionArg)
    {
        // get equipped weapon's raycast range
        float castRange = _characterMasterController.GetEquippedWeapon().weaponStats.
            raycastRange;

        // get character's fire point
        Transform originTrans = _characterMasterController.CharSight.FirePoint;

        // get values for raycast
        Vector3 rayOrigin = originTrans.position;
        //Vector3 rayDirection = originTrans.forward;
        Vector3 rayDirection = fireDirectionArg;

        // cast raycast and get all hits
        RaycastHit[] raycastHits = GeneralMethods.FireRaycast(true, rayOrigin, rayDirection, 
            castRange, effectableRaycastLayers);

        // if should draw the raycast
        if (drawRaycasts)
        {
            // get the raycast's ending position
            Vector3 rayEndpoint = rayOrigin + (rayDirection * castRange);

            // draw the raycast
            _lineDrawer.DrawLine(rayOrigin, rayEndpoint);
        }

        return raycastHits;
    }

    /// <summary>
    /// Returns whether an enemy is currenly within weapon sights.
    /// </summary>
    /// <returns></returns>
    private bool IsEnemyInSights()
    {
        // fire a weapon based raycast and returns any hit sets that were hit by it
        List<(HitboxController, Vector3)> hitSets = FireWeaponRaycastAndGetHitSets(
            GetStraightFiringDirection());

        // if list retrieved
        if (hitSets != null)
        {
            // return whether list is empty or not
            return hitSets.Count > 0;
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
    private List<(HitboxController, Vector3)> FireWeaponRaycastAndGetHitSets(Vector3 fireDirectionArg)
    {
        // fire weapon raycast and get the hits
        RaycastHit[] wepRaycastHits = FireWeaponRaycast(fireDirectionArg);

        // if nothing hit by raycast
        if (wepRaycastHits == null)
        {
            // DONT continue code
            return null;
        }

        // get all enemy hit sets from the raycast
        List<(HitboxController, Vector3)> enemyHitSets = GetAllEnemyHitSetsFromRaycastHits(wepRaycastHits);

        return enemyHitSets;
    }

    /// <summary>
    /// Returns all enemy hitboxes and hit-points from given raycast hits.
    /// </summary>
    /// <param name="raycastHitsArg"></param>
    /// <returns></returns>
    private List<(HitboxController, Vector3)> GetAllEnemyHitSetsFromRaycastHits(RaycastHit[] raycastHitsArg)
    {
        // initialize return var as fresh empty list
        List<(HitboxController, Vector3)> enemyHitSets = new List<(HitboxController, Vector3)>();

        // initialize var for upcoming loop
        HitboxController iterHitbox;

        // loop through all raycast hits
        foreach (RaycastHit iterRayHit in raycastHitsArg)
        {
            // get hitbox component of enemy that was hit
            iterHitbox = iterRayHit.collider.gameObject.GetComponent<HitboxController>();

            // if object hit was NOT an enemy hitbox
            if (iterHitbox == null)
            {
                // skip to next loop iteration
                continue;
            }

            // if hitbox can be harmed by this character
            if (GeneralMethods.CanHarmHitboxTarget(_characterMasterController.CharData, iterHitbox))
            {
                // add the enemy's hit set to list
                enemyHitSets.Add((iterHitbox, iterRayHit.point));
            }
        }

        // return populated list
        return enemyHitSets;
    }

    /// <summary>
    /// Damage a hitbox based on equipped weapon.
    /// </summary>
    /// <param name="hitboxArg"></param>
    private void DamageHitboxWithEquippedWeapon(HitboxController hitboxArg, Vector3 impactPointArg)
    {
        hitboxArg.CharMaster.CharHealth.TakeDamage(_characterMasterController, GetAttackDamage(), 
            impactPointArg);
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




    #region General Functions

    private Vector3 GetStraightFiringDirection()
    {
        //return _characterMasterController.CharSight.FirePoint.localRotation.eulerAngles;
        //return _characterMasterController.CharSight.FirePoint.localEulerAngles;
        return _characterMasterController.CharSight.FirePoint.forward;
    }

    /// <summary>
    /// Returns a useable directional spread value from a weapon's spread stat value.
    /// </summary>
    /// <param name="spreadStatArg"></param>
    /// <returns></returns>
    private float GetDirectionSpreadFromShotSpreadStat(float spreadStatArg)
    {
        // get the direction spread based on given stat value
        //float dirSpread = (1f - spreadStatArg) * 100f;
        float dirSpread = spreadStatArg * 100f;

        // ensure spread is a valid value
        dirSpread = Mathf.Max(0f, dirSpread);

        // return the calculated value
        return dirSpread;
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
    private void FireProjectileRaycast(Vector3 fireDirectionArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdFireProjectileRaycast(fireDirectionArg);
        }
        else
        {
            FireProjectileRaycastInternal(fireDirectionArg);
        }
    }

    [Command]
    private void CmdFireProjectileRaycast(Vector3 fireDirectionArg)
    {
        FireProjectileRaycastInternal(fireDirectionArg);
    }

    private void FireProjectileRaycastInternal(Vector3 fireDirectionArg)
    {
        // fire a weapon based raycast and returns any hit sets that were hit by it
        List<(HitboxController, Vector3)> hitSets = FireWeaponRaycastAndGetHitSets(fireDirectionArg);

        // if retreived actual hit set list
        if (hitSets != null)
        {
            // if some hitboxes were hit
            if (hitSets.Count > 0)
            {
                // damage the front hitbox with equipped weapon
                DamageHitboxWithEquippedWeapon(hitSets[0].Item1, hitSets[0].Item2);
            }
        }
    }

    #endregion




    #region Stat Attack Power Functions

    /// <summary>
    /// Returns the appropriate amount of attack damage.
    /// </summary>
    /// <returns></returns>
    private int GetAttackDamage()
    {
        // get dmg based on equipped weapon
        int atkPower = GetEquippedWeaponRandomDamage();

        // add to damage based on char's stats
        atkPower += GetCharacterStatAttackPowerIncrease();

        // add/substract damage based on status effects
        atkPower += GetStatusAttackPowerChange(atkPower);

        // ensure the atk power is a valid value
        atkPower = Mathf.Max(0, atkPower);

        // return the calculated value
        return atkPower;
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
    /// Returns the amount to increase the attack power by based on the character's stats.
    /// </summary>
    /// <returns></returns>
    private int GetCharacterStatAttackPowerIncrease()
    {
        // get front-facign char's data
        CharacterData charBeingUsed = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);

        // return the char's stat that is relevant to atk power
        int powerStat = charBeingUsed.characterStats.statStrength;

        // return the retrieved value
        return powerStat;
    }

    /// <summary>
    /// Returns the change to attack power based on the current status effects.
    /// </summary>
    /// <param name="baseAttackPowerArg"></param>
    /// <returns></returns>
    private int GetStatusAttackPowerChange(int baseAttackPowerArg)
    {
        // get status effect change value to attack power as an unrounded float
        float statusAttackPowerChangeFloat = ((float)baseAttackPowerArg) * 
            temporaryChangePercentageAttackPower;

        // round change value to an int
        int statusAttackPowerChange = Mathf.RoundToInt(statusAttackPowerChangeFloat);

        // return calcualted value
        return statusAttackPowerChange;
    }

    #endregion




    #region Stat Attack Speed Functions

    /// <summary>
    /// Returns the current character's attack rate cooldown.
    /// </summary>
    /// <returns></returns>
    private float GetAttackSpeedCooldown()
    {
        // get atk cooldown based on current weapon's attack speed
        float atkRateCooldown = _characterMasterController.GetEquippedWeapon().weaponStats.
            attackRateRarityModified;

        // reduce atk cooldown based on char's stats
        atkRateCooldown -= GetCharacterStatAttackCooldownReduction();

        // change atk cooldown based on current stats effects
        atkRateCooldown += (atkRateCooldown * temporaryChangePercentageAttackSpeed);

        // ensure the atk cooldown is a valid value
        atkRateCooldown = Mathf.Max(0.01f, atkRateCooldown);

        // return the calcualted value
        return atkRateCooldown;
    }

    /// <summary>
    /// Returns the amount to reduce the attack cooldown by based on the character's stats.
    /// </summary>
    /// <returns></returns>
    private float GetCharacterStatAttackCooldownReduction()
    {
        // get front-facign char's data
        CharacterData charBeingUsed = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);

        // get the char's stat that is relevant to atk speed
        int speedStat = charBeingUsed.characterStats.statDexterity;

        // get the amount to reduce cooldown by
        float cooldownReduction = speedStat / 100f;

        // return calculated value
        return cooldownReduction;
    }

    #endregion


}
