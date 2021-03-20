using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractableController : MonoBehaviour
{
    #region MonoBehaviour Functions

    protected virtual void OnTriggerEnter(Collider other)
    {
        // if colliding object is NOT a player character
        if (!other.CompareTag("Player"))
        {
            // DONT continue code
            return;
        }

        // call player ENTER proximity method if right to do so
        CallLocalPlayerProximityMethodIfAppropriate(other, true);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // if colliding object is NOT a player character
        if (!other.CompareTag("Player"))
        {
            // DONT continue code
            return;
        }

        // call player EXIT proximity method if right to do so
        CallLocalPlayerProximityMethodIfAppropriate(other, false);
    }

    #endregion




    #region Interact Functions

    /// <summary>
    /// Call when this interactable STARTS being targeted by an interactor 
    /// for a potential interaction.
    /// </summary>
    public virtual void OnInteractionTargetingStart(CharacterMasterController interactorArg)
    {
        // IMPL in child class
    }

    /// <summary>
    /// Call when this interactable STOPS being targeted by an interactor 
    /// for a potential interaction.
    /// </summary>
    public virtual void OnInteractionTargetingEnd(CharacterMasterController interactorArg)
    {
        // IMPL in child class
    }

    public virtual void Interact(CharacterMasterController interactorArg)
    {
        // IMPL in child class
    }

    /// <summary>
    /// When the player who is associated with the machine running this 
    /// game instance comes close to this interactable.
    /// </summary>
    public virtual void OnLocalPlayerEnterProximity(CharacterMasterController interactorArg)
    {
        // IMPL in child class
    }

    /// <summary>
    /// When the player who is associated with the machine running this 
    /// game instance moves away from this interactable.
    /// </summary>
    public virtual void OnLocalPlayerExitProximity(CharacterMasterController interactorArg)
    {
        // IMPL in child class
    }

    #endregion




    #region General Functions

    /// <summary>
    /// Executes the local player proximity method based on given details.
    /// </summary>
    /// <param name="colliderArg"></param>
    /// <param name="isEnteringProximityArg"></param>
    private void CallLocalPlayerProximityMethodIfAppropriate(Collider colliderArg, 
        bool isEnteringProximityArg)
    {
        // get net identity comp from given object
        NetworkIdentity _networkIdentity = colliderArg.GetComponent<NetworkIdentity>();

        /// if NO net identity comp found OR player is online but NOT associated with 
        /// the machine running this
        if (_networkIdentity == null || GeneralMethods.
            IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // get char master comp from given object
        CharacterMasterController _charMaster = colliderArg.GetComponent<CharacterMasterController>();
        // if no char master comp found
        if (_charMaster == null)
        {
            // print warning to console
            Debug.LogWarning("Player entering interactable proximity does not have CharacterMasterController! " +
                $"Object name: {colliderArg.name}");
            // DONT continue code
            return;
        }

        // if ENTERING the proximity
        if (isEnteringProximityArg)
        {
            // call ENTER proximity method
            OnLocalPlayerEnterProximity(_charMaster);
        }
        // else EXITING the proximity
        else
        {
            // call EXIT proximity method
            OnLocalPlayerExitProximity(_charMaster);
        }
    }

    #endregion


}
