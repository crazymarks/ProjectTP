using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spines : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "BoxWood")
        {
            Destroy(col.gameObject);
        }
    }
}
