using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArenaCoordinator : MonoBehaviour
{
    #region Class Variables

    // the data for the arena set the player is currently on
    private LevelArenaSetData arenaSetData = null;
    // the max number of arena sets that exist
    private int totalNumberOfArenaSets = 1;

    // the player's current arena within their current arena set
    private int arenaNumInSet = 1;
    /// the IDs od the arenas that have already been used in the current set. 
    /// This needs to be tracked so that the same room is not used in the same 
    /// set in current run.
    private List<string> usedArenaIdsInSet = new List<string>();

    // the current arena object that the player is on
    private LevelArenaController createdArena = null;

    #endregion




    #region Level Functions

    /// <summary>
    /// Returns a random arena prefab that has NOT been used in this set yet.
    /// </summary>
    /// <returns></returns>
    private LevelArenaController GetAppropriateRandomArenaPrefab()
    {
        // load all arena prefab object for current arena set
        GameObject[] loadedArenaPrefabObjs = AssetRefMethods.
            LoadAllBundleAssetLevelArenaPrefab(arenaSetData.setNum);

        // if already used all of the arenas that were loaded
        if (usedArenaIdsInSet.Count > loadedArenaPrefabObjs.Length)
        {
            // print warning to console
            Debug.LogWarning("No arenas that haven't already been used!");

            // return some default value
            return null;
        }

        // initialize list of arena prefabs that have not been used yet as empty list
        List<LevelArenaController> potentialArenaPrefab = new List<LevelArenaController>();

        // loop through all loaded arena objects
        foreach (GameObject iterObj in loadedArenaPrefabObjs)
        {
            // if iterating arena object has NOT been used in this set yet
            if (!usedArenaIdsInSet.Contains(GetArenaIdFromObject(iterObj)))
            {
                // add the iterating object's arena component to list of potentials
                potentialArenaPrefab.Add(iterObj.GetComponent<LevelArenaController>());
            }
        }

        // get a random index in potentials list
        int randomPrefabListIndex = Random.Range(0, potentialArenaPrefab.Count);

        // return a random arena prefab by using the random index
        return potentialArenaPrefab[randomPrefabListIndex];
    }

    /// <summary>
    /// Returns the arena object ID based on the given arena object.
    /// </summary>
    /// <param name="objArg"></param>
    /// <returns></returns>
    private string GetArenaIdFromObject(GameObject objArg)
    {
        // initialize beginning format of arena object's name
        string ARENA_OBJ_NAME_PREFIX = "arena-";

        // return rest of object's name which should hold the arena ID
        return objArg.name.Substring(ARENA_OBJ_NAME_PREFIX.Length);
    }

    /// <summary>
    /// Instantiates a random arena into the game world.
    /// </summary>
    private void CreateArena()
    {
        // destroys the currenly instantiated arena
        DestoryArena();

        // get a random arena prefab
        LevelArenaController randomArenaPrefab = GetAppropriateRandomArenaPrefab();

        // denote that arena was used in this set
        usedArenaIdsInSet.Add(GetArenaIdFromObject(randomArenaPrefab.gameObject));

        // create arena object from prefab
        GameObject createdArenaObj = Instantiate(randomArenaPrefab.gameObject);
        // save reference to created object's arena component
        createdArena = createdArenaObj.GetComponent<LevelArenaController>();
    }

    /// <summary>
    /// Destroys the currenly instantiated arena.
    /// </summary>
    public void DestoryArena()
    {
        // if a created arena currently exists
        if (createdArena != null)
        {
            // destory current arena
            Destroy(createdArena);
        }
    }

    /// <summary>
    /// Begins a fresh new arena run.
    /// </summary>
    public void StartNewRun()
    {
        // resets the arena set values that are related to starting a new run
        ResetArenaSetPropertiesNewRun();

        // resets the arena set values that are related to starting a new arena set
        ResetArenaSetPropertiesNewSet();

        // instantiates a random arena into the game world
        CreateArena();
    }

    /// <summary>
    /// Progresses to the next level arena based on where the player currenly is within the run.
    /// </summary>
    private void ProgressToNextArena()
    {
        // increment the arena num
        arenaNumInSet++;

        // if current arena set has been completed
        if (arenaNumInSet > arenaSetData.totalArenasInSet)
        {
            // resets the arena set values that are related to starting a new arena set
            ResetArenaSetPropertiesNewSet();

            Debug.Log("NEED IMPL: Trigger event to denote arena set completed."); // NEED IMPL!!!

            // get the next arena set number
            int nextSetNum = arenaSetData.setNum + 1;

            // if all arena sets have been completed
            if (nextSetNum > totalNumberOfArenaSets)
            {
                Debug.Log("NEED IMPL: Trigger event to denote all arena sets have been completed."); // NEED IMPL!!!

                // DONT continue code
                return;
            }
            // else still some arena sets left
            else
            {
                // set arena set data to next arena set
                SetupArenaSetData(nextSetNum);
            }
        }

        // instantiates a random arena into the game world
        CreateArena();
    }

    #endregion




    #region Set Properties Functions

    /// <summary>
    /// Sets the arena set data based on the given set number.
    /// </summary>
    /// <param name="arenaSetNumArg"></param>
    private void SetupArenaSetData(int arenaSetNumArg)
    {
        // get set template related to given set number
        LevelArenaSetDataTemplate startingSetTemp = AssetRefMethods.
            LoadBundleAssetLevelArenaSetDataTemplate(arenaSetNumArg);

        // set the set data from the template
        arenaSetData = new LevelArenaSetData(startingSetTemp);
    }

    /// <summary>
    /// Resets the arena set values that are related to starting a new run.
    /// </summary>
    private void ResetArenaSetPropertiesNewRun()
    {
        // set arena set data to first arena set
        SetupArenaSetData(1);

        // set total num of arena sets to how many set templates exist
        totalNumberOfArenaSets = AssetRefMethods.
            LoadAllBundleAssetLevelArenaSetDataTemplate().Length;
    }

    /// <summary>
    /// Resets the arena set values that are related to starting a new arena set.
    /// </summary>
    private void ResetArenaSetPropertiesNewSet()
    {
        arenaNumInSet = 1;

        usedArenaIdsInSet = new List<string>();
    }

    #endregion


}
