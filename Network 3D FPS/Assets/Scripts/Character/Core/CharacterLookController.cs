using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterLookController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    /*[SerializeField]
    private CharacterMasterController _characterMasterController;*/
    [SerializeField]
    private GameObject charMasterObj = null;
    [SerializeField]
    private Transform sightPivotPoint = null;

    //private FixedTouchField touchFieldLook;

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [Header("Look Properties")]

    //the easily readibly mouse sensitivity value that will need to be multiplied to be of actual use
    [SerializeField]
    private int lookSensitivity = 10;
    private static float SENSITIVITY_MULTIPLIER_MOUSE = 100f;
    private static float SENSITIVITY_MULTIPLIER_TOUCH = 5f;

    [SerializeField]
    private bool onlyHorizontalLook = false;
    [SerializeField]
    private bool lockMouseCursor = false;

    private float lookRotationX = 0f;
    private float lookInputX = 0f;
    private float lookInputY = 0f;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        //setup all variables that reference singleton instance related components
        //InitializeSingletonReferences();

        //locks mouse cursor movement if properties set to do so
        //LockMouseCursorIfAppropriate();
    }

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        //set the look input values based on the user inputs given by the player
        SetLookInputFromPlayerInput();
        //performs the actions necessary to turn the sight pivot point based on look input
        ProcessCharacterLooking();
    }

    private void OnEnable()
    {
        // resets the sight pivot point to default rotation
        //ResetCharacterLook();
    }

    #endregion




    #region Initialization Functions

    /*/// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        touchFieldLook = UIManager.Instance.GetTouchFieldRight();
    }*/

    #endregion




    #region Look Functions

    /// <summary>
    /// Performs the actions necessary to turn the sight pivot point based on look input. 
    /// Call in Update().
    /// </summary>
    private void ProcessCharacterLooking()
    {
        //rotate character's body based on x look input
        //_characterMasterController.transform.Rotate(Vector3.up * lookInputX);
        charMasterObj.transform.Rotate(Vector3.up * lookInputX);

        //if can look vertically
        if (!onlyHorizontalLook)
        {
            //change rotation value based on Y look input
            lookRotationX -= lookInputY;
            //ensure the rotation does not flip behind the character
            lookRotationX = Mathf.Clamp(lookRotationX, -90f, 90f);
            //rotate camera on y value based on current rotation
            /*_characterMasterController.GetSightPivotPoint().localRotation = Quaternion.
                Euler(lookRotationX, 0f, 0f);*/
            sightPivotPoint.localRotation = Quaternion.
                Euler(lookRotationX, 0f, 0f);
        }
    }

    /*/// <summary>
    /// Locks mouse cursor movement if properties set to do so. 
    /// Call in Start().
    /// </summary>
    private void LockMouseCursorIfAppropriate()
    {
        //if should lock mouse cursor movement AND the player input type is KB+M
        if (lockMouseCursor && 
            _characterMasterController.GetPlayerInputControlType() == 
            PlayerInputControlType.KeyboardAndMouse)
        {
            //lock down mouse cursor
            Cursor.lockState = CursorLockMode.Locked;
        }
    }*/

    /// <summary>
    /// Resets the sight pivot point to default rotation. 
    /// Call in OnEnable().
    /// </summary>
    private void ResetCharacterLook()
    {
        // reset camera trans rotation to default
        sightPivotPoint.localRotation = Quaternion.identity;
    }

    #endregion




    #region Input Functions

    /// <summary>
    /// Set the look input values based on the user inputs given by the player. 
    /// Call in Update().
    /// </summary>
    private void SetLookInputFromPlayerInput()
    {
        /*//check cases based on what type of user input method the player is using
        switch (_characterMasterController.GetPlayerInputControlType())
        {
            //case where player is playing with KB+M
            case PlayerInputControlType.KeyboardAndMouse:
                //get user mouse inputs
                lookInputX = Input.GetAxis("Mouse X") * (lookSensitivity * SENSITIVITY_MULTIPLIER_MOUSE) * Time.deltaTime;
                lookInputY = Input.GetAxis("Mouse Y") * (lookSensitivity * SENSITIVITY_MULTIPLIER_MOUSE) * Time.deltaTime;
                break;
            //case where player is playing with touch screen options
            case PlayerInputControlType.TouchScreen:
                lookInputX = touchFieldLook.TouchDist.x * (lookSensitivity * SENSITIVITY_MULTIPLIER_TOUCH) * Time.deltaTime;
                lookInputY = touchFieldLook.TouchDist.y * (lookSensitivity * SENSITIVITY_MULTIPLIER_TOUCH) * Time.deltaTime;
                break;
            //case where no other case was matched
            default:
                //print warning to console
                Debug.LogWarning(@"Problem in PlayerCharacterController's SetLookInputFromPlayerInput. 
                                No case matched.");
                break;
        }*/
        // get user mouse inputs
        lookInputX = Input.GetAxis("Mouse X") * (lookSensitivity * SENSITIVITY_MULTIPLIER_MOUSE) * Time.deltaTime;
        lookInputY = Input.GetAxis("Mouse Y") * (lookSensitivity * SENSITIVITY_MULTIPLIER_MOUSE) * Time.deltaTime;
    }

    #endregion


}
