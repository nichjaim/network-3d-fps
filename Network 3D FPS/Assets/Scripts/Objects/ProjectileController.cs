using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileController : NetworkBehaviour
{
    #region Class Variables

    private CharacterMasterController charOwner = null;
    private CharacterData _frontFacingCharData = null;

    private WeaponData firingWeapon = null;

    private Rigidbody _rigidbody = null;
    private PooledObjectController _pooledObjectController = null;

    private float projectileSpeed = 1f;
    private float projectileLifetime = 5f;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup all vars that reference relevant components
        InitializeComponentReferences();
    }

    private void FixedUpdate()
    {
        // move the projectile
        MoveProjectile();
    }

    private void OnEnable()
    {
        // turn off this object after lifetime is over
        DeactivateAfterLifetime();
    }

    private void OnTriggerEnter(Collider other)
    {
        // get collididng object's hitbox component
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if comp found
        if (collidingHitbox != null)
        {
            // if CAN harm hitbox target
            if (collidingHitbox.CharHealth.CanAttackerCauseHarmBasedOnReputation(
                _frontFacingCharData.factionReputation))
            {
                // deal weapon damage to hitbox target
                collidingHitbox.CharHealth.TakeDamage(GetRandomWeaponDamage());

                // unspawn this object
                _pooledObjectController.UnspawnObject();
            }
        }
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all vars that reference relevant components. 
    /// Call in Awake().
    /// </summary>
    private void InitializeComponentReferences()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _pooledObjectController = GetComponent<PooledObjectController>();
    }

    #endregion




    #region Projectile Functions

    public void SetupProjectile(CharacterMasterController charArg)
    {
        charOwner = charArg;
        _frontFacingCharData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            charOwner);

        firingWeapon = charArg.GetEquippedWeapon();

        projectileSpeed = firingWeapon.weaponStats.projectileSpeed;
    }

    /// <summary>
    /// Moves the projectile. 
    /// Call in FixedUpdate().
    /// </summary>
    private void MoveProjectile()
    {
        _rigidbody.velocity = transform.forward * projectileSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Turn off this object after lifetime is over.
    /// </summary>
    private void DeactivateAfterLifetime()
    {
        // call internal function as coroutine
        StartCoroutine(DeactivateAfterLifetimeInternal());
    }

    private IEnumerator DeactivateAfterLifetimeInternal()
    {
        // wait lifetime span
        yield return new WaitForSeconds(projectileLifetime);

        // unspawn this object
        _pooledObjectController.UnspawnObject();
    }

    /*private void UnspawnObject()
    {
        if (NetworkClient.isConnected)
        {
            CmdUnspawnObject();
        }
        else
        {
            UnspawnObjectInternal();
        }
    }

    [Command]
    public void CmdUnspawnObject()
    {
        UnspawnObjectInternal();
    }

    private void UnspawnObjectInternal()
    {
        // return object to pool on server
        objectPoolerOwner.PutBackInPool(gameObject);

        // tell server to send ObjectDestroyMessage, which will call UnspawnHandler on client
        NetworkServer.UnSpawn(gameObject);
    }*/

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
