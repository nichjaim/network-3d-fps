using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Class Variables

    [Header("World Generation Properties")]

    [SerializeField]
    private PlaneWorldGenerator planeWorldGenerator = null;
    [SerializeField]
    private WorldLayoutParameters TEST_worldLayoutParameters = null;

    [Header("Spawn Properties")]

    [SerializeField]
    private Transform spawnedStructureInteriorParent = null;
    public Transform SpawnedStructureInteriorParent
    {
        get { return spawnedStructureInteriorParent; }
    }

    // the positions for creating structure interiors at
    private float availableStructureInteriorPosY = -1000f;
    private float currentAvailableStructureInteriorPosX = 0f;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Level Functions

    /// <summary>
    /// Creates a random world level.
    /// </summary>
    private void GenerateLevel()
    {
        // reset the state of the interior structure spawning
        ResetStructureInteriors();

        //create the game world using parameters
        planeWorldGenerator.WorldGeneration(TEST_worldLayoutParameters);

        // spawns level keys and places them in random world chunk locations
        //SpawnLevelKeys();

        //trigger event to denote that level generation is done
        GenerateWorldLevelEvent.Trigger(ActionProgressType.Finished, planeWorldGenerator.GeneratedWorld);
    }

    /// <summary>
    /// Returns the currently available structure interior position and denotes that the spot is taken.
    /// </summary>
    /// <returns></returns>
    public Vector3 ReserveStructureInteriorPosition(float sturctIntrSizeXArg)
    {
        // ensure the given spot has enough space between the last reserved spot
        currentAvailableStructureInteriorPosX += sturctIntrSizeXArg;

        // get the position that is being reserved
        Vector3 returnSpot = new Vector3(currentAvailableStructureInteriorPosX, availableStructureInteriorPosY, 0f);

        // set the current pos as reserved and move to next available plot
        currentAvailableStructureInteriorPosX += sturctIntrSizeXArg;

        // return the reserved position
        return returnSpot;
    }

    /// <summary>
    /// Reset the state of the interior structure spawning.
    /// </summary>
    private void ResetStructureInteriors()
    {
        //set the current available spot to the origin
        currentAvailableStructureInteriorPosX = 0f;

        // destory all created structure interior objects
        DestroyAllCreatedStructureInteriors();
    }

    /// <summary>
    /// Destorys all created structure interior objects.
    /// </summary>
    private void DestroyAllCreatedStructureInteriors()
    {
        // loop through all structure interiors
        foreach (Transform child in spawnedStructureInteriorParent)
        {
            //destory iterating structure interior object
            Destroy(child.gameObject);
        }
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        //this.MMEventStartListening<StartNewGameRoundEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        //this.MMEventStopListening<StartNewGameRoundEvent>();
    }

    #endregion


}
