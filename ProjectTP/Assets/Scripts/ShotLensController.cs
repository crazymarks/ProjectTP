using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLensController : MonoBehaviour {

    public Slice2DLayer sliceLayer = Slice2DLayer.Create();

    public struct ShotItem
    {
        public GameObject Items;  
    }
    static public List<ShotItem> ItemList = new List<ShotItem>();    // 撮ったモノのリスト
    static public List<ShotItem> CopyList = new List<ShotItem>();    //コピーするモノのリスト

    public Vector3 CameraCoordinate;      //カメラ座標
    private bool IsShoted=false;       //写真を撮った状態かどうか
    private Vector3 PhotoCameraCoordinate; //写真を表示するカメラの座標
    private bool CanShoted = true;     //写真が撮れるか
    private GameObject DeleteFrame;
    static public bool CanTrace = true;   //写真が再現できるかどうか
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        PhotoCameraCoordinate = GameObject.Find("PhotoCamera").transform.position;
        DeleteFrame= GameObject.Find("DeleteFrame");
        DeleteFrame.SetActive(false);
    }

    void Update () {
        //写真を撮る
        if (Input.GetButtonDown("Shot")&&IsShoted==false){

            CameraCoordinate = this.transform.position;

            for (int i = 0; i < ItemList.Count; i++)
            {
                GameObject TempObject = Instantiate(ItemList[i].Items,
                    new Vector3(ItemList[i].Items.transform.position.x - CameraCoordinate.x + PhotoCameraCoordinate.x, 
                    ItemList[i].Items.transform.position.y - CameraCoordinate.y + PhotoCameraCoordinate.y, 
                    ItemList[i].Items.transform.position.z),
                    Quaternion.Euler(ItemList[i].Items.transform.eulerAngles));
                TempObject.GetComponent<Pauser>().Pause();
                ShotItem TempItem;
                TempItem.Items = TempObject;
                CopyList.Add(TempItem);
            }
            IsShoted = true;
            Invoke("PolygonSlice", 0.2f);

        }

        //モノを再現する
        if (Input.GetButtonDown("Trace")&&CanTrace==true)
        {
            CameraCoordinate = this.transform.position;
            if (CopyList.Count > 0)
            {
                for (int i = 0; i < CopyList.Count; i++)
                {
                    if (CopyList[i].Items != null)
                    {
                        Vector3 pos2 = new Vector3(CopyList[i].Items.transform.position.x - PhotoCameraCoordinate.x + CameraCoordinate.x,
                            CopyList[i].Items.transform.position.y - PhotoCameraCoordinate.y + CameraCoordinate.y,
                            CopyList[i].Items.transform.position.z);
                        CopyList[i].Items.transform.position = pos2;
                    }
                }
                Pauser.Resume();
                CopyList.Clear();
            }
            IsShoted = false;
            GameObject.Find("Overlap").SendMessage("DeleteTrigger");
        }
	}

    //カメラレンズの範囲内のモノを記録
    void OnTriggerEnter2D(Collider2D TempObject)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (TempObject.gameObject == ItemList[i].Items)
            {
                Debug.Log(i);
                return;
            }
        }
        ShotItem NewShotObject;
        NewShotObject.Items = TempObject.transform.gameObject;
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

    //切り枠を発動
    private void PolygonSlice()
    {
        Polygon newPolygon = new Polygon();
        GameObject Photo1 =  GameObject.Find("Photo1");
        Vector2f pos = new Vector2f(-40f,-15f);//Photo1.transform.position.x,Photo1.transform.position.y
        newPolygon.AddPoint(-Photo1.GetComponent<BoxCollider2D>().size.x/2,Photo1.GetComponent<BoxCollider2D>().size.y/2);
        newPolygon.AddPoint(Photo1.GetComponent<BoxCollider2D>().size.x / 2,Photo1.GetComponent<BoxCollider2D>().size.y / 2);
        newPolygon.AddPoint(Photo1.GetComponent<BoxCollider2D>().size.x / 2,-Photo1.GetComponent<BoxCollider2D>().size.y / 2);
        newPolygon.AddPoint(-Photo1.GetComponent<BoxCollider2D>().size.x / 2,-Photo1.GetComponent<BoxCollider2D>().size.y / 2);

        Polygon slicePolygonDestroy = null;
        Slicer2D.PolygonSliceAll(pos, newPolygon, slicePolygonDestroy, sliceLayer);
        HideDeleteFrame();
    }
    private void HideDeleteFrame()
    {
        DeleteFrame.SetActive(true);
        Invoke("HideDeleteFrame2", 0.5f);
    }
    private void HideDeleteFrame2()
    {
        DeleteFrame.SetActive(false);
        GameObject.Find("Overlap").SendMessage("GetTrigger");
    }

    //カットしたモノをcopylistに追加する
    public void ItemListAdd(GameObject obj)
    {
        ShotItem sItem;
        sItem.Items = obj;
        CopyList.Add(sItem);
    }


}
