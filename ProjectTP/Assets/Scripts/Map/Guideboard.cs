﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guideboard : MonoBehaviour {
    public GameObject Readboard;
    bool canRead = false;
    bool isReading = false;
    GameObject ShotLens;
    GameObject UIController;

    private void Start()
    {
        ShotLens = GameObject.Find("ShotLens");
        UIController = GameObject.Find("UIcontroller");
    }
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Pull") && canRead == true
            && GameObject.Find("Player").GetComponent<PlayerController>().canMove == true)
        {
            if (Readboard != null)
            {
                Readboard.SetActive(true);
                Time.timeScale = 0;
                canRead = false;
                isReading = true;
                ShotLens.GetComponent<ShotLensController>().canWork = false;
                UIController.GetComponent<UIcontroller>().setReading (isReading);
            }

        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && isReading == true)
        {
            if (Readboard != null)
            {
                Readboard.SetActive(false);
                Time.timeScale = 1;
                isReading = false;
                canRead = true;
                ShotLens.GetComponent<ShotLensController>().canWork = true;
                UIController.GetComponent<UIcontroller>().setReading(isReading);
            }

        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            canRead = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            canRead = false;
        }
    }

}
