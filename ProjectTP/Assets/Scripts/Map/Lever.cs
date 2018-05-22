using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
    public Sprite levelOn;
    public Sprite levelOff;

    [HideInInspector]
    public bool isOpen=false;
    bool canPull = false;

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Pull")&&canPull==true)
        {
            isOpen = !isOpen;
            if (isOpen == true)
            {
                GetComponent<SpriteRenderer>().sprite = levelOn;
            }else
            {
                GetComponent<SpriteRenderer>().sprite = levelOff;
            }

        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            canPull = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag=="Player")
        {
            canPull = false;
        }
    }
}
