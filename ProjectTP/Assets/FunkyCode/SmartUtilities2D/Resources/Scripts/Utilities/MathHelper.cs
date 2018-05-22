using System.Collections.Generic;
using UnityEngine;

public class MathHelper {

	public static Rect GetListBounds(List<Vector2f> pointsList)
	{
		float rMinX = 1e+10f;
		float rMinY = 1e+10f;
		float rMaxX = -1e+10f;
		float rMaxY = -1e+10f;

		foreach (Vector2f id in pointsList) {
			rMinX = Mathf.Min (rMinX, id.GetX ());
			rMinY = Mathf.Min (rMinY, id.GetY ());
			rMaxX = Mathf.Max (rMaxX, id.GetX ());
			rMaxY = Mathf.Max (rMaxY, id.GetY ());
		}

		return(new Rect(rMinX, rMinY, Mathf.Abs(rMinX - rMaxX), Mathf.Abs(rMinY - rMaxY))); 
	}

	public static bool PolyInPoly(List<Vector2f> polyIn, List<Vector2f> poly)
	{
		foreach (Pair2f p in Pair2f.GetList(poly))
			if (PointInPoly (p.A, polyIn) == false)
				return(false);

		if (PolyIntersectPoly (polyIn, poly) == true)
			return(false);
		
		return(true);
	}

	// not finished
	public static bool PolyCollidePoly(List <Vector2f> polyA, List <Vector2f> polyB)
	{
		if (PolyIntersectPoly (polyA, polyB) == true)
			return(true);

		if (PolyInPoly (polyA, polyB) == true)
			return(true);

		if (PolyInPoly (polyB, polyA) == true)
			return(true);
		
		return(false);
	}

	public static bool PolyIntersectPoly(List <Vector2f> polyA, List <Vector2f> polyB)
	{
		foreach (Pair2f a in Pair2f.GetList(polyA)) 
			foreach (Pair2f b in Pair2f.GetList(polyB)) 
				if (LineIntersectLine (a, b))
					return(true);

		return(false);
	}

	public static bool SliceIntersectPoly(List <Vector2f> slice, List <Vector2f> polyB)
	{
		foreach (Pair2f a in Pair2f.GetList(slice, false)) 
			foreach (Pair2f b in Pair2f.GetList(polyB)) 
				if (LineIntersectLine (a, b))
					return(true);

		return(false);
	}
		
	public static bool LineIntersectPoly(Pair2f line, List <Vector2f> poly)
	{
		foreach (Pair2f b in Pair2f.GetList(poly)) 
			if (LineIntersectLine (line, b))
				return(true);
		
		return(false);
	}

	public static bool LineIntersectLine(Pair2f lineS, Pair2f lineE)
	{
		if (GetPointLineIntersectLine (lineS, lineE) != null)
			return(true);

		return(false);
	}

	public static bool SliceIntersectItself(List<Vector2f> slice)
	{
		foreach (Pair2f pairA in Pair2f.GetList(slice, true))
			foreach (Pair2f pairB in Pair2f.GetList(slice, true))
				if (MathHelper.GetPointLineIntersectLine (pairA, pairB) != null)
					if (pairA.A != pairB.A && pairA.B != pairB.B && pairA.A != pairB.B && pairA.B != pairB.A)
						return(true);
		return(false);
	}

	public static Vector2f GetPointLineIntersectLine(Pair2f lineS, Pair2f lineE)
	{
		float ay_cy, ax_cx, px, py;
		float dx_cx = lineE.B.GetX() - lineE.A.GetX();
		float dy_cy = lineE.B.GetY() - lineE.A.GetY();
		float bx_ax = lineS.B.GetX() - lineS.A.GetX();
		float by_ay = lineS.B.GetY() - lineS.A.GetY();
		float de = (bx_ax) * (dy_cy) - (by_ay) * (dx_cx);
		float tor = 1E-10f;

		if (Mathf.Abs(de) < 0.01f) 
			return(null);

		if (de > - tor && de < tor)
			return(null);

		ax_cx = lineS.A.GetX() - lineE.A.GetX();
		ay_cy = lineS.A.GetY() - lineE.A.GetY();

		float r = ((ay_cy) * (dx_cx) - (ax_cx) * (dy_cy)) / de;
		float s = ((ay_cy) * (bx_ax) - (ax_cx) * (by_ay)) / de;

		px = lineS.A.Get().x + r * (bx_ax);
		py = lineS.A.Get().y + r * (by_ay);

		if ((r < 0) || (r > 1) || (s < 0)|| (s > 1))
			return(null);

		return(new Vector2f (px, py));
	}

	public static bool PointInPoly(Vector2f point, List<Vector2f> xy)
	{
		if (xy.Count < 3)
			return(false);

		int total = 0;
		int diff = 0;

		foreach (Pair2f id in Pair2f.GetList(xy)) {
			diff = (GetQuad (point, id.A) - GetQuad (point, id.B));

			switch (diff) {
				case -2: case 2:
					if (( id.B.GetX() - (((id.B.GetY() - point.GetY()) * (id.A.GetX() - id.B.GetX())) / (id.A.GetY() - id.B.GetY()))) < point.GetX())
						diff = -diff;

					break;

				case 3:
					diff = -1;
					break;

				case -3:
					diff = 1;
					break;

				default:
					break;   
			}

			total += diff;
		}

		return(Mathf.Abs(total) == 4);
	}

	private static int GetQuad(Vector2f axis, Vector2f vert)
	{
		if (vert.GetX() < axis.GetX()) {
			if (vert.GetY() < axis.GetY())
				return(1);
			return(4);
		}
		if (vert.GetY() < axis.GetY())
			return(2);
		return(3);
	}
		
	// Getting List is Slower
	public static List <Vector2f> GetListLineIntersectPoly(Pair2f line, List <Vector2f> poly)
	{
		List <Vector2f> result = new List <Vector2f>() ;
		foreach (Pair2f b in Pair2f.GetList(poly)) {
			Vector2f intersection = GetPointLineIntersectLine (line, b);
			if (intersection != null)
				result.Add(intersection);
		}
		return(result);
	}

	public static List<Vector2f> GetListPolyIntersectSlice(List<Vector2f> pointsList, Pair2f slice)
	{
		List<Vector2f> resultList = new List<Vector2f> ();
		foreach (Pair2f id in Pair2f.GetList(pointsList)) {
			Vector2f result = GetPointLineIntersectLine(id, slice);
			if (result != null)
				resultList.Add(result);
		}
		return(resultList);
	}

	public static List<Vector2f> GetListLineIntersectSlice(Pair2f pair, List<Vector2f> slice)
	{
		List<Vector2f> resultList = new List<Vector2f> ();
		foreach (Pair2f id in Pair2f.GetList(slice, false)) {
			Vector2f result = GetPointLineIntersectLine(id, pair);
			if (result != null)
				resultList.Add(result);
		}
		return(resultList);
	}

	public static Vector2f ReflectAngle(Vector2f v, float wallAngle)
	{
		//normal vector to the wall
		Vector2 n = new Vector2( Mathf.Cos(wallAngle + Mathf.PI / 2), Mathf.Sin(wallAngle + Mathf.PI / 2));

		// p is the projection of V onto the normal
		float dotproduct = v.GetX() * n.x + v.GetY() * n.y;

		// the velocity after hitting the wall is V - 2p, so just subtract 2*p from V
		return(new Vector2f(v.GetX() - 2f * (dotproduct * n.x), v.GetY() - 2f * (dotproduct * n.y)));
	}
}
