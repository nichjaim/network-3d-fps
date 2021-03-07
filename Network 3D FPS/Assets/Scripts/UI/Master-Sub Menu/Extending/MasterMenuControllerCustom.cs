using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;

public class MasterMenuControllerCustom : MasterMenuController
{
    #region Override Functions

    protected override void SetMenuActivation(bool activeArg)
    {
        base.SetMenuActivation(activeArg);

        // trigger event to denote that menu is opening/closing
        MenuActivationEvent.Trigger(this, activeArg);
    }

    #endregion


}
