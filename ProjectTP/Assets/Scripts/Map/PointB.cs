using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointB : MonoBehaviour {

    void FixedUpdate()
    {
        this.transform.localPosition = this.transform.parent.GetComponent<MovingItem>().PositionJudge();
    }
}
