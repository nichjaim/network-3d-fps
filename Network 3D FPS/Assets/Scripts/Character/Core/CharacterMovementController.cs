using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMovementController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterController _characterController;
    /*[SerializeField]
    private CharacterMasterController _characterMasterController;

    private Joystick touchJoystickMove;*/

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [Header("Move Properties")]

    [SerializeField]
    [Range(0, 1)]
    //initialize the minimum amount of movement input needed to be considered moving
    private float moveInputMinimum = 0.4f;
    [SerializeField]
    private float moveSpeed = 5f;

    private float moveInputX = 0f;
    private float moveInputZ = 0f;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        //setup all variables that reference singleton instance related components
        //InitializeSingletonReferences();
    }

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        //sets the movement input values based on the user inputs given by the player
        SetMoveInputFromPlayerInput();
    }

    private void FixedUpdate()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        //performs the actions necessary to move the character based on move input
        ProcessCharacterMovement();
    }

    #endregion




    #region Initialization Functions

    /*/// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        touchJoystickMove = UIManager.Instance.GetTouchJoystickLeft();
    }*/

    #endregion




    #region Movement Functions

    /// <summary>
    /// Performs the actions necessary to move the character based on move input. 
    /// Call in FixedUpdate().
    /// </summary>
    private void ProcessCharacterMovement()
    {
        //get the unprocessed move motion from the move input
        Vector3 moveMotionUnprocessed = (transform.right * moveInputX) + (transform.forward * moveInputZ);
        //get processed version the move motion
        Vector3 moveMotionProcessed = moveMotionUnprocessed.normalized * moveSpeed * Time.fixedDeltaTime;

        //move the character based on the retrieved move motion
        _characterController.Move(moveMotionProcessed);
    }

    #endregion




    #region Input Functions

    /// <summary>
    /// Sets the movement input values based on the user inputs given by the player. 
    /// Call in Update().
    /// </summary>
    private void SetMoveInputFromPlayerInput()
    {
        /*//check cases based on what type of user input method the player is using
        switch (_characterMasterController.GetPlayerInputControlType())
        {
            //case where player is playing with KB+M
            case PlayerInputControlType.KeyboardAndMouse:
                moveInputX = Input.GetAxisRaw("Horizontal");
                moveInputZ = Input.GetAxisRaw("Vertical");
                break;
            //case where player is playing with touch screen options
            case PlayerInputControlType.TouchScreen:
                if (Mathf.Abs(touchJoystickMove.Horizontal) >= moveInputMinimum)
                {
                    moveInputX = touchJoystickMove.Horizontal;
                }
                else
                {
                    moveInputX = 0f;
                }

                if (Mathf.Abs(touchJoystickMove.Vertical) >= moveInputMinimum)
                {
                    moveInputZ = touchJoystickMove.Vertical;
                }
                else
                {
                    moveInputZ = 0f;
                }

                //moveInputX = touchJoystickMove.Horizontal;
                //moveInputZ = touchJoystickMove.Vertical;
                break;
            //case where no other case was matched
            default:
                //print warning to console
                Debug.LogWarning("Unknown playerInputControlType of " + 
                    _characterMasterController.GetPlayerInputControlType().ToString());
                break;
        }*/
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputZ = Input.GetAxisRaw("Vertical");
    }

    #endregion


}
