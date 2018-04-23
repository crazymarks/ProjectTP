using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLensController : MonoBehaviour {

    struct ShotItem
    {
        public GameObject Items;  
    }
    List<ShotItem> ItemList = new List<ShotItem>();    // 撮ったモノのリスト

    public Vector3 CameraCoordinate;      //カメラ座標
    private bool IsShoted=false;       //写真を撮った状態かどうか
    private Vector3 PhotoCameraCoordinate; //写真を表示するカメラの座標

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        PhotoCameraCoordinate = GameObject.Find("PhotoCamera").transform.position;
    }

    void Update () {
        //写真を撮る
        if (Input.GetButtonDown("Shot")&&IsShoted==false){

            CameraCoordinate = this.transform.position;

            for (int i = 0; i < ItemList.Count; i++)
            {
                GameObject TempObject = Instantiate(ItemList[i].Items,
                    new Vector3(ItemList[i].Items.transform.position.x - CameraCoordinate.x + PhotoCameraCoordinate.x, ItemList[i].Items.transform.position.y - CameraCoordinate.y + PhotoCameraCoordinate.y, 0),
                    Quaternion.Euler(ItemList[i].Items.transform.eulerAngles));
                TempObject.GetComponent<Pauser>().Pause();
            }
            IsShoted = true;
        }
        //モノを再現する
        if (Input.GetButtonDown("Trace"))
        {
            IsShoted = false;
            Pauser.Resume();
        }
	}

    //カメラレンズの範囲内のモノを記録
    void OnTriggerEnter2D(Collider2D TempObject)
    {
        GameObject TempObject1 = TempObject.transform.gameObject;

        ShotItem NewShotObject;
        NewShotObject.Items = TempObject1;
        ItemList.Add(NewShotObject);         
    }

    //レンズ範囲に離れた時、ItemListから削除する
    void OnTriggerExit2D(Collider2D TempObject)
    {
        int Index1 = -1;
        Index1 =ItemList.FindIndex(x => x.Items == TempObject.transform.gameObject);
        if (Index1!=-1)
        {
            ItemList.RemoveAt(Index1);
        }
    }
}
