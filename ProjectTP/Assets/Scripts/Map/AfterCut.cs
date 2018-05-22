using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCut : MonoBehaviour {

    private MeshRenderer MR;
    private float Transparency = 1.0f;
    private int count = 0;

    // 各オブジェクトの対応が違う
    void Start () {
        MR = this.gameObject.GetComponent<MeshRenderer>();
        get_transparency();
    }
	

    void get_transparency() //透明化する
    {
        
        MR.material.color = new Vector4(MR.material.color.r, MR.material.color.g, MR.material.color.b, Transparency);
        Transparency -= 0.025f;

        Invoke("get_transparency", 0.05f);
        if (Transparency <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
