using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {
    public int INDEX_reborn=0;
    GameObject gameController;
    GameObject shotLens;
    public bool faceRight = true;

    void Start()
    {
        gameController = GameObject.Find("GameController");
        shotLens = GameObject.Find("ShotLens");
    }

    void OnTriggerEnter2D(Collider2D TempObject)  //save
    {
        if (TempObject.gameObject.tag == "Player")
        {
            gameController.GetComponent<GameController>().PlayerSave(this.transform.position,faceRight);
            shotLens.GetComponent<ShotLensController>().ShotClear();
        }    
    }

    void OnTriggerExit2D(Collider2D TempObject)  //save
    {
        if (TempObject.gameObject.tag == "Player")
        {
            shotLens.GetComponent<ShotLensController>().ShotRecovery();
        }
    }
}
