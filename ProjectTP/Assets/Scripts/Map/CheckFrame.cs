using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFrame : MonoBehaviour {
    public GameObject buttonOff;
    public GameObject buttonOn;
    public GameObject leverOn;
    public GameObject leverOff;
    
     void OnTriggerEnter2D(Collider2D tempObject)
    {
        if (tempObject.tag == "Lever")
        {
            if (tempObject.GetComponent<Lever>().isOpen == true)
            {
                GameObject tempObject2 = Instantiate(leverOn,tempObject.transform.position,Quaternion.identity);
                tempObject2.AddComponent<Slicer2D>();
                Destroy(tempObject);
            }
            else
            {
                GameObject tempObject2 = Instantiate(leverOff, tempObject.transform.position, Quaternion.identity);
                tempObject2.AddComponent<Slicer2D>();
                Destroy(tempObject);
            }


        }else if(tempObject.tag == "Button")
        {

        }
       
    }
    void OnCollisionEnter2D(Collision2D TempObject)
    {
        Debug.Log(TempObject.gameObject.name);
    }
}
