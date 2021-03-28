using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlaneWorldGenerator : MonoBehaviour
{
    #region Class Variables

    private static int WORLD_CHUNK_SIZE = 64;

    private WorldChunkData[][] generatedWorldLayout = null;

    private WorldChunkController[][] generatedWorld = null;
    public WorldChunkController[][] GeneratedWorld
    {
        get { return generatedWorld; }
    }


    [SerializeField]
    private GameObject worldBoundaryPrefab = null;

    private GameObject worldBoundaryUp = null;
    private GameObject worldBoundaryDown = null;
    private GameObject worldBoundaryLeft = null;
    private GameObject worldBoundaryRight = null;

    [SerializeField]
    private NavMeshSurface navMeshSurface = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the world boundary objects
        InitializeWorldBoundaries();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all the world boundary objects. 
    /// Call in Start().
    /// </summary>
    private void InitializeWorldBoundaries()
    {
        // instantiate all world boundary objects
        worldBoundaryUp = Instantiate(worldBoundaryPrefab, Vector3.zero, Quaternion.identity, transform);
        worldBoundaryDown = Instantiate(worldBoundaryPrefab, Vector3.zero, Quaternion.identity, transform);
        worldBoundaryLeft = Instantiate(worldBoundaryPrefab, Vector3.zero, Quaternion.identity, transform);
        worldBoundaryRight = Instantiate(worldBoundaryPrefab, Vector3.zero, Quaternion.identity, transform);

        // set boundary roatations
        worldBoundaryUp.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        worldBoundaryDown.transform.eulerAngles = new Vector3(-90f, 0f, 180f);
        worldBoundaryLeft.transform.eulerAngles = new Vector3(-90f, 0f, -90f);
        worldBoundaryRight.transform.eulerAngles = new Vector3(-90f, 0f, 90f);

        // name the world boundary objects accordingly
        worldBoundaryUp.gameObject.name = "World Boundary Up";
        worldBoundaryDown.gameObject.name = "World Boundary Down";
        worldBoundaryLeft.gameObject.name = "World Boundary Left";
        worldBoundaryRight.gameObject.name = "World Boundary Right";
    }

    #endregion




    #region World Generation Functions

    /// <summary>
    /// Complete process for generating a game world from given parameters.
    /// </summary>
    /// <param name="worldLayoutParametersArg"></param>
    public void WorldGeneration(WorldLayoutParameters worldLayoutParametersArg)
    {
        // create a world layout from given parameters
        GenerateWorldLayout(worldLayoutParametersArg);

        // generate the actual world from the created layout
        GenerateWorld(generatedWorldLayout);
    }

    /// <summary>
    /// Setup the world layout to use based on given world parameters.
    /// </summary>
    /// <param name="worldLayoutParametersArg"></param>
    private void GenerateWorldLayout(WorldLayoutParameters worldLayoutParametersArg)
    {
        //initialize the returning world layout as a blank world layout
        generatedWorldLayout = GetBlankWorldLayout(worldLayoutParametersArg.worldLayoutDimensionsSize);

        //setup all needed world chunks in the generated world layout
        SetupMandatoryWorldChunks(worldLayoutParametersArg);

        //initialize var for upcoming loop
        WorldBiomeType biomeType;

        //loop through all layout rows
        for (int i = 0; i < generatedWorldLayout.Length; i++)
        {
            //loop through iterating row's layout columns
            for (int y = 0; y < generatedWorldLayout[i].Length; y++)
            {
                //if iterating chunk spot has not been filled
                if (generatedWorldLayout[i][y] == null)
                {
                    //get the biome that the iterating chunk spot is in
                    biomeType = worldLayoutParametersArg.GetBiomeTypeFromCoord(i + 1, y + 1);
                    //if iterating chunk spot is NOT in any known biome
                    if (biomeType == WorldBiomeType.None)
                    {
                        //set biome to some default biome type
                        biomeType = WorldBiomeType.Forest;
                    }

                    /// set world layout's iterating non-unique chunk spot to the randomly chosen chunk within 
                    /// the appropriate biome
                    generatedWorldLayout[i][y] = new WorldChunkData(false, biomeType,
                        GetRandomWorldChunkId(biomeType), GeneralMethods.GetRandomQuadDirectionType());
                }
            }
        }
    }

    /// <summary>
    /// Returns a random world chunk ID based on their spawn chance.
    /// </summary>
    /// <param name="biomeArg"></param>
    /// <returns></returns>
    private string GetRandomWorldChunkId(WorldBiomeType biomeArg)
    {
        // load all world chunk prefabs from the given biome
        GameObject[] loadedChunkPrefabs = AssetRefMethods.LoadAllBundleAssetWorldChunk(biomeArg);

        // initialize these vars for the upcoming loop
        List<(float, int)> tupleListChanceRateToChunkId = new List<(float, int)>();
        WorldChunkController chunkComp;
        // loop through all loaded world chunk prefabs
        for (int i = 0; i < loadedChunkPrefabs.Length; i++)
        {
            // get world chunk component from iterating prefab object
            chunkComp = loadedChunkPrefabs[i].GetComponent<WorldChunkController>();
            // if iterating object DID have a world chunk component
            if (chunkComp != null)
            {
                // add the iterating chunk's spawn chance and ID to tuple
                tupleListChanceRateToChunkId.Add((chunkComp.ChunkSpawnChance, (i + 1)));
            }
            // else iterating object did NOT have a world chunk component
            else
            {
                // print warning to console
                Debug.LogWarning("World chunk prefab object did NOT have a world chunk component! " +
                    "Named: " + loadedChunkPrefabs[i].name);
            }
        }

        // get a random chunk ID from the chunk tuple based on the spawn chances
        int randomChunkId = GeneralMethods.GetRandomEntryFromChanceToValueCouple<int>(
            RollChancePickType.TargetRarest, tupleListChanceRateToChunkId);

        // return the ID in it's proper data type
        return randomChunkId.ToString();
    }

    /// <summary>
    /// Sets up all needed world chunks in the generated world layout in a random position confined 
    /// to their potential layout spots.
    /// </summary>
    /// <param name="worldLayoutParametersArg"></param>
    private void SetupMandatoryWorldChunks(WorldLayoutParameters worldLayoutParametersArg)
    {
        //initialize vars for upcoming loop
        List<Vector2> potentialChunkPositions;
        int randomPosIndex;
        Vector2 chosenChunkPositionIndices;

        //loop through all world chunks that need to be setup
        foreach (WorldLayoutPotentialChunk iterWorldChunk in worldLayoutParametersArg.mandatoryWorldLayoutChunks)
        {
            //get all layout chunk coordinates that are still vacant within the world layout
            potentialChunkPositions = GetAllViableChunkSpots(iterWorldChunk.potentialStartCoordinate, 
                iterWorldChunk.potentialEndCoordinate);

            //if there is at least one potential spot
            if (potentialChunkPositions.Count > 0)
            {
                //get random index from list of positions
                randomPosIndex = Random.Range(0, potentialChunkPositions.Count);
                //get the chosen layout position's indices
                chosenChunkPositionIndices = new Vector2(potentialChunkPositions[randomPosIndex].x - 1, 
                    potentialChunkPositions[randomPosIndex].y - 1);
                //setup the iterating world chunk on the chosen position in the generated world layout
                generatedWorldLayout[(int)chosenChunkPositionIndices.x][(int)chosenChunkPositionIndices.y] = 
                    new WorldChunkData(iterWorldChunk.worldChunkData);
            }
        }
    }

    /// <summary>
    /// Returns list of chunk spots within given cooordinate's area that is still vacant in the generated layout.
    /// </summary>
    /// <param name="startCoordinateArg"></param>
    /// <param name="endCoordinateArg"></param>
    /// <returns></returns>
    private List<Vector2> GetAllViableChunkSpots(Vector2 startCoordinateArg, Vector2 endCoordinateArg)
    {
        //initialize return var as empty
        List<Vector2> potentialChunkPositions = new List<Vector2>();
        //get all world layout chunk spots within the coordinate area
        List<Vector2> allChunkPositions = GeneralMethods.GetAllCoordinatesWithinCoordinateArea(startCoordinateArg, 
            endCoordinateArg);

        //loop through all world layout chunk spots within the coordinate area
        foreach (Vector2 iteratingPos in allChunkPositions)
        {
            //if the iterating chunk spot is not already taken in the world layout
            if (generatedWorldLayout[(int)(iteratingPos.x - 1)][(int)(iteratingPos.y - 1)] == null)
            {
                //add the iterating chunk spot to list of viable chunk spots
                potentialChunkPositions.Add(new Vector2(iteratingPos.x, iteratingPos.y));
            }
        }

        //return populated list
        return potentialChunkPositions;
    }

    /// <summary>
    /// Instantiate map world based on given world layout.
    /// </summary>
    /// <param name="worldLayoutArg"></param>
    private void GenerateWorld(WorldChunkData[][] worldLayoutArg)
    {
        // destory all generated world chunk objects
        DestroyGeneratedWorld();
        // reset the generated world reference to an empty world
        generatedWorld = GetBlankGeneratedWorld(worldLayoutArg.Length);

        // initialize var for upcoming loop
        GameObject worldChunkPrefab;
        Vector3 worldChunkPosition;
        GameObject generatedWorldChunkObject;
        WorldChunkData chunkData;

        // loop through all layout rows
        for (int i = 0; i < worldLayoutArg.Length; i++)
        {
            //loop through iterating row's layout columns
            for (int y = 0; y < worldLayoutArg[i].Length; y++)
            {
                //get the iterating world chunk's data
                chunkData = worldLayoutArg[i][y];
                //load prefab object from files based on world chunk ID
                worldChunkPrefab = AssetRefMethods.LoadBundleAssetWorldChunk(chunkData.isUniqueChunk, chunkData.worldBiomeType, 
                    chunkData.chunkId);
                //get world chunk's appropriate world position
                worldChunkPosition = new Vector3((i * WORLD_CHUNK_SIZE), 0f, (y * WORLD_CHUNK_SIZE));
                //instantiate world chunk prefab object
                generatedWorldChunkObject = Instantiate(worldChunkPrefab, worldChunkPosition, Quaternion.identity, transform);
                // set created world chunk's rotation based on the chunk data's quad direction property
                generatedWorldChunkObject.transform.eulerAngles = new Vector3(0f, 
                    GeneralMethods.GetYRotationValueFromQuadDirection(chunkData.quadDirectionType), 0f);
                // set the created world chunk object's name appropriately based on the chunk data and world positioning
                generatedWorldChunkObject.name = GetChunkObjectName(chunkData, new Vector2(i + 1, y + 1));
                //set iterating generated world coordinates to the instantiated world chunk
                generatedWorld[i][y] = generatedWorldChunkObject.GetComponent<WorldChunkController>();

                // if instantiated world chunk prefab did NOT have the appropriate controller component attached
                if (generatedWorld[i][y] == null)
                {
                    //print warning to console
                    Debug.LogWarning(worldChunkPrefab.name + " does NOT have a WorldChunkController component!");
                }
                // else world chunk did have appropriate controller component attached
                else
                {
                    // chooses an object for all potential object sets within the world chunk
                    //generatedWorld[i][y].SetupPotentialObjectSets();
                    // instantiates and sets up any structure interior for all structure exteriors within the world chunk
                    generatedWorld[i][y].SetupStructures();
                    //RefreshObjectRenderingMaterials(generatedWorld[i][y].gameObject, true); //TEST LINE!!!!!!!
                }
            }
        }

        // sets the position and scale of the world boundaries appropriately
        RefreshWorldBoundary(worldLayoutArg.Length, worldLayoutArg[0].Length);

        // remove any current navigation mesh
        navMeshSurface.RemoveData();
        // build navigation mesh for generated world
        navMeshSurface.BuildNavMesh();
    }

    /// <summary>
    /// Destorys all generated world chunk objects. 
    /// Call before generating a new world.
    /// </summary>
    private void DestroyGeneratedWorld()
    {
        // if a generated world is NOT setup
        if (generatedWorld == null)
        {
            // DONT continue code
            return;
        }

        // loop through all generated world chunk columns
        for (int i = 0; i < generatedWorld.Length; i++)
        {
            // loop through all rows of iterating generated world chunk columns
            for (int y = 0; y < generatedWorld[i].Length; y++)
            {
                // destory iterating world chunk object
                Destroy(generatedWorld[i][y].gameObject);
            }
        }
    }

    /// <summary>
    /// Returns a world layout with null values for each world chunk coordinate.
    /// </summary>
    /// <param name="worldLayoutSizeArg"></param>
    /// <returns></returns>
    private WorldChunkData[][] GetBlankWorldLayout(int worldLayoutSizeArg)
    {
        //initialize world layout rows
        WorldChunkData[][] blankWorldLayout = new WorldChunkData[worldLayoutSizeArg][];

        //loop through rows
        for (int i = 0; i < worldLayoutSizeArg; i++)
        {
            //initialize iterating row's columns
            blankWorldLayout[i] = new WorldChunkData[worldLayoutSizeArg];
        }

        return blankWorldLayout;
    }

    /// <summary>
    /// Returns a world listing with null values for each world chunk coordinate.
    /// </summary>
    /// <param name="worldLayoutSizeArg"></param>
    /// <returns></returns>
    private WorldChunkController[][] GetBlankGeneratedWorld(int worldLayoutSizeArg)
    {
        //initialize world layout rows
        WorldChunkController[][] blankWorldLayout = new WorldChunkController[worldLayoutSizeArg][];

        //loop through rows
        for (int i = 0; i < worldLayoutSizeArg; i++)
        {
            //initialize iterating row's columns
            blankWorldLayout[i] = new WorldChunkController[worldLayoutSizeArg];
        }

        return blankWorldLayout;
    }

    /// <summary>
    /// Returns a world chunk gameobject name based on the given chunk info.
    /// </summary>
    /// <param name="chunkDataArg"></param>
    /// <param name="worldCoordsArg"></param>
    /// <returns></returns>
    private string GetChunkObjectName(WorldChunkData chunkDataArg, Vector2 worldCoordsArg)
    {
        // initialize the return object name with the base name prefix
        string chunkObjectName = "WorldChunk";

        // if the given chunk is a unique world chunk
        if (chunkDataArg.isUniqueChunk)
        {
            // add the unique name prefix
            chunkObjectName += "-Unique";
        }

        // add the chunk's biome to the name
        chunkObjectName += ("-" + chunkDataArg.worldBiomeType.ToString());

        // add the chunk's ID to the name
        chunkObjectName += ("-" + chunkDataArg.chunkId);

        // add the chunk's world coordinates to the name
        chunkObjectName += ("_" + worldCoordsArg.x + "x" + worldCoordsArg.y);

        // return the fully setup name
        return chunkObjectName;
    }

    #endregion




    #region World Boundary Functions

    /// <summary>
    /// Sets the position and scale of the world boundaries appropriately.
    /// </summary>
    /// <param name="worldDimensionsXArg"></param>
    /// <param name="worldDimensionsZArg"></param>
    private void RefreshWorldBoundary(int worldDimensionsXArg, int worldDimensionsZArg)
    {
        RefreshWorldBoundaryPositions(worldDimensionsXArg, worldDimensionsZArg);
        RefreshWorldBoundaryScales(worldDimensionsXArg, worldDimensionsZArg);
    }

    /// <summary>
    /// Sets the position of the world boundaries appropriately.
    /// </summary>
    /// <param name="worldDimensionsXArg"></param>
    /// <param name="worldDimensionsZArg"></param>
    private void RefreshWorldBoundaryPositions(int worldDimensionsXArg, int worldDimensionsZArg)
    {
        // initialize useful world boundary values
        float mapCenterX = (((float)(worldDimensionsXArg - 1) / 2f) * (float)WORLD_CHUNK_SIZE);
        float mapCenterZ = (((float)(worldDimensionsZArg - 1) / 2f) * (float)WORLD_CHUNK_SIZE);
        float WORLD_CHUNK_HALF = ((float)WORLD_CHUNK_SIZE / 2f);

        // position upper boundary
        worldBoundaryUp.transform.position = new Vector3(mapCenterX, 0f, (mapCenterZ * 2f) + WORLD_CHUNK_HALF);
        // position lower boundary
        worldBoundaryDown.transform.position = new Vector3(mapCenterX, 0f, -WORLD_CHUNK_HALF);
        // position left boundary
        worldBoundaryLeft.transform.position = new Vector3(-WORLD_CHUNK_HALF, 0f, mapCenterZ);
        // position right boundary
        worldBoundaryRight.transform.position = new Vector3((mapCenterX * 2f) + WORLD_CHUNK_HALF, 0f, mapCenterZ);
    }

    /// <summary>
    /// Sets the scale of the world boundaries appropriately.
    /// </summary>
    /// <param name="worldDimensionsXArg"></param>
    /// <param name="worldDimensionsZArg"></param>
    private void RefreshWorldBoundaryScales(int worldDimensionsXArg, int worldDimensionsZArg)
    {
        // initialize the scaling multiplier
        float WORLD_BOUNDARY_SCALE_MULTIPLIER = (float)WORLD_CHUNK_SIZE / 10f;
        // get the scale values for the boundaries of both axis
        float boundaryScaleX = (float)worldDimensionsXArg * WORLD_BOUNDARY_SCALE_MULTIPLIER;
        float boundaryScaleZ = (float)worldDimensionsZArg * WORLD_BOUNDARY_SCALE_MULTIPLIER;

        // set the scales of the boundary objects
        worldBoundaryUp.transform.localScale = new Vector3(boundaryScaleX, 1f, boundaryScaleX);
        worldBoundaryDown.transform.localScale = new Vector3(boundaryScaleX, 1f, boundaryScaleX);
        worldBoundaryLeft.transform.localScale = new Vector3(boundaryScaleZ, 1f, boundaryScaleZ);
        worldBoundaryRight.transform.localScale = new Vector3(boundaryScaleZ, 1f, boundaryScaleZ);
    }

    #endregion


}
