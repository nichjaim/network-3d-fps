using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterActionInteractController : MonoBehaviour
{
    #region Class Variables
    
    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;
    [SerializeField]
    private CharacterMasterController _characterMasterController = null;
    [SerializeField]
    private SightPivotPointController _sightPoint = null;

    [Header("Detection Properties")]

    [SerializeField]
    private float detectRange = 2f;
    [SerializeField]
    private float detectRefreshRate = 0.3f;

    [SerializeField]
    private LayerMask interactableLayerMask = new LayerMask();

    // target of interaction
    private InteractableController interactTarget = null;

    #endregion




    #region MonoBehaviour Functions

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // interacts with target if appropriate input is given by player
        InteractIfPlayerInputted();
    }

    private void OnEnable()
    {
        /// starts continuously cycle that tries to detect any interactables that 
        /// are within sights between time delays
        DetectInteractableCycle();
    }

    #endregion




    #region Interact Functions

    /*/// <summary>
    /// Returns all enemy hitboxes from given raycast hits.
    /// </summary>
    /// <param name="raycastHitsArg"></param>
    /// <returns></returns>
    private List<HitboxController> GetAllEnemyHitboxesFromRaycastHits(RaycastHit[] raycastHitsArg)
    {
        // initialize return var as dresh empty list
        List<HitboxController> enemyHitboxes = new List<HitboxController>();

        // initialize vars for upcoming loop
        GameObject raycastHitObject;
        HitboxController iterHitbox;

        // loop through all raycast hits
        foreach (RaycastHit iterRayHit in raycastHitsArg)
        {
            // get the object of the iterating raycast hit
            raycastHitObject = iterRayHit.transform.gameObject;
            // if raycast hit an enemy object
            if (raycastHitObject.CompareTag("Enemy"))
            {
                // get hitbox component of enemy that was hit
                iterHitbox = raycastHitObject.GetComponent<HitboxController>();
                // if object hit was NOT an enemy hitbox
                if (iterHitbox == null)
                {
                    // skip to next loop iteration
                    continue;
                }

                // add the enemy's hitbox to list
                enemyHitboxes.Add(iterHitbox);
            }
        }

        // return populated list
        return enemyHitboxes;
    }*/

    /// <summary>
    /// Interact with the interaction target.
    /// </summary>
    private void Interact()
    {
        // if have an interaction target
        if (interactTarget != null)
        {
            // interact with interaction target
            interactTarget.Interact(_characterMasterController);
        }
    }

    /// <summary>
    /// Attempt to detect any interactables that are within sights.
    /// </summary>
    private void DetectInteractable()
    {
        // initialize vars for upcoming conditional
        RaycastHit raycastHit;
        InteractableController hitInteractable;

        // if a fired raycast hit an interactable object
        if (Physics.Raycast(_sightPoint.transform.position, _sightPoint.transform.forward, out raycastHit,
            detectRange, interactableLayerMask))
        {
            // get interactable component from hit interactable object
            hitInteractable = raycastHit.transform.GetComponent<InteractableController>();
            // if no interactable component found on the interactable object
            if (hitInteractable == null)
            {
                // print warning to console
                Debug.LogWarning("Detected interactable object did NOT have a InteractableController component! " +
                    $"Object name: {raycastHit.transform.name}");
                // DONT continue code
                return;
            }

            // if hit interactable is different than the one currently being targeted
            if (interactTarget != hitInteractable)
            {
                // if something was being targeted before
                if (interactTarget != null)
                {
                    // denote that interaction targeted has ended on old target
                    interactTarget.OnInteractionTargetingEnd(_characterMasterController);
                }

                // set the current interaction target to the new target
                interactTarget = hitInteractable;
                // denote that interaction targeted has started on new target
                interactTarget.OnInteractionTargetingStart(_characterMasterController);
            }
        }
        // else fired raycast did NOT hit any interactables
        else
        {
            // if an interactable was being targeted before
            if (interactTarget != null)
            {
                // denote that interaction targeted has ended on old target
                interactTarget.OnInteractionTargetingEnd(_characterMasterController);
                // deselect current targeted interactable
                interactTarget = null;
            }
        }
    }

    /// <summary>
    /// Continuously tries to detect any interactables that are within sights between time delays.
    /// Call in OnEnable().
    /// </summary>
    private void DetectInteractableCycle()
    {
        // call internal function as coroutine
        StartCoroutine(DetectInteractableCycleInternal());
    }

    private IEnumerator DetectInteractableCycleInternal()
    {
        // wait till network component properties are setup
        yield return new WaitForEndOfFrame();

        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            yield break;
        }

        // infinitely loop
        while (true)
        {
            // wait refresh rate time
            yield return new WaitForSeconds(detectRefreshRate);

            // detect any interactables
            DetectInteractable();
        }
    }

    #endregion




    #region General Functions

    /*/// <summary>
    /// Fires a raycast based on given specificities. 
    /// </summary>
    /// <param name="castOriginArg"></param>
    /// <param name="castRangeArg"></param>
    /// <param name="penetrateThroughAllArg"></param>
    /// <returns></returns>
    private RaycastHit[] FireRaycast(Transform castOriginArg, float castRangeArg, LayerMask castMaskArg, 
        bool penetrateThroughAllArg)
    {
        // initialize var for upcoming conditionals
        RaycastHit[] raycastHits;

        // if raycast will penetrate through all in it's path
        if (penetrateThroughAllArg)
        {
            // fire raycast and get ALL objects hit
            raycastHits = Physics.RaycastAll(castOriginArg.position, castOriginArg.forward,
                castRangeArg, castMaskArg);
        }
        // else raycast will stop at first hit
        else
        {
            // initialize the results of the raycast contact
            RaycastHit raycastHit;
            // if a fired raycast did NOT hit something
            if (!Physics.Raycast(castOriginArg.position, castOriginArg.forward, out raycastHit,
                castRangeArg, castMaskArg))
            {
                //DONT continue code
                return null;
            }

            // initialize ray hit array with one entry
            raycastHits = new RaycastHit[1];
            // set array entry to the fired ray hit
            raycastHits[0] = raycastHit;
        }

        return raycastHits;
    }*/

    #endregion




    #region Input Functions

    /// <summary>
    /// Execute an interaction if appropriate input is given by player. 
    /// Call in Update().
    /// </summary>
    private void InteractIfPlayerInputted()
    {
        // if appropriaye input given by player
        if (Input.GetKeyDown(KeyCode.E))
        {
            // interact with the interaction target
            Interact();
        }
    }

    #endregion


}
