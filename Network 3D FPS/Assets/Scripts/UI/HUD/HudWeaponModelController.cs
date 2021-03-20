using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudWeaponModelController : MonoBehaviour
{
    #region Class Variables

    private Image _image = null;
    private Animator _animator = null;

    private WeaponData equippedWeapon = null;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup vars that hold references to relevant components
        InitializeComponentReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up vars that hold references to relevant components. 
    /// Call in Awake().
    /// </summary>
    private void InitializeComponentReferences()
    {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    #endregion




    #region Weapon Model Functions

    /// <summary>
    /// Setup image comp and sets the image's animator based on the equipped weapon.
    /// </summary>
    private void RefreshWeaponModel()
    {
        // initialize weapon aniamtor as null anim
        AnimatorOverrideController wepAnimator = null;
        // if a weapon is equipped
        if (equippedWeapon != null)
        {
            // get equipped weapon's associated HUD model animator
            wepAnimator = AssetRefMethods.LoadBundleAssetHudWeaponModelAnimator(
                equippedWeapon.itemInfo.itemId);
        }

        // turn ON image comp if got valid animator and turn OFF if got null animator
        _image.enabled = wepAnimator != null;

        // if got valid aniamtor
        if (wepAnimator != null)
        {
            // set aniamtor's controller to the one retrieved
            _animator.runtimeAnimatorController = wepAnimator;
        }
    }

    #endregion


}
