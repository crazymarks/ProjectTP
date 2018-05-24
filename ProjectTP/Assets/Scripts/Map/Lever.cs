using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
    public Sprite levelOn;
    public Sprite levelOff;
    public GameObject Door;

    [HideInInspector]
    public bool isOpen=false;
    bool canPull = false;
    public int idNumber = 1;//ドアの何番目のスイッチ

    void Start()
    {
        Door.SendMessage("SwitchHandleOff", idNumber);
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Pull")&&canPull==true)
        {
            isOpen = !isOpen;
            if (isOpen == true)
            {
                GetComponent<SpriteRenderer>().sprite = levelOn;
                Door.SendMessage("SwitchHandleOn", idNumber);
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = levelOff;
                Door.SendMessage("SwitchHandleOff", idNumber);
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
