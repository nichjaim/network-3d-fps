using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavenSubMenuController : SubMenuController
{
    #region Class Variables

    [Header("Haven Sub Menu References")]

    [SerializeField]
    private Sprite bgSprite = null;

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        ((HavenMenuController)menuMaster).BackgroundTransition(bgSprite, 1f);
    }

    #endregion


}
