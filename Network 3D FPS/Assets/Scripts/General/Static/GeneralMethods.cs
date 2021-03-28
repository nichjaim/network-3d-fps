﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;

public static class GeneralMethods
{
    /// <summary>
    /// Returns the color represented by the given hex color code.
    /// </summary>
    /// <param name="hexColorCodeArg"></param>
    /// <returns></returns>
    public static Color GetColorFromHexColorCode(string hexColorCodeArg)
    {
        // put argument into temp variable
        string tempHexCode = hexColorCodeArg;

        // if the given hex color code does NOT start with a hashtag
        if (tempHexCode[0] != '#')
        {
            // add a hashtag to the start of the code
            tempHexCode = "#" + tempHexCode;
        }

        // initialize returning color
        Color returnColor;
        // put the representative color into the returning variable
        ColorUtility.TryParseHtmlString(tempHexCode, out returnColor);
        // return the color
        return returnColor;
    }

    /// <summary>
    /// Returns bool that denotes if the given player is online but NOT associated 
    /// with the machine running this.
    /// NOTE: In current Mirror build, this method will only give correct bool if 
    /// called in Start() or later.
    /// </summary>
    /// <param name="networkIdArg"></param>
    /// <returns></returns>
    public static bool IsNetworkConnectedButNotLocalClient(NetworkIdentity 
        networkIdArg)
    {
        return networkIdArg.netId != 0 && !networkIdArg.isLocalPlayer;
    }

    public static Sprite GetSpriteFromFilePath(string filePathArg)
    {
        // converts desired path into byte array
        byte[] pngBytes = File.ReadAllBytes(filePathArg);

        // create new texture for upcoming image
        Texture2D tex = new Texture2D(2, 2);
        // load byte array to create image
        tex.LoadImage(pngBytes);

        // creates a new Sprite based on the Texture2D
        Sprite texSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        // return created sprite
        return texSprite;
    }

    /// <summary>
    /// Returns extension for PNG files.
    /// </summary>
    /// <returns></returns>
    public static string GetPngFileExtension()
    {
        return ".png";
    }

    /// <summary>
    /// Swaps the content of the given list indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listArg"></param>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    public static void ListIndexSwap<T>(List<T> listArg, int indexA, int indexB)
    {
        T tmp = listArg[indexA];
        listArg[indexA] = listArg[indexB];
        listArg[indexB] = tmp;
    }

    /// <summary>
    /// Returns the user's local IP address.
    /// </summary>
    /// <returns></returns>
    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    /// <summary>
    /// Returns random entry from list. 
    /// Returns empty string if no entrys in given list.
    /// </summary>
    /// <param name="listArg"></param>
    /// <returns></returns>
    public static string GetRandomStringListEntry(List<string> listArg)
    {
        // if given list has at least one entry
        if (listArg.Count > 0)
        {
            // get a random index from list
            int randomIndex = Random.Range(0, listArg.Count);
            // use the random index to return random list entry
            return listArg[randomIndex];
        }
        // else given list is empty
        else
        {
            // return a default empty string
            return string.Empty;
        }
    }

    /// <summary>
    /// Returns random entry from list of given couples based on the associated percent chances.
    /// </summary>
    /// <returns></returns>
    public static T GetRandomEntryFromChanceToValueCouple<T>(RollChancePickType rollChancePickTypeArg, List<(float, T)> entriesArg)
    {
        //get random float between 0 and 1 (1 representing 100% chance). This acts as drop/spawn roll.
        float randomRollNumber = Random.Range(0f, 1f);

        //intitalizes list of entries whose correpsonding chance passed the roll
        List<(float, T)> entriesThatPassedRoll = new List<(float, T)>();

        //loop through each of the given entries
        foreach ((float, T) iteratingEntry in entriesArg)
        {
            //if the iterating table entry passed the roll
            if (randomRollNumber <= iteratingEntry.Item1)
            {
                //add the iterating entry's to the list of entries that passed the roll
                entriesThatPassedRoll.Add(iteratingEntry);
            }
        }

        //if SOME entries passed the roll
        if (entriesThatPassedRoll.Count > 0)
        {
            //check cases based on roll type
            switch (rollChancePickTypeArg)
            {
                //case where the RNG pick type should have no bias
                case RollChancePickType.NoBias:
                    //get a random index of the list of entries that passed the roll
                    int randomPassedRollListIndex = Random.Range(0, entriesThatPassedRoll.Count);
                    //return the value of the randomly chosen entry that passed the roll
                    return entriesThatPassedRoll[randomPassedRollListIndex].Item2;

                //case where the RNG pick type should choose the rarest of the list of passed roll entries
                case RollChancePickType.TargetRarest:
                    ///initialize the lowest passed percent chance entry with the first in entry list that passed roll, 
                    ///it's a list because we need to kepp all the lowest entries that have the same chance
                    List<(float, T)> lowestPassedPercentChanceEntries = new List<(float, T)>();
                    lowestPassedPercentChanceEntries.Add(entriesThatPassedRoll[0]);
                    //loop through all entries that passed the roll
                    foreach ((float, T) iteratingEntry in entriesThatPassedRoll)
                    {
                        //if the iterating entry's chance is the lowest percent chance entry seen so far
                        if (iteratingEntry.Item1 < lowestPassedPercentChanceEntries[0].Item1)
                        {
                            lowestPassedPercentChanceEntries = new List<(float, T)>();
                            //set the iterating entry as the currentl lowest percent chance entry
                            lowestPassedPercentChanceEntries.Add(iteratingEntry);
                        }
                        //else if the iterating entry's chance is the same as the current lowest percent chance entry seen
                        else if (iteratingEntry.Item1 == lowestPassedPercentChanceEntries[0].Item1)
                        {
                            lowestPassedPercentChanceEntries.Add(iteratingEntry);
                        }
                    }

                    //get a random entry index from the lowest chance entries
                    //int randomLowestEntryIndex = Random.Range(0, lowestPassedPercentChanceEntries.Count - 1);
                    int randomLowestEntryIndex = Random.Range(0, lowestPassedPercentChanceEntries.Count);
                    //return value of the randomly retrieved entry within the lowest percent chance entries
                    return lowestPassedPercentChanceEntries[randomLowestEntryIndex].Item2;

                //case where no other case was matched
                default:
                    //print warning to console
                    Debug.LogWarning("Unknown roll chance type: " + rollChancePickTypeArg.ToString());
                    //return default NULL value
                    return default(T);
            }
        }
        //else NO entries passed the roll
        else
        {
            //return default NULL value
            return default(T);
        }
    }

    /// <summary>
    /// Returns a random quad direction enum.
    /// </summary>
    /// <returns></returns>
    public static QuadDirectionType GetRandomQuadDirectionType()
    {
        /*Array values = Enum.GetValues(typeof(QuadDirectionType));
        System.Random random = new System.Random();
        QuadDirectionType randomEnum = (QuadDirectionType)values.GetValue(random.Next(values.Length));
        return randomEnum;*/

        // initialize total number of quad direction enums
        int QUAD_DIRECTION_ENUM_TOTAL = 4;
        // get random enum index num
        int randomIndex = Random.Range(0, QUAD_DIRECTION_ENUM_TOTAL);
        // return enum associated with random index
        return (QuadDirectionType)randomIndex;
    }

    /// <summary>
    /// Returns all coordinates that are within the confines of a cooordinate area.
    /// </summary>
    /// <param name="startCoordinateArg"></param>
    /// <param name="endCoordinateArg"></param>
    /// <returns></returns>
    public static List<Vector2> GetAllCoordinatesWithinCoordinateArea(Vector2 startCoordinateArg,
        Vector2 endCoordinateArg)
    {
        //initialize return var as empty list
        List<Vector2> returnPositions = new List<Vector2>();

        //loop through all X coordinates (columns) within coordinate area
        for (int i = (int)startCoordinateArg.x; i <= (int)endCoordinateArg.x; i++)
        {
            //loop through all Y coordinates (rows) within iterating column
            for (int y = (int)startCoordinateArg.y; y <= (int)endCoordinateArg.y; y++)
            {
                //add iterating coordinate to position list
                returnPositions.Add(new Vector2(i, y));
            }
        }

        //return populated list
        return returnPositions;
    }

    /// <summary>
    /// Returns the Y rotational value associated with the given quad direction.
    /// </summary>
    /// <param name="quadDirectionTypeArg"></param>
    /// <returns></returns>
    public static float GetYRotationValueFromQuadDirection(QuadDirectionType quadDirectionTypeArg)
    {
        // check cases based on given quad direction
        switch (quadDirectionTypeArg)
        {
            // case where given FRONT direction
            case QuadDirectionType.Front:
                return 0f;
            // case where given FRONT direction
            case QuadDirectionType.Right:
                return -90f;
            // case where given FRONT direction
            case QuadDirectionType.Left:
                return -180f;
            // case where given FRONT direction
            case QuadDirectionType.Back:
                return -270f;
            // case where no other case matched
            default:
                // print wanring to console
                Debug.LogWarning("Unknown quadDirectionTypeArg given!");
                // return some default value
                return 0f;
        }
    }

    /// <summary>
    /// Returns the character data that the character draws from for surface level attributes 
    /// (i.e. health, portrait, etc.). NOT deep rooted persistent properties like the inventory.
    /// </summary>
    /// <param name="charMasterArg"></param>
    /// <returns></returns>
    public static CharacterData GetFrontFacingCharacterDataFromCharMaster(CharacterMasterController 
        charMasterArg)
    {
        // get a player master from the given char master
        PlayerCharacterMasterController playerMaster = charMasterArg as PlayerCharacterMasterController;
        // if the given char is actually a player
        if (playerMaster != null)
        {
            // return the player's secondary char data
            return playerMaster.CharDataSecondary;
        }
        // else given char is just a generic char
        else
        {
            // return given char's data
            return charMasterArg.CharData;
        }
    }
}
