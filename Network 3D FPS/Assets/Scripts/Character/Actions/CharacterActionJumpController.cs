using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterActionJumpController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;
    [SerializeField]
    private CharacterGravityController _characterGravityController = null;

    [Header("Jump Properties")]

    [SerializeField]
    private float jumpHeigth = 3f;

    #endregion




    #region MonoBehaviour Functions

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        //performs the jump action if the appropriate player input was made
        JumpIfPlayerInputted();
    }

    #endregion




    #region Input Functions

    /// <summary>
    /// Performs the jump action if the appropriate player input was made. 
    /// Call in Update().
    /// </summary>
    private void JumpIfPlayerInputted()
    {
        //if input given
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //perform a jump
            _characterGravityController.Jump(jumpHeigth);
        }
    }

    #endregion


}
