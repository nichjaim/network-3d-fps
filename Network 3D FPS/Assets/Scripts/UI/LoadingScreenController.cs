using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour, MMEventListener<BundleSystemSetupEvent>
{
    #region Class Variables

    [SerializeField]
    private GameObject loadingScreen = null;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // turn ON the loading screen
        SetLoadingScreenActivation(true);
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Loadiong Screen Functions

    /// <summary>
    /// Turns on/off the loading screen.
    /// </summary>
    /// <param name="activeArg"></param>
    private void SetLoadingScreenActivation(bool activeArg)
    {
        loadingScreen.SetActive(activeArg);
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<BundleSystemSetupEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<BundleSystemSetupEvent>();
    }

    public void OnMMEvent(BundleSystemSetupEvent eventType)
    {
        // turn OFF the loading screen
        SetLoadingScreenActivation(false);
    }

    #endregion


}
