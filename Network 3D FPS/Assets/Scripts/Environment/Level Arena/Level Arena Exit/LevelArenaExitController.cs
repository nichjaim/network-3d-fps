using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArenaExitController : MonoBehaviour
{
    #region Class Variables

    // whether the exit is activated or not (has player completed the arena?)
    protected bool isExitActive = false;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // setup the exit properties based on if the exit is active or not
        RefreshExitProperties();
    }

    private void OnTriggerEnter(Collider other)
    {
        // get colliding object's hitbox component
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if the collision was with a player
        if (GeneralMethods.IsPlayerHitbox(collidingHitbox))
        {
            OnPlayerEnterTrigger();
        }
    }

    #endregion




    #region Exit Functions

    /// <summary>
    /// Should be called when player enters the exit's trigger.
    /// </summary>
    private void OnPlayerEnterTrigger()
    {
        // if the exit is active
        if (isExitActive)
        {
            Debug.Log("NEED IMPL: Trigger event to denota that player has exited the arena."); // NEED IMPL!
        }
    }

    public void SetExitActivation(bool activeArg)
    {
        // set current exit activeness to given bool
        isExitActive = activeArg;

        // setup the exit properties based on if the exit is active or not
        RefreshExitProperties();
    }

    /// <summary>
    /// Sets up the exit properties absed on if the exit is active or not.
    /// </summary>
    protected virtual void RefreshExitProperties()
    {
        // IMPL in child
    }

    #endregion


}
