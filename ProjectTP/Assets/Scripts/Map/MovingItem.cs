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
    private int stopCount = 0;  //止まる時間を数える
    private Vector3 lastPosition;//動いたか　を判断する
    private bool canMove = true; //スイッチによって。移動するかどうかを確認
    bool firstSwitchState= false;　//一つ目のスイッチの状態
    bool secondSwitchState = false;  //二つ目のスイッチの状態
    [HideInInspector]
    public List<GameObject> firstSwitch = null;
    [HideInInspector]
    public List<GameObject> secondSwitch = null;


    void Start()
    {
        initialPosition = new Vector3(this.transform.position.x,this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate() {
        Debug.Log(firstSwitch);
        float movingX = pointB.transform.position.x - pointA.transform.position.x;
        float movingY = pointB.transform.position.y - pointA.transform.position.y;
        float movingVectorSize = Mathf.Sqrt(movingX * movingX + movingY * movingY);
        movingVector = new Vector2(movingX / movingVectorSize, movingY / movingVectorSize);

        if (Vector3.Distance(this.transform.position,pointA.transform.position)>Vector3.Distance(pointA.transform.position,pointB.transform.position)&&directionAB==true)
        {
            directionAB = !directionAB;
            DirectionChange();           
        }else if(Vector3.Distance(this.transform.position, pointB.transform.position) > Vector3.Distance(pointA.transform.position, pointB.transform.position) && directionAB == false)
        {
            directionAB = !directionAB;
            DirectionChange();
        }

        if (canMove)    //スイッチによって、コントロールする
        {
            DirectionChange();
            if (lastPosition == this.transform.position)
            {

                stopCount++;
                if (stopCount > 20)
                {
                    directionAB = !directionAB;
                    stopCount = 0;
                    DirectionChange();
                }
            }
            else
            {
                stopCount = 0;
            }
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }    
        lastPosition = this.transform.position;
    }


    public Vector3 PositionJudge()
    {
        changedPosition = new Vector3(initialPosition.x-this.transform.position.x, initialPosition.y - this.transform.position.y,initialPosition.z);
        return changedPosition;
    }


    void DirectionChange()
    {
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
    /// 各レバーとボタンnの　オン　状態　
    /// 順番も付ける
    /// </summary>
    void SwitchHandleOn(int number)
    {
        Debug.Log("On");
        switch (number)
        {   
            case 1:
                firstSwitch = true;
                canMove = firstSwitch;
                break;

        }
    }
    /// <summary>
    /// 各レバーとボタンnの　オフ　状態　
    /// 順番も付ける
    /// </summary>
    void SwitchHandleOff(int number)
    {
        Debug.Log("Off");
        switch (number)
        {
            case 1:
                firstSwitch = false;
                canMove = firstSwitch;
                break;
        }
    }
}

