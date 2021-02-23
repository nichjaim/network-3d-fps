using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterActionPauseController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    #endregion




    #region MonoBehaviour Functions

    // Update is called once per frame
    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // pause/unpause game if player gave appropriate input
        GamePausingIfInputted();
    }

    #endregion




    #region Pause Functions

    /// <summary>
    /// Pause/unpause game if player gave appropriate input.
    /// </summary>
    private void GamePausingIfInputted()
    {
        // if pause input given by player
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // trigger pause attempt event
            GamePausingActionEvent.Trigger();
        }
    }

    #endregion


}
