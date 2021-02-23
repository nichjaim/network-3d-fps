using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSubMenuController : MonoBehaviour, ISubMenu
{
    #region Class Variables

    [SerializeField]
    protected GameObject menuObject = null;

    protected MasterMenuController menuMaster = null;

    #endregion




    #region Interface Functions

    public virtual void RefreshSubMenu()
    {
        //IMPL in extension
    }

    public virtual void SetMenuObjectActive(bool isActiveArg)
    {
        menuObject.SetActive(isActiveArg);
    }

    public void SetMenuMaster(MasterMenuController menuMasterArg)
    {
        menuMaster = menuMasterArg;
    }

    #endregion


}
