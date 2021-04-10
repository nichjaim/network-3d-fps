using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPooledObjectController : NetworkBehaviour
{
    #region Class Variables

    // pooler where this object resides
    private NetworkObjectPooler _objectPoolerOwner = null;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup the reference to the object pool owner
        InitializePoolOwner();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the reference to the object pool owner.
    /// </summary>
    private void InitializePoolOwner()
    {
        _objectPoolerOwner = GetComponentInParent<NetworkObjectPooler>();
    }

    #endregion




    #region Object Pooling Functions

    /// <summary>
    /// Un-spawns this pooled object.
    /// </summary>
    public void UnspawnObject()
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

    [Command(ignoreAuthority = true)]
    private void CmdUnspawnObject()
    {
        UnspawnObjectInternal();
    }

    private void UnspawnObjectInternal()
    {
        // return object to pool on server
        _objectPoolerOwner.PutBackInPool(gameObject);

        // tell server to send ObjectDestroyMessage, which will call UnspawnHandler on client
        NetworkServer.UnSpawn(gameObject);
    }

    #endregion


}
