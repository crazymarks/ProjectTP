using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isJumping = false;

    public float jumpVelocity = 6.0f;
    public float maxSpeed = 7.0f;
    private int landFlag=0;    //着陸かどうかを確認  連続3フレーム跳びスピードが同じなら、着陸した
    private float jumpSpeedY = 0.0f; //今の跳びスピード
    private bool canClimb = false;
    private bool isClimbing = false;

    void Start()
    {
            this.transform.position = GameObject.Find("GameController").GetComponent<GameController>().GetBornPosition();
    }

	void FixedUpdate () {
        float move = Input.GetAxis("Horizontal");
       if (isJumping == false && move!=0f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if(isJumping == true && move != 0f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed *0.6f, GetComponent<Rigidbody2D>().velocity.y);
        }
       
        //向きを変わる
        if ((move>0.0f && isFacingRight==false) || (move<0.0f && isFacingRight==true))
        {
            PlayerFlip();
        }

        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpVelocity);
            isJumping = true;
        }
        //跳びチェック
        if (jumpSpeedY == (int)(GetComponent<Rigidbody2D>().velocity.y * 100))
        {
            landFlag++;
            if (landFlag == 1)
            {
                isJumping = false;
            }
        }
        else
        {
            landFlag = 0;
            isJumping = true;
        }
        jumpSpeedY =(int) (GetComponent<Rigidbody2D>().velocity.y*100);

        //梯子を登る
        if(canClimb==true&& Input.GetAxis("Vertical") !=0)
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, Input.GetAxis("Vertical"));
        }
    }
    
    /// <summary>
    /// to flip player`s spirit
    /// </summary>
    
    void PlayerFlip()
    {
        isFacingRight = !isFacingRight;
        Vector3 PlayerScale = transform.localScale;
        PlayerScale.x = PlayerScale.x * (-1);
        transform.localScale = PlayerScale;
    }

    public void PlayerDie()
    {

        GameObject.Find("GameController").GetComponent<GameController>().ResetScene();   
    }
        
    public void ClimbLadder(float positionX)
    {
        canClimb = true;
    }
    public void OutLadder()
    {
        canClimb = false;
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    public void FobidShot()
    {

    }
}
