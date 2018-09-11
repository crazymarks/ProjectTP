using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guideboard : MonoBehaviour {
    public GameObject Readboard;
    bool canRead = false;
    bool isReading = false;
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
                GameObject.Find("ShotLens").GetComponent<ShotLensController>().canWork = false;
                GameObject.Find("UIcontroller").GetComponent<UIcontroller>().setReading (isReading);
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
                GameObject.Find("ShotLens").GetComponent<ShotLensController>().canWork = true;
                GameObject.Find("UIcontroller").GetComponent<UIcontroller>().setReading(isReading);
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
