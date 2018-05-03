using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlap : MonoBehaviour {
    /// <summary>
    /// 写真が再現できるかどうかを確認するため
    /// triggerをget
    /// </summary>
    public void GetTrigger()
    {
        GameObject Photo1 = GameObject.Find("Photo1");
        for (int i=0;i<ShotLensController.CopyList.Count;i++)
        {
            if (ShotLensController.CopyList[i].Items != null)  //チェック
            {
                if (ShotLensController.CopyList[i].Items.GetComponent<PolygonCollider2D>()!=null)  //polygonの場合
                {
                    PolygonCollider2D TempCollider= this.gameObject.AddComponent<PolygonCollider2D>();
                    TempCollider.isTrigger = true;

                    Vector2[] TempPoint= ShotLensController.CopyList[i].Items.GetComponent<PolygonCollider2D>().points;

                    for (int j = 0; j <TempPoint.Length; j++)
                    {
                        TempPoint[j].x = TempPoint[j].x * ShotLensController.CopyList[i].Items.transform.lossyScale.x;
                        TempPoint[j].y = TempPoint[j].y * ShotLensController.CopyList[i].Items.transform.lossyScale.y;

                    }
                    TempCollider.SetPath(0, TempPoint);
                    TempCollider.offset =new Vector2( ShotLensController.CopyList[i].Items.transform.position.x - Photo1.transform.position.x+ ShotLensController.CopyList[i].Items.GetComponent<PolygonCollider2D>().offset.x, 
                        ShotLensController.CopyList[i].Items.transform.position.y - Photo1.transform.position.y + ShotLensController.CopyList[i].Items.GetComponent<PolygonCollider2D>().offset.y);
                }
                if (ShotLensController.CopyList[i].Items.GetComponent<BoxCollider2D>() != null)　　//boxの場合
                {
                    BoxCollider2D TempCollider = this.gameObject.AddComponent<BoxCollider2D>();
                    TempCollider.isTrigger = true;

                    Vector2 TempSize = ShotLensController.CopyList[i].Items.GetComponent<BoxCollider2D>().size;

                    TempSize.x = TempSize.x * ShotLensController.CopyList[i].Items.transform.lossyScale.x;
                    TempSize.y = TempSize.y * ShotLensController.CopyList[i].Items.transform.lossyScale.y;

                    TempCollider.size = TempSize;
                    TempCollider.offset = new Vector2(ShotLensController.CopyList[i].Items.transform.position.x - Photo1.transform.position.x + ShotLensController.CopyList[i].Items.GetComponent<BoxCollider2D>().offset.x,
                        ShotLensController.CopyList[i].Items.transform.position.y - Photo1.transform.position.y + ShotLensController.CopyList[i].Items.GetComponent<BoxCollider2D>().offset.y);
                }
                if (ShotLensController.CopyList[i].Items.GetComponent<CircleCollider2D>() != null)　　//circleの場合
                {
                    CircleCollider2D TempCollider = this.gameObject.AddComponent<CircleCollider2D>();
                    TempCollider.isTrigger = true;

                    float TempRadius = ShotLensController.CopyList[i].Items.GetComponent<CircleCollider2D>().radius;

                    if(ShotLensController.CopyList[i].Items.transform.lossyScale.x> ShotLensController.CopyList[i].Items.transform.lossyScale.y)
                    {
                        TempRadius = TempRadius * ShotLensController.CopyList[i].Items.transform.lossyScale.x;
                    }
                    else
                    {
                        TempRadius = TempRadius * ShotLensController.CopyList[i].Items.transform.lossyScale.y;
                    }

                    TempCollider.radius = TempRadius;    
                    TempCollider.offset = new Vector2(ShotLensController.CopyList[i].Items.transform.position.x - Photo1.transform.position.x + ShotLensController.CopyList[i].Items.GetComponent<CircleCollider2D>().offset.x,
                    ShotLensController.CopyList[i].Items.transform.position.y - Photo1.transform.position.y + ShotLensController.CopyList[i].Items.GetComponent<CircleCollider2D>().offset.y);
                }
           }
        }      
    }
    public void DeleteTrigger()
    {
        Destroy( this.gameObject.GetComponent<Collider2D>());
    }
    void OnTriggerStay2D(Collider2D col)
    {
            ShotLensController.CanTrace = false;

    }
    void OnTriggerExit2D(Collider2D col)
    {
        ShotLensController.CanTrace = true;
        Debug.Log(ShotLensController.CanTrace);
    }
}
