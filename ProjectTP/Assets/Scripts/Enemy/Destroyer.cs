﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    private Vector2 lastPosition;//動いたか　を判断する
    public bool directionAB = true; //右へ向いている
    public float speed = 2.0f;
    private float stopCount = 0;
    public bool stopReturn = false;
    [HideInInspector]
    public bool isFacingLeft = false;
    public bool canMove = true;
    public float waitTime = 0.0f;


    // Update is called once per frame
    void FixedUpdate () {
        if (canMove == true)
        {
            if (directionAB == true)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, this.GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, this.GetComponent<Rigidbody2D>().velocity.y);
            }

            if (stopReturn == false)
            {
                if (lastPosition == new Vector2((int)(this.transform.position.x * 100), (int)(this.transform.position.y * 100))) //移動出来なくなる場合
                {
                    stopCount = stopCount + 1f;
                    if (stopCount > 20f)
                    {
                        directionAB = !directionAB;
                        stopCount = 0f;
                    }
                }
                else
                {
                    stopCount = 0f;
                }
            }

            lastPosition = new Vector2((int)(this.transform.position.x * 100), (int)(this.transform.position.y * 100));

            float move = this.GetComponent<Rigidbody2D>().velocity.x;
            //向きを変わる
            if ((move > 0.0f && isFacingLeft == false) || (move < 0.0f && isFacingLeft == true))
            {
                EnemyFlip();
            }
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
        }      
    }

    void EnemyFlip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 enemyScale = transform.localScale;
        enemyScale.x = enemyScale.x * (-1);
        transform.localScale = enemyScale;
    }
    /// <summary>
    /// directionAB つまり方向が変じる
    /// </summary>
   public void GetReturn() 
    {
        directionAB = !directionAB;
    }

}
