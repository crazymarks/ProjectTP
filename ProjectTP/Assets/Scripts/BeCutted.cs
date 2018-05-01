using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeCutted : MonoBehaviour {

    void OnTriggerStay2D(Collider2D TempObject)
    {
        if (TempObject.name == "DeleteFrame")
        {
            Destroy(this.gameObject);
        }
    }
}
