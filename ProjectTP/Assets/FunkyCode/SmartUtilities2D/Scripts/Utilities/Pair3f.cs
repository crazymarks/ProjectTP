using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pair3f {

	public Vector2f A;
	public Vector2f B;
	public Vector2f C;

	public Pair3f(Vector2f pointA, Vector2f pointB, Vector2f pointC)
	{
		A = pointA;
		B = pointB;
		C = pointC;
	}

	static public List<Pair3f> GetList(List<Vector2f> list, bool connect = true)
	{
		List<Pair3f> pairsList = new List<Pair3f>();
		if (list.Count > 0)
			foreach (Vector2f pB in list) {
				int indexB = list.IndexOf (pB);

				int indexA = (indexB - 1);
				if (indexA < 0)
					indexA += list.Count;

				int indexC = (indexB + 1);
				if (indexC >= list.Count)
					indexC -= list.Count;

				pairsList.Add (new Pair3f (list[indexA], pB, list[indexC]));
			}
		return(pairsList);
	}
}
