using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //START OF CLASS VARIABLES===========================================
    public static ObjectPooler Instance;

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    //END OF CLASS VARIABLES=============================================



    //START OF POOL CLASS================================================
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    //END OF POOL CLASS==================================================



    //START OF UNITY FUNCTIONS===========================================
    private void Awake()
    {
        InstantiateInstance();
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializePoolDictionary();
    }
    //END OF UNITY FUNCTIONS=============================================



    //START OF INITIALIZATION FUNCTIONS==================================
    private void InstantiateInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void InitializePoolDictionary()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject instantiatedObject = Instantiate(pool.prefab);
                instantiatedObject.SetActive(false);

                instantiatedObject.transform.SetParent(this.gameObject.transform); //this line not in original.

                objectPool.Enqueue(instantiatedObject);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    //END OF INITIALIZATION FUNCTIONS====================================



    //START OF SPAWNER FUNCTIONS=========================================
    public GameObject SpawnFromPool(string tagArg, Vector3 positionArg, Quaternion rotationArg)
    {
        if (!poolDictionary.ContainsKey(tagArg))
        {
            Debug.LogWarning("Pool with tag "+ tagArg + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tagArg].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = positionArg;
        objectToSpawn.transform.rotation = rotationArg;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[tagArg].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    //END OF SPAWNER FUNCTIONS===========================================


}
