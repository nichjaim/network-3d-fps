using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Class Variables

    [Header("Object Poolers")]

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




    #region Pooler Functions

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


}
