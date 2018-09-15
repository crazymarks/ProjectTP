using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {
    public Sprite SavePointOpen;
    public Sprite SavePointOff;
    public int INDEX_reborn=0;
    GameObject gameController;
    GameObject shotLens;
    public bool faceRight = true;


    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = SavePointOpen;
        gameController = GameObject.Find("GameController");
        shotLens = GameObject.Find("ShotLens");
    }

    void OnTriggerEnter2D(Collider2D TempObject)  //save
    {
        if (TempObject.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = SavePointOff;
            gameController.GetComponent<GameController>().PlayerSave(this.transform.position,faceRight);
            shotLens.GetComponent<ShotLensController>().ShotClear();
            GameObject.Find("UIcontroller").GetComponent<UIcontroller>().AutosaveShow();
        }    
    }

    void OnTriggerExit2D(Collider2D TempObject)  //save
    {
        if (TempObject.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = SavePointOpen;
            shotLens.GetComponent<ShotLensController>().ShotRecovery();

        }
    }
}
