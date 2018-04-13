using System.Collections.Generic;
using UnityEngine;

public class Demo6Connector : MonoBehaviour {
	public GameObject connector;
	private Polygon polygon;

	void Awake () {
		Slicer2D slicer = GetComponent<Slicer2D> ();
		slicer.AddResultEvent (OnSliceResult);

		polygon = Polygon.CreateFromCollider (connector);
	}

	void OnSliceResult(List<GameObject> gList)
	{
		Polygon polyA = polygon.ToWorldSpace (transform);
		foreach (GameObject p in gList) {
			if (MathHelper.PolyCollidePoly (polyA.pointsList, Polygon.CreateFromCollider (p).ToWorldSpace(p.transform).pointsList) == false) 
				if (p.GetComponent<Rigidbody2D>() == null)
					p.AddComponent<Rigidbody2D> ();
		}
	}
}