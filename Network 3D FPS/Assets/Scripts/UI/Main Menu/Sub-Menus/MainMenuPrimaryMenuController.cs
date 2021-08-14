using Mirror;
using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPrimaryMenuController : SubMenuController
{
    #region Class Variables

    private NetworkManagerCustom _networkManagerCustom = null;

    [Header("Primary Menu Properties")]

    [Tooltip("The warning image object that will be turned on/off based on if " +
        "H-Content is NOT installed.")]
    [SerializeField]
    private GameObject isHcontentNotInstalledIconWarningObject = null;

    [Tooltip("The warning image object that will be turned on/off based on if " +
        "H-Content is disabled.")]
    [SerializeField]
    private GameObject isHcontentDisabledIconWarningObject = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup reference vars to their respective singletons
        InitializeSingetonReferences();
    }

    #endregion




    #region Initializatioon Functions

    /// <summary>
    /// Sets up reference vars to their respective singletons. 
    /// Call in Start() so as to give time for singleton instances to actually instantiate.
    /// </summary>
    private void InitializeSingetonReferences()
    {
        _networkManagerCustom = (NetworkManagerCustom)NetworkManager.singleton;
    }

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        // turn on/off the warning icon objects associated with H-content
        RefreshHcontentIconObjectActivations();
    }

    #endregion




    #region Primary Menu Functions

    /// <summary>
    /// Turn on/off the warning icon objects associated with H-content.
    /// </summary>
    private void RefreshHcontentIconObjectActivations()
    {
        // get whether the H-content files are installed
        bool isHcontentInstalled = AssetRefMethods.IsHcontentInstalled();

        // if H-content is NOT installed then turn this warning icon ON
        isHcontentNotInstalledIconWarningObject.SetActive(!isHcontentInstalled);

        // if the H-content is NOT installed
        if (!isHcontentInstalled)
        {
            /// turn OFF the access warning object as it's not relevant if the H-content 
            /// files aren't even installed.
            isHcontentNotInstalledIconWarningObject.SetActive(false);

            // DONT continue code
            return;
        }

        // if net manager reference has NOT been setup yet
        if (_networkManagerCustom == null)
        {
            // setup reference vars to their respective singletons
            InitializeSingetonReferences();
        }

        // if access to H-Content is DISabled then turn this warning icon ON
        isHcontentDisabledIconWarningObject.SetActive(!_networkManagerCustom.IsHcontentAccessEnabled());
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressPlay()
    {
        // switch main menu's sub menu to the play menu
        ((MainMenuController)menuMaster).SwitchToPlayMenu();
    }

    public void ButtonPressOptions()
    {
        Debug.LogWarning("Options button not implemented!"); // NEED IMPL
    }

    public void ButtonPressExit()
    {
        Debug.LogWarning("Exit button not implemented!"); // NEED IMPL
    }

    #endregion


}
