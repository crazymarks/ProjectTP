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

        }
    }
    void Start()
    {
        firstBornPosition = bornPositionObject.transform.position;
        if (bornPosition == new Vector3(0f, 0f, 0f))
        {
            bornPosition = firstBornPosition;
        }
        if (!playerFaceRight)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().PlayerFlip();
        }
    }
    public void ResetScene () {
        GameObject.Find("FadeManager").GetComponent<FadeManager>().Fade(stageName, 1f);
        Invoke("Reborn", 1f); 
    }
    private void Reborn()
    {
        GameObject.Find("Player").transform.position = bornPosition;
        GameObject.Find("Player").GetComponent<PlayerController>().canMove = true;
        if (!playerFaceRight)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().PlayerFlip();
        }
    }

    public void PlayerSave(Vector3 pos,bool faceRight)
    {
        bornPosition = pos;
        playerFaceRight = faceRight;
    }

    public Vector3 GetBornPosition()
    {          
        return new Vector3(bornPosition.x,bornPosition.y,0);
    }

    public void SavePointDestroy(Vector3 pos)
    {
        if (pos == bornPosition)
        {
            bornPosition = firstBornPosition;
        }
    }
}
