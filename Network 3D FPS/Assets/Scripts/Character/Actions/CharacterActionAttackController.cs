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

    [SerializeField]
    private Transform firePoint = null;

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
        _bulletPooler = GameManager.Instance.SpawnManager.BulletPooler;
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

        // spawns and fires a projectile
        SpawnProjectile();
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




    #region Spawn Projectile Functions

    /// <summary>
    /// Spawns and fires a projectile.
    /// </summary>
    private void SpawnProjectile()
    {
        if (NetworkClient.isConnected)
        {
            CmdSpawnProjectile();
        }
        else
        {
            SpawnProjectileInternal();
        }
    }

    [Command]
    private void CmdSpawnProjectile()
    {
        SpawnProjectileInternal();
    }

    private void SpawnProjectileInternal()
    {
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


}
