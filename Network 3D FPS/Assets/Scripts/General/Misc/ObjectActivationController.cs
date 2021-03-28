using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectActivationController : MonoBehaviour
{
    #region Class Variables

    private List<Component> allObjectComponents;
    private List<Component> allObjectColliders;

    private List<GameObject> allChildObjects;

    [SerializeField]
    private List<MonoBehaviour> ignoredObjectComponents = new List<MonoBehaviour>();

    [SerializeField]
    private List<GameObject> ignoredChildObjects = new List<GameObject>();

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup the activation object component list
        InitializeObjectComponentList();
        // setup the activation object collider list
        InitializeObjectColliderList();

        // setup the activation child object list
        InitializeChildObjectList();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the activation object component list. 
    /// Call in Awake().
    /// </summary>
    private void InitializeObjectComponentList()
    {
        // get all the components of this object without any checks
        allObjectComponents = GetComponents(typeof(MonoBehaviour)).ToList();
        // remove all obj activators from the list
        allObjectComponents.RemoveAll(iterComp => (iterComp as ObjectActivationController) != null);
        // remove all object component that should be ignored
        allObjectComponents.RemoveAll(iterComp => ignoredObjectComponents.Exists(
            iterIgnrComp => iterIgnrComp == iterComp));
    }

    /// <summary>
    /// Sets up the activation object collider list. 
    /// Call in Awake().
    /// </summary>
    private void InitializeObjectColliderList()
    {
        // get all the colliders of this object without any checks
        allObjectColliders = GetComponents(typeof(Collider)).ToList();
    }

    /// <summary>
    /// Sets up the activation child object list. 
    /// Call in Awake().
    /// </summary>
    private void InitializeChildObjectList()
    {
        // set child obj list as fresh empty list
        allChildObjects = new List<GameObject>();
        // loop through all of this object's child objects
        foreach (Transform childTrans in transform)
        {
            // if the iterating transform's object is NOT one of the ignored objects
            if (!(ignoredChildObjects.Any(iterObj => iterObj == childTrans.gameObject)))
            {
                // add the iterating non-ignored child object to list
                allChildObjects.Add(childTrans.gameObject);
            }
        }
    }

    #endregion




    #region Activation Functions

    /// <summary>
    /// Sets the component and child object activation absed on given bool.
    /// </summary>
    /// <param name="shouldActivateArg"></param>
    public void SetObjectActivation(bool shouldActivateArg)
    {
        // loop through all object list components
        foreach (MonoBehaviour iterComp in allObjectComponents)
        {
            // set component activation based on given bool
            iterComp.enabled = shouldActivateArg;
        }

        // loop through all object list colliders
        foreach (Collider iterCldr in allObjectColliders)
        {
            // set component activation based on given bool
            iterCldr.enabled = shouldActivateArg;
        }

        // loop through all child objects
        foreach (GameObject iterObj in allChildObjects)
        {
            // set child object activation based on given bool
            iterObj.SetActive(shouldActivateArg);
        }
    }

    #endregion


}
