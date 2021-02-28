using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwapController : MonoBehaviour
{
    #region Class Variables

    private CharacterData defaultCharacter = null;

    private List<CharacterData> swappableCharacters = new List<CharacterData>();

    #endregion




    #region Swap Functions

    /// <summary>
    /// Sets the default and swappable characters.
    /// </summary>
    /// <param name="defaultCharArg"></param>
    /// <param name="swappableCharsArg"></param>
    private void SetupSwapCharacters(CharacterData defaultCharArg, List<CharacterData> swappableCharsArg)
    {
        defaultCharacter = defaultCharArg;
        swappableCharacters = swappableCharsArg;
    }

    /// <summary>
    /// Returns character associated with given character slot.
    /// </summary>
    /// <param name="charSlotArg"></param>
    /// <returns></returns>
    private CharacterData GetSwappingCharacter(int charSlotArg)
    {
        // if given invalid char slot number
        if (charSlotArg <= 0)
        {
            // print warning to console
            Debug.LogWarning($"Given invalid charSlotArg of {charSlotArg}");
            // return the default character
            return defaultCharacter;
        }
        // else if given char slot number for slot that does not currently have an entry
        else if (swappableCharacters.Count > charSlotArg)
        {
            // return the default character
            return defaultCharacter;
        }
        // else given char slot number for slot with existing entry
        else
        {
            // return associated character
            return swappableCharacters[charSlotArg - 1];
        }
    }

    #endregion


}
