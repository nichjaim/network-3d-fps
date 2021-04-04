using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Experimental;

[RequireComponent(typeof(PooledObjectController), typeof(Rigidbody), 
    typeof(NetworkRigidbody))]
public abstract class ProjectileController : NetworkBehaviour
{
    #region Class Variables

    protected PooledObjectController _pooledObject = null;
    protected Rigidbody _rigBody = null;

    private CharacterMasterController charOwner = null;
    private CharacterData charOwnerData = null;
    public CharacterData CharOwnerData
    {
        get { return charOwnerData; }
    }

    [Header("Base Projectile Properties")]

    [SerializeField]
    protected float projectileSpeed = 1f;
    [SerializeField]
    protected float projectileLifetime = 5f;

    #endregion




    #region MonoBehaviour Functions

    protected virtual void Awake()
    {
        // setup all vars that reference relevant components
        InitializeComponentReferences();
    }

    protected virtual void FixedUpdate()
    {
        // move the projectile
        MoveProjectile();
    }

    protected virtual void OnEnable()
    {
        // turn off this object after lifetime is over
        DespawnAfterLifetime();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // get collididng object's hitbox component
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if comp found
        if (collidingHitbox != null)
        {
            // if CAN harm hitbox target
            if (collidingHitbox.CharHealth.CanAttackerCauseHarmBasedOnReputation(
                charOwnerData.factionReputation))
            {
                // call to denote that made contact with an opposing character
                OnContactWithEnemy(collidingHitbox);
            }
        }
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all vars that reference relevant components. 
    /// Call in Awake().
    /// </summary>
    protected virtual void InitializeComponentReferences()
    {
        _rigBody = GetComponent<Rigidbody>();
        _pooledObject = GetComponent<PooledObjectController>();
    }

    #endregion




    #region Projectile Functions

    public virtual void SetupProjectile(CharacterMasterController charArg)
    {
        charOwner = charArg;
        charOwnerData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            charOwner);
    }

    /// <summary>
    /// Moves the projectile. 
    /// Call in FixedUpdate().
    /// </summary>
    private void MoveProjectile()
    {
        _rigBody.velocity = transform.forward * projectileSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Called when this projectile makes contact with an enemies hitbox.
    /// </summary>
    /// <param name="enemyHitboxArg"></param>
    protected virtual void OnContactWithEnemy(HitboxController enemyHitboxArg)
    {
        // IMPL in child class
    }

    #endregion




    #region Object Functions

    /// <summary>
    /// Turn off this object after lifetime is over.
    /// </summary>
    private void DespawnAfterLifetime()
    {
        // call internal function as coroutine
        StartCoroutine(DespawnAfterLifetimeInternal());
    }

    private IEnumerator DespawnAfterLifetimeInternal()
    {
        // wait lifetime span
        yield return new WaitForSeconds(projectileLifetime);

        // unspawn this object
        DespawnObject();
    }

    /// <summary>
    /// Despawns this pooled object.
    /// </summary>
    public virtual void DespawnObject()
    {
        _pooledObject.UnspawnObject();
    }

    #endregion


}
