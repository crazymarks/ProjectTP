using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followItem;
    public GameObject targetItem;
    public bool firstMove = true;
    public float speed=0.0f;
    float timecount = 0.0f;
    public float waitTime = 0.0f;

    void Update()
    {
        if (followItem != null&&firstMove==false)
        {
            Vector3 followPos = followItem.transform.position;
            this.transform.position = new Vector3(followPos.x, followPos.y, this.transform.position.z);

        }
        if(firstMove==true){
            if (timecount < waitTime)
            {
                timecount += Time.deltaTime;
            }
            else
            {
                float step = speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetItem.transform.position, step);
            }
        }
    }
    public void setFirstMove()
    {
        firstMove = false;
    }

}
