using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScript : MonoBehaviour, IPooledObject
{
    PoolManager poolManager;
    private Transform playerTransform;
    private GameObject[] coinArray;
    private float minDistance = 2f;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        poolManager = PoolManager.Instance;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        coinArray = poolManager.poolDictionary["coin"].ToArray();

    }
    public void OnObjectSpawn()
    {
        //turn coin right side up
        transform.Rotate(90.0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //if player passes coin add coin back to queue 
        if(this.transform.position.z + 10 < playerTransform.position.z)
        {
            this.gameObject.SetActive(false);
            poolManager.poolDictionary["coin"].Enqueue(this.gameObject);
        }

        AvoidSpawningTooClose();
        
        AvoidSpawningInObstacles();
    }

    void AvoidSpawningTooClose()
    {   
        
        for(int i = 0; i < coinArray.Length; i++)
        { 
            distance = Vector3.Distance(this.transform.position, coinArray[i].transform.position);

            //if distnace is too close to another coin send back to queue
            if(distance < minDistance)
            {
                this.gameObject.SetActive(false);
                poolManager.poolDictionary["coin"].Enqueue(this.gameObject);
            }
        }
    }

    void AvoidSpawningInObstacles()
    {
        //array of colliders
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 2.0f);
        
        foreach(Collider collider in hitColliders)
        {
            //if coin spawns in obstacle 
            if(collider.tag == "Obstacle")
            {
                //deactivate and add add back to queue 
                this.gameObject.SetActive(false);
                poolManager.poolDictionary["coin"].Enqueue(this.gameObject);
            }
        }
    }

    void OnTriggerEnter()
    {
        //add coin back to pool to get reused
        poolManager.poolDictionary["coin"].Enqueue(this.gameObject);
    }
}
