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
    private int landFlag=0;    //着陸かどうかを確認  連続５フレーム跳びスピードが同じなら、着陸した
    private float jumpSpeedY = 0.0f; //今の跳びスピード

	void FixedUpdate () {
        float move = Input.GetAxis("Horizontal");
       if (isJumping == false && move!=0f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if(isJumping == true && move != 0f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed *0.4f, GetComponent<Rigidbody2D>().velocity.y);
        }
       
        //向きを変わる
        if ((move>0.0f && isFacingRight==false) || (move<0.0f && isFacingRight==true))
        {
            player_flip();
        }

        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpVelocity);
            isJumping = true;
        }
        //跳びチェック
        if (jumpSpeedY == GetComponent<Rigidbody2D>().velocity.y)
        {
            landFlag++;
            if (landFlag == 5)
            {
                isJumping = false;
            }
        }
        else
        {
            landFlag = 0;
        }
        jumpSpeedY = GetComponent<Rigidbody2D>().velocity.y;
    }
    
    /// <summary>
    /// to flip player`s spirit
    /// </summary>
    
    void player_flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 PlayerScale = transform.localScale;
        PlayerScale.x = PlayerScale.x * (-1);
        transform.localScale = PlayerScale;
    }


}
