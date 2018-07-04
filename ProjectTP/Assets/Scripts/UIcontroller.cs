using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour {
    GameObject UI_Pause;

    // Use this for initialization
    void Start () {
        UI_Pause = GameObject.Find("UI");
        UI_Pause.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause") && UI_Pause != null)
        {
            UI_Pause.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void BackToGame()
    {
        Time.timeScale = 1;
        UI_Pause.SetActive(false);
    }

    public void BackToTitle()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
        GameObject.Find("FadeManager").GetComponent<FadeManager>().Fade("Title", 1f);
        GameObject.Find("GameController").GetComponent<GameController>().ResetCreated();
        Destroy(GameObject.Find("GameController"));
    }
}
