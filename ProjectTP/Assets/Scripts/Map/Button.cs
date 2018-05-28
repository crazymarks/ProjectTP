using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public Sprite buttonOn;
    public Sprite buttonOff;
    public GameObject target;

    [HideInInspector]
    public bool isOpen = false;
    private bool isStay = false;
    private int count = 0;

    void Start()
    {
        target.SendMessage("SwitchHandle", this.gameObject);
    }

    void FixedUpdate()
    {
        if (isStay == false)
        {
            count++;
            if (count == 5)
            {
                isOpen = false;
            }
        }
        else
        {
            count = 0;
        }

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
        if (col.gameObject.name != "ShotLens" && col.gameObject.name != "Overlap")
        {
            isStay = true;
            isOpen = true;
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name != "ShotLens" && col.gameObject.name != "Overlap")
            isStay = false;
    }
}
