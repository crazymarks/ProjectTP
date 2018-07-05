using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearZone : MonoBehaviour {
    public string stageName;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.Find("GameController").GetComponent<GameController>().ResetCreated();
            Destroy(GameObject.Find("GameController"));
            GameObject.Find("FadeManager").GetComponent<FadeManager>().Fade(stageName, 1f);
        }

    }
}
