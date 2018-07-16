using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guideboard : MonoBehaviour {
    private bool canRead = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player" )
        {
            canRead = true;
        }
    }
    
}
