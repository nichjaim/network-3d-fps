using UnityEngine;

[System.Serializable]
public class CharacterOutfit
{
    #region Class Variables

    public string outfitId;
    //public string characterIdOfAssociatedCharacter;

    [Tooltip("Is this outfit associated with 18+ content")]
    public bool isHContent;

    public string outfitName;
    [TextArea(3,15)]
    public string outfitDescription;

    #endregion




    #region Constructors

    public CharacterOutfit()
    {
        Setup();
    }

    public CharacterOutfit(CharacterOutfit templateArg)
    {
        Setup(templateArg);
    }

    public CharacterOutfit(CharacterOutfitTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        outfitId = "0";
        //characterIdOfAssociatedCharacter = "0";

        isHContent = false;

        outfitName = "NAME";
        outfitDescription = "DESC";
    }

    private void Setup(CharacterOutfit templateArg)
    {
        outfitId = templateArg.outfitId;
        //characterIdOfAssociatedCharacter = templateArg.characterIdOfAssociatedCharacter;

        isHContent = false;

        outfitName = templateArg.outfitName;
        outfitDescription = templateArg.outfitDescription;
    }

    #endregion


}
