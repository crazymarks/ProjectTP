using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 2D points list connected by pairs
/// </summary>
public class Pair2f {

	/// <summary>
	/// First vector of a pair
	/// </summary>
	public Vector2f A;

	/// <summary>
	/// Second vector of a pair
	/// </summary>
	public Vector2f B;

	/// <summary>
	/// 2D points list connected by pairs
	/// </summary>
	public Pair2f(Vector2f pointA, Vector2f pointB)
	{
		A = pointA;
		B = pointB;
	}
		
	/// <summary>
	/// 2D points list connected by pairs
	/// </summary>
	static public List<Pair2f> GetList(List<Vector2f> list, bool connect = true)
	{
		List<Pair2f> pairsList = new List<Pair2f>();
		if (list.Count > 0)
		{
			Vector2f p0 = null;
			if (connect == true)
				p0 = list.Last ();
			foreach (Vector2f p1 in list) {
				if (p0 != null)
					pairsList.Add (new Pair2f (p0, p1));
				p0 = p1;
			}
		}
		return(pairsList);
	}

	/// <summary>
	/// Creates a pair with 2 vectors using (0, 0) coordinates
	/// </summary>
	public static Pair2f Zero()
	{
		return(new Pair2f (new Vector2f (0, 0), new Vector2f (0, 0)));
	}
}
