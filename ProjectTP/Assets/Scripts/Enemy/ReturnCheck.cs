using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCheck : MonoBehaviour {
    int count1=0;    //空いているかどうかを計数
    int count2 = 0;  //計数を計数

	// Update is called once per frame
	void Update () {
        if (count1 == 0)
        {
            count2++;
        }
        else
        {
            count2 = 0;
        }
        if (count2 == 3)
        {
            transform.GetComponentInParent<Destroyer>().GetReturn();
        }
    }

    //モノがあるかどうかを確認する
    void OnTriggerStay2D(Collider2D tempObject)
    {
        count1++;

    }
    void OnTriggerExit2D(Collider2D tempObject)
    {
        count1 = 0;
    }
}
