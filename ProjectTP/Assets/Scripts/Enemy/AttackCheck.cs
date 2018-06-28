using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().PlayerDie();
        }
        if (col.gameObject.tag == "BoxWood")
        {
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "Spines")
        {
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "SavePoint")
        {
            Destroy(col.gameObject);
            GameObject.Find("GameController").GetComponent<GameController>().SavePointDestroy(col.transform.position);
        }
    }
}
