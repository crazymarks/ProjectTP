using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルからシーン遷移時の制御するためのクラス
/// </summary>
public class TitleManager : MonoBehaviour {
    GameObject titleLayer;
    GameObject stageSelectLayer;

    void Start()
    {
        titleLayer = GameObject.Find("TitleLayer");
        stageSelectLayer = GameObject.Find("StageSelectLayer");
        stageSelectLayer.SetActive(false);
    }
    public void GameStart(string scene)
    {
        GameObject.Find("FadeManager").GetComponent<FadeManager>().Fade(scene,2f);
        
    }

    public void StageSelect(){
        titleLayer.SetActive(false);
        stageSelectLayer.SetActive(true);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void BackToTitle()
    {
        stageSelectLayer.SetActive(false);
        titleLayer.SetActive(true);
    }
}
