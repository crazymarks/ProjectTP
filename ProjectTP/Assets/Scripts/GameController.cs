using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject player;
    static Vector3 bornPosition=new Vector3(0f,0f,0f);
    static Vector3 firstBornPosition= new Vector3(0f, 0f, 0f);
    public GameObject bornPositionObject;
    private static bool created = false;
    public string stageName;
    public bool playerFaceRight=true;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            firstBornPosition = bornPositionObject.transform.position;
            bornPosition = firstBornPosition;
            GameObject.Find("Player").GetComponent<PlayerController>().canMove = false;
            if (!playerFaceRight)
            {
                GameObject.Find("Player").GetComponent<PlayerController>().PlayerFlip();
            }
            Invoke("SetCanMove",2f);
        }
    }

    public void ResetScene () {
        Time.timeScale = 1;
        GameObject.Find("FadeManager").GetComponent<FadeManager>().Fade(stageName, 1f);
    }

    public void SetCanMove()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().canMove = true;
    }
    /// <summary>
    /// プレイヤ死亡後復活
    /// </summary>
    public void Reborn()
    {
        GameObject.Find("Player").transform.position = bornPosition;
        GameObject.Find("Player").GetComponent<PlayerController>().canMove = true;
        if (!playerFaceRight)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().PlayerFlip();
        }
    }
    /// <summary>
    /// セーブポイントに触った
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="faceRight">方向</param>
    public void PlayerSave(Vector3 pos,bool faceRight)
    {
        bornPosition = pos;
        playerFaceRight = faceRight;
    }

    public Vector3 GetBornPosition()
    {          
        return new Vector3(bornPosition.x,bornPosition.y,0);
    }
    /// <summary>
    /// セーブポイントが破壊された
    /// </summary>
    /// <param name="pos"></param>
    public void SavePointDestroy(Vector3 pos)
    {
        if (pos == bornPosition)
        {
            bornPosition = firstBornPosition;
        }
    }
    public void ResetCreated()
    {
        created = false;
    }
}
