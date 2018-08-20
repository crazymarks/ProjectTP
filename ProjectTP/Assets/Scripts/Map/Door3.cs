using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//右へ移動
public class Door3 : MonoBehaviour {

    private bool canOpen = false; //スイッチによって。開くかどうかを確認
    private Vector3 oldPosition;  //最初の位置/戻すべき位置
    bool firstSwitchState = false;　//一つ目のスイッチの状態
    bool secondSwitchState = false;  //二つ目のスイッチの状態
    [HideInInspector]
    public List<GameObject> firstSwitch = null;
    [HideInInspector]
    public List<GameObject> secondSwitch = null;

    // Use this for initialization
    void Start()
    {
        oldPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchJudge();
        if (canOpen == true && this.transform.position.x < (oldPosition.x + 4))
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(8, 0);
        }
        else if (canOpen == false && this.transform.position.x > oldPosition.x)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-8, 0);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// 各レバーとボタンを管理
    /// 順番も付ける
    /// </summary>
    void SwitchHandle(GameObject obj)
    {

        switch (obj.GetComponent<SwitchID>().idNumber)
        {
            case 1:    //一番目のスイッチ
                if (firstSwitch.Count != 0)
                {
                    for (int i = 0; i < firstSwitch.Count; i++)
                    {
                        if (obj == firstSwitch[i])
                        {
                            break;
                        }
                        if (i == firstSwitch.Count - 1)
                        {
                            firstSwitch.Add(obj);
                        }
                    }
                }
                else
                {
                    firstSwitch.Add(obj);
                }
                break;

            case 2:　　　　//二番目のスイッチ
                if (secondSwitch.Count != 0)
                {
                    for (int i = 0; i < secondSwitch.Count; i++)
                    {
                        if (obj == secondSwitch[i])
                        {
                            break;
                        }
                        if (i == secondSwitch.Count - 1)
                        {
                            secondSwitch.Add(obj);
                        }
                    }
                }
                else
                {
                    secondSwitch.Add(obj);
                }
                break;
        }
    }

    void SwitchJudge()
    {
        if (firstSwitch.Count == 0 && secondSwitch.Count == 0)
        {
            canOpen = false;
        }
        else
        {
            //一つ目のスイッチ判定
            for (int i = 0; i < firstSwitch.Count; i++)
            {
                if (firstSwitch[i] != null && firstSwitch[i].tag == "Button")
                {
                    if (firstSwitch[i].GetComponent<Button>().isOpen == true)
                    {
                        firstSwitchState = true;
                        break;
                    }
                }
                else if (firstSwitch[i] != null && firstSwitch[i].tag == "Lever")
                {
                    if (firstSwitch[i].GetComponent<Lever>().isOpen == true)
                    {
                        firstSwitchState = true;
                        break;
                    }
                }
                if (firstSwitch[i] != null && i == firstSwitch.Count - 1)
                {
                    firstSwitchState = false;
                }
                else if (firstSwitch[i] == null && i == firstSwitch.Count - 1)
                {
                    firstSwitchState = false;
                }
            }
            //二つ目のスイッチ判定
            for (int i = 0; i < secondSwitch.Count; i++)
            {
                if (secondSwitch[i] != null && secondSwitch[i].tag == "Button")
                {
                    if (secondSwitch[i].GetComponent<Button>().isOpen == true)
                    {
                        secondSwitchState = true;
                        break;
                    }
                }
                else if (secondSwitch[i] != null && secondSwitch[i].tag == "Lever")
                {
                    if (secondSwitch[i].GetComponent<Lever>().isOpen == true)
                    {
                        secondSwitchState = true;
                        break;
                    }
                }
                if (secondSwitch[i] != null && i == secondSwitch.Count - 1)
                {
                    secondSwitchState = false;
                }
                else if (secondSwitch[i] == null && i == secondSwitch.Count - 1)
                {
                    secondSwitchState = false;
                }
            }

            //スイッチ判定 スイッチ１がある場合　スイッチ２がある場合　両方もある場合
            if (firstSwitch.Count == 0 && secondSwitch.Count != 0)
            {
                canOpen = secondSwitchState;
            }
            else if (firstSwitch.Count != 0 && secondSwitch.Count == 0)
            {
                canOpen = firstSwitchState;
            }
            else if (firstSwitch.Count != 0 && secondSwitch.Count != 0)
            {
                canOpen = firstSwitchState && secondSwitchState;
            }
        }
    }
}