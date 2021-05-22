using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArenaEntranceController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private GameObject entrancePointsParent = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // places and activates all player characters at the appropriate entrance positions
        SetupPlayersAtEntrance();
    }

    #endregion




    #region Entrance Functions

    /// <summary>
    /// Places and activated all player characters at the appropriate entrance positions.
    /// </summary>
    private void SetupPlayersAtEntrance()
    {
        // if the entrance point parent object has no child objects
        if (entrancePointsParent.transform.childCount == 0)
        {
            // print warning to console
            Debug.LogWarning("No entrance points for level arena entrance!");

            // DONT continue code
            return;
        }

        // find all player characters in the game
        PlayerCharacterMasterController[] foundPlayers = FindObjectsOfType<PlayerCharacterMasterController>(true);

        // initialize var for upcoming loop
        Transform childPoint = null;
        ObjectActivationController objActivator;

        // loop through all found player characters
        for (int i = 0; i < foundPlayers.Length; i++)
        {
            // if there exists a entrance point child that should be used on the iterating player character
            if (entrancePointsParent.transform.childCount > i)
            {
                // set child point to point related to iterating player
                childPoint = entrancePointsParent.transform.GetChild(i);
            }

            // set iterating player to related entrance point position
            foundPlayers[i].transform.SetPositionAndRotation(childPoint.position, childPoint.rotation);

            // get object activator component from iterating player object
            objActivator = foundPlayers[i].GetComponent<ObjectActivationController>();

            // if component found
            if (objActivator != null)
            {
                // turn ON player
                objActivator.SetObjectActivation(true);
            }
        }
    }

    #endregion


}
