using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public GameObject objectToSpawn;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //create a queue of coin gameobjects
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //create gameobjects 
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                //add to the end of queue
                objectPool.Enqueue(obj);
            }

            //add newly created obj to dict 
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    //spawn object onto world
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }
        //pull out first element in queue
        // GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn = poolDictionary[tag].Dequeue();

        //enable object and set pos and rotation
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;


        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }        

        //add object back to queue so we can reuse later 
        //poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
