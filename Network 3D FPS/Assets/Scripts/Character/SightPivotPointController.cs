using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SightPivotPointController : MonoBehaviour
{
    #region Class Variables

    private GameManager _gameManager;

    private Camera sightCamera = null;

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;

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
        _gameManager = GameManager.Instance;
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
            if (_gameManager != null)
            {
                // parent the camera to the network manager
                sightCamera.transform.SetParent(_gameManager.transform);
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

    #endregion


}
