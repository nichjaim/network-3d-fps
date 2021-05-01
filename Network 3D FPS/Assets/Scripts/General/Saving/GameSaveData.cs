using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    #region Class Variables

    public GameFlags gameFlags;

    public List<CharacterData> playableCharacterData;
    public PartyInventory partyInventory;

    public HavenData havenData;

    #endregion




    #region Constructors

    public GameSaveData()
    {
        Setup();
    }

    #endregion




    #region Setup Functions

    public void Setup()
    {
        gameFlags = new GameFlags();

        playableCharacterData = new List<CharacterData>();
        partyInventory = new PartyInventory();

        havenData = new HavenData();
    }

    #endregion


}
