using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class WorldChunkController : MonoBehaviour
{
    #region Class Variables

    //private GameManager _gameManager;
    private SpawnManager _spawnManager;

    /*[Header("Spawn Points")]

    [SerializeField]
    private GameObject enemySpawnPointsParent = null;
    [SerializeField]
    private GameObject levelKeySpawnPointsParent = null;

    private List<Transform> enemySpawnPoints = new List<Transform>();
    private List<Transform> levelKeySpawnPoints = new List<Transform>();*/

    private StructureExteriorController[] structureExteriors = null;

    [SerializeField]
    private float chunkSpawnChance = 1f;
    public float ChunkSpawnChance
    {
        get { return chunkSpawnChance; }
    }

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
        // setsup the list of enemy spawn points
        //InitializeEnemySpawnPointReferences();

        // spawn enemy objects
        //SpawnEnemies();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _spawnManager = GameManager.Instance.SpawnManager;
    }

    /*/// <summary>
    /// Sets up the list of enemy spawn points. 
    /// Call in Start().
    /// </summary>
    private void InitializeEnemySpawnPointReferences()
    {
        // reset spawn point list to empty list
        enemySpawnPoints = new List<Transform>();

        // loop through all parent spawn point's child objects
        foreach (Transform child in enemySpawnPointsParent.transform)
        {
            // add iterating child trans to spawn point list
            enemySpawnPoints.Add(child);
        }

        // loop through all structure exteriors
        foreach (StructureExteriorController iterExterior in structureExteriors)
        {
            // loop through all of the iterating exterior's linked interior enemy spawn points
            foreach (Transform child in iterExterior.GetLinkedInterior().
                GetEnemySpawnPointsParent().transform)
            {
                // add iterating child trans to spawn point list
                enemySpawnPoints.Add(child);
            }
        }
    }*/

    /*/// <summary>
    /// Sets up the list of level key spawn points. 
    /// Call in Start().
    /// </summary>
    private void InitializeLevelKeySpawnPointReferences()
    {
        // reset spawn point list to empty list
        levelKeySpawnPoints = new List<Transform>();

        // loop through all parent spawn point's child objects
        foreach (Transform child in levelKeySpawnPointsParent.transform)
        {
            // add iterating child trans to spawn point list
            levelKeySpawnPoints.Add(child);
        }

        // loop through all structure exteriors
        foreach (StructureExteriorController iterExterior in structureExteriors)
        {
            // loop through all of the iterating exterior's linked interior level key spawn points
            foreach (Transform child in iterExterior.GetLinkedInterior().
                GetLevelKeySpawnPointsParent().transform)
            {
                // add iterating child trans to spawn point list
                levelKeySpawnPoints.Add(child);
            }
        }
    }*/

    #endregion




    #region World Chunk Functions

    /*/// <summary>
    /// Spawn enemy objects based on random chunk spawn points.
    /// </summary>
    private void SpawnEnemies()
    {
        _spawnManager.SpawnEnemies(GetRandomEnemySpawnPoints());
    }*/

    /*/// <summary>
    /// Returns randomly chosen enemy spawns whose count is based on current game round.
    /// </summary>
    /// <returns></returns>
    private List<Transform> GetRandomEnemySpawnPoints()
    {
        // get the number of enemies to spawn as an unrounded float
        float enemiesToSpawnFloat = (float)enemySpawnPoints.Count * GetPercentageOfAreaEnemiesToSpawn();
        // round that num to a whole integer
        int enemiesToSpawnInt = Mathf.RoundToInt(enemiesToSpawnFloat);

        // get random spawn list indices
        List<int> enemySpawnIndices = GeneralMethods.GetRandomIndicesFromListCount(enemySpawnPoints.Count, 
            enemiesToSpawnInt);
        // initialize the chosen enemy spawn points as an empty list
        List<Transform> chosenEnemySpawns = new List<Transform>();
        // loop through all chosen spawn point list indices
        foreach (int iterIndex in enemySpawnIndices)
        {
            // add the iterating index to return list
            chosenEnemySpawns.Add(enemySpawnPoints[iterIndex]);
        }

        // return populated spawn point list
        return chosenEnemySpawns;
    }*/

    /*/// <summary>
    /// Returns the percentage of potential enemies in the area that should be spawned based 
    /// on the current game round.
    /// </summary>
    /// <returns></returns>
    private float GetPercentageOfAreaEnemiesToSpawn()
    {
        /// initialize how much the percentage should be increase by based on the current 
        /// game round number
        float PERCENT_INCREASE_PER_ROUND = 0.25f;
        /// return the appropriate percentage based on the game round while ensuring the 
        /// percentage does NOT exceed 100%
        return Mathf.Min(1f, PERCENT_INCREASE_PER_ROUND * (float)_gameManager.GetGameRound());
    }*/

    /*/// <summary>
    /// Return a random level key spawn point. 
    /// Returns NULL if no spawn points.
    /// </summary>
    /// <returns></returns>
    public Transform GetRandomLevelKeySpawnPoint()
    {
        //setup the list of level key spawn points
        InitializeLevelKeySpawnPointReferences();

        //if no level key spawn points
        if (levelKeySpawnPoints.Count == 0)
        {
            //return null trans
            return null;
        }

        //get a random spawn point list index
        int randomIndex = Random.Range(0, levelKeySpawnPoints.Count);
        //return spawn point associated with random index
        return levelKeySpawnPoints[randomIndex];
    }*/

    /*/// <summary>
    /// Sets up all the properties for the potential object sets that are inhabiting this world chunk.
    /// </summary>
    public void SetupPotentialObjectSets()
    {
        // find all potential object sets within this world chunk
        PotentialObjectSetController[] potentialObjSets = GetComponentsInChildren<PotentialObjectSetController>();
        // loop through all potential object sets
        foreach (PotentialObjectSetController iterSet in potentialObjSets)
        {
            // choose an object from the iterating potential object set
            iterSet.PickObject();
        }
    }*/

    /// <summary>
    /// Sets up all the properties for the structures that are inhabiting this world chunk.
    /// </summary>
    public void SetupStructures()
    {
        // find all structure exteriors within this world chunk
        structureExteriors = GetComponentsInChildren<StructureExteriorController>();
        // loop through all structure exteriors
        foreach (StructureExteriorController iterExterior in structureExteriors)
        {
            // create the iterating structure exterior's interior
            iterExterior.InstantiateStructureInterior();
        }
    }

    #endregion


}
