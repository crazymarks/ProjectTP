using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spines : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().PlayerDie();
        }
        if(col.gameObject.tag == "BoxWood")
        {
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "MovingItem"&&(this.transform.localPosition.y-1.3f)>col.transform.localPosition.y)
        {
            this.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation);

        }
        if (col.gameObject.tag == "Destroyer")
        {
            Destroy(this.gameObject);
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MovingItem")
        {
            this.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX) | (RigidbodyConstraints2D.FreezeRotation);

        }
    }
}
