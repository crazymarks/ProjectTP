using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followItem;

    void Update()
    {
        Vector3 followPos = followItem.transform.position;
        this.transform.position = new Vector3(followPos.x,followPos.y,this.transform.position.z);
    }
   

}
