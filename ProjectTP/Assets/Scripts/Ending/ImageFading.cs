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
}
