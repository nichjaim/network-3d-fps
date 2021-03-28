using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Object Poolers")]

    [SerializeField]
    private NetworkObjectPooler projectilePooler = null;
    public NetworkObjectPooler ProjectilePooler
    {
        get { return projectilePooler; }
    }

    [SerializeField]
    private NetworkObjectPooler enemyPooler = null;
    public NetworkObjectPooler EnemyPooler
    {
        get { return enemyPooler; }
    }

    [SerializeField]
    private NetworkObjectPooler pickupAmmoPooler = null;


}
