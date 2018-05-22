using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject switchButton;
    public bool isOpen;

    private Vector3 oldPosition;
	// Use this for initialization
	void Start () {
        oldPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (isOpen == true)
        {
            float step = 5 * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, oldPosition-new Vector3(0,gameObject.GetComponent<BoxCollider2D>().size.y*gameObject.transform.localScale.y,0), step);
        }
        else
        {
            float step = 5 * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, oldPosition , step);
        }

	}
}
