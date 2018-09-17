using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingController : MonoBehaviour {
    GameObject Player;
    GameObject[] gameController;
    public bool canInput=false;
    /// <summary>
    /// 0 ゴーグル出現
    /// 1 文字１出現
    /// 2 文字１消すと文字２出現
    /// 3 文字2消す　文字３と写真出現
    /// ４　文字３消す　文字４出現
    /// ５　文字４　グーグル　写真消す　
    ///　　　右下の写真出現　
    ///６　ヒロイン出現
    ///７　ｃｇ
    ///8  文字５出現
    ///9　文字５消す　文字６と黒幕出現
    ///10　文字６消す　ロゴ出現
    ///11　タイトル画面に戻る
    /// </summary>
    private int phase=0;

    public GameObject Next;//続きのui
    public GameObject finalphoto1; //左下の写真
    public GameObject finalphoto2; //再現したヒロイン
    public GameObject UI_EndingTrace;

    private void Start()
    {
        Player = GameObject.Find("Player");
        Next.SetActive(false);
        UI_EndingTrace.SetActive(false);
        finalphoto1.SetActive(false);
        finalphoto2.SetActive(false);
        gameController = GameObject.FindGameObjectsWithTag("GameController");
    }

    private void Update()
    {
        if (phase == 7 && canInput == true)
        {
            phase++;
            PhaseController();
        }
        if (canInput == true && phase==0)
        {
            canInput = false;
            phase++;
            PhaseController();
        }

        if (canInput == true && Input.GetButtonDown("Shot")&&phase!=0&&phase!=7)
        {
            canInput = false;
            phase++;
            PhaseController();
        }

    }

    void OnTriggerEnter2D(Collider2D TempObject)
    {
        if (TempObject.gameObject.tag == "Player")
        {
            for(int i=0; i<gameController.Length; i++)
            {
                gameController[i].GetComponent<AudioSource>().Stop();
            }
            Player.GetComponent<PlayerController>().EndingMove();            
            this.GetComponent<AudioSource>().Play();
            GameObject.Find("Goggle").GetComponent<ImageFading>().startFading = true;
        }
    }
    /// <summary>
    /// 各段階と転換のコントロール
    /// </summary>
    public void PhaseController()
    {
        switch (phase)
        {
            case 1:
                GameObject.Find("EndingMessage1").GetComponent<ImageFading>().startFading = true;
                Next.SetActive(true);
                break;
            case 2:
                GameObject.Find("EndingMessage1").SetActive(false);
                GameObject.Find("EndingMessage2").GetComponent<ImageFading>().startFading = true;
                break;
            case 3:
                GameObject.Find("EndingMessage2").SetActive(false);
                GameObject.Find("EndingMessage3").GetComponent<ImageFading>().startFading = true;
                GameObject.Find("HeroinePhoto").GetComponent<ImageFading>().startFading = true;
                finalphoto1.SetActive(true);
                break;
            case 4:
                GameObject.Find("EndingMessage3").SetActive(false);
                GameObject.Find("EndingMessage4").GetComponent<ImageFading>().startFading = true;
                break;
            case 5:
                GameObject.Find("HeroinePhoto").SetActive(false);
                GameObject.Find("Goggle").SetActive(false);
                GameObject.Find("EndingMessage4").SetActive(false);
                Next.SetActive(false);
                UI_EndingTrace.SetActive(true);
                canInput = true;
                break;
            case 6:
                Player.GetComponent<PlayerController>().EndingShot();
                GameObject.Find("SEPlayer").GetComponent<PlaySE>().CameraShot();
                finalphoto1.SetActive(false);
                finalphoto2.SetActive(true);
                UI_EndingTrace.SetActive(false);
                Next.SetActive(true);
                canInput = true;
                break;
            case 7:
                GameObject.Find("EndingCG").GetComponent<ImageFading>().startFading = true;
                break;
            case 8:
                GameObject.Find("EndingMessage5").GetComponent<ImageFading>().startFading = true;
                break;
            case 9:
                GameObject.Find("EndingMessage5").SetActive(false);
                GameObject.Find("Black").GetComponent<ImageFading>().startFading = true;
                GameObject.Find("EndingMessage6").GetComponent<ImageFading>().startFading = true;
                break;
            case 10:
                GameObject.Find("EndingMessage6").SetActive(false);
                GameObject.Find("Logo").GetComponent<ImageFading>().startFading = true;
                break;
            case 11:
                GameObject.Find("GameController").GetComponent<GameController>().ResetCreated();
                Destroy(GameObject.Find("GameController"));
                GameObject.Find("FadeManager").GetComponent<FadeManager>().Fade("Title", 1f);
                break;
        }
    }
}
