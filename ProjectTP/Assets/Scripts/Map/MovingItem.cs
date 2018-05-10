using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : MonoBehaviour {
    public GameObject pointA = null;
    public GameObject pointB = null;
    public float speed = 1f;
    public bool directionAB = true;
    private Vector2 movingVector= new Vector2 (0,0);
    private Vector3 initialPosition;
    private Vector3 changedPosition;

    void Start()
    {
        initialPosition = new Vector3(this.transform.position.x,this.transform.position.y, this.transform.position.z);
    }

	// Update is called once per frame
	void FixedUpdate () {
        float movingX = pointB.transform.position.x - pointA.transform.position.x;
        float movingY = pointB.transform.position.y - pointA.transform.position.y;
        float movingVectorSize = Mathf.Sqrt(movingX * movingX + movingY * movingY);
        movingVector = new Vector2(movingX / movingVectorSize, movingY / movingVectorSize);

        if (false)
        {
            directionAB = !directionAB;
        }
        if (directionAB == true)
        {
            this.GetComponent<Rigidbody2D>().velocity= (movingVector * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = (movingVector * speed);
        }
    }

    public bool DirectionJudge()
    {
        return directionAB;
    }

    public Vector3 PositionJudge()
    {
        changedPosition = new Vector3(initialPosition.x-this.transform.position.x, initialPosition.y - this.transform.position.y,initialPosition.z);
        return changedPosition;
    }
}
