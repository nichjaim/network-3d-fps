using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldLayoutPotentialChunk
{
    public WorldChunkData worldChunkData;

    [Header("Coordinate Properties")]

    public Vector2 potentialStartCoordinate;
    public Vector2 potentialEndCoordinate;
}
