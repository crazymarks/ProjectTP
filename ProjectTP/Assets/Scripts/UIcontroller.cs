using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour {
    GameObject UI_Pause;
    GameObject UI_Autosave;
    public bool isSaving = false;
    // Use this for initialization
    void Start () {
        UI_Pause = GameObject.Find("UI");
        UI_Pause.SetActive(false);
        UI_Autosave = GameObject.Find("Autosave");
        UI_Autosave.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause") && UI_Pause != null&& GameObject.Find("FadeManager").GetComponent<FadeManager>().isFading==false)
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
    public void AutosaveShow()
    {
        if (!isSaving)
        {
            GameObject.Find("SEPlayer").GetComponent<PlaySE>().CameraShot();    //SE再生
            isSaving = true;
            UI_Autosave.SetActive(true);
            Invoke("AutosaveEnd", 1f);
        }
    }
    private void AutosaveEnd()
    {
        UI_Autosave.SetActive(false);
        isSaving = false;
    }
}
