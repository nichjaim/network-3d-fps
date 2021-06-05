using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Class Variables

    [Header("Local Object Poolers")]

    [SerializeField]
    private ObjectPooler localObjectPoolers = null;

    [Header("Network Object Poolers")]

    [SerializeField]
    private List<SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler> 
        netObjectPoolers = new 
        List<SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler>();

    /*[SerializeField]
    private NetworkObjectPooler bulletPooler = null;
    public NetworkObjectPooler BulletPooler
    {
        get { return bulletPooler; }
    }

    [SerializeField]
    private NetworkObjectPooler weakeningProjectilePooler = null;
    public NetworkObjectPooler WeakeningProjectilePooler
    {
        get { return weakeningProjectilePooler; }
    }

    [SerializeField]
    private NetworkObjectPooler enemyPooler = null;
    public NetworkObjectPooler EnemyPooler
    {
        get { return enemyPooler; }
    }

    [SerializeField]
    private NetworkObjectPooler pickupAmmoPooler = null;*/

    [SerializeField]
    private List<SerializableDataActiveAbilityTemplateAndNetworkObjectPooler> 
        abilityTempAndAbilityPrefabPooler = new 
        List<SerializableDataActiveAbilityTemplateAndNetworkObjectPooler>();

    #endregion




    #region MonoBehaviour Functions

    private void OnDisable()
    {
        // despawns all pooled objects
        DespawnAllPooledObjects();
    }

    #endregion




    #region Network Pooler Functions

    public NetworkObjectPooler GetNetworkObjectPooler(ObjectPoolerContentType 
        poolerContentArg)
    {
        SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler match = 
            netObjectPoolers.FirstOrDefault(iterData => iterData.value1 == poolerContentArg);

        if (match != null)
        {
            return match.value2;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns network pooler associated with given active ability.
    /// </summary>
    /// <param name="abilityArg"></param>
    /// <returns></returns>
    public NetworkObjectPooler GetAbilityPrefabPooler(ActiveAbility abilityArg)
    {
        // get serial data that matches given ability
        SerializableDataActiveAbilityTemplateAndNetworkObjectPooler matchingData = 
            abilityTempAndAbilityPrefabPooler.FirstOrDefault(
            iterData => iterData.refAbilityTemplate.template.abilityId == abilityArg.abilityId);

        // if data found
        if (matchingData != null)
        {
            // return the matching data's pooler object reference
            return matchingData.refNetworkPooler;
        }
        // else no match found
        else
        {
            // return a null pooler object reference
            return null;
        }
    }

    /// <summary>
    /// Despawns all pooled objects.
    /// </summary>
    private void DespawnAllPooledObjects()
    {
        // despawns all network pooled objects
        DespawnAllNetworkPooledObjects();

        // despawns all local pooled objects
        DespawnAllLocalPooledObjects();
    }

    /// <summary>
    /// Despawns all network pooled objects.
    /// </summary>
    private void DespawnAllNetworkPooledObjects()
    {
        // get all network pooled objects (need to make sure to get the inactive ones as well)
        NetworkPooledObjectController[] netPoolObjs = GetComponentsInChildren<NetworkPooledObjectController>(true);

        // loop though all network pooled objects
        foreach (NetworkPooledObjectController iterPoolObj in netPoolObjs)
        {
            // despawn the iterating net spawned object
            iterPoolObj.UnspawnObject();
        }
    }

    /// <summary>
    /// Despawns all local pooled objects.
    /// </summary>
    private void DespawnAllLocalPooledObjects()
    {
        // loop through all the local object poolers' child objects (which would be the pooled objects)
        foreach (Transform iterChildObj in localObjectPoolers.transform)
        {
            // deactivate the iterating pooled object
            iterChildObj.gameObject.SetActive(false);
        }
    }

    #endregion




    #region Local Pooler Functions

    /// <summary>
    /// Returns a pooled object from the local object poolers.
    /// </summary>
    /// <param name="poolTagArg"></param>
    /// <param name="posArg"></param>
    /// <param name="rotArg"></param>
    /// <returns></returns>
    private GameObject GetLocalPooledObject(string poolTagArg, Vector3 posArg, Quaternion rotArg)
    {
        return localObjectPoolers.SpawnFromPool(poolTagArg, posArg, rotArg);
    }

    public GameObject GetPooledActionTextPopup(Vector3 posArg, Quaternion rotArg)
    {
        return GetLocalPooledObject("actiontextpopup", posArg, rotArg);
    }

    #endregion




    /*#region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<ExitGameSessionEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ExitGameSessionEvent>();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        // despawns all pooled objects
        DespawnAllPooledObjects();
    }

    #endregion*/


}
