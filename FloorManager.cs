using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject[] floorPrefabs;
    //keep track of player 
    private Transform playerTransform;
    //z position of floor 
    private float spawnZ = 0.0f;
    //floor length
    private float floorLength = 12.0f;
    private float safeZone = 15.0f;
    //number of floor prefabs on screen 
    private int floorsOnScreen = 7;
    private int lastPrefabIndex = 0;
    //list of floors
    private List<GameObject> activeFloor;

    // Start is called before the first frame update
    void Start()
    {
        //instatiate list
        activeFloor = new List<GameObject>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        //create first seven floor prefabs
        for(int i = 0; i < floorsOnScreen; i++)
        {
            if(i < 2)
            {
                //spawn empty floor the first two spawns
                SpawnFloor(0);
            }
            else
            {
                SpawnFloor();
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - safeZone > (spawnZ - floorsOnScreen * floorLength))
        {
            SpawnFloor();
            DeleteFloor();
        }
    }

    private void SpawnFloor(int prefabIndex = -1)
    {
        //create/instantiate floor
        GameObject go;
        if(prefabIndex == -1)
        {
            go = Instantiate(floorPrefabs[RandomPrefabIndex()]) as GameObject;
        }
        else
        {
            go = Instantiate(floorPrefabs[prefabIndex]) as GameObject;
        }
        
        go.transform.SetParent(transform);

        //position instantiated floor
        go.transform.position = Vector3.forward * spawnZ;

        //increment spawnZ by length of floor
        spawnZ += floorLength;

        //add newly instantiated floor to list of activeFloors
        activeFloor.Add(go);
    }

    private void DeleteFloor()
    {
        //Destroy and remove floor that player has already passed
        Destroy(activeFloor[0]);
        activeFloor.RemoveAt(0);

    }

    private int RandomPrefabIndex()
    {
        if(floorPrefabs.Length <= 1)
        {
            return 0;
        }

        int randomIndex = lastPrefabIndex;

        //if index is the same, choose a different index randomly
        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, floorPrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
