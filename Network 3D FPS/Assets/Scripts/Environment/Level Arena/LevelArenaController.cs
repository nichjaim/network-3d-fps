﻿using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArenaController : MonoBehaviour, MMEventListener<SpawnedCharacterDeathEvent>
{
    #region Class Variables

    private GameManager _gameManager = null;

    [Header("Component References")]

    [Tooltip("The parent object that holds all the spawn point parent objects.")]
    [SerializeField]
    private Transform spawnPointMasterParent = null;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    // the enemy characters that were spawned that are still alive
    private List<CharacterMasterController> aliveSpawnedCharacters = new List<CharacterMasterController>();

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup vars that hold reference to singletons
        InitializeSingletonReferences();
    }

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




    #region Initialization Functions

    /// <summary>
    /// Sets up vars that hold reference to singletons. 
    /// Call in Start(), to give time for singletons to instantiate.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _gameManager = GameManager.Instance;
    }

    #endregion




    #region Spawn Functions

    /// <summary>
    /// Processes all spawning on all spawn points related to the spawn point 
    /// master parent.
    /// </summary>
    private void PerformAllSpawning()
    {
        // de-spawn all objects that have been spawned
        DespawnAllSpawnedObjects();

        // get all spawn point parents from spawn point master parent
        SpawnPointsParentController[] spawnPointParents = spawnPointMasterParent.
            GetComponentsInChildren<SpawnPointsParentController>();

        // loop through all spawn point parents
        foreach (SpawnPointsParentController iterSpawnParent in spawnPointParents)
        {
            // process all spawning on iterating spawn point parent
            SpawnToSpawnPoints(iterSpawnParent);
        }

        // setup the arena exit based on current arena conditions
        RefreshArenaExitStatus();
    }

    /// <summary>
    /// De-spawns all objects that have been spawned.
    /// </summary>
    private void DespawnAllSpawnedObjects()
    {
        // loop trhough all spawned objects
        foreach (GameObject iterObj in spawnedObjects)
        {
            // if iterating object exists
            if (iterObj != null)
            {
                // deactivate iteratign spawned object
                iterObj.SetActive(false);
            }
        }

        // set spawn object lists to fresh empty lists
        spawnedObjects = new List<GameObject>();
        aliveSpawnedCharacters = new List<CharacterMasterController>();
    }

    /// <summary>
    /// Processes all spawning on given spawn point parent.
    /// </summary>
    /// <param name="spawnPointParentArg"></param>
    private void SpawnToSpawnPoints(SpawnPointsParentController spawnPointParentArg)
    {
        // get random spawn points from given spawn point parent
        List<Transform> randomSpawnPoints = GetRandomSpawnPoints(spawnPointParentArg.transform);

        // initialize iterating vars for upcoming loop
        NetworkObjectPooler iterObjPooler;
        GameObject iterSpawnedObj;

        // loop through all retreieved spawn points
        foreach (Transform iterPoint in randomSpawnPoints)
        {
            // get random pooler based on given spawn parent's potential spawning data
            iterObjPooler = GetRandomObjectPooler(spawnPointParentArg.SpawnChanceToPooler);

            // if a pooler was retreived
            if (iterObjPooler != null)
            {
                // spawn an object from the pooler at the iterating spawn point's position
                iterSpawnedObj = iterObjPooler.GetFromPool(iterPoint.position, iterPoint.rotation);

                // add the object to the relevant spawn lists
                AddObjectToSpawnsList(iterSpawnedObj);
            }
        }
    }

    /// <summary>
    /// Returns a random object pooler based on the given potential spawn data.
    /// </summary>
    /// <param name="potentialSpawnsDataArg"></param>
    /// <returns></returns>
    private NetworkObjectPooler GetRandomObjectPooler(
        List<SerializableDataFloatAndObjectPoolerContentType> potentialSpawnsDataArg)
    {
        // initialize tuple list as empty list
        List<(float, ObjectPoolerContentType)> tupleList = new List<(float, ObjectPoolerContentType)>();

        // loop through all potential spawns
        foreach (SerializableDataFloatAndObjectPoolerContentType iterData in potentialSpawnsDataArg)
        {
            // add potential spawn data as tuple to tuple list
            tupleList.Add((iterData.value1, iterData.value2));
        }

        // get random object pooler based on spawn chance
        ObjectPoolerContentType objPoolerContent = GeneralMethods.
            GetRandomEntryFromChanceToValueCouple(RollChancePickType.TargetRarest, tupleList);

        // inittialize return pooler as NULL
        NetworkObjectPooler objPooler = null;

        // if actually got a non-default value from the random entries
        if (objPoolerContent != ObjectPoolerContentType.None)
        {
            // set object pooler to associated pooler reference
            objPooler = _gameManager.SpawnManager.GetNetworkObjectPooler(objPoolerContent);
        }

        // return the retreived pooler
        return objPooler;
    }

    /// <summary>
    /// Get random spawn points based on the given spawn point parent object.
    /// </summary>
    /// <param name="spawnPointParentArg"></param>
    /// <returns></returns>
    private List<Transform> GetRandomSpawnPoints(Transform spawnPointParentArg)
    {
        // initialize list of spawn points
        List<Transform> childObjs = new List<Transform>();
        // loop through all spawn point parent's child objects
        foreach (Transform iterChild in spawnPointParentArg)
        {
            // add iterating spawn point to spawn point list
            childObjs.Add(iterChild);
        }

        // get shuffled list of the child transform spawn points
        List<Transform> shuffledSpawnPoints = GeneralMethods.Shuffle(childObjs);

        // get the number of spawn points that the current game state dictates be used
        int numOfSpawnPointsToUse = GetNumberOfSpawnPointsToUse(shuffledSpawnPoints.Count);

        // return the spawn points that are to be used
        return shuffledSpawnPoints.GetRange(0, numOfSpawnPointsToUse);
    }

    /// <summary>
    /// Returns the number of spawn points that the current game state dictates be used.
    /// </summary>
    /// <param name="maxSpawnPointCountArg"></param>
    /// <returns></returns>
    private int GetNumberOfSpawnPointsToUse(int maxSpawnPointCountArg)
    {
        Debug.Log("NEED IMPL: Get how many spawn points will be used based on how far into set the player is."); // NEED IMPL!!!
        float spawnPointPercToUse = 1f; // TEMP VAR VALUE!!!

        // get number of spawn points to be used, as a float
        float numOfSpawnPointsToUseUnrounded = maxSpawnPointCountArg * spawnPointPercToUse;
        // rount value to an int
        int numOfSpawnPointsToUse = Mathf.RoundToInt(numOfSpawnPointsToUseUnrounded);
        // ensure rounded value does stays within valid boundary
        numOfSpawnPointsToUse = Mathf.Clamp(numOfSpawnPointsToUse, 0, maxSpawnPointCountArg);

        // return calculated number
        return numOfSpawnPointsToUse;
    }

    #endregion




    #region Character Spawn Functions

    /// <summary>
    /// Adds the given objects to the relevant spawn lists.
    /// </summary>
    /// <param name="spawnedObjArg"></param>
    private void AddObjectToSpawnsList(GameObject spawnedObjArg)
    {
        // add spawned object to spawned list for easy cleanup later
        spawnedObjects.Add(spawnedObjArg);

        // get the given spawned object's character component
        CharacterMasterController spawnCharMaster = spawnedObjArg.GetComponent<CharacterMasterController>();
        // if given object is a character
        if (spawnCharMaster != null)
        {
            // heal char to full health
            spawnCharMaster.CharHealth.HealHealth(null, 999999);

            // add enemy to alive spawn chars
            aliveSpawnedCharacters.Add(spawnCharMaster);
        }
    }

    /// <summary>
    /// Denotes that given spawned enemy is dead.
    /// </summary>
    /// <param name="charMasterArg"></param>
    private void RemoveEnemyFromAliveSpawns(CharacterMasterController charMasterArg)
    {
        // if given  spawn character still considered alive
        if (aliveSpawnedCharacters.Contains(charMasterArg))
        {
            // denote spawn char is dead
            aliveSpawnedCharacters.Remove(charMasterArg);
        }

        // setup the arena exit based on current arena conditions
        RefreshArenaExitStatus();
    }

    /// <summary>
    /// Sets up the arena exit based on current arena conditions.
    /// </summary>
    private void RefreshArenaExitStatus()
    {
        // get whether any arena enemies are still alive
        bool areEnemiesLeft = (aliveSpawnedCharacters.Count > 0);

        Debug.Log("NEED IMPL: finish rest of RefreshArenaExitStatus()");
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<SpawnedCharacterDeathEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<SpawnedCharacterDeathEvent>();
    }

    public void OnMMEvent(SpawnedCharacterDeathEvent eventType)
    {
        // denotes that event's spawned enemy is dead
        RemoveEnemyFromAliveSpawns(eventType.charMaster);
    }

    #endregion


}