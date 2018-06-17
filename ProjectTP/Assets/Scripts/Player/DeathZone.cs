using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
   void OnTriggerEnter2D(Collider2D col)//挟むによる死亡
    {
        GetComponentInParent<PlayerController>().PlayerDie();
    }
}
