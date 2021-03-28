using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureInteriorController : MonoBehaviour
{
    #region Class Variables

    private StructureExteriorController linkedExterior = null;
    public StructureExteriorController LinkedExterior
    {
        get { return linkedExterior; }
    }
    
    [Header("Structure Properties")]

    [SerializeField]
    private BoxCollider structureSpace = null;

    [SerializeField]
    private List<TeleporterController> exitTeleporters = new List<TeleporterController>();
    public List<TeleporterController> ExitTeleporters
    {
        get { return exitTeleporters; }
    }

    [Header("Spawn Points")]

    // parent object for all the spawn point parent objects
    [SerializeField]
    private GameObject spawnPointContainersParent = null;

    /*[SerializeField]
    private GameObject enemySpawnPointsParent = null;
    public GameObject EnemySpawnPointsParent
    {
        get { return enemySpawnPointsParent; }
    }

    [SerializeField]
    private GameObject levelKeySpawnPointsParent = null;*/

    [SerializeField]
    private float interiorSpawnChance = 1f;
    public float InteriorSpawnChance
    {
        get { return interiorSpawnChance; }
    }

    #endregion




    #region Structure Functions

    /// <summary>
    /// Returns the space needed for world space reservation.
    /// </summary>
    /// <returns></returns>
    public float GetRequiredSpaceX()
    {
        return structureSpace.size.x;
    }

    /// <summary>
    /// Sets the exterior that this interior is linked to.
    /// </summary>
    /// <param name="exteriorArg"></param>
    public void LinkExterior(StructureExteriorController exteriorArg)
    {
        linkedExterior = exteriorArg;
    }

    #endregion


}
