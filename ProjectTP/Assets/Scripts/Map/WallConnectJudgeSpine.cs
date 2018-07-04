using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConnectJudgeSpine : MonoBehaviour {
    private int terrainCount = 0;//地形の数を計数
    private int timeCount = 0; // 時間を計算
    private MeshRenderer MR;

    void Update()
    {
        if (terrainCount == 0)
        {
            timeCount++;
            if (timeCount >= 5)//５フレーム後、地形と接触しない場合は消える
            {
                MR = this.transform.parent.gameObject.GetComponent<MeshRenderer>();
                MR.material.color = new Vector4(MR.material.color.r, MR.material.color.g, MR.material.color.b, MR.material.color.a - 0.01f);
                if (MR.material.color.a < 0)
                {
                    Destroy(this.transform.parent.gameObject);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            terrainCount = terrainCount + 1;
        }

    }
}
