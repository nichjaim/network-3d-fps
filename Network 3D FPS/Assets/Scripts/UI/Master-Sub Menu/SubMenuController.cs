using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nichjaim.MasterSubMenu
{
    public class SubMenuController : MonoBehaviour
    {
        #region Class Variables

        [Header("Base Sub Menu Properties")]

        [Tooltip("UI menu head object acting as sub menu.")]
        [SerializeField]
        protected GameObject menuObject = null;

        [Tooltip("Used by master menu to identify a specific sub menu.")]
        [SerializeField]
        protected string menuId = string.Empty;

        // master menu in charge of this sub menu
        protected MasterMenuController menuMaster = null;

        #endregion




        #region Sub Menu Functions

        /// <summary>
        /// Sets the reference to the master menu in charge of this sub menu.
        /// </summary>
        /// <param name="menuMasterArg"></param>
        public virtual void SetMenuMaster(MasterMenuController menuMasterArg)
        {
            menuMaster = menuMasterArg;
        }

        /// <summary>
        /// Returns the ID that the master menu uses to differentiate sub menus.
        /// </summary>
        /// <returns></returns>
        public virtual string GetMenuId()
        {
            return menuId;
        }

        /// <summary>
        /// Returns bool that denotes if menu is currenlty open.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMenuOpen()
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
                Debug.LogWarning($"Sub menu controller does not have a menuObject! Name: {gameObject.name}");
                // return some default bool
                return false;
            }
        }

        /// <summary>
        /// Activates/deactivates menu based on given bool.
        /// </summary>
        /// <param name="isActiveArg"></param>
        public virtual void SetMenuObjectActive(bool isActiveArg)
        {
            // if menu object reference is setup
            if (menuObject != null)
            {
                menuObject.SetActive(isActiveArg);
            }
            // else menu object reference was NOT setup
            else
            {
                // print warning to console
                Debug.LogWarning($"Sub menu controller does not have a menuObject! Name: {gameObject.name}");
            }
        }

        /// <summary>
        /// Called when this menu is opened.
        /// </summary>
        public virtual void OnMenuOpen()
        {
            // implement in child classes
        }

        /// <summary>
        /// Called when this menu is closed.
        /// </summary>
        public virtual void OnMenuClose()
        {
            // implement in child classes
        }

        #endregion


    }
}
