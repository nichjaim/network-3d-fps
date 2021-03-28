using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSeedManager : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private int seed = 0;
    public int Seed
    {
        get { return seed; }
    }

    [SerializeField]
    private bool initSeedOnAwake = false;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // if supposed to initialize seed on Awake
        if (initSeedOnAwake)
        {
            // setup gam seed with current seed
            SetupSeed(seed);
        }
    }

    #endregion




    #region Seed Functions

    /// <summary>
    /// Sets up game seed based on given seed.
    /// </summary>
    /// <param name="seedArg"></param>
    public void SetupSeed(int seedArg)
    {
        // set game seed to given seed
        seed = seedArg;

        // setup the randomizer's state based on current seed
        InitializeRandomSeedState();
    }

    /// <summary>
    /// Sets up game seed based on given seed.
    /// </summary>
    /// <param name="seedArg"></param>
    public void SetupSeed(string seedArg)
    {
        // set game seed to hash of given string
        seed = seedArg.GetHashCode();

        // setup the randomizer's state based on current seed
        InitializeRandomSeedState();
    }

    /// <summary>
    /// Sets up the randomizer's state based on current seed.
    /// </summary>
    private void InitializeRandomSeedState()
    {
        Random.InitState(seed);
    }

    /// <summary>
    /// Sets up game seed to random seed. 
    /// Returns the random seed.
    /// </summary>
    /// <returns></returns>
    public int RandomizeSeed()
    {
        // setup a random seed
        SetupSeed((int)System.DateTime.Now.Ticks);

        // return the random set seed
        return seed;
    }

    #endregion


}
