using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlap : MonoBehaviour {
    void Update()
    {
        if (Input.GetButtonDown("Cut"))
        {
            GetTrigger();
        }
    }

    public void GetTrigger()
    {
        if (ShotLensController.CopyList.Count > 0)
        {

            for (int i=0;i<ShotLensController.CopyList.Count;i++)
            {
                if (ShotLensController.CopyList[i].Items != null)
                {
                    Debug.Log(ShotLensController.CopyList[i].Items.name);
                    Collider2D ColliderTrigger = ShotLensController.CopyList[i].Items.GetComponent<Collider2D>();
                    ColliderTrigger.isTrigger = true;
                     //ここで分類
                }

            }
        }
    }
}
