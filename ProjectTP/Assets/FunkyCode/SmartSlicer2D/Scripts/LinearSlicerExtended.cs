using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class LinearSlicerExtended {

	static public Slice2D PointExplode(Polygon polygon, Vector2f point)
	{
		if (polygon.PointInPoly (point) == false)
			return(Slice2D.Create());

		return(Explode(polygon));
	}

	static public Slice2D Explode(Polygon polygon)
	{
		Slice2D result = Slice2D.Create();

		Rect polyRect = polygon.GetBounds ();

		result.AddPolygon (polygon);
		int tries = 0;
		while (result.polygons.Count < Slicer2D.explosionPieces) {
			foreach (Polygon p in new List<Polygon>(result.polygons))
			{
				Slice2D newResult = SliceFromPoint (p, new Vector2f(polyRect.x + UnityEngine.Random.Range(0, polyRect.width), polyRect.y + UnityEngine.Random.Range(0, polyRect.height)), UnityEngine.Random.Range(0, Mathf.PI * 2));
				if (newResult.polygons.Count > 0) {
					foreach (Polygon poly in newResult.polygons)
						result.AddPolygon (poly);
					result.RemovePolygon (p);
				}
			}
			tries += 1;
			if (tries > 20)
				return(result);
		}

		return(result);
	}

	// Slice From Point
	static public Slice2D SliceFromPoint(Polygon polygon, Vector2f point, float rotation)
	{
		Slice2D result = Slice2D.Create();
	
		// Normalize into clockwise
		polygon.Normalize();

		Vector2f sliceA = new Vector2f (point);
		Vector2f sliceB = new Vector2f (point);

		sliceA.Push (rotation, 1e+10f / 2);
		sliceB.Push (rotation, -1e+10f / 2);

		if (polygon.PointInPoly (point) == false)
			return(result);

		// Getting the list of intersections
		List<Vector2f> intersectionsA = polygon.GetListSliceIntersectPoly(new Pair2f(point, sliceA));
		List<Vector2f> intersectionsB = polygon.GetListSliceIntersectPoly(new Pair2f(point, sliceB));

		// Sorting intersections from one point
		if (intersectionsA.Count > 0 && intersectionsB.Count > 0) {
			sliceA = VectorList2f.GetListSortedToPoint (intersectionsA, point) [0];
			sliceB = VectorList2f.GetListSortedToPoint (intersectionsB, point) [0];
		} else 
			return(result);

		List<Pair2f> collisionList = new List<Pair2f>();
		collisionList.Add (new Pair2f (sliceA, sliceB));

		result.AddPolygon(polygon);

		foreach (Pair2f id in collisionList) {
			// Sclice line points generated from intersections list
			Vector2f vec0 = new Vector2f(id.A);
			Vector2f vec1 = new Vector2f(id.B);

			float rot = Vector2f.Atan2 (vec0, vec1);

			// Slightly pushing slice line so it intersect in all cases
			vec0.Push (rot, LinearSlicer.precision);
			vec1.Push (rot, -LinearSlicer.precision);

			// For each in polygons list attempt convex split
			List<Polygon> temp = new List<Polygon>(result.polygons); // necessary?
			foreach (Polygon poly in temp) {
				// NO, that's the problem
				Slice2D resultList = LinearSlicer.Slice(poly, new Pair2f(vec0, vec1));

				if (resultList.polygons.Count > 0)
				{
					foreach (Polygon i in resultList.polygons)
						result.AddPolygon(i);

					// If it's possible to perform splice, remove parent polygon from result list
					result.RemovePolygon(poly);
				}
			}
		}
		result.RemovePolygon (polygon);
		return(result);
	}
}
