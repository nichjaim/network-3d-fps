using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterOutfitLoadout
{
    #region Class Variables

    public List<CharacterOutfit> characterOutfits;
    public int characterOutfitEquippedIndex;

    #endregion




    #region Constructor Functions

    public CharacterOutfitLoadout()
    {
        Setup();
    }

    public CharacterOutfitLoadout(CharacterOutfitLoadout templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        characterOutfits = new List<CharacterOutfit>();
        characterOutfitEquippedIndex = -1;
    }

    private void Setup(CharacterOutfitLoadout templateArg)
    {
        characterOutfits = new List<CharacterOutfit>();
        foreach (CharacterOutfit iteratingOutfit in templateArg.characterOutfits)
        {
            characterOutfits.Add(new CharacterOutfit(iteratingOutfit));
        }

        characterOutfitEquippedIndex = templateArg.characterOutfitEquippedIndex;
    }

    #endregion




    #region Outfit Functions

    /// <summary>
    /// Add character outfit to list of available outfits.
    /// </summary>
    /// <param name="outfitArg"></param>
    public void AddOutfit(CharacterOutfitTemplate outfitArg)
    {
        //make outfit from given template and add to list
        characterOutfits.Add(new CharacterOutfit(outfitArg));

        //if equipped index not pointing at a outfit
        if (characterOutfitEquippedIndex < 0)
        {
            //point equipped index at first outfit
            characterOutfitEquippedIndex = 0;
        }
    }

    /// <summary>
    /// Returns the currently equipped character outfit.
    /// </summary>
    public CharacterOutfit GetEquippedOutfit()
    {
        //if equipped index is NOT pointing at a available outfit
        if (characterOutfitEquippedIndex < 0 || characterOutfitEquippedIndex >= characterOutfits.Count)
        {
            //return a null outfit
            return null;
        }
        //else equipped index IS pointing at a available outfit
        else
        {
            //return the outfit the equipped index is pointing to
            return characterOutfits[characterOutfitEquippedIndex];
        }
    }

    #endregion


}
