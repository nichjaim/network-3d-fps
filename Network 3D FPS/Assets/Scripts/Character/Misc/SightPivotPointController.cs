using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SightPivotPointController : MonoBehaviour
{
    #region Class Variables

    private NetworkManagerCustom _networkManagerCustom;

    private Camera sightCamera = null;

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

    [SerializeField]
    private PlayerCharacterMasterController _playerCharacterMasterController = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
        // setup the camera object reference, if haven't already
        InitializeSightCameraIfAppropriate();

        // sets the camera to follow this sight pivot point
        AttachCameraToSight();
        // reset player camera's transform values to default
        ResetPlayerCameraTransform();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // sets the camera heigth based on based on current char's heigth
        RefreshCameraHeight();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    private void OnDestroy()
    {
        // removes the camera from this pivot point
        DetachCameraFromSight();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _networkManagerCustom = (NetworkManagerCustom)NetworkManager.singleton;
    }

    #endregion




    #region Camera Functions

    /// <summary>
    /// Sets up the camera object reference, if haven't done already. 
    /// Call in Awake().
    /// </summary>
    private void InitializeSightCameraIfAppropriate()
    {
        // if not already setup
        if (sightCamera == null)
        {
            sightCamera = FindObjectOfType<Camera>();
        }
    }

    /// <summary>
    /// Sets the camera to follow this sight pivot point.
    /// </summary>
    private void AttachCameraToSight()
    {
        sightCamera.transform.SetParent(transform);
    }

    /// <summary>
    /// Reset player camera's transform values to default.
    /// </summary>
    private void ResetPlayerCameraTransform()
    {
        sightCamera.transform.localPosition = Vector3.zero;
        sightCamera.transform.localRotation = Quaternion.identity;
        sightCamera.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Removes the camera from this pivot point.
    /// </summary>
    private void DetachCameraFromSight()
    {
        // if camera reference setup
        if (sightCamera != null)
        {
            // if game manager reference setup
            if (_networkManagerCustom != null)
            {
                // parent the camera to the network manager
                sightCamera.transform.SetParent(_networkManagerCustom.transform);
                ResetPlayerCameraTransform();
            }
            // else game manager reference NOT setup
            else
            {
                // print warning to log
                Debug.LogWarning("No persistent object found that sightCamera can be deatched to!");
            }
        }
        else
        {
            // print warning to log
            Debug.LogWarning("No sightCamera reference to detach!");
        }
    }

    /// <summary>
    /// Sets the camera heigth based on based on current char's heigth.
    /// </summary>
    private void RefreshCameraHeight()
    {
        // get scondary character data (which is the char used for visuals such as heigth)
        CharacterData charDataSecondary = _playerCharacterMasterController.CharDataSecondary;
        // if NO valid char data set
        if (charDataSecondary == null)
        {
            // DONT continue code
            return;
        }

        // set the camera heigth based on char's heigth
        SetupCameraHeight(charDataSecondary.characterInfo.characterHeightInCm);
    }

    /// <summary>
    /// Sets the camera heigth based on given heigth value.
    /// </summary>
    /// <param name="heightInCmArg"></param>
    private void SetupCameraHeight(float heightInCmArg)
    {
        /// get height in world space units from given cm heigth (calculated on assumption 
        /// that 170cm (5'7, which is around average) is equal to 1 local world space unit)
        float worldSpaceUnitHeight = heightInCmArg / 170f;
        // get the new position based on current position and calculated heigth
        Vector3 newPos = new Vector3(transform.localPosition.x, worldSpaceUnitHeight, 
            transform.localPosition.z);
        // set local pos to new position
        transform.localPosition = newPos;
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        _playerCharacterMasterController.OnCharDataSecondaryChangedAction += RefreshCameraHeight;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        _playerCharacterMasterController.OnCharDataSecondaryChangedAction -= RefreshCameraHeight;
    }

    #endregion


}
