using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMenuController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private GameObject menuObject = null;

    [Header("Sub Menu Controller Properties")]

    /// gets the sub menu controllers from the child objects of this master menu 
    /// controller object
    [SerializeField]
    private bool SetSubMenuControllersFromChildren = false;
    [SerializeField]
    private List<GameObject> subMenuControllerObjects = new List<GameObject>();

    #endregion




    #region MonoBehaviour Functions

    protected virtual void Start()
    {
        /// sets the sub menu controller object references to the child objects 
        /// if appropriate to do so
        InitializeSubMenuControllerReferencesIfAppropriate();
        // set the important variable data for all sub menus
        InitializeSubMenuData();

        // turn off the master menu
        DeactivateMenu();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets the sub menu controller object references to the child objects 
    /// if appropriate to do so. 
    /// Call in Start().
    /// </summary>
    protected void InitializeSubMenuControllerReferencesIfAppropriate()
    {
        // if sub menu controllers are NOT the children of this object
        if (!SetSubMenuControllersFromChildren)
        {
            // DONT continue code
            return;
        }

        // reset sub menu controller list to empty list
        subMenuControllerObjects = new List<GameObject>();

        // loop through all child objects
        foreach (Transform child in transform)
        {
            // add iterating object to list
            subMenuControllerObjects.Add(child.gameObject);
        }
    }

    /// <summary>
    /// Sets the important variable data for all sub menus. 
    /// Call in Start().
    /// </summary>
    protected void InitializeSubMenuData()
    {
        // initialize this variable for the upcoming loop
        ISubMenu subMenuInterface;
        // loop through all sub menu controller objects
        for (int i = 0; i < subMenuControllerObjects.Count; i++)
        {
            //get sub menu interface component off the iterating object
            subMenuInterface = subMenuControllerObjects[i].GetComponent<ISubMenu>();
            // if there actually was a sub menu interface component on the iterating object
            if (subMenuInterface != null)
            {
                // set sub menu's master menu reference
                subMenuInterface.SetMenuMaster(this);
            }
            // else there was NOT a sub menu interface component on the iterating object
            else
            {
                // print warning to console
                Debug.LogWarning("object named " + subMenuControllerObjects[i].name +
                    " does not have component that implements sub menu interface!");
            }
        }
    }

    #endregion




    #region Menu Functions

    /// <summary>
    /// Switches to the sub menu associated with the given index.
    /// </summary>
    /// <param name="listIndexArg"></param>
    protected void SwitchSubMenu(int listIndexArg)
    {
        // if index argument is outside list range
        if (listIndexArg < 0 || listIndexArg >= subMenuControllerObjects.Count)
        {
            // print warning to console
            Debug.LogWarning("menu index argument is not valid index!");
            // DONT continue code
            return;
        }

        // turn ON master menu object
        menuObject.SetActive(true);

        // initialize this variable for the upcoming loop
        ISubMenu subMenuInterface;
        // loop through all sub menu controller objects
        for (int i = 0; i < subMenuControllerObjects.Count; i++)
        {
            // get sub menu interface component off the iterating object
            subMenuInterface = subMenuControllerObjects[i].GetComponent<ISubMenu>();
            // if there actually was a sub menu interface component on the iterating object
            if (subMenuInterface != null)
            {
                /// if the iterating sub menu controller objects IS the one associated with 
                /// the sub menu being switched to
                if (i == listIndexArg)
                {
                    // turn ON the menu
                    subMenuInterface.SetMenuObjectActive(true);
                    // refresh menu contents
                    subMenuInterface.RefreshSubMenu();
                }
                /// if the iterating sub menu controller objects is NOT the one associated with 
                /// the sub menu being switched to
                else
                {
                    // turn OFF the menu
                    subMenuInterface.SetMenuObjectActive(false);
                }
            }
            // else there was NOT a sub menu interface component on the iterating object
            else
            {
                //print warning to console
                Debug.LogWarning("object named " + subMenuControllerObjects[i].name + 
                    " does not have component that implements sub menu interface!");
            }
        }
    }

    /// <summary>
    /// Turn off the master menu.
    /// </summary>
    public void DeactivateMenu()
    {
        // deactivate menu object
        menuObject.SetActive(false);
    }

    /// <summary>
    /// Returns bool that denotes if menu is currently open or not.
    /// </summary>
    /// <returns></returns>
    protected bool IsMenuOpen()
    {
        return menuObject.activeSelf;
    }

    #endregion


}
