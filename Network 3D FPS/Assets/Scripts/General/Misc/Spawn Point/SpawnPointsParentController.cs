using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsParentController : MonoBehaviour
{
    [SerializeField]
    private List<SerializableDataFloatAndNetworkObjectPooler> spawnChanceToPooler = new 
        List<SerializableDataFloatAndNetworkObjectPooler>();
    public List<SerializableDataFloatAndNetworkObjectPooler> SpawnChanceToPooler
    {
        get { return spawnChanceToPooler; }
    }
}
