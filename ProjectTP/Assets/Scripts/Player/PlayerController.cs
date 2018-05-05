using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [HideInInspector]
    public bool IsFacingRight = true;
    [HideInInspector]
    public bool IsJumping = false;

    public float JumpForce = 650.0f;
    public float MaxSpeed = 7.0f;
    private int LandFlag=0;    //to show is it touch land


	void Start () {
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump")&&IsJumping==false)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpForce));
            IsJumping = true;
        }
    }
	
	void FixedUpdate () {
        float move = Input.GetAxis("Horizontal");
        if (IsJumping == false)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * MaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * MaxSpeed *0.4f, GetComponent<Rigidbody2D>().velocity.y);
        }
        //change facing
        if ((move>0.0f && IsFacingRight==false) || (move<0.0f && IsFacingRight==true))
        {
            player_flip();
        }
        //jump check 
        if (IsJumping == true && GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            land_check();
        }      
	}
    
    void land_check() {
        LandFlag++;
        if (LandFlag == 2)
        {
            LandFlag = 0;
            IsJumping = false;
        }
    }
    /// <summary>
    /// to flip player`s spirit
    /// </summary>
    
    void player_flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 PlayerScale = transform.localScale;
        PlayerScale.x = PlayerScale.x * (-1);
        transform.localScale = PlayerScale;
    }

}
