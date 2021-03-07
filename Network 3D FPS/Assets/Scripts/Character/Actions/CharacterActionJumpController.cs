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
    [SerializeField]
    private GroundCheckController _groundCheckController = null;

    [Header("Jump Properties")]

    [SerializeField]
    private float jumpHeigth = 3f;

    /// number of jumps char can perform before needing to be grounded (ex. if 2, then 
    /// char can double jump)
    [SerializeField]
    private int consecutiveJumpsTotal = 1;
    private int consecutiveJumpsCurrent = 0;

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

        // if char has been grounded
        if (_groundCheckController.IsGrounded())
        {
            // resets the current number of consecutive jumps available to char
            ResetConsecutiveJumpsCurrent();
        }

        //performs the jump action if the appropriate player input was made
        JumpIfPlayerInputted();
    }

    #endregion




    #region Jump Functions

    /// <summary>
    /// Returns bool that denotes if the char still has some consecutive jumps 
    /// available to them.
    /// </summary>
    /// <returns></returns>
    private bool HaveConsecutiveJumpsLeft()
    {
        return consecutiveJumpsCurrent < consecutiveJumpsTotal;
    }

    /// <summary>
    /// Resets the current number of consecutive jumps available to char.
    /// </summary>
    private void ResetConsecutiveJumpsCurrent()
    {
        consecutiveJumpsCurrent = 0;
    }

    /// <summary>
    /// Performs the act of jumping.
    /// </summary>
    private void JumpAction()
    {
        // if char can NOT perform any more consecutive jumps
        if (!HaveConsecutiveJumpsLeft())
        {
            // DONT continue code
            return;
        }

        //perform a jump
        _characterGravityController.Jump(jumpHeigth);
        // increment current number of consecutive jumps made
        consecutiveJumpsCurrent++;
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
            // performs the act of jumping
            JumpAction();
        }
    }

    #endregion


}
