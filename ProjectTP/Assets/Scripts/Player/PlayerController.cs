using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isJumping = false;
    public bool canJump　= true;
    [HideInInspector]
    public bool canMove = true;

    public float jumpVelocity = 6.0f;
    public float maxSpeed = 7.0f;
    private int landFlag=0;    //着陸かどうかを確認  連続3フレーム跳びスピードが同じなら、着陸した
    private float jumpSpeedY = 0.0f; //今の跳びスピード
    private bool canClimb = false;
    private bool isClimbing = false;
    private float climbPositionX = 0;//梯子のX位置
    private float climbPositionY = 0;
    private GameObject overlap;
    private GameObject shotLens;
    private GameObject lineDot;
    private Collider2D ladderCol;
    private GameObject movingItem=null;//立っている移動床

    private Animator anim; //アニメーション

    void Start()
    {
            this.transform.position = GameObject.Find("GameController").GetComponent<GameController>().GetBornPosition();
        overlap = GameObject.Find("Overlap");
        shotLens = GameObject.Find("ShotLens");
        lineDot = GameObject.Find("LineDot");
        anim = this.GetComponent<Animator>();
    }

	void FixedUpdate () {
        if (canMove)
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("speed",Mathf.Abs( move)); //移動のアニメへ遷移
            if (isJumping == false && move != 0f && isClimbing == false)
            {
                if (movingItem != null)  //移動床のスピードを追加
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed+movingItem.GetComponent<Rigidbody2D>().velocity.x*0.5f, GetComponent<Rigidbody2D>().velocity.y);
                }
                else {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }             
            }
            else if (isJumping == true && move != 0f && isClimbing == false)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed * 0.5f, GetComponent<Rigidbody2D>().velocity.y);
            }
            if ( move == 0f)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x*0.85f, GetComponent<Rigidbody2D>().velocity.y);
            }

            //向きを変わる
            if ((move > 0.0f && isFacingRight == false) || (move < 0.0f && isFacingRight == true))
            {
                PlayerFlip();
            }
            //着陸チェック
            if (jumpSpeedY == (int)(GetComponent<Rigidbody2D>().velocity.y * 100))
            {
                landFlag++;
                if (landFlag == 2)
                {
                    isJumping = false;
                    if (isClimbing == false)
                    {
                        ResumeShot();
                    }
                }
            }
            else
            {
                landFlag = 0;
                isJumping = true;
                FobidShot();
            }
            jumpSpeedY = (int)(GetComponent<Rigidbody2D>().velocity.y * 100);
            //梯子で下がる
            if (isClimbing == false && canClimb == true && Input.GetAxis("Vertical") < 0 && this.transform.position.y > climbPositionY)
            {
                this.gameObject.layer = LayerMask.NameToLayer("LadderFall");
                Invoke("LadderFall", 0.5f);

            }
            //梯子を登る
            if (canClimb == true && Input.GetAxis("Vertical") != 0)
            {
                isClimbing = true;
                FobidShot();
            }
            if (isClimbing == true && Input.GetAxis("Vertical") == 0)
            {
                canJump = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0f);
            }
            else if (isClimbing == true && Input.GetAxis("Vertical") != 0)
            {
                canJump = false;
                this.transform.position = new Vector3(climbPositionX, this.transform.position.y, this.transform.position.z);
                this.GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, Input.GetAxis("Vertical") * 3f);
            }
            if (isClimbing == false)
            {
                this.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            //ジャンプの入力
            if (Input.GetButtonDown("Jump") && canJump == true)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y * 0.8f + jumpVelocity);
                canJump = false;
                isClimbing = false;
            }
            anim.SetBool("isJump",isJumping);
            anim.SetBool("isClimb",isClimbing);
        }       
    }
    
    /// <summary>
    /// to flip player`s spirit
    /// </summary>
    
    public void PlayerFlip()
    {
        isFacingRight = !isFacingRight;
        Vector3 PlayerScale = transform.localScale;
        PlayerScale.x = PlayerScale.x * (-1);
        transform.localScale = PlayerScale;
        if (overlap != null)
        {
            if (overlap.GetComponent<Collider2D>() != null)
            {
                Vector3 overlapScale = overlap.transform.localScale;
                overlapScale.x = overlapScale.x * (-1);
                overlap.transform.localScale = overlapScale;
            }
        }
    }

    public void PlayerDie()
    {
        canMove = false;
        GameObject.Find("GameController").GetComponent<GameController>().ResetScene();   
    }
   //梯子の位置をゲット     
    public void ClimbLadder(float positionX,float positionY)
    {
        canClimb = true;
        climbPositionX = positionX;
        climbPositionY = positionY;
    }
    //梯子の範囲外に出る
    public void OutLadder()
    {
        canClimb = false;
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        isClimbing = false;
        if (isJumping == false&&this.transform.position.y>climbPositionY)
        {
            ResumeShot();
        }
    }
    //写真を禁止する
    public void FobidShot()
    {
        overlap.SetActive(false);
        shotLens.SetActive(false);
        lineDot.SetActive(false);
    }
    //写真を回復する
    public void ResumeShot()
    {
        overlap.SetActive(true);
        shotLens.SetActive(true);
        lineDot.SetActive(true);
    }
    //上から梯子で移動する
    public void LadderFall()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        isClimbing = true;
        this.transform.position = new Vector3(climbPositionX, this.transform.position.y, this.transform.position.z);
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, Input.GetAxis("Vertical") * 3f);
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            isClimbing = false;
            if (isJumping == false)
            {
                ResumeShot();
            }
        }
        if (col.gameObject.tag == "MovingItem")
        {
            movingItem = col.gameObject;
        }
    }

    public void ResumeJump()
    {
        canJump = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MovingItem")
        {
            movingItem = null;
        }
    }
}
