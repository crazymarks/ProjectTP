﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCut : MonoBehaviour {

    private MeshRenderer MR;
    private float Transparency;
    private int count = 0;
    Vector4 aa;

    // 各オブジェクトの対応が違う
    void Start () {
        MR = this.gameObject.GetComponent<MeshRenderer>();
      aa= MR.material.color;
        //  get_transparency();
    }	
    void Update()
    {
        if (false)
        {
            MR.material.color = aa;
        }
        else
        {

            MR.material.color = new Vector4(MR.material.color.r, MR.material.color.g, MR.material.color.b, MR.material.color.a - 0.01f);
        }

        if (MR.material.color.a<0)
        {
            Destroy(gameObject);
        }
    }

    void get_transparency() //透明化する
    {

    }
}
