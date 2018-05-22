using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointB : MonoBehaviour {
    Vector3 pointBPosition;

    void Start()
    {
        pointBPosition = this.transform.localPosition;
    }

    void FixedUpdate()
    {
        this.transform.localPosition = pointBPosition + this.transform.parent.GetComponent<MovingItem>().PositionJudge();
    }
}
