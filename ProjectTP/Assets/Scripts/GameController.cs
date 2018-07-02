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
    }
    public void ResetScene () {
        SceneManager.LoadScene(stageName);
        GameObject.Find("Player").transform.position = bornPosition;      
    }

    public void PlayerSave(Vector3 pos)
    {
        bornPosition = pos;
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
