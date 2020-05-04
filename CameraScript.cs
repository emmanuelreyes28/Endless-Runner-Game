using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        //set camera distance between ball and camera
        distance = this.transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //cameras position following ball
        this.transform.position = player.transform.position + distance;
    }
}
