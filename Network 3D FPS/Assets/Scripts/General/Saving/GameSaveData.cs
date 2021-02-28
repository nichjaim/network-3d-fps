using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    #region Class Variables

    public List<CharacterData> playableCharacterData;

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
        playableCharacterData = new List<CharacterData>();
    }

    #endregion


}
