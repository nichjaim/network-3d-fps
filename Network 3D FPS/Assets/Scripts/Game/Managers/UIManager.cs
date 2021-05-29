using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UIManager : MonoBehaviour
{
    #region Class Variables

    public static UIManager Instance;

    // player char that this UI is asscoiated with
    private PlayerCharacterMasterController uiPlayerCharacter = null;
    public PlayerCharacterMasterController UiPlayerCharacter
    {
        get { return uiPlayerCharacter; }
    }

    [SerializeField]
    private DialogueGameCoordinator dialgGameCoordr = null;
    public DialogueGameCoordinator DialgGameCoordr
    {
        get { return dialgGameCoordr; }
    }

    [SerializeField]
    private DialogueUiCoordinator dialgUICoordr = null;
    public DialogueUiCoordinator DialgUICoordr
    {
        get { return dialgUICoordr; }
    }

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup singelton instance
        InstantiateInstance();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the singleton instance. 
    /// Call in Awake().
    /// </summary>
    private void InstantiateInstance()
    {
        // if an instance of this singeton is already setup
        if (Instance != null)
        {
            // destroy this object
            Destroy(gameObject);
        }
        // else no such singleton object exists yet
        else
        {
            // set the singelton isntance to this entity
            Instance = this;
            // ensure this object is not destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion




    #region UI Functions

    /// <summary>
    /// Sets up the player char that the UI is associated with, that is if the player is appropriate.
    /// </summary>
    /// <param name="playerCharArg"></param>
    public void SetPlayerCharacterIfAppropriate(PlayerCharacterMasterController playerCharArg)
    {
        // call internal function as coroutine
        StartCoroutine(SetPlayerCharacterIfAppropriateInternal(playerCharArg));
    }

    private IEnumerator SetPlayerCharacterIfAppropriateInternal(PlayerCharacterMasterController playerCharArg)
    {
        // wait to ensure that player's network component properties are fully setup
        yield return new WaitForEndOfFrame();

        // get net identity comp from player obj
        NetworkIdentity netIdentity = playerCharArg.GetComponent<NetworkIdentity>();
        // if NO such component found
        if (netIdentity == null)
        {
            // DONT continue code
            yield break;
        }

        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(netIdentity))
        {
            // DONT continue code
            yield break;
        }

        //set UI player char to given char
        uiPlayerCharacter = playerCharArg;
        // trigger event to denote that the UI player char has been changed
        UiPlayerCharChangedEvent.Trigger(uiPlayerCharacter);
    }

    #endregion


}
