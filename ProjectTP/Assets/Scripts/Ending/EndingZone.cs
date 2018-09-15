using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingZone : MonoBehaviour {
    private GameObject shotlens;

    private void Start()
    {
        shotlens = GameObject.Find("ShotLens");
    }

    void OnTriggerEnter2D(Collider2D TempObject)  
    {
        if (TempObject.gameObject.tag == "Player")
        {
            shotlens.GetComponent<ShotLensController>().ShotClear();

        }
    }
    void OnTriggerExit2D(Collider2D TempObject)  
    {
        if (TempObject.gameObject.tag == "Player")
        {
            shotlens.GetComponent<ShotLensController>().ShotRecovery();
        }
    }
}
