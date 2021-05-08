using UnityEngine;

public class SpawnPointContainerController : MonoBehaviour
{
    [SerializeField]
    private SpawnEntityType spawnPointType = SpawnEntityType.None;
    public SpawnEntityType SpawnPointType
    {
        get { return spawnPointType; }
    }
}
