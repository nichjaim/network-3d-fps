using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldChunkData
{
    #region Class Variables

    public bool isUniqueChunk;
    public WorldBiomeType worldBiomeType;
    public string chunkId;
    public QuadDirectionType quadDirectionType;

    #endregion




    #region Constructor Functions

    public WorldChunkData()
    {
        isUniqueChunk = false;
        worldBiomeType = WorldBiomeType.None;
        chunkId = string.Empty;
        quadDirectionType = QuadDirectionType.Front;
    }

    public WorldChunkData(bool isUniqueChunkArg, WorldBiomeType worldBiomeTypeArg, 
        string chunkIdArg, QuadDirectionType quadDirectionTypeArg)
    {
        isUniqueChunk = isUniqueChunkArg;
        worldBiomeType = worldBiomeTypeArg;
        chunkId = chunkIdArg;
        quadDirectionType = quadDirectionTypeArg;
    }

    public WorldChunkData(WorldChunkData templateArg)
    {
        isUniqueChunk = templateArg.isUniqueChunk;
        worldBiomeType = templateArg.worldBiomeType;
        chunkId = templateArg.chunkId;
        quadDirectionType = templateArg.quadDirectionType;
    }

    #endregion


}
