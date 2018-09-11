using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    public GameObject wKey;  //Wという図

    void OnTriggerEnter2D(Collider2D col)//挟むによる死亡
    {
        if (col.tag == "Trigger" || 
            col.tag =="SavePoint"||
            col.tag=="Lever"||
            col.tag=="Button"||
            col.tag=="Ladder"||
            col.tag=="Guideboard")
        {
            return;
        }
        GetComponentInParent<PlayerController>().PlayerDie();
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Guideboard"||
            col.gameObject.tag=="Lever")
        {
            wKey.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Guideboard"||
            col.gameObject.tag == "Lever")
        {
            wKey.SetActive(false);
        }
    }
}
