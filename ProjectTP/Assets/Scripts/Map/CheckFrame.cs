using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFrame : MonoBehaviour {
    public GameObject buttonOff;
    public GameObject buttonOn;
    public GameObject leverOn;
    public GameObject leverOff;

    public List<GameObject>  checkList=null;
    

     void OnTriggerEnter2D(Collider2D tempObject)
    {
        if (checkList.Count == 0)
        {
            checkList.Add( tempObject.gameObject);
        }

        for (int i = 0; i < checkList.Count; i++)
        {

           if (tempObject.gameObject == checkList[i])
            {
                return;
            }
        }
        checkList.Add(tempObject.gameObject);      
    }


    void OnCollisionEnter2D(Collision2D TempObject)
    {
        Debug.Log(TempObject.gameObject.name);
    }


    public void HandleItem()　　//チェックしたものを扱う
    {

        if (checkList.Count > 0)
        {
            for (int i = 0; i < checkList.Count; i++)
            {
                switch (checkList[i].tag)
                {
                    case "Lever":
                        if (checkList[i].GetComponent<Lever>().isOpen == true)
                        {
                            GameObject tempObject2 = Instantiate(leverOn, checkList[i].transform.position, Quaternion.identity);
                            tempObject2.GetComponent<Pauser>().Pause();
                            Destroy(checkList[i]);
                        }
                        else
                        {
                            GameObject tempObject2 = Instantiate(leverOff, checkList[i].transform.position, Quaternion.identity);
                            tempObject2.GetComponent<Pauser>().Pause();
                            Destroy(checkList[i]);
                        }
                        break;

                    case "Button":
                        if (checkList[i].GetComponent<Button>().isOpen == true)
                        {
                            GameObject tempObject2 = Instantiate(buttonOn, checkList[i].transform.position, Quaternion.identity);
                            tempObject2.GetComponent<Pauser>().Pause();
                            Destroy(checkList[i]);
                        }
                        else
                        {
                            GameObject tempObject2 = Instantiate(buttonOff, checkList[i].transform.position, Quaternion.identity);
                            tempObject2.GetComponent<Pauser>().Pause();
                            Destroy(checkList[i]);
                        }
                        break;


                    case "Door"://未完成

                        break;
                }
            }
            checkList.Clear();
        }

    }
}
