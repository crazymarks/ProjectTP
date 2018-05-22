using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public Sprite buttonOn;
    public Sprite buttonOff;

    [HideInInspector]
    public bool isOpen = false;

    void FixedUpdate()
    {
            if (isOpen == true)
            {
                GetComponent<SpriteRenderer>().sprite = buttonOn;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = buttonOff;
            }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name!="ShotLens")
        isOpen = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name != "ShotLens")
            isOpen = false;
    }
}
