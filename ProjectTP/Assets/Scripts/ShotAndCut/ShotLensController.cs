﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotLensController : MonoBehaviour {

    public Slice2DLayer sliceLayer = Slice2DLayer.Create();

    public struct ShotItem
    {
        public GameObject Items;  
    }
    static public List<ShotItem> ItemList = new List<ShotItem>();    // 撮ったモノのリスト
    static public List<ShotItem> CopyList = new List<ShotItem>();    //コピーするモノのリスト

    public Vector3 CameraCoordinate;      //カメラ座標
    public bool IsShoted=false;       //写真を撮った状態かどうか
    private Vector3 PhotoCameraCoordinate; //写真を表示するカメラの座標
    private GameObject DeleteFrame;
    private GameObject checkFrame;
    public bool CanTrace = false;   //写真が再現できるかどうか
    private bool CanTrace2 = false;
    private int CanTraceCount = 0;
    public GameObject overlap;
    public bool canWork=true;    //pause用
    private Animator anim; //アニメーション
    private SpriteRenderer photoFrameSR;
    public GameObject UI_takephoto; //写真が取れるかを確認するUI
    public Sprite takephoto;
    public Sprite tracephoto;
    

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        PhotoCameraCoordinate = GameObject.Find("PhotoCamera").transform.position;
        DeleteFrame= GameObject.Find("DeleteFrame");
        DeleteFrame.SetActive(false);
        checkFrame = GameObject.Find("CheckFrame");
        ItemList.Clear();    // 撮ったモノのリスト
        CopyList.Clear();    //コピーするモノのリスト
        anim =GameObject.Find("Player").GetComponent<Animator>();
        photoFrameSR = GameObject.Find("PhotoFrame").GetComponent<SpriteRenderer>();
        photoFrameSR.material.color = new Vector4(1f, 1f, 1f, 0.1f); //写真枠が見えなくなる状態
        UI_takephoto = GameObject.Find("UI_Takephoto");
    }

    void Update () {
        if (canWork == true)
        {
            UI_takephoto.SetActive(true);
            anim.SetBool("isShot", false);//姿勢を戻る
            //写真を撮る
            if (Input.GetButtonDown("Shot") && IsShoted == false && GameObject.Find("Player").GetComponent<PlayerController>().isJumping == false)
            {
                GameObject.Find("SEPlayer").GetComponent<PlaySE>().CameraShot();    //SE再生
                anim.SetBool("isShot", true);//写真撮る姿勢になる
                CameraCoordinate = this.transform.position;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    GameObject TempObject = Instantiate(ItemList[i].Items,
                        new Vector3(ItemList[i].Items.transform.position.x - CameraCoordinate.x + PhotoCameraCoordinate.x,
                        ItemList[i].Items.transform.position.y - CameraCoordinate.y + PhotoCameraCoordinate.y,
                        ItemList[i].Items.transform.position.z),
                        Quaternion.Euler(ItemList[i].Items.transform.eulerAngles));
                    //移動敵の進行方向を継承
                    if (TempObject.tag == "Destroyer")
                    {
                        TempObject.GetComponent<Destroyer>().directionAB = ItemList[i].Items.GetComponent<Destroyer>().directionAB;
                    }

                    if (ItemList[i].Items.GetComponent<MeshRenderer>() != null)
                    {
                        TempObject.GetComponent<MeshRenderer>().material.color = new Vector4(ItemList[i].Items.GetComponent<MeshRenderer>().material.color.r,
                            ItemList[i].Items.GetComponent<MeshRenderer>().material.color.g, ItemList[i].Items.GetComponent<MeshRenderer>().material.color.b,
                            ItemList[i].Items.GetComponent<MeshRenderer>().material.color.a);
                    }

                    TempObject.GetComponent<Pauser>().Pause();
                    ShotItem TempItem;
                    TempItem.Items = TempObject;
                    CopyList.Add(TempItem);
                }
                if (CopyList.Count != 0)
                {
                    IsShoted = true;
                    photoFrameSR.material.color = new Vector4(1f, 1f, 1f, 1f);   //写真枠が見えるようになる
                    Invoke("CheckFrameWork", 0.1f);  //チェックフレーム起動
                    Invoke("PolygonSlice", 0.3f);  //切り枠発動
                }
            }

            //モノを再現する
            if (Input.GetButtonDown("Trace") && CanTrace2 == true && GameObject.Find("Player").GetComponent<PlayerController>().isJumping == false)
            {
                anim.SetBool("isShot", true);//写真撮る姿勢になる
                CanTrace = false;
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
                }
                //写真を消す
                CopyList.Clear();
                IsShoted = false;
                photoFrameSR.material.color = new Vector4(1f, 1f, 1f, 0.1f); //写真枠が見えなくなる状態
                GameObject.Find("Overlap").GetComponent<Overlap>().DeleteTrigger();
                checkFrame.SetActive(true);
                UI_takephoto.GetComponent<Image>().sprite = takephoto;
            }

            //再現チェックのバグ修正　連続5フレームモノがない場合　ｏｋ
            if (CanTrace == true)
            {
                CanTraceCount++;
                if (CanTraceCount == 5)
                {
                    CanTrace2 = true;
                    UI_takephoto.GetComponent<Image>().sprite = tracephoto;
                }
            }
            else
            {
                CanTraceCount = 0;
                CanTrace2 = false;
            }
        }
        else
        {
            UI_takephoto.SetActive(false);
        }
    }

    //カメラレンズの範囲内のモノを記録
    void OnTriggerEnter2D(Collider2D TempObject)
    {
        if (TempObject.gameObject.tag == "Trigger")
        {
            return;
        }

        for (int i = 0; i < ItemList.Count; i++)
        {
            if (TempObject.gameObject == ItemList[i].Items)
            {
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
        Vector2f pos = new Vector2f(Photo1.transform.position.x, Photo1.transform.position.y);//-40f -15f
        newPolygon.AddPoint(-Photo1.GetComponent<BoxCollider2D>().size.x/2,Photo1.GetComponent<BoxCollider2D>().size.y/2);
        newPolygon.AddPoint(Photo1.GetComponent<BoxCollider2D>().size.x / 2,Photo1.GetComponent<BoxCollider2D>().size.y / 2);
        newPolygon.AddPoint(Photo1.GetComponent<BoxCollider2D>().size.x / 2,-Photo1.GetComponent<BoxCollider2D>().size.y / 2);
        newPolygon.AddPoint(-Photo1.GetComponent<BoxCollider2D>().size.x / 2,-Photo1.GetComponent<BoxCollider2D>().size.y / 2);

        Polygon slicePolygonDestroy = null;
        Slicer2D.PolygonSliceAll(pos, newPolygon, slicePolygonDestroy, sliceLayer);
        HideDeleteFrame();
    }
    /// <summary>
    /// deleteframeが出現する
    /// </summary>
    private void HideDeleteFrame()
    {
        DeleteFrame.SetActive(true);
        Invoke("HideDeleteFrame2", 0.3f);
    }
    private void HideDeleteFrame2()
    {
        DeleteFrame.SetActive(false);
        overlap.GetComponent<Overlap>().GetTrigger();
    }

    /// <summary>
    /// checkframe  消す　チェックした物を扱う
    /// </summary>g
    /// <param name="obj"></param>
    private void CheckFrameWork()
    {
        checkFrame.SetActive(false);
        checkFrame.GetComponent<CheckFrame>().HandleItem();
    }


    //カットしたモノをcopylistに追加する
    public void ItemListAdd(GameObject obj)
    {
        ShotItem sItem;
        sItem.Items = obj;
        CopyList.Add(sItem);
    }
    /// <summary>
    /// 写真を消します
    /// </summary>
    public void ShotClear()
    {
        CanTrace = false;
        if (CopyList.Count > 0)
        {
            for (int i = 0; i < CopyList.Count; i++)
            {
                if (CopyList[i].Items != null)
                {
                    Destroy(CopyList[i].Items);
                }
            }
        }
        //写真を消す
        CopyList.Clear();
        IsShoted = true;//範囲内写真をとれなくなる
        photoFrameSR.material.color = new Vector4(1f, 1f, 1f, 0.1f); //写真枠が見えなくなる状態
        overlap.GetComponent<Overlap>().DeleteTrigger();
        checkFrame.SetActive(true);
        //uiを追加します
        UI_takephoto.GetComponent<Image>().sprite = takephoto;
        canWork = false;

    }
    /// <summary>
    /// SavePointから出たら、写真が撮れる
    /// </summary>
    public void ShotRecovery()
    {
        IsShoted = false;
        canWork = true;
    }
}
