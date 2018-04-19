using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLensController : MonoBehaviour {

    struct ShotItem
    {
        public Vector3 AbsoluteCoordinate;  //絶対座標
        public Vector3 RotateOfItem;        //モノの回転状態
        public string NameOfItem;　　　　　　//撮ったモノの名前
        public string TagOfItem;     　　　　//撮ったモノのタグ
    }

    struct ShotItem2
    {
        public GameObject Items;  
    }
    List<ShotItem2> ItemList2 = new List<ShotItem2>();

    List<ShotItem> ItemList = new List<ShotItem>();  // 撮ったモノのリスト
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
            /* for (int i=0;i<ItemList.Count;i++)
             {
                 GameObject TempObject=Instantiate(GameObject.Find(ItemList[i].NameOfItem), 
                     new Vector3(ItemList[i].AbsoluteCoordinate.x-CameraCoordinate.x+PhotoCameraCoordinate.x, ItemList[i].AbsoluteCoordinate.y - CameraCoordinate.y + PhotoCameraCoordinate.y,0),
                     Quaternion.Euler(ItemList[i].RotateOfItem));
                 Debug.Log(ItemList[i].AbsoluteCoordinate);
                 TempObject.GetComponent<Pauser>().Pause();
             }*/
            for (int i = 0; i < ItemList2.Count; i++)
            {
                GameObject TempObject = Instantiate(ItemList2[i].Items,
                    new Vector3(ItemList2[i].Items.transform.position.x - CameraCoordinate.x + PhotoCameraCoordinate.x, ItemList2[i].Items.transform.position.y - CameraCoordinate.y + PhotoCameraCoordinate.y, 0),
                    Quaternion.Euler(ItemList2[i].Items.transform.eulerAngles));
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
        NewShotObject.NameOfItem = TempObject1.name;
        NewShotObject.AbsoluteCoordinate = new Vector3(TempObject1.transform.position.x,
        TempObject1.transform.position.y, TempObject1.transform.position.z);
        NewShotObject.RotateOfItem =new Vector3( TempObject1.transform.rotation.eulerAngles.x,
            TempObject1.transform.rotation.eulerAngles.y,TempObject1.transform.rotation.eulerAngles.z);
        NewShotObject.TagOfItem = TempObject1.tag;

        ItemList.Add(NewShotObject);
        ShotItem2 NewShotObject2;
        NewShotObject2.Items = TempObject1;
        ItemList2.Add(NewShotObject2);
         
    }

    //レンズ範囲に離れた時、ItemListから削除する
    void OnTriggerExit2D(Collider2D TempObject)
    {
        int Index1 = -1;
        Index1 =ItemList2.FindIndex(x => x.Items == TempObject.transform.gameObject);
        if (Index1!=-1)
        {
            ItemList2.RemoveAt(Index1);
        }
    }
}
