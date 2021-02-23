using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using BundleSystem;

public class GameManager : MonoBehaviour, MMEventListener<GamePausingActionEvent>
{
    #region Class Variables

    public static GameManager Instance;

    [Header("Performance Properties")]

    [SerializeField]
    private int targetFramerate = 60;

    [Header("Bundle System Properties")]

    [SerializeField]
    private bool showBundleSystemDebugGUI = false;
    [SerializeField]
    private bool logBundleSystemMessages = false;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup singelton instance
        InstantiateInstance();
        // limit FPS to the targeted framerate
        SetupTargetFramerate();
    }

    private void Start()
    {
        // setup the bundle system for use
        InitializeBundleSystem();
    }

    private void OnEnable()
    {
        //starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        //stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the singleton instance. 
    /// Call in Awake().
    /// </summary>
    private void InstantiateInstance()
    {
        // if an instance of this singeton is already setup
        if (Instance != null)
        {
            // destroy this object
            Destroy(gameObject);
        }
        // else no such singleton object exists yet
        else
        {
            // set the singelton isntance to this entity
            Instance = this;
            // ensure this object is not destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Limit FPS to the targeted framerate. 
    /// Call in Awake().
    /// </summary>
    private void SetupTargetFramerate()
    {
        // if trying to target a valid framerate
        if (targetFramerate > 0)
        {
            // limit FPS to the targeted framerate
            Application.targetFrameRate = targetFramerate;
        }
        // else not trying to target a specific framerate
        else
        {
            /// set FPS to special value that indicates that the game 
            /// should render at the platform's default framerate
            Application.targetFrameRate = -1;
        }
    }

    /// <summary>
    /// Sets up the bundle system for use. 
    /// Call in Start().
    /// </summary>
    private void InitializeBundleSystem()
    {
        StartCoroutine(InitializeBundleSystemInternal());
    }

    private IEnumerator InitializeBundleSystemInternal()
    {
        // set whether the bundle system debug UI is shown based on set bool
        BundleManager.ShowDebugGUI = showBundleSystemDebugGUI;
        // set whether the bundle system logs messages based on set bool
        BundleManager.LogMessages = logBundleSystemMessages;

        // initialize the bundle system and wait till finished
        yield return BundleManager.Initialize();

        // trigger event to denote that the bundle system is finished initializing
        BundleSystemSetupEvent.Trigger();
    }

    #endregion




    #region Game Functions

    /// <summary>
    /// Switches the game speed between paused and unpaused, if not in online session.
    /// </summary>
    private void SwitchGamePausingIfAppropriate()
    {
        // if NOT in online session
        if (!NetworkClient.isConnected)
        {
            // if game unpaused
            if (Time.timeScale > 0f)
            {
                // pause game
                Time.timeScale = 0f;
            }
            // else game paused
            else
            {
                // unpause game
                Time.timeScale = 1f;
            }
        }
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<GamePausingActionEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<GamePausingActionEvent>();
    }

    public void OnMMEvent(GamePausingActionEvent eventType)
    {
        // switches the game speed between paused and unpaused, if not in online session
        SwitchGamePausingIfAppropriate();
    }

    #endregion


}
