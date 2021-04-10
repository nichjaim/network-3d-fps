using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterGravityController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private GroundCheckController _groundCheckController = null;
    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [Header("Slope Limit Properties")]

    /// sets whether the character controller's slope limit will change between being airborne and grounded 
    /// NOTE: changing slope limits will cause any overlapping triggers to be entered and exited
    [SerializeField]
    private bool changeSlopeLimitWhenAirborne = true;
    // the appropriate slope limit value for the character controller when it is on the ground
    [SerializeField]
    private float slopeLimitGrounded = 45f;
    // the appropriate slope limit value for the character controller when it is in the air
    [SerializeField]
    private float slopeLimitAirborne = 100f;

    [Header("Gravity Properties")]

    [SerializeField]
    // the strength of gravity on this character (MUST BE NEGATIVE)
    private float gravityForceStrength = -9.81f;

    // the current velocty at which the character is falling
    private Vector3 fallVelocity;
    // the maximum velocity speed the character can fall at (MUST BE NEGATIVE)
    [SerializeField]
    private float maxFallVelocity = -50f;

    #endregion




    #region MonoBehaviour Functions

    private void FixedUpdate()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        //performs the actions necessary to have the character be affected by gravity
        ProcessCharacterFalling();
    }

    #endregion




    #region Gravity Functions

    /// <summary>
    /// Performs the actions necessary to have the character be affected by gravity. 
    /// Call in FixedUpdate().
    /// </summary>
    private void ProcessCharacterFalling()
    {
        //if the character is grounded AND has been falling
        if (_groundCheckController.IsGrounded() && fallVelocity.y < 0f)
        {
            // if char contr's slope limit is dynamic AND is NOT set for being grounded
            if (changeSlopeLimitWhenAirborne && _characterController.slopeLimit != slopeLimitGrounded)
            {
                /// set the char controller's slope limit to the grounded slope limit value 
                /// (NOTE: this will cause any overlapping triggers to be entered and exited)
                _characterController.slopeLimit = slopeLimitGrounded;
            }

            // resets the speed at which the character falls
            ResetFallVelocity();
        }

        /// increase the current fall velocity by the gravity force strength while ensuring it does 
        /// not go past the maximum possible fall velocity
        fallVelocity.y = Mathf.Max(fallVelocity.y + (gravityForceStrength * Time.fixedDeltaTime), 
            maxFallVelocity);
        /// move the character down based on the current fall velocity. 
        /// NOTE: need to use Time.deltaTime again due to the nature of freefall physics.
        _characterController.Move(fallVelocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Resets the speed at which the character falls.
    /// </summary>
    private void ResetFallVelocity()
    {
        /// reset gravity velocity. NOTE: using a small value rather than zero to ensure that the 
        /// character gets to the ground and isn't ground checked before the char actually hit the ground.
        fallVelocity.y = -2f;
    }

    /// <summary>
    /// Performs a jump movement action based on the character's jumping properties.
    /// </summary>
    /// <param name="jumpHeigthArg"></param>
    public void Jump(float jumpHeigthArg)
    {
        // if char contr's slope limit is dynamic AND is NOT set for being airborne
        if (changeSlopeLimitWhenAirborne && _characterController.slopeLimit != slopeLimitAirborne)
        {
            /// set the char controller's slope limit to the AIRBORNE slope limit value 
            /// (NOTE: this will cause any overlapping triggers to be entered and exited)
            _characterController.slopeLimit = slopeLimitGrounded;
        }

        //set the current fall velocity to the appropriate value. This value is derived from the physics...
        //...equation used calculate the amount of velocity needed to jump a certain heigth.
        fallVelocity.y = Mathf.Sqrt(jumpHeigthArg * -2f * gravityForceStrength);
    }

    /// <summary>
    /// Reset character gravity properties to the standard.
    /// </summary>
    private void ResetCharacterGravity()
    {
        // resets the speed at which the character falls
        ResetFallVelocity();
    }

    #endregion


}
