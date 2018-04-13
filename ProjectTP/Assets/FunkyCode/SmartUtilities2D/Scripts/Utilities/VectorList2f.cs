using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VectorList2f {
	
	/// <summary>
	/// Return a sorted list by distance to a given 2D point reference
	/// </summary>
	static public List<Vector2f> GetListSortedToPoint(List<Vector2f> pointsList, Vector2f point)
	{
		List<Vector2f> resultList = new List<Vector2f>();
		List<Vector2f> listCopy = new List<Vector2f> (pointsList);
		while (listCopy.Count > 0)
		{
			float dist = 1e+10f;
			Vector2f obj = null;
			foreach (Vector2f p in listCopy) {
				float d = Vector2f.Distance(point, p);
				if (d < dist)
				{
					obj = p;
					dist = d;
				}
			}
			if (obj != null)
			{
				resultList.Add(obj);
				listCopy.Remove(obj);
			}
		}
		return(resultList);
	}

	/// <summary>
	/// Return a list which starts with given 2D vector
	/// </summary>
	static public List<Vector2f> GetListStartingPoint(List<Vector2f> pointsList, Vector2f point)
	{
		// What if pointList does not contain point? 
		List<Vector2f> result = new List<Vector2f> ();
		bool start = false;
		foreach (Vector2f p in pointsList) 
			if (p == point || start == true) {
				result.Add (p);
				start = true;
			}

		foreach (Vector2f p in pointsList) {
			if (p == point)
				start = false;
			if (start == true) 
				result.Add (p);
		}
		return(result);
	}

	/// <summary>
	/// Return a list which starts with first interesction with given line
	/// </summary>
	public static List<Vector2f> GetListStartingIntersectLine(List<Vector2f> pointsList, Pair2f line)
	{
		List<Vector2f> result = new List<Vector2f> ();
		bool start = false;
		foreach (Pair2f p in Pair2f.GetList(pointsList)) {
			Vector2f r = MathHelper.GetPointLineIntersectLine (p, line);
			if (start == true)
				result.Add (p.A);

			if (r != null) {
				result.Add (r);
				start = true;
			}
		}

		foreach (Pair2f p in Pair2f.GetList(pointsList)) {
			Vector2f r = MathHelper.GetPointLineIntersectLine (p, line);
			if (start == true)
				result.Add (p.A);

			if (r != null) {
				result.Add (r);
				start = false;
			}
		}
		return(result);
	}

	// Might Break (Only for 2 collisions)
	/// <summary>
	/// Return a list which starts with first interesction with given slice
	/// </summary>
	public static List<Vector2f> GetListStartingIntersectSlice(List<Vector2f> pointsList, List<Vector2f> slice)
	{
		List<Vector2f> result = new List<Vector2f> ();
		bool start = false;
		foreach (Pair2f p in Pair2f.GetList(pointsList)) {
			List<Vector2f> r = MathHelper.GetListLineIntersectSlice (p, slice);
			if (start == true)
				result.Add (p.A);

			if (r.Count > 0) {
				result.Add (r.First());
				start = true;
			}
		}

		foreach (Pair2f p in Pair2f.GetList(pointsList)) {
			List<Vector2f> r = MathHelper.GetListLineIntersectSlice (p, slice);
			if (start == true)
				result.Add (p.A);

			if (r.Count > 0) {
				result.Add (r.First());
				start = false;
			}
		}
		return(result);
	}
}
