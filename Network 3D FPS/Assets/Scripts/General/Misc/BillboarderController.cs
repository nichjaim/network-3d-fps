using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboarderController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private Transform billboardingObject = null;

    [Header("Billboarding Properties")]

    [Tooltip("Does billboard the object up and down?")]
    [SerializeField]
    private bool billboardVertically = false;

    [Tooltip("Billboards away from the camera. Useful for world-based UI objects.")]
    [SerializeField]
    private bool invertBillboarding = false;

    private Camera _playerCamera;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup variable that holds reference to the player camera
        InitializePlayerCamera();
    }

    private void FixedUpdate()
    {
        // refresh the billboarding effect visual based on observer perspective
        RefreshBillboardingEffect();
    }

    #endregion




    #region Initialization Functions

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
            billboardingObject.LookAt(GetLookPosition());
        }
    }

    /// <summary>
    /// Returns the position to look at.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetLookPosition()
    {
        // get position to look at
        Vector3 lookAtPos = new Vector3(
            _playerCamera.transform.position.x,
            GetLookPositionY(),
            _playerCamera.transform.position.z);

        // if should invert the billboarding effect
        if (invertBillboarding)
        {
            // invert the pos to look at
            lookAtPos = billboardingObject.position - lookAtPos;
        }

        // return the calculated position
        return lookAtPos;
    }

    /// <summary>
    /// Returns the appropriate look position for Y-axis.
    /// </summary>
    /// <returns></returns>
    private float GetLookPositionY()
    {
        // if should billboard the object vertically
        if (billboardVertically)
        {
            return _playerCamera.transform.position.y;
        }
        // else should only be billboarding horizontal
        else
        {
            return billboardingObject.position.y;
        }
    }

    #endregion


}
