using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Class Variables

    public static UIManager Instance;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup singelton instance
        InstantiateInstance();
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

    #endregion
}
