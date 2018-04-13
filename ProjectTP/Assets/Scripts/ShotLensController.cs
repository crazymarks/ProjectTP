using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLensController : MonoBehaviour {

    struct ShotItem
    {
        public Vector3 AbsoluteCoordinate;  //the coordinate relative to lens's coordinate
        public string NameOfItem;
        public string TagOfItem;     
    }

    List<ShotItem> ItemList = new List<ShotItem>();  // the list of shot object

	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Shot")){
            for(int i=0;i<ItemList.Count;i++)
            {
                GameObject TempObject=Instantiate(GameObject.Find(ItemList[i].NameOfItem), ItemList[i].AbsoluteCoordinate, Quaternion.identity);

            }
        }
	}

    void OnTriggerEnter2D(Collider2D TempObject)
    {
        GameObject TempObject1 = TempObject.transform.gameObject;
        while (TempObject1.transform.parent!=null)
        {
              TempObject1 = TempObject.transform.parent.gameObject;
        }
        ShotItem NewShotObject;
        NewShotObject.NameOfItem = TempObject1.name;
        NewShotObject.AbsoluteCoordinate = new Vector3(TempObject1.transform.position.x,
        TempObject1.transform.position.y + 5f, TempObject1.transform.position.z);
        NewShotObject.TagOfItem = TempObject1.tag;

        ItemList.Add(NewShotObject);
        Debug.Log(ItemList[ItemList.Count - 1].NameOfItem);
        Debug.Log(ItemList[ItemList.Count - 1].AbsoluteCoordinate);
        Debug.Log(ItemList.Count);          
         
    }
    void OnTriggerExit2D(Collider2D TempObject)
    {
        int Index1 = -1;
        Index1 =ItemList.FindIndex(x => x.NameOfItem == TempObject.name);
        if (Index1!=-1)
        {
            ItemList.RemoveAt(Index1);
        }
    }
}
