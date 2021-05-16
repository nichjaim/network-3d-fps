using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArenaExitStandardController : LevelArenaExitController
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private SpriteRenderer modelSprite = null;
    [SerializeField]
    private Light emphasisLight = null;

    [Header("Sprite Color Properties")]

    [SerializeField]
    private Color modelSpriteColorActive = Color.white;
    [SerializeField]
    private Color modelSpriteColorInactive = Color.grey;

    #endregion




    #region Override Functions

    protected override void RefreshExitProperties()
    {
        base.RefreshExitProperties();

        // enable light component if exit active, disable if not
        emphasisLight.enabled = isExitActive;

        // if exit is active
        if (isExitActive)
        {
            // set model sprite color to ACTIVE sprite color
            modelSprite.color = modelSpriteColorActive;
        }
        // else the exit is still inactive
        else
        {
            // set model sprite color to INACTIVE sprite color
            modelSprite.color = modelSpriteColorInactive;
        }
    }

    #endregion


}
