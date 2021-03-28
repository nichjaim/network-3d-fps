using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    #region Class Variables

    // bool that denotes if this teleporter should currently ne working
    [SerializeField]
    private bool isActive = true;

    // the destination that objects will be sent too
    [SerializeField]
    private TeleporterController receivingTeleporter = null;

    /// list of objects expected to be teleported here, this is kept track of to ensure 
    /// incoming objects are not just immeately sent back
    private List<GameObject> incomingTeleportedObjects = new List<GameObject>();

    #endregion




    #region MonoBehaviour Functions

    private void OnTriggerEnter(Collider other)
    {
        // teleport the given object if appropriate
        TeleportAttempt(other.gameObject);
    }

    #endregion




    #region Teleporter Functions

    /// <summary>
    /// Teleport the given object if appropriate.
    /// </summary>
    private void TeleportAttempt(GameObject teleportingObjectArg)
    {
        // if teleporation process should NOT be executed on the given object
        if (!ShouldTeleport(teleportingObjectArg))
        {
            // DONT continue code
            return;
        }

        // notify teleporter destination that the given object is coming and don't just immidately sent it back
        receivingTeleporter.NotifyIncomingTeleportingObject(teleportingObjectArg);
        // moves the given object to the receiving teleporter
        MoveObjectToReceivingTeleporter(teleportingObjectArg);

        // trigger event that denotes that teleporation was executed
        TeleportingEvent.Trigger(this, teleportingObjectArg);
    }

    /// <summary>
    /// Returns bool that denotes if a teleportation process should be executed on the given object.
    /// </summary>
    /// <param name="teleportingObjectArg"></param>
    /// <returns></returns>
    private bool ShouldTeleport(GameObject teleportingObjectArg)
    {
        /// return bool denoting that SHOULD teleport if: 
        /// - this teleport is currently active
        /// - the object is NOT being expected (was NOT sent to this teleport from another teleporter).
        /// - teleport destination is setup.
        /// - given object is teleportable.
        return isActive 
            && !ProcessTeleportingObjectIfExpected(teleportingObjectArg)
            && receivingTeleporter != null
            && IsTeleportable(teleportingObjectArg);
    }

    /// <summary>
    /// Removes the object from the incoming teleport list if expected.
    /// Returns true if was expected and false if not.
    /// </summary>
    /// <param name="teleportingObjectArg"></param>
    /// <returns></returns>
    private bool ProcessTeleportingObjectIfExpected(GameObject teleportingObjectArg)
    {
        // check if expecting this object
        GameObject incomingPortObj = incomingTeleportedObjects.Find(
            iterObj => iterObj == teleportingObjectArg);
        // if this object is being expected (and thus should NOT be immedately sent back)
        if (incomingPortObj != null)
        {
            // remove the teleporting object from list of incoming objects
            incomingTeleportedObjects.Remove(incomingPortObj);
            // denote that given object WAS expected
            return true;
        }
        // else object NOT expected
        else
        {
            // denote that given object was NOT expected
            return false;
        }
    }

    /// <summary>
    /// Returns bool that denotes if given object is teleportable.
    /// </summary>
    /// <param name="teleportingObjectArg"></param>
    /// <returns></returns>
    private bool IsTeleportable(GameObject teleportingObjectArg)
    {
        return teleportingObjectArg.CompareTag("Player") &&
            teleportingObjectArg.GetComponent<CharacterMasterController>() != null;
    }

    /// <summary>
    /// Moves the given object to the receiving teleporter.
    /// </summary>
    /// <param name="objectToMoveArg"></param>
    private void MoveObjectToReceivingTeleporter(GameObject objectToMoveArg)
    {
        // get a character controller component from the given object
        CharacterController charContr = objectToMoveArg.GetComponent<CharacterController>();
        // if given object is governed by a character controller component
        if (charContr != null)
        {
            /// turn OFF the character controller component before moving the object, this is done because 
            /// this component has it's own way of calculating the object's appropriate positioning.
            charContr.enabled = false;

            // set the teleporting object's position and rotation based on the teleport destination
            objectToMoveArg.transform.SetPositionAndRotation(receivingTeleporter.transform.position,
                receivingTeleporter.transform.rotation);

            // turn back ON the component
            charContr.enabled = true;
        }
        // else given object does NOT have a character controller component attached
        else
        {
            // set the teleporting object's position and rotation based on the teleport destination
            objectToMoveArg.transform.SetPositionAndRotation(receivingTeleporter.transform.position,
                receivingTeleporter.transform.rotation);
        }
    }

    /// <summary>
    /// Sets the teleportation destination.
    /// </summary>
    /// <param name="receivingTeleporterArg"></param>
    public void LinkReceivingTeleporter(TeleporterController receivingTeleporterArg)
    {
        receivingTeleporter = receivingTeleporterArg;
    }

    /// <summary>
    /// Adds the given object to list of expected objects so that the transported object is not 
    /// immdiately sent back.
    /// </summary>
    /// <param name="teleportingObjectArg"></param>
    public void NotifyIncomingTeleportingObject(GameObject teleportingObjectArg)
    {
        incomingTeleportedObjects.Add(teleportingObjectArg);

        // ensure object is eventually removed from expected list
        ProcessTeleportingObjectIfExpectedAfterDelay(teleportingObjectArg);
    }

    /// <summary>
    /// Removes the object from the incoming teleport list if expected after a small delay.
    /// This is to ensure that a object's expectedness does not last longer than should be possible 
    /// which could cause the teleporter to not respond to them when it should.
    /// Call in NotifyIncomingTeleportingObject().
    /// </summary>
    /// <param name="teleportingObjectArg"></param>
    private void ProcessTeleportingObjectIfExpectedAfterDelay(GameObject teleportingObjectArg)
    {
        StartCoroutine(ProcessTeleportingObjectIfExpectedAfterDelayInternal(teleportingObjectArg));
    }

    private IEnumerator ProcessTeleportingObjectIfExpectedAfterDelayInternal(GameObject teleportingObjectArg)
    {
        // wait a short time
        yield return new WaitForSeconds(1f);

        // process given object if not already processed
        ProcessTeleportingObjectIfExpected(teleportingObjectArg);
    }

    #endregion


}
