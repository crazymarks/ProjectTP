using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : MonoBehaviour {
    public GameObject pointA = null;
    public GameObject pointB = null;

    public float speed = 1f;
    [HideInInspector]
    public bool directionAB = true;
    private Vector2 movingVector= new Vector2 (0,0);
    private Vector3 initialPosition;
    private Vector3 changedPosition;
    private int stopCount = 0;  //止まる時間を数える
    private Vector3 lastPosition;//動いたか　を判断する
    private bool canMove = true; //スイッチによって。移動するかどうかを確認
    bool firstSwitchState= false;　//一つ目のスイッチの状態
    bool secondSwitchState = false;  //二つ目のスイッチの状態
    [HideInInspector]
    public List<GameObject> firstSwitch = null;
    [HideInInspector]
    public List<GameObject> secondSwitch = null;


    void Start()
    {
        initialPosition = new Vector3(this.transform.position.x,this.transform.position.y, this.transform.position.z);
        float movingX = pointB.transform.position.x - pointA.transform.position.x;
        float movingY = pointB.transform.position.y - pointA.transform.position.y;
        float movingVectorSize = Mathf.Sqrt(movingX * movingX + movingY * movingY);
        movingVector = new Vector2(movingX / movingVectorSize, movingY / movingVectorSize);
    }

    // Update is called once per frame
    void FixedUpdate() {
        SwitchJudge();
        if (Vector3.Distance(this.transform.position,pointA.transform.position)>Vector3.Distance(pointA.transform.position,pointB.transform.position)&&directionAB==true)
        {
            directionAB = !directionAB;
            DirectionChange();           
        }else if(Vector3.Distance(this.transform.position, pointB.transform.position) > Vector3.Distance(pointA.transform.position, pointB.transform.position) && directionAB == false)
        {
            directionAB = !directionAB;
            DirectionChange();
        }

        if (canMove)    //スイッチによって、コントロールする
        {
            DirectionChange();
            if (lastPosition == this.transform.position)
            {

                stopCount++;
                if (stopCount > 20)
                {
                    directionAB = !directionAB;
                    stopCount = 0;
                    DirectionChange();
                }
            }
            else
            {
                stopCount = 0;
            }
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }    
        lastPosition = this.transform.position;
    }


    public Vector3 PositionJudge()
    {
        changedPosition = new Vector3(initialPosition.x-this.transform.position.x, initialPosition.y - this.transform.position.y,initialPosition.z);
        return changedPosition;
    }


    void DirectionChange()
    {
        if (directionAB == true)
        {
            this.GetComponent<Rigidbody2D>().velocity = (movingVector * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = -(movingVector * speed);
        }
    }

    /// <summary>
    /// 各レバーとボタンを管理
    /// 順番も付ける
    /// </summary>
    void SwitchHandle(GameObject obj)
    {

        switch (obj.GetComponent<SwitchID>().idNumber)
        {   
            case 1:    //一番目のスイッチ
                if (firstSwitch.Count != 0)
                {
                    for(int i=0;i< firstSwitch.Count; i++)
                    {
                        if (obj == firstSwitch[i])
                        {
                            break;
                        }
                        if (i == firstSwitch.Count - 1)
                        {
                            firstSwitch.Add(obj);
                        }
                    }
                }
                else
                {
                    firstSwitch.Add(obj);
                }
                break;

            case 2:　　　　//二番目のスイッチ
                if (secondSwitch.Count != 0)
                {
                    for (int i = 0; i < secondSwitch.Count; i++)
                    {
                        if (obj == secondSwitch[i])
                        {
                            break;
                        }
                        if (i == secondSwitch.Count-1)
                        {
                            secondSwitch.Add(obj);
                        }
                    }
                }
                else
                {
                    secondSwitch.Add(obj);
                }
                break;
        }
    }

    void SwitchJudge()
    {
        if (firstSwitch.Count == 0 && secondSwitch.Count == 0)
        {
            canMove = true;
        }
        else
        {
            //一つ目のスイッチ判定
            for (int i = 0; i < firstSwitch.Count; i++)
            {
                if (firstSwitch[i] != null && firstSwitch[i].tag == "Button")
                {
                    if (firstSwitch[i].GetComponent<Button>().isOpen == true)
                    {
                        firstSwitchState = true;
                        break;
                    }
                }
                else if (firstSwitch[i] != null && firstSwitch[i].tag == "Lever")
                {
                    if (firstSwitch[i].GetComponent<Lever>().isOpen == true)
                    {
                        firstSwitchState = true;
                        break;
                    }
                }
                if (firstSwitch[i] != null && i == firstSwitch.Count - 1)
                {
                    firstSwitchState = false;
                }
            }
            //二つ目のスイッチ判定
            for (int i = 0; i < secondSwitch.Count; i++)
            {
                if (secondSwitch[i] != null && secondSwitch[i].tag == "Button")
                {
                    if (secondSwitch[i].GetComponent<Button>().isOpen == true)
                    {
                        secondSwitchState = true;
                        break;
                    }
                }
                else if (secondSwitch[i] != null && secondSwitch[i].tag == "Lever")
                {
                    if (secondSwitch[i].GetComponent<Lever>().isOpen == true)
                    {
                        secondSwitchState = true;
                        break;
                    }
                }
                if (secondSwitch[i] != null && i == secondSwitch.Count - 1)
                {
                    secondSwitchState = false;
                }
            }

            //スイッチ判定 スイッチ１がある場合　スイッチ２がある場合　両方もある場合
            if (firstSwitch.Count == 0 && secondSwitch.Count != 0)
            {
                canMove = secondSwitchState;
            }
            else if (firstSwitch.Count != 0 && secondSwitch.Count == 0)
            {
                canMove = firstSwitchState;
            }
            else if(firstSwitch.Count != 0 && secondSwitch.Count != 0)
            {
                canMove = firstSwitchState && secondSwitchState;
            }
        } 
    }
}

