using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArenaCoordinator : MonoBehaviour
{
    #region Class Variables

    // the data for the arena set the player is currently on
    private LevelArenaSetData arenaSetData = null;
    public LevelArenaSetData ArenaSetData
    {
        get { return arenaSetData; }
    }

    [Tooltip("The object that the instantiated arenas will be parented under.")]
    [SerializeField]
    private Transform arenaParentObject = null;

    [Tooltip("The arena set data for the game's tutorial section.")]
    [SerializeField]
    private LevelArenaSetDataTemplate tutorialArenaSet = null;

    // the max number of arena sets that exist
    private int totalNumberOfArenaSets = 1;

    // the player's current arena within their current arena set
    private int arenaNumInSet = 1;
    public int ArenaNumInSet
    {
        get { return arenaNumInSet; }
    }

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
    /// Returns the tutorial arena prefab associated with curren arena number in set.
    /// </summary>
    /// <returns></returns>
    private LevelArenaController GetAppropriateTutorialArenaPrefab()
    {
        // get tutorial arena prefab object
        GameObject arenaPrefab = AssetRefMethods.
            LoadBundleAssetTutorialLevelArenaPrefab(arenaNumInSet);

        // return prefab's arena component
        return arenaPrefab.GetComponent<LevelArenaController>();
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
    /// Resets all arena run's progress data to new run.
    /// </summary>
    public void ResetRunProgress()
    {
        // resets the arena set values that are related to starting a new run
        ResetArenaSetPropertiesNewRun();

        // resets the arena set values that are related to starting a new arena set
        ResetArenaSetPropertiesNewSet();
    }

    /// <summary>
    /// Progresses to the next level arena based on where the player currenly is within the run.
    /// </summary>
    public void AdvanceArenaProgress()
    {
        // increment the arena num
        arenaNumInSet++;

        // if current arena set has been completed
        if (arenaNumInSet > arenaSetData.totalArenasInSet)
        {
            // resets the arena set values that are related to starting a new arena set
            ResetArenaSetPropertiesNewSet();

            // get the next arena set number
            int nextSetNum = arenaSetData.setNum + 1;

            // set arena set data to next arena set
            SetupArenaSetData(nextSetNum);
        }
    }

    /// <summary>
    /// Returns whether all arenas have been completed.
    /// </summary>
    /// <returns></returns>
    public bool AreAllArenasCompleted()
    {
        return arenaSetData.setNum == GetEndArenaSetData().setNum;
    }

    /// <summary>
    /// Returns whether starting a new arena set.
    /// </summary>
    /// <returns></returns>
    public bool AreStartingNewSet()
    {
        return arenaSetData.setNum == 1;
    }

    #endregion




    #region Arena Creation Functions

    /// <summary>
    /// Instantiates the appropriate arena prefab into the game world.
    /// </summary>
    public void CreateAppropriateArena()
    {
        // if in the tutorial arena set
        if (arenaSetData.setNum == GetTutorialArenaSetData().setNum)
        {
            // instantiate the appropriate tutorial arena prefab into the game world
            CreateTutorialArena();
        }
        // else if all the arena sets are completed
        else if (arenaSetData.setNum == GetEndArenaSetData().setNum)
        {
            // print warning to console
            Debug.LogWarning("Trying to create an arena when all arena sets are finished!");
        }
        // else in a normal arena set
        else
        {
            // instantiate a random arena prefab into the game world
            CreateRandomArena();
        }
    }

    /// <summary>
    /// Instantiates the appropriate tutorial arena prefab into the game world.
    /// </summary>
    private void CreateTutorialArena()
    {
        CreateArena(GetAppropriateTutorialArenaPrefab());
    }

    /// <summary>
    /// Instantiates a random arena prefab into the game world.
    /// </summary>
    private void CreateRandomArena()
    {
        CreateArena(GetAppropriateRandomArenaPrefab());
    }

    /// <summary>
    /// Instantiates the given arena prefab into the game world.
    /// </summary>
    /// <param name="prefabArenaArg"></param>
    private void CreateArena(LevelArenaController prefabArenaArg)
    {
        // destroys the currently instantiated arena
        DestoryArena();

        // denote that arena was used in this set
        usedArenaIdsInSet.Add(GetArenaIdFromObject(prefabArenaArg.gameObject));

        // create arena object from prefab and parent to this coordinator
        GameObject createdArenaObj = Instantiate(prefabArenaArg.gameObject, arenaParentObject);
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
            // destory current arena's gameobject (IMPORTANT to destroy the gameobject and not just the component)
            Destroy(createdArena.gameObject);
        }

        /// loop through all the arena objects and destory them to ensure there are 
        /// NO extra arenas hanging around in scene
        foreach (Transform iterChild in arenaParentObject)
        {
            Destroy(iterChild.gameObject);
        }
    }

    #endregion




    #region Arena Set Properties Functions

    /// <summary>
    /// Sets the arena set data based on the given set number.
    /// </summary>
    /// <param name="arenaSetNumArg"></param>
    private void SetupArenaSetData(int arenaSetNumArg)
    {
        // else if all arena sets have been completed
        if (arenaSetNumArg > totalNumberOfArenaSets)
        {
            // set arena set to an invalid arena set to denote that no next arena
            arenaSetData = GetEndArenaSetData();
        }
        // else still some arena sets left
        else
        {
            // get set template related to given set number
            LevelArenaSetDataTemplate startingSetTemp = AssetRefMethods.
                LoadBundleAssetLevelArenaSetDataTemplate(arenaSetNumArg);

            // set the set data from the template
            arenaSetData = new LevelArenaSetData(startingSetTemp);
        }
    }

    /// <summary>
    /// Sets the arena data to the tutorial start.
    /// </summary>
    public void SetupTutorialArenaData()
    {
        ResetRunProgress();

        arenaSetData = GetTutorialArenaSetData();
    }

    /// <summary>
    /// Returns arena set data associated to tutorial.
    /// </summary>
    /// <returns></returns>
    private LevelArenaSetData GetTutorialArenaSetData()
    {
        return new LevelArenaSetData(tutorialArenaSet);
    }

    /// <summary>
    /// Returns an invalid arena set. 
    /// Used for denoting when there will be no next arena.
    /// </summary>
    /// <returns></returns>
    private LevelArenaSetData GetEndArenaSetData()
    {
        LevelArenaSetData returnArenaSetData = new LevelArenaSetData();

        returnArenaSetData.setNum = totalNumberOfArenaSets + 1;

        return returnArenaSetData;
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
