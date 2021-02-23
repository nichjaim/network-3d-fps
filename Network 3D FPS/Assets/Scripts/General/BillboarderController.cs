using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboarderController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private Transform billboardingObject = null;

    //private bool shouldBillboard = false;

    private Camera _playerCamera;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        //setup all variables that reference singleton instance related components
        //InitializeSingletonReferences();
        // setup variable that holds reference to the player camera
        InitializePlayerCamera();
    }

    private void FixedUpdate()
    {
        /*//if should do billboarding effect
        if (shouldBillboard)
        {
            //refresh the billboarding effect visual based on observer perspective
            RefreshBillboardingEffect();
        }*/
        // refresh the billboarding effect visual based on observer perspective
        RefreshBillboardingEffect();
    }

    private void OnEnable()
    {
        //denote whether billboarding is warranted based on if there is a player nearby
        //shouldBillboard = IsZoneOccupied();
    }

    #endregion




    /*#region Override Functions

    protected override void OnInhabitantNumberChange()
    {
        base.OnInhabitantNumberChange();

        //denote whether billboarding is warranted based on if there is a player nearby
        shouldBillboard = IsZoneOccupied();
    }

    #endregion*/




    #region Initialization Functions

    /*/// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _playerCamera = CameraManager.Instance.GetPlayerCamera();
    }*/

    /// <summary>
    /// Sets up variable that holds reference to the player camera. 
    /// Call in Start().
    /// </summary>
    private void InitializePlayerCamera()
    {
        _playerCamera = FindObjectOfType<Camera>();
    }

    #endregion




    #region Billboarding Functions

    /// <summary>
    /// Refresh the billboarding effect visual based on observer perspective.
    /// </summary>
    private void RefreshBillboardingEffect()
    {
        // if player camera found
        if (_playerCamera != null)
        {
            // rotate object being billboarded towards camera
            billboardingObject.LookAt(new Vector3(
                _playerCamera.transform.position.x,
                billboardingObject.transform.position.y,
                _playerCamera.transform.position.z));
        }
    }

    #endregion


}
