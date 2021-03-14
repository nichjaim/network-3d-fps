using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterActionAttackController : NetworkBehaviour
{
    #region Class Variables

    private NetworkManagerCustom _networkManagerCustom = null;

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    //[SerializeField]
    //private MMSimpleObjectPoolerCustom projectilePooler = null;
    private NetworkObjectPooler _projectilePooler = null;

    [SerializeField]
    private Transform firePoint = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
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
        _projectilePooler = GameManager.Instance.ProjectilePooler;
    }

    #endregion




    #region Action Functions

    /// <summary>
    /// Perform the act of attacking.
    /// </summary>
    private void AttackAction()
    {
        if (NetworkClient.isConnected)
        {
            CmdAttackAction();
        }
        else
        {
            AttackActionInternal();
        }
    }

    [Command]
    private void CmdAttackAction()
    {
        AttackActionInternal();
    }

    private void AttackActionInternal()
    {
        // get pooled projectile object
        //GameObject projectileObj = projectilePooler.GetPooledGameObject();
        /*GameObject projectileObj = GameManager.Instance.CmdSpawnObject(projectilePooler, 
            firePoint.position, _characterMasterController.transform.rotation);*/
        // get object from given pooler
        GameObject projectileObj = _projectilePooler.GetFromPool(firePoint.position,
            firePoint.rotation);
        // spawn pooled object on network
        NetworkServer.Spawn(projectileObj);

        // get projectile from pooled object
        ProjectileController projectile = projectileObj.GetComponent<ProjectileController>();
        // if no such component found
        if (projectile == null)
        {
            // print warning to console
            Debug.LogWarning("Projectile object does NOT have ProjectileController component! " +
                $"Object name: {projectileObj.name}");
            // DONT continue code
            return;
        }

        // set projectile object to appropriate position and rotation
        /*projectileObj.transform.SetPositionAndRotation(firePoint.position, 
            _characterMasterController.transform.rotation);*/
        // setup projectile data
        projectile.SetupProjectile(_characterMasterController);
        //projectileObj.SetActive(true);
    }

    /*[Command]
    public GameObject CmdSpawnObject(Vector3 posArg,
        Quaternion rotArg)
    {
        // get object from given pooler
        GameObject pooledObj = projectilePooler.GetFromPool(posArg, rotArg);

        // spawn pooled object on network
        NetworkServer.Spawn(pooledObj);

        // return the pooled object
        return pooledObj;
    }

    [Command]
    public void CmdUnspawnObject(NetworkObjectPooler objectPoolerArg, GameObject pooledObjArg)
    {
        // return object to pool on server
        objectPoolerArg.PutBackInPool(pooledObjArg);

        // tell server to send ObjectDestroyMessage, which will call UnspawnHandler on client
        NetworkServer.UnSpawn(pooledObjArg);
    }*/

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


}
