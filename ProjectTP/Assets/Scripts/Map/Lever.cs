using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
    public Sprite levelOn;
    public Sprite levelOff;
    public GameObject target;

    [HideInInspector]
    public bool isOpen=false;
    bool canPull = false;

    void Start()
    {
        target.SendMessage("SwitchHandle", this.gameObject);
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Pull")&&canPull==true)
        {
            isOpen = !isOpen;
            if (isOpen == true)
            {
                GetComponent<SpriteRenderer>().sprite = levelOn;
            }
            else
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
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "MovingItem" && (this.transform.localPosition.y - 1.3f) > col.transform.localPosition.y)
        {
            this.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation);
        }
        if (col.gameObject.tag == "Destroyer" && (this.transform.localPosition.y - 1.3f) > col.transform.localPosition.y)
        {
            this.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation);
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MovingItem"|| col.gameObject.tag == "Destroyer")
        {
            this.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX) | (RigidbodyConstraints2D.FreezeRotation);

        }
    }
}
