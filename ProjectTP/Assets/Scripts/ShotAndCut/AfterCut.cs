using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCut : MonoBehaviour {

    private MeshRenderer MR;
    private float Transparency;
    private int count = 0;
    Vector4 aa;

    // 各オブジェクトの対応が違う
    void Start () {
        if (this.gameObject.GetComponent<MeshRenderer>() != null)
        {
            MR = this.gameObject.GetComponent<MeshRenderer>();
            aa = MR.material.color;
        }
    }	
    void Update()
    {
        if (MR != null)
        {
            //地形、ボタンすぐ消える
            if (this.gameObject.tag=="Terrain"||
                this.gameObject.tag == "Button"||
                this.gameObject.tag=="Destroyer")
            {
                MR.material.color = new Vector4(MR.material.color.r, MR.material.color.g, MR.material.color.b, MR.material.color.a - 0.01f);
            }
            else if(this.gameObject.tag == "Ladder"||
                  this.gameObject.tag == "Lever")//一瞬消える
            {
                MR.material.color = new Vector4(MR.material.color.r, MR.material.color.g, MR.material.color.b, MR.material.color.a - 0.02f);
            }
            else
            {
                MR.material.color = new Vector4(MR.material.color.r, MR.material.color.g, MR.material.color.b, MR.material.color.a - 0.003f);
            }

            if (MR.material.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public　void GetTransparency() //透明値を継承する
    {

    }
}
