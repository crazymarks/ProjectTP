using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 一つスイッチの場合　on→開く　off→閉じる
/// </summary>
public class DoorOneSwitch : MonoBehaviour {
    bool isOpen=false;
    private Vector3 oldPosition;
    bool firstSwitch=false;   //一つ目のスイッチ

    void Start()
    {
        oldPosition = this.transform.position;
    }

    void Update()
    {
        isOpen = firstSwitch;
        if (isOpen == true)
        {
            float step = 5 * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, oldPosition - new Vector3(0, gameObject.GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y, 0), step);
        }
        else
        {
            float step = 5 * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, oldPosition, step);
        }

    }
    /// <summary>
    /// 各レバーとボタンnの　オン　状態　
    /// 順番も付ける
    /// </summary>
    void SwitchHandleOn(int number)
    {
        switch (number)
        {
            case 1:
                firstSwitch = true;
                break;
                
        }           
    }
    /// <summary>
    /// 各レバーとボタンnの　オフ　状態　
    /// 順番も付ける
    /// </summary>
    void SwitchHandleOff(int number)
    {
        switch (number)
        {
            case 1:
                firstSwitch = false;
                break;
        }
    }
}
