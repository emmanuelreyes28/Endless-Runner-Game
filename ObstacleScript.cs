 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0,1,0));
    }
}
