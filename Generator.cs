using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    PoolManager poolManager;
    private Transform playerTransform;
    float xPos;
    float yPos;
    float zPos;

    private void Start()
    {
        poolManager = PoolManager.Instance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        xPos = Random.Range(-4.5f, 4.5f);
        yPos = 0.55f;
        zPos = playerTransform.position.z + Random.Range(42f, 84f);

        Vector3 pos = new Vector3(xPos, yPos, zPos);

        GameObject coin = poolManager.SpawnFromPool("coin", pos, Quaternion.identity);
    }

}
