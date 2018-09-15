using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFading : MonoBehaviour {
    public float fadingTime=1f;//完全出現までの時間
    float tempTime=0f;//時間計算用変数
    public bool startFading = false;
    bool isTriggered = false;
    // Use this for initialization
	void Start () {
        this.GetComponent<Image>().color = new Vector4(this.GetComponent<Image>().color.r,
            this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (startFading == true && tempTime < fadingTime)
        {
            tempTime = tempTime + Time.deltaTime;

            this.GetComponent<Image>().color = new Vector4(this.GetComponent<Image>().color.r,
           this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, tempTime/fadingTime);
        }else if (tempTime >= fadingTime&&isTriggered==false)
        {
            isTriggered = true;
            GameObject.Find("EndingTrigger").GetComponent<EndingController>().canInput = true;
        }
    }


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
}
