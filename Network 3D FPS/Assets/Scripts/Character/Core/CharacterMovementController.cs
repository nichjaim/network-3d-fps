using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.AI;

public class CharacterMovementController : MonoBehaviour
{
    #region Class Variables

    private NetworkManagerCustom _networkManagerCustom;

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _charMaster = null;
    private PlayerCharacterMasterController _playerMaster = null;

    [SerializeField]
    private CharacterController _characterController;
    /*[SerializeField]
    private CharacterMasterController _characterMasterController;

    private Joystick touchJoystickMove;*/

    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private Animator modelAnimator = null;

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [Header("Move Properties")]

    /*// initialize the minimum amount of movement input needed to be considered moving
    [Range(0, 1)]
    [SerializeField]
    private float moveInputMinimum = 0.4f;*/

    [SerializeField]
    private float moveSpeed = 5f;

    private float moveInputX = 0f;
    private float moveInputZ = 0f;

    private bool isMoving = false;
    public bool IsMoving
    {
        get { return isMoving; }
    }
    public Action OnIsMovingChangedAction;

    [Header("AI Navigation Properties")]

    [Tooltip("The time between re-calculation of the navigation destination.")]
    [SerializeField]
    private float navRefreshRate = 0.25f;

    private Transform navTarget = null;
    public Transform NavTarget
    {
        set { navTarget = value; }
    }

    /// positive for increased movement, negative for decreased movement. 
    /// Used for status effects.
    private float temporaryMovementChangePercentage = 0f;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup all the relevent component references
        InitializeComponentReferences();

        // starts persistent listening for all relevant events
        StartAllEventPersistentListening();
    }

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    private void OnEnable()
    {
        // start all the necessary AI navigation properties, if character requires it
        SetupAllNavigationIfAppropriate();
    }

    private void Update()
    {
        // if NOT a player
        if (!IsPlayer())
        {
            // DONT continue code
            return;
        }

        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // if the current state of menus does NOT allow the player chars to perform actions
        if (!_networkManagerCustom.DoesMenusStateAllowPlayerCharacterAction())
        {
            // DONT continue code
            return;
        }

        //sets the movement input values based on the user inputs given by the player
        SetMoveInputFromPlayerInput();
    }

    private void FixedUpdate()
    {
        // if NOT a player
        if (!IsPlayer())
        {
            // DONT continue code
            return;
        }

        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // if the current state of menus does NOT allow the player chars to perform actions
        if (!_networkManagerCustom.DoesMenusStateAllowPlayerCharacterAction())
        {
            // DONT continue code
            return;
        }

        // performs the actions necessary to move the character based on move input
        ProcessCharacterMovement();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all the relevent component references.
    /// Call in Awake().
    /// </summary>
    private void InitializeComponentReferences()
    {
        _playerMaster = _charMaster as PlayerCharacterMasterController;
    }

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _networkManagerCustom = (NetworkManagerCustom)NetworkManager.singleton;
        //touchJoystickMove = UIManager.Instance.GetTouchJoystickLeft();
    }

    #endregion




    #region Movement Functions

    /// <summary>
    /// Performs the actions necessary to move the character based on move input. 
    /// Call in FixedUpdate().
    /// </summary>
    private void ProcessCharacterMovement()
    {
        // get the unprocessed move motion from the move input
        Vector3 moveMotionUnprocessed = (transform.right * moveInputX) + (transform.forward * moveInputZ);
        // get processed version the move motion
        Vector3 moveMotionProcessed = moveMotionUnprocessed.normalized * GetTrueMovementSpeed() * Time.fixedDeltaTime;

        // move the character based on the retrieved move motion
        _characterController.Move(moveMotionProcessed);

        // changes the current state that denotes if moving based on current movement motion
        ProcessIsMovingChangeFromMotion(moveMotionProcessed);
    }

    /// <summary>
    /// Gets movement speed with all movement factors accounted for.
    /// </summary>
    /// <returns></returns>
    private float GetTrueMovementSpeed()
    {
        // get move speed change value
        float moveSpeedChange = moveSpeed * temporaryMovementChangePercentage;

        // return move speed with change added
        return moveSpeed + moveSpeedChange;
    }

    /// <summary>
    /// Sets move speed based on current character data.
    /// </summary>
    private void RefreshMoveSpeed()
    {
        // get current surface-level character data
        CharacterData frontFacingCharData = GeneralMethods.
            GetFrontFacingCharacterDataFromCharMaster(_charMaster);
        // if char data found
        if (frontFacingCharData != null)
        {
            // set move speed to the char's movement stat
            moveSpeed = frontFacingCharData.characterStats.statMovementSpeed;
        }
        // else no data found
        else
        {
            // set move speed to some default value
            moveSpeed = 1f;
        }

        // set navigation speed based on current movement properties
        RefreshNavigationSpeed();
    }

    #endregion




    #region Is-Moving Functions

    private void ProcessIsMovingChangeFromMotion(Vector3 moveMotionArg)
    {
        // get the minimum value needed to denote movement
        float minimumMovement = 0.1f;

        // get current move state from given movement
        bool doesInputDenoteMoving = (Mathf.Abs(moveMotionArg.x) > minimumMovement) ||
            (Mathf.Abs(moveMotionArg.z) > minimumMovement);

        ProcessIsMovingChange(doesInputDenoteMoving);
    }

    private void ProcessIsMovingChangeFromNavTarget(Transform navTargetArg)
    {
        ProcessIsMovingChange(navTarget != null);
    }

    /// <summary>
    /// Changes the current state that denotes if moving based on move bool.
    /// </summary>
    /// <param name="isMovingArg"></param>
    private void ProcessIsMovingChange(bool isMovingArg)
    {
        // if started to move or stopped from moving
        if (isMoving != isMovingArg)
        {
            // set move state to the new move state
            isMoving = isMovingArg;

            // refreshes movement related animation condition values based on whether moving
            RefreshMoveAnimation();

            // call is moving change actions if NOT null
            OnIsMovingChangedAction?.Invoke();
        }
    }

    #endregion




    #region Character Functions

    /// <summary>
    /// Returns bool that denotes if asscoaited character is a player character.
    /// </summary>
    /// <returns></returns>
    private bool IsPlayer()
    {
        return _playerMaster != null;
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




    #region AI Navigation Functions

    /// <summary>
    /// Begins the cycle that continuously sets navigation to the target to follow 
    /// between refresh rate delays.
    /// Call in OnEnable().
    /// </summary>
    private void TargetNavigationCycle()
    {
        // call the internal function as a coroutine
        StartCoroutine(TargetNavigationCycleInternal());
    }

    private IEnumerator TargetNavigationCycleInternal()
    {
        // infinietly loop
        while (true)
        {
            // wait the refresh rate time
            yield return new WaitForSeconds(navRefreshRate);

            // if have a current target to follow
            if (navTarget != null)
            {
                // set target position to navigate to
                _navMeshAgent.SetDestination(navTarget.position);
                // ensure mvement along path is NOT paused
                _navMeshAgent.isStopped = false;

                // plays the detection sound feedback if NOT on cooldown
                //PlayDetectSoundIfAppropriate();
            }

            // changes the current state that denotes if moving based on current nav target
            ProcessIsMovingChangeFromNavTarget(navTarget);
        }
    }

    /// <summary>
    /// Stops following any target.
    /// </summary>
    public void ResetNavigationTarget()
    {
        // if NO nav agent reference
        if (_navMeshAgent == null)
        {
            // DONT continue code
            return;
        }

        // clear following target
        navTarget = null;

        // if the nav mesh has a current path
        if (_navMeshAgent.hasPath)
        {
            // stop moving along that path
            _navMeshAgent.isStopped = true;
        }
    }

    /// <summary>
    /// Sets navigation speed based on current movement properties. 
    /// Call in OnEnable().
    /// </summary>
    private void RefreshNavigationSpeed()
    {
        // if NO nav agent reference
        if (_navMeshAgent == null)
        {
            // DONT continue code
            return;
        }

        // set navigation speed to true movement value
        _navMeshAgent.speed = GetTrueMovementSpeed();
    }

    /// <summary>
    /// Starts all the necessary AI navigation properties, if character requires it. 
    /// Call in OnEnable().
    /// </summary>
    private void SetupAllNavigationIfAppropriate()
    {
        /// if char is NOT a player (i.e. is an NPC). NOTE: have to write condition this way because 
        /// IsPlayer() is setup in start, which is setup later in Start()
        if (!(_charMaster as PlayerCharacterMasterController))
        {
            // set no target to follow
            ResetNavigationTarget();

            // set navigation speed based on current movement properties
            RefreshNavigationSpeed();

            /// begins the cycle that continuously sets navigation to the target to follow 
            /// between refresh rate delays
            TargetNavigationCycle();
        }
    }

    #endregion




    #region Animation Functions

    /// <summary>
    /// Refreshes movement related animation condition values based on whether moving. 
    /// </summary>
    private void RefreshMoveAnimation()
    {
        // if NO model animator reference
        if (modelAnimator == null)
        {
            // DONT continue code
            return;
        }

        modelAnimator.SetBool("IsMoving", isMoving);
    }

    #endregion




    #region Status Effect Functions

    /// <summary>
    /// Adds given percent value to the movement speed chance percentage. 
    /// Positive value means faster speed, negative means slower speed.
    /// </summary>
    /// <param name="percentValueArg"></param>
    public void AddTemporaryMovementChangePercentage(float percentValueArg)
    {
        temporaryMovementChangePercentage += percentValueArg;

        // set navigation speed based on current movement properties
        RefreshNavigationSpeed();
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts persistent listening for all relevant events. 
    /// Call in Awake().
    /// </summary>
    private void StartAllEventPersistentListening()
    {
        _charMaster.OnCharDataChangedAction += RefreshMoveSpeed;

        // if char is a player
        if (IsPlayer())
        {
            _playerMaster.OnCharDataSecondaryChangedAction += RefreshMoveSpeed;
        }
    }

    #endregion


}
