using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for the object which determines if on the ground
/// </summary>
public class GroundCheckController : MonoBehaviour
{
    #region Class Variables

    [Header("Grounding Main Properties")]

    // the distance to which ground is checked for
    [SerializeField]
    private float groundCheckDistance = 0.4f;
    // the layers which are considered ground
    [SerializeField]
    private LayerMask groundLayerMask;

    [Header("Grounding Refresh Properties")]

    // auto-refreshes grounding bool based on refresh rate
    [SerializeField]
    private bool automaticallyRefreshGrounding = true;
    // time (in seconds) between grounding refreshes
    [SerializeField]
    private float automaticRefreshRate = 0.1f;

    // the value that denotes if the object is on the ground
    private bool isGrounded = false;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // continuously refreshes whether grounded between refresh rate times
        RefreshGroundingCycle();
    }

    #endregion




    #region Grounded Check Functions

    /// <summary>
    /// Checks if is on the ground
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        return isGrounded;
    }

    /// <summary>
    /// Refreshes the value that denotes if on the ground.
    /// </summary>
    public void RefreshGrounding()
    {
        //create a sphere that checks if this object is within acceptable distance of a object that is...
        //...considered ground.
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, 
            groundLayerMask, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// Continuously refreshes whether grounded between refresh rate times. 
    /// Call in OnEnable().
    /// </summary>
    private void RefreshGroundingCycle()
    {
        // if NOT auto-refreshing
        if (!automaticallyRefreshGrounding)
        {
            // DONT continue code
            return;
        }

        // call the internal function as a coroutine
        StartCoroutine(RefreshGroundingCycleInternal());
    }

    private IEnumerator RefreshGroundingCycleInternal()
    {
        // infinitely loop
        while (true)
        {
            // wait refresh rate time
            yield return new WaitForSeconds(automaticRefreshRate);

            // refresh whether grounded
            RefreshGrounding();
        }
    }

    #endregion


}
