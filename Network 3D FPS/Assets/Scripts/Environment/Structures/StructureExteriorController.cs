using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureExteriorController : MonoBehaviour
{
    #region Class Variables

    private LevelManager _levelManager = null;

    private StructureInteriorController linkedInterior = null;
    public StructureInteriorController LinkedInterior
    {
        get { return linkedInterior; }
    }

    [SerializeField]
    private StructureType structureType = StructureType.Shack;

    [SerializeField]
    private List<TeleporterController> entranceTeleporters = new List<TeleporterController>();

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components, 
    /// if they are not already setup. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        // if references ALREADY setup
        if (_levelManager != null)
        {
            //DONT continue code
            return;
        }

        _levelManager = GameManager.Instance.LevelManager;
    }

    #endregion




    #region Structure Functions

    /// <summary>
    /// Creates a structure interior and links the two together. 
    /// Call in Start().
    /// </summary>
    public void InstantiateStructureInterior()
    {
        // setup singeton refs if they are not already setup
        InitializeSingletonReferences();

        // get randomly chosen interior prefab
        GameObject randomInteriorPrefab = GetRandomInteriorPrefab();
        // reserve a spot for chosen interior
        Vector3 reserveSpot = GetInstantiationPosition(randomInteriorPrefab);

        // instantiate interior prefab object
        GameObject createdInteriorObject = Instantiate(randomInteriorPrefab, reserveSpot, Quaternion.identity, 
            _levelManager.SpawnedStructureInteriorParent);
        // set linked interior structure to component from created object
        linkedInterior = createdInteriorObject.GetComponent<StructureInteriorController>();

        // set the created interior's linked exterior to this exterior
        linkedInterior.LinkExterior(this);
        // link the exterior and interior teleporters together
        LinkTeleporters(linkedInterior);
    }

    /// <summary>
    /// Returns randomly chosen interior prefab object that is asscoaited with this structure type.
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomInteriorPrefab()
    {
        // load all structure interiors that are similar to this structure type
        GameObject[] loadedInteriorPrefabs = AssetRefMethods.LoadAllBundleAssetStructureInterior(structureType);

        // initialize these vars for the upcoming loop
        List<(float, GameObject)> tupleListChanceRateToInterior = new List<(float, GameObject)>();
        StructureInteriorController interiorComp;
        // loop through all loaded structure interior prefabs
        foreach (GameObject iterObj in loadedInteriorPrefabs)
        {
            // get structure interior component from iterating prefab object
            interiorComp = iterObj.GetComponent<StructureInteriorController>();
            // if iterating object DID have a structure interior component
            if (interiorComp != null)
            {
                // add the iterating chunk's spawn chance and ID to tuple
                tupleListChanceRateToInterior.Add((interiorComp.InteriorSpawnChance, iterObj));
            }
            // else iterating object did NOT have a structure interior component
            else
            {
                // print warning to console
                Debug.LogWarning("Structure interior prefab object did NOT have a structure interior compoenent! " +
                    "Named: " + iterObj.name);
            }
        }

        // return random interior prefab based on chance rate
        return GeneralMethods.GetRandomEntryFromChanceToValueCouple<GameObject>(
            RollChancePickType.TargetRarest, tupleListChanceRateToInterior);
    }

    /// <summary>
    /// Returns available position for given interior prefab object.
    /// </summary>
    /// <param name="interiorPrefabArg"></param>
    /// <returns></returns>
    private Vector3 GetInstantiationPosition(GameObject interiorPrefabArg)
    {
        // get controller of chosen prefab
        StructureInteriorController randomInteriorPrefabController = interiorPrefabArg.
            GetComponent<StructureInteriorController>();
        // get the needed space for the chosen interior object
        float reserveSpace = randomInteriorPrefabController.GetRequiredSpaceX();
        // reserve a spot for chosen interior and return it
        return _levelManager.ReserveStructureInteriorPosition(reserveSpace);
    }

    /// <summary>
    /// Links the exterior's entrance teleporters with the interior's exit teleporters and vice versa.
    /// </summary>
    /// <param name="structureInteriorArg"></param>
    private void LinkTeleporters(StructureInteriorController structureInteriorArg)
    {
        // get the given interior's exit teleporters
        List<TeleporterController> exitTeleporters = structureInteriorArg.ExitTeleporters;
        // loop through all entrance teleporters
        for (int i = 0; i < entranceTeleporters.Count; i++)
        {
            //if iterating entrance teleporter has a matching exit teleporter
            if (exitTeleporters.Count > i)
            {
                // link iterating entrance teleporter to matching exit teleporter and vice versa
                entranceTeleporters[i].LinkReceivingTeleporter(exitTeleporters[i]);
                exitTeleporters[i].LinkReceivingTeleporter(entranceTeleporters[i]);
            }
        }
    }

    #endregion


}
