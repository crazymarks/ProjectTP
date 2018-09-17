using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearZone : MonoBehaviour {
    public string stageName;
    GameObject GameController;
    GameObject FadeManager;

    private void Start()
    {
        GameController = GameObject.Find("GameController");
        FadeManager = GameObject.Find("FadeManager");

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (GameObject.Find("GameController") != null)
            {
                GameObject.Find("GameController").GetComponent<GameController>().ResetCreated();
                Destroy(GameObject.Find("GameController"));

            }
            FadeManager.GetComponent<FadeManager>().Fade(stageName, 1f);
        }

    }
}
