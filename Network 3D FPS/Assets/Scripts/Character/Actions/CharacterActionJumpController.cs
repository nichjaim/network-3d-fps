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
    private CharacterMasterController _charMaster = null;
    private PlayerCharacterMasterController _playerMaster = null;

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

    /// bool that determines if jumps are stopped from being reset. 
    /// This is needed so that the consecutive jump count won't immediately be reset at 
    /// start of jump when still so close to ground.
    private bool onJumpResetCooldown = false;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup all the relevent component references
        InitializeComponentReferences();

        // starts persistent listening for all relevant events
        StartAllEventPersistentListening();
    }

    private void OnEnable()
    {
        // ensure jump resetting is OFF cooldown
        onJumpResetCooldown = false;
    }

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

        // performs the jump action if the appropriate player input was made
        JumpIfPlayerInputted();
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
        // if jump count resetting is currently on cooldown
        if (onJumpResetCooldown)
        {
            // DONT continue code
            return;
        }

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

        // perform a jump
        _characterGravityController.Jump(jumpHeigth);

        // increment current number of consecutive jumps made
        consecutiveJumpsCurrent++;

        /// put the ability for your consecutive jumps to be reset on cooldown for 
        /// a short time
        JumpResetCooldown();
    }

    /// <summary>
    /// Sets jump properties based on current character data.
    /// </summary>
    private void RefreshJumpProperties()
    {
        // get current surface-level character data
        CharacterData frontFacingCharData = GeneralMethods.
            GetFrontFacingCharacterDataFromCharMaster(_charMaster);
        // if char data found
        if (frontFacingCharData != null)
        {
            // set jump property values based on char data
            jumpHeigth = frontFacingCharData.characterStats.statJumpHeight;
            consecutiveJumpsTotal = frontFacingCharData.characterStats.statConsecutiveJumps;
        }
        // else no data found
        else
        {
            // set jump properties to some default values
            jumpHeigth = 1f;
            consecutiveJumpsTotal = 1;
        }
    }

    /// <summary>
    /// Puts the ability for your consecutive jumps to be reset on cooldown for a short time. 
    /// This is so won't immediately be reset at start of jump when still so close to ground.
    /// </summary>
    private void JumpResetCooldown()
    {
        // call internal function as coroutine
        StartCoroutine(JumpResetCooldownInternal());
    }

    private IEnumerator JumpResetCooldownInternal()
    {
        // put jump resetting ON cooldown
        onJumpResetCooldown = true;

        // wait enough time for player to be far enough from ground
        yield return new WaitForSeconds(0.65f);

        // take jump resetting OFF cooldown
        onJumpResetCooldown = false;
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




    #region Event Functions

    /// <summary>
    /// Starts persistent listening for all relevant events. 
    /// Call in Awake().
    /// </summary>
    private void StartAllEventPersistentListening()
    {
        _charMaster.OnCharDataChangedAction += RefreshJumpProperties;

        // if char is a player
        if (IsPlayer())
        {
            _playerMaster.OnCharDataSecondaryChangedAction += RefreshJumpProperties;
        }
    }

    #endregion


}
