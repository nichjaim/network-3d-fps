using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CharacterMasterController : NetworkBehaviour
{
    #region Class Variables

    [SyncVar(hook = nameof(OnCharDataChanged))]
    private CharacterData charData = null;
    public Action OnCharDataChangedAction;

    #endregion




    #region Sync Functions

    public void OnCharDataChanged(CharacterData oldCharArg,
        CharacterData newCharArg)
    {
        // call char data change actions if NOT null
        OnCharDataChangedAction?.Invoke();
    }

    #endregion




    #region Getter Functions

    public CharacterData GetCharData()
    {
        return charData;
    }

    #endregion




    #region Setter Functions

    public void SetCharData(CharacterData charDataArg)
    {
        if (NetworkClient.isConnected)
        {
            SetCharDataCommand(charDataArg);
        }
        else
        {
            SetCharDataInternal(charDataArg);
        }
    }

    //[Command(ignoreAuthority = true)]
    [Command]
    private void SetCharDataCommand(CharacterData charDataArg)
    {
        SetCharDataInternal(charDataArg);
    }

    private void SetCharDataInternal(CharacterData charDataArg)
    {
        charData = charDataArg;
    }

    #endregion


}
