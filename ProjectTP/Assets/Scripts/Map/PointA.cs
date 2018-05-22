using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointA : MonoBehaviour {
    Vector3 pointAPosition;
    
    void Start()
    {
        pointAPosition = this.transform.localPosition;
    }

	void FixedUpdate () {
        this.transform.localPosition = pointAPosition + this.transform.parent.GetComponent<MovingItem>().PositionJudge();
    }
}
