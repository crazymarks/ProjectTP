using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour {

    void OnTriggerStay2D(Collider2D col)//プレイヤは大地に立つ
    {
        if (col.tag == "Trigger" ||
            col.tag == "SavePoint" ||
            col.tag == "Lever" ||
            col.tag == "Button"||
             col.tag == "Ladder")           
        {
            return;
        }
        GetComponentInParent<PlayerController>().ResumeJump();
    }
    void OnTriggerEnter2D(Collider2D col)//プレイヤは大地に立つ
    {
        if (col.tag == "Ladder" && this.transform.position.y > (col.transform.position.y + col.GetComponent<BoxCollider2D>().size.y / 2))
        {
            GetComponentInParent<PlayerController>().ResumeJump();
        }     
    }
}
