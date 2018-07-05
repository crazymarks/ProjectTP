﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : MonoBehaviour {
    public GameObject pointA = null;
    public GameObject pointB = null;

    public float speed = 1f;
    [HideInInspector]
    public bool directionAB = true;
    private Vector2 movingVector= new Vector2 (0,0);
    private Vector3 initialPosition;
    private Vector3 changedPosition;
    private float stopCount = 0;  //止まる時間を数える
    private Vector2 lastPosition;//動いたか　を判断する
    private bool canMove = true; //スイッチによって。移動するかどうかを確認
    bool firstSwitchState= false;　//一つ目のスイッチの状態
    bool secondSwitchState = false;  //二つ目のスイッチの状態
    [HideInInspector]
    public List<GameObject> firstSwitch = null;
    [HideInInspector]
    public List<GameObject> secondSwitch = null;
    public List<GameObject> attachedObj = null;
    private bool reachFlag = false; //目標点に到着する
    private float reachCount = 0;   //目標を到着後時間を計算する
    public float stopTime = 2.0f;   //止まる時間
    private bool changeFlag = false; //止まるから初めて動くフラグ　

    void Start()
    {
        initialPosition = new Vector3(this.transform.position.x,this.transform.position.y, this.transform.position.z);
        float movingX = pointB.transform.position.x - pointA.transform.position.x;
        float movingY = pointB.transform.position.y - pointA.transform.position.y;
        float movingVectorSize = Mathf.Sqrt(movingX * movingX + movingY * movingY);
        movingVector = new Vector2(movingX / movingVectorSize, movingY / movingVectorSize);
    }

    // Update is called once per frame
    void FixedUpdate() {
        SwitchJudge();
        // 目標に到着して、引き戻す
        if (Vector3.Distance(this.transform.position,pointA.transform.position)>Vector3.Distance(pointA.transform.position,pointB.transform.position)&&directionAB==true)
        {
            directionAB = !directionAB;
            CancelInertance(); //慣性を消す
            reachFlag = true;
        }
        else if(Vector3.Distance(this.transform.position, pointB.transform.position) > Vector3.Distance(pointA.transform.position, pointB.transform.position) && directionAB == false)
        {
            directionAB = !directionAB;
            CancelInertance(); //慣性を消す
            reachFlag = true;
        }

        if (canMove&&(!reachFlag))    //スイッチによって、コントロールする
        {
            if (lastPosition == new Vector2((int)(this.transform.position.x * 100), (int)(this.transform.position.y * 100))) //移動出来なくなる場合
            {
                stopCount =stopCount+Time.deltaTime;
                if (stopCount > stopTime)
                {
                    directionAB = !directionAB;
                    stopCount = 0f;
                    DirectionChange();
                    changeFlag = true;
                }
            }
            else
            {
                stopCount = 0f;
            }
            DirectionChange();
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        if (reachFlag == true)  //目標に到着止まって、時間を計算する
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            reachCount = reachCount + Time.deltaTime;
            if (reachCount > stopTime)
            { 
                reachFlag = false;
                changeFlag = true;
                reachCount = 0f;
            }
        }

        lastPosition =new Vector2 ((int)(this.transform.position.x*100),(int)(this.transform.position.y*100));
    }


    public Vector3 PositionJudge()
    {
        changedPosition = new Vector3(initialPosition.x-this.transform.position.x, initialPosition.y - this.transform.position.y,initialPosition.z);
        return changedPosition;
    }


    void DirectionChange()
    {
        if (changeFlag == true)
        {
            CancelInertance();
            changeFlag = false;
        }
        if (directionAB == true)
        {
            this.GetComponent<Rigidbody2D>().velocity = (movingVector * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = -(movingVector * speed);
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
                    for(int i=0;i< firstSwitch.Count; i++)
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
                        if (i == secondSwitch.Count-1)
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
            canMove = true;
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
            }

            //スイッチ判定 スイッチ１がある場合　スイッチ２がある場合　両方もある場合
            if (firstSwitch.Count == 0 && secondSwitch.Count != 0)
            {
                canMove = secondSwitchState;
            }
            else if (firstSwitch.Count != 0 && secondSwitch.Count == 0)
            {
                canMove = firstSwitchState;
            }
            else if(firstSwitch.Count != 0 && secondSwitch.Count != 0)
            {
                canMove = firstSwitchState && secondSwitchState;
            }
        } 
    }
    /// <summary>
    /// 接触したものを記録
    /// </summary>
    /// <param name="tempObject"></param>
    void OnCollisionEnter2D(Collision2D tempObject)
    {
        for (int i = 0; i < attachedObj.Count; i++)
        {
            if (tempObject.gameObject == attachedObj[i])
            {
                return;
            }
        }

        attachedObj.Add(tempObject.gameObject);
    }

    void OnCollisionExit2D(Collision2D tempObject)
    {
        int Index1 = -1;
        Index1 = attachedObj.FindIndex(x => x == tempObject.gameObject);
        if (Index1 != -1)
        {
            attachedObj.RemoveAt(Index1);
        }
    }
    /// <summary>
    /// 慣性を消す
    /// </summary>
    void CancelInertance()
    {
        if (this.GetComponent<Rigidbody2D>().constraints == (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation))
        {
            if (attachedObj.Count != 0)
            {
                for (int i = 0; i < attachedObj.Count; i++)
                {
                    if (attachedObj[i].gameObject.GetComponent<Rigidbody2D>() != null && attachedObj[i].tag != "Terrain" && attachedObj[i].tag != "Spines")
                    {
                        attachedObj[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(attachedObj[i].gameObject.GetComponent<Rigidbody2D>().velocity.x, -2f * speed);
                    }
                }
            }
        }
        else if (this.GetComponent<Rigidbody2D>().constraints == (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation))
        {
            if (attachedObj.Count != 0)
            {
                for (int i = 0; i < attachedObj.Count; i++)
                {
                    if (attachedObj[i].gameObject.GetComponent<Rigidbody2D>() != null && attachedObj[i].tag != "Terrain" && attachedObj[i].tag != "Spines")
                    {
                        attachedObj[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, attachedObj[i].gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    }
                }
            }
        }      
    }
}


