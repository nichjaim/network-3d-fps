using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileController : NetworkBehaviour
{
    #region Class Variables

    private CharacterMasterController charOwner = null;
    // pooler where this object resides
    //private NetworkObjectPooler objectPoolerOwner = null;

    [Header("Component References")]

    private Rigidbody _rigidbody = null;

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

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all vars that reference relevant components. 
    /// Call in Awake().
    /// </summary>
    private void InitializeComponentReferences()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    #endregion




    #region Projectile Functions

    public void SetupProjectile(CharacterMasterController charArg)
    {
        //objectPoolerOwner = objectPoolerArg;
        charOwner = charArg;
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
        //GameManager.Instance.CmdUnspawnObject(objectPoolerOwner, gameObject);
        GetComponent<PooledObjectController>().UnspawnObject();
        //UnspawnObject();
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

    #endregion


}
