using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelScript : MonoBehaviour
{
    private Transform playerTransform;
    private float xPos;
    private float yPos;
    private float zPos;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        AvoidSpawningInObstacles();
        
        //disable fuel gameObject if player passes it 
        if(this.transform.position.z + 15 < playerTransform.position.z)
        {
            this.gameObject.SetActive(false);
            SpawnFuel();
        }
        
        if(PlayerController.isFuelDeactivated)
        {
            PlayerController.isFuelDeactivated = false;
            this.gameObject.SetActive(false);
            SpawnFuel();
        }

        transform.Rotate(0, 1, 0);
    }

    private void SpawnFuel()
    {
        xPos = Random.Range(-4.5f, 4.5f);
        yPos = this.transform.position.y;
        zPos = playerTransform.position.z + Random.Range(40, 80);
        
        //enable fuel gameObject 
        this.gameObject.SetActive(true);

        //new spawn position 
        this.transform.position = new Vector3(xPos, yPos, zPos);
    }

    private void AvoidSpawningInObstacles()
    {
        this.gameObject.SetActive(true);
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 0.04f);

        //if fuel spawns in obstacle or coin, respawn elsewhere 
        foreach (Collider hit in hits)
        {
            if(hit.tag == "Obstacle" || hit.tag == "coin")
            {
                this.gameObject.SetActive(false);
                SpawnFuel();
            }
            
        }
    }
}
