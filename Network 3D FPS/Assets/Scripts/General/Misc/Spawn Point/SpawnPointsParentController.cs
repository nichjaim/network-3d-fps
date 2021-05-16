using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsParentController : MonoBehaviour
{
    [SerializeField]
    private List<SerializableDataFloatAndObjectPoolerContentType> spawnChanceToPoolerContent = new 
        List<SerializableDataFloatAndObjectPoolerContentType>();
    public List<SerializableDataFloatAndObjectPoolerContentType> SpawnChanceToPooler
    {
        get { return spawnChanceToPoolerContent; }
    }
}
