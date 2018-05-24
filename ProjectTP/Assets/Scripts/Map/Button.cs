using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public Sprite buttonOn;
    public Sprite buttonOff;
    public GameObject Door;
    public int idNumber = 1;//ドアの何番目のスイッチ

    [HideInInspector]
    public bool isOpen = false;

    void FixedUpdate()
    {
            if (isOpen == true)
            {
                GetComponent<SpriteRenderer>().sprite = buttonOn;
                Door.SendMessage("SwitchHandleOn",idNumber);
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = buttonOff;
                Door.SendMessage("SwitchHandleOff", idNumber);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name!="ShotLens"&&col.gameObject.name!="Overlap")
        isOpen = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
            isOpen = false;
    }
}
