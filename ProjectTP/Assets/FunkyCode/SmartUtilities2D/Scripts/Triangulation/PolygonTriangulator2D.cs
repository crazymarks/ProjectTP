using System.Collections.Generic;
using UnityEngine;

public class PolygonTriangulator2D : MonoBehaviour {
	static float precision = 0.001f;

	public static Mesh Triangulate(Polygon polygon, Vector2 UVScale)
	{
		PreparePolygon(polygon);
		foreach (Polygon hole in polygon.holesList) 
			PreparePolygon(hole);

		return(TriangulateAdvanced(polygon, UVScale));
	}

	// Not finished - still has some artifacts
	public static void PreparePolygon(Polygon polygon)
	{
		foreach (Pair3f pA in Pair3f.GetList(polygon.pointsList)) {
			foreach (Pair3f pB in  Pair3f.GetList(polygon.pointsList)) {
				if (pA.B != pB.B && Vector2f.Distance(pA.B, pB.B) < precision) {
					pA.B.Push (Vector2f.Atan2 (new Vector2f(pA.A), new Vector2f(pA.B)), precision);
					pA.B.Push (Vector2f.Atan2 (new Vector2f(pA.B), new Vector2f(pA.C)), -precision);
					pB.B.Push (Vector2f.Atan2 (new Vector2f(pB.A), new Vector2f(pB.B)), precision);
					pB.B.Push (Vector2f.Atan2 (new Vector2f(pB.B), new Vector2f(pB.C)), -precision);
				}
			}
		}
	}

	public static Mesh TriangulateAdvanced(Polygon polygon, Vector2 UVScale)
	{
		polygon.Normalize ();
		AdvancedTriangulator.Polygon poly = new AdvancedTriangulator.Polygon();

		List<Vector3> pointsList = null;
		List<Vector2> UVpointsList = null;

		Vector3 v = Vector3.zero, uv = Vector3.zero;

		foreach (Vector2f p in polygon.pointsList) {
			v = p.Get();
			poly.outside.Add (v);
			uv = new Vector2(v.x / UVScale.x + .5f, v.y / UVScale.y + .5f);
			poly.outsideUVs.Add (uv);
		}

		foreach (Polygon hole in polygon.holesList) {
			pointsList = new List<Vector3> ();
			UVpointsList = new List<Vector2> ();
			foreach (Vector2f p in hole.pointsList) {
				v = p.Get ();
				pointsList.Add (v);
				uv = new Vector2(v.x / UVScale.x + .5f, v.y / UVScale.y + .5f);
				UVpointsList.Add (uv);
			}
			poly.holes.Add (pointsList);
			poly.holesUVs.Add (UVpointsList);
		}

		return(AdvancedTriangulator.CreateMesh (poly));
	}
}