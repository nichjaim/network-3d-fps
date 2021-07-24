using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nichjaim.MasterSubMenu
{
    public class MasterMenuController : MonoBehaviour
    {
        #region Class Variables

        [Header("Base Master Menu Properties")]

        [Tooltip("UI menu head object acting as master menu.")]
        [SerializeField]
        protected GameObject menuObject = null;

        [Tooltip("Object that parents all the sub menu controller objects.")]
        [SerializeField]
        protected GameObject subMenuControllersParent = null;

        // sub menu controllers held by the sub menu controller parent
        protected List<SubMenuController> _subMenuControllers;

        // is this the first time the menu is being opened
        protected bool isFirstMenuOpening = true;

        #endregion




        #region MonoBehaviour Functions

        protected virtual void Awake()
        {
            // setup list of sub menu controller references
            InitializeSubMenuControllerReferences();
            // sets up the master menu for all sub menus
            SetSubMenusMenuMaster();

            // turn off the master menu
            DeactivateMenu();
        }

        #endregion




        #region Initialization Functions

        /// <summary>
        /// Sets up list of sub menu controller references. 
        /// Call in Awake().
        /// </summary>
        protected void InitializeSubMenuControllerReferences()
        {
            // reset controller list to fresh empty list
            _subMenuControllers = new List<SubMenuController>();

            // initialize var for upcoming loop
            SubMenuController iterSubMenuController;

            // loop through all children of the sub menu controllers parent
            foreach (Transform child in subMenuControllersParent.transform)
            {
                // get sub menu controller component from iterating child object
                iterSubMenuController = child.GetComponent<SubMenuController>();
                // if component found
                if (iterSubMenuController != null)
                {
                    // add component to list of references
                    _subMenuControllers.Add(iterSubMenuController);
                }
                // else component didn't exist on object
                else
                {
                    // print warning to console
                    Debug.LogWarning("Sub menu controller object does not have " +
                        $"SubMenuController component! Name: {child.name}");
                }
            }
        }

        /// <summary>
        /// Sets up the master menu for all sub menus. 
        /// Call in Awake().
        /// </summary>
        protected virtual void SetSubMenusMenuMaster()
        {
            // loop through all sub menu controllers
            foreach (SubMenuController iterContr in _subMenuControllers)
            {
                // setup iterating sub menu's master menu
                iterContr.SetMenuMaster(this);
            }
        }

        #endregion




        #region Menu Functions

        /// <summary>
        /// Switches to the sub menu associated with the given ID.
        /// </summary>
        /// <param name="subMenuIdArg"></param>
        protected virtual void SwitchSubMenu(string subMenuIdArg)
        {
            // activate master menu object
            SetMenuActivation(true);

            // loop through all sub menu controllers
            foreach (SubMenuController iterContr in _subMenuControllers)
            {
                /// if iterating menu should be opened (lowercase both IDs to ensure 
                /// capitalization doesn't affect comparison)
                if (iterContr.GetMenuId().ToLower() == subMenuIdArg.ToLower())
                {
                    /// if iterating menu is closed OR if first time sub-menus being opened. 
                    /// NOTE: need to check if first time because otherwise this menu will 
                    /// likely already be on and thus OnMenuOpen will NOT be called.
                    if (!iterContr.IsMenuOpen() || isFirstMenuOpening)
                    {
                        // open iterating menu
                        iterContr.SetMenuObjectActive(true);
                        // call on open function on iterating menu
                        iterContr.OnMenuOpen();
                    }
                }
                // else iterating menu should be closed
                else
                {
                    // if menu is open
                    if (iterContr.IsMenuOpen())
                    {
                        // close iterating menu
                        iterContr.SetMenuObjectActive(false);
                        // call on close function on iterating menu
                        iterContr.OnMenuClose();
                    }
                }
            }

            // if this was first menu switching
            if (isFirstMenuOpening)
            {
                // denote that NO longer on first menu opening
                isFirstMenuOpening = false;
            }
        }

        /// <summary>
        /// Turn off the master menu.
        /// </summary>
        public virtual void DeactivateMenu()
        {
            // deactivate master menu object
            SetMenuActivation(false);
        }

        /// <summary>
        /// Activates/deactivates the menu object based on given bool.
        /// </summary>
        /// <param name="activeArg"></param>
        protected virtual void SetMenuActivation(bool activeArg)
        {
            // if menu object reference is setup
            if (menuObject != null)
            {
                // deactivate menu object
                menuObject.SetActive(activeArg);
            }
            // else menu object reference was NOT setup
            else
            {
                // print warning to console
                Debug.LogWarning($"Master menu controller does not have a menuObject! Name: {gameObject.name}");
            }
        }

        /// <summary>
        /// Returns bool that denotes if menu is currently open or not.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsMenuOpen()
        {
            // if menu object reference is setup
            if (menuObject != null)
            {
                return menuObject.activeSelf;
            }
            // else menu object reference was NOT setup
            else
            {
                // print warning to console
                Debug.LogWarning($"Master menu controller does not have a menuObject! Name: {gameObject.name}");
                return false;
            }
        }

        #endregion


    }
}
