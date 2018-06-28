using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConnectJudge : MonoBehaviour {
    private int terrainCount=0;//地形の数を計数
    private int timeCount=0; // 時間を計算
    private SpriteRenderer SR;
    Vector4 aa;  //color

    void Start()
    {
            SR = this.transform.parent.gameObject.GetComponent<SpriteRenderer>();
            aa = SR.material.color;
    }
    void Update () {
        if (terrainCount == 0)
        {
            timeCount++;
            if (timeCount >= 5)//５フレーム後、地形と接触しない場合は消える
            {
                SR.material.color = new Vector4(SR.material.color.r, SR.material.color.g, SR.material.color.b, SR.material.color.a - 0.01f);
                if (SR.material.color.a < 0)
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
