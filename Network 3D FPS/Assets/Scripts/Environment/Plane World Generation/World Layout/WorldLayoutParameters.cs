using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Layout Parameters", menuName = "Scriptable Objects/Environment/World Layout Parameters")]
public class WorldLayoutParameters : ScriptableObject
{
    #region Class Variables

    public int worldLayoutDimensionsSize;

    public List<WorldLayoutBiome> worldLayoutBiomes;

    public List<WorldLayoutPotentialChunk> mandatoryWorldLayoutChunks;

    #endregion




    #region World Parameter Functions

    /// <summary>
    /// Returns the biome type for the chunk that the given coordinates reside in. 
    /// Returns None biome type if notin any of the layout biomes.
    /// </summary>
    /// <param name="coordXArg"></param>
    /// <param name="coordZArg"></param>
    /// <returns></returns>
    public WorldBiomeType GetBiomeTypeFromCoord(int coordXArg, int coordZArg)
    {
        //loop through each layout biome
        foreach (WorldLayoutBiome iterLayoutBiome in worldLayoutBiomes)
        {
            //if coord is within the iterating layout biome area
            if (coordXArg >= iterLayoutBiome.biomeStartCoordinate.x && 
                coordXArg <= iterLayoutBiome.biomeEndCoordinate.x && 
                coordZArg >= iterLayoutBiome.biomeStartCoordinate.y && 
                coordZArg <= iterLayoutBiome.biomeEndCoordinate.y)
            {
                //return the iterating layout biome's type
                return iterLayoutBiome.worldBiomeType;
            }
        }

        //given coord not in any biome type, so return the null biome type
        return WorldBiomeType.None;
    }

    #endregion


}
