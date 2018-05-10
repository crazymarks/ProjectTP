using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour {

    void OnTriggerStay2D(Collider2D TempObject)
    {
        Destroy(TempObject.gameObject);
    }
    void OnCollisionStay2D(Collision2D TempObject)
    {
        Destroy(TempObject.gameObject);
    }

}
