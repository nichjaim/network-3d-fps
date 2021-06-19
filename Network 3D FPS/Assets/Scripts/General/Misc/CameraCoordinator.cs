using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCoordinator : MonoBehaviour
{
    #region Class Variables

    [Tooltip("The game object that all the camera objects are parented to.")]
    [SerializeField]
    private Transform camerasHolder = null;

    [Tooltip("The game object that the camera holder object should be parented to when not held by a player object.")]
    [SerializeField]
    private Transform camerasHolderRestParent = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // reset camera holder's transform values to default
        ResetCamertHolderTransform();
        // ensure that player camera transforms values are at default
        ResetCameraTransforms();
    }

    #endregion




    #region Camera Functions

    /// <summary>
    /// Reset player camera transforms values to default.
    /// </summary>
    private void ResetCameraTransforms()
    {
        // loop through all camera objects
        foreach (Transform iterChild in camerasHolder)
        {
            // set iterating camera's transform to the default values
            iterChild.localPosition = Vector3.zero;
            iterChild.localRotation = Quaternion.identity;
            iterChild.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Reset camera holder's transform values to default.
    /// </summary>
    private void ResetCamertHolderTransform()
    {
        camerasHolder.localPosition = Vector3.zero;
        camerasHolder.localRotation = Quaternion.identity;
        camerasHolder.localScale = Vector3.one;
    }

    /// <summary>
    /// Removes camera holder from current object and returns them to the rest parent.
    /// </summary>
    public void DetachCameraHolder()
    {
        camerasHolder.SetParent(camerasHolderRestParent);

        // reset camera holder's transform values to default
        ResetCamertHolderTransform();
    }

    /// <summary>
    /// Attaches camera holder to the given transform.
    /// </summary>
    /// <param name="attachTargetArg"></param>
    public void AttachCameraHolder(Transform attachTargetArg)
    {
        camerasHolder.SetParent(attachTargetArg);

        // reset camera holder's transform values to default
        ResetCamertHolderTransform();
    }

    #endregion


}
