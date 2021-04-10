﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkObjectPooler : MonoBehaviour
{
    [Header("Settings")]
    public int startSize = 5;
    public int maxSize = 20;
    public GameObject prefab;

    [Header("Debug")]
    [SerializeField] Queue pool;
    [SerializeField] int currentCount;


    void Start()
    {
        InitializePool();

        ClientScene.RegisterPrefab(prefab, SpawnHandler, UnspawnHandler);
    }

    void OnDestroy()
    {
        ClientScene.UnregisterPrefab(prefab);
    }

    private void InitializePool()
    {
        pool = new Queue();
        for (int i = 0; i < startSize; i++)
        {
            GameObject next = CreateNew();

            pool.Enqueue(next);
        }
    }

    GameObject CreateNew()
    {
        if (currentCount > maxSize)
        {
            Debug.LogError($"Pool has reached max size of {maxSize}");
            return null;
        }

        // use this object as parent so that objects dont crowd hierarchy
        GameObject next = Instantiate(prefab, transform);
        next.name = $"{prefab.name}_pooled_{currentCount}";
        next.SetActive(false);
        currentCount++;
        return next;
    }

    // used by ClientScene.RegisterPrefab
    GameObject SpawnHandler(SpawnMessage msg)
    {
        return GetFromPool(msg.position, msg.rotation);
    }

    // used by ClientScene.RegisterPrefab
    void UnspawnHandler(GameObject spawned)
    {
        PutBackInPool(spawned);
    }

    /// 
    /// Used to take Object from Pool.
    /// Should be used on server to get the next Object
    /// Used on client by ClientScene to spawn objects
    /// 
    /// 
    /// 
    /// 
    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        /*GameObject next = pool.Count > 0
            ? pool.Dequeue() // take from pool
            : CreateNew(); // create new because pool is empty*/ // =====ORIGINAL CODE LINES HAVING ERRORS====
        GameObject next = (GameObject)(pool.Count > 0 ? pool.Dequeue() : CreateNew()); // =====CUSTOM FIX CODE LINE=====

        // CreateNew might return null if max size is reached
        if (next == null) { return null; }

        // set position/rotation and set active
        next.transform.position = position;
        next.transform.rotation = rotation;
        next.SetActive(true);
        return next;
    }

    /// 
    /// Used to put object back into pool so they can b
    /// Should be used on server after unspawning an object
    /// Used on client by ClientScene to unspawn objects
    /// 
    /// 
    public void PutBackInPool(GameObject spawned)
    {
        // disable object
        spawned.SetActive(false);

        // add back to pool
        pool.Enqueue(spawned);
    }


}
