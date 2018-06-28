using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {
    public int INDEX_reborn=0;
    GameObject gameController;

    void Start()
    {
        gameController = GameObject.Find("GameController");
    }

    void OnTriggerEnter2D(Collider2D TempObject)  //save
    {
        if (TempObject.gameObject.tag == "Player")
        {
            gameController.GetComponent<GameController>().PlayerSave(this.transform.position);
            GameObject.Find("ShotLens").GetComponent<ShotLensController>().ShotClear();
        }    
    }

    void OnTriggerExit2D(Collider2D TempObject)  //save
    {
        if (TempObject.gameObject.tag == "Player")
        {
            GameObject.Find("ShotLens").GetComponent<ShotLensController>().ShotRecovery();
        }
    }
}
