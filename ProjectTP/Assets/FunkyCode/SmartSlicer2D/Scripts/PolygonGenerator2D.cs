using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator2D {

	static public void GenerateMesh(GameObject gameObject, Polygon polygon, Vector2 UVScale)
	{		
		if (gameObject.GetComponent<MeshRenderer>() == null)
			gameObject.AddComponent<MeshRenderer>();

		MeshFilter filter = gameObject.GetComponent<MeshFilter> ();
		if (filter == null)
			filter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
		
		filter.mesh = PolygonTriangulator2D.Triangulate (polygon, UVScale);
	}

	static public void GenerateCollider(GameObject gameObject, Polygon polygon)
	{
		PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D> ();

		if (collider == null)
			collider = gameObject.AddComponent<PolygonCollider2D> ();

		List<Vector2> points = new List<Vector2> ();

		foreach (Vector2f p in polygon.pointsList)
			points.Add(p.Get());

		collider.pathCount = (1 + polygon.holesList.Count);

		collider.enabled = false;

		collider.SetPath(0, points.ToArray());

		if (polygon.holesList.Count > 0) {
			int pathID = 1;
			List<Vector2> pointList = null;

			foreach (Polygon poly in polygon.holesList) {
				pointList = new List<Vector2> ();

				foreach (Vector2f p in poly.pointsList)
					pointList.Add (p.Get ());

				collider.SetPath (pathID, pointList.ToArray ());
				pathID += 1;
			}
		}

		collider.enabled = true;
	}
}