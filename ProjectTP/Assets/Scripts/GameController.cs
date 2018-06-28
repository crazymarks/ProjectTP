using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject player;
    static Vector3 bornPosition=new Vector3(0,3.5f,0);
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
    public void ResetScene () {
        SceneManager.LoadScene(stageName);
        
    }

    public void PlayerSave(Vector3 pos)
    {
        bornPosition = pos;
    }

    public Vector3 GetBornPosition()
    {          
        return new Vector3(bornPosition.x,bornPosition.y,0);
    }
}
