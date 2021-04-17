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
    private NetworkObjectPooler pickupAmmoPooler = null;

    [SerializeField]
    private List<SerializableDataActiveAbilityTemplateAndNetworkObjectPooler> 
        abilityTempAndAbilityPrefabPooler = new 
        List<SerializableDataActiveAbilityTemplateAndNetworkObjectPooler>();

    #endregion




    #region Network Pooler Functions

    public void GetWew()
    {

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


}
