using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
   void OnTriggerEnter2D(Collider2D col)//挟むによる死亡
    {
        if (col.tag == "Trigger" || 
            col.tag =="SavePoint"||
            col.tag=="Lever"||
            col.tag=="Button"||
            col.tag=="Ladder")
        {
            return;
        }
        GetComponentInParent<PlayerController>().PlayerDie();
    }
}
