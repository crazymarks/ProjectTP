using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    void  OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().ClimbLadder(this.transform.position.x);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().OutLadder();
        }
    }
}
