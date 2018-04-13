using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ComplexSlicer {
	public static float precision = 0.0001f;

	static public Slice2D Slice(Polygon polygon, List<Vector2f> slice)
	{
		Slice2D result = Slice2D.Create ();

		if (slice.Count < 2)
			return(result);	

		// Normalize into clockwise
		polygon.Normalize ();

		if (Slicer2D.complexSliceType != Slicer2D.SliceType.Regular) {
			result = SlicePolygonInside (polygon, slice);
			if (result.polygons.Count > 0)
				return(result);
		}

		// Optimization (holes?)
		// if (polygon.SliceIntersectPoly (slice) == false)
		//	return(result);

		List<List<Vector2f>> slices = new List<List<Vector2f>>();

		bool entered = polygon.PointInPoly (slice.First ());
	
		List<Vector2f> currentSlice = new List<Vector2f> ();

		foreach (Pair2f pair in Pair2f.GetList(slice, false)) {
			List<Vector2f> stackList = polygon.GetListSliceIntersectPoly(pair);
			stackList = VectorList2f.GetListSortedToPoint (stackList, pair.A);

			foreach (Vector2f id in stackList) {
				if (entered == true) {
					currentSlice.Add (id);
					slices.Add (currentSlice);
				} else {
					currentSlice = new List<Vector2f> ();
					currentSlice.Add (id);
				}
				entered = !entered;
			}

			if (entered == true)
				currentSlice.Add (pair.B);
		}

		// Adjusting split lines before performing convex split
		result.AddPolygon(polygon);

		foreach (List<Vector2f> id in slices) 
			if (id.Count > 1) {
				foreach (Vector2f p in id) 
					result.AddCollision (p);

				// Sclice line points generated from intersections list
				Vector2f vec0 = id.First();
				vec0.Push (Vector2f.Atan2 (vec0, id[1]), precision);

				Vector2f vec1 = id.Last();
				vec1.Push (Vector2f.Atan2 (vec1, id[id.Count - 2]), precision);

				// For each in polygons list attempt convex split
				List<Polygon> temp = new List<Polygon>(result.polygons); // necessary?
				foreach (Polygon poly in temp) {
					Slice2D resultList = SingleSlice(poly, id); 

					if (resultList.polygons.Count > 0)
					{
						foreach (Polygon i in resultList.polygons)
							result.AddPolygon(i);

						// If it's possible to perform convex split, remove parent polygon from result list
						result.RemovePolygon(poly);
					}
				}
			}
		result.RemovePolygon (polygon);
		return(result);
	}

	//if (slice.Count == 2) return(PolygonSimpleSlicer.SplitLineConcave (polygon, new Pair2f (slice.First (), slice.Last ())));
	static private Slice2D SingleSlice(Polygon polygon, List<Vector2f> slice)
	{
		Slice2D result = Slice2D.Create ();

		if (polygon.PointInPoly (slice.First ()) == true || polygon.PointInPoly (slice.Last ()) == true) 
			return(result);

		slice = new List<Vector2f> (slice);

		List <Vector2f> collisionSlice = new List<Vector2f> ();

		int collisionCount = 0;
		bool enterCollision = false;

		// Generate correct intersected slice
		foreach (Pair2f pair in Pair2f.GetList(slice, false)) {
			List<Vector2f> intersections = polygon.GetListSliceIntersectPoly(pair);

			if (intersections.Count > 0) {
				if (intersections.Count == 1) {
					collisionCount += 1;
					enterCollision = !enterCollision;
				}
				if (intersections.Count == 2)
					collisionCount += intersections.Count; // Check if it's okay
				
				if (intersections.Count > 2) {
					//Debug.LogError ("Slicer2D: Unexpected Error 2"); // When this happens?
					return(result);
				}
			}

			if (enterCollision == true)
				collisionSlice.Add (pair.B);
		}

		List<Polygon> intersectHoles = polygon.SliceIntersectHoles (slice);

		switch (intersectHoles.Count) {
			case 0:
				if (collisionCount == 2)
					return(SliceWithoutHoles (polygon, slice, collisionSlice));
				break;

			case 1:
				return(SliceWithOneHole(polygon, slice, collisionSlice));

			case 2:
				return(SliceWithTwoHoles (polygon, slice, collisionSlice));

			default:
				break;
			}

		return(result);
	}

	static private Slice2D SliceWithOneHole(Polygon polygon, List<Vector2f> slice, List<Vector2f> collisionSlice) {
		Slice2D result = Slice2D.Create ();
	
		Polygon holeA = polygon.PointInHole (slice.First ());
		Polygon holeB = polygon.PointInHole (slice.Last ());
		Polygon holePoly = (holeA != null) ? holeA : holeB;


		if (polygon.PointInPoly (slice.First ()) == false || polygon.PointInPoly (slice.Last ()) == false) { 
			if (holeA == holeB) { 

				if (Slicer2D.complexSliceType == Slicer2D.SliceType.Regular)
					return(result);

				if (holeA == null) {
					Debug.LogError ("Slicer2D: This happened when collider had a lot of paths but they were not holes");
					return(result);
				}

				List<Vector2f> slice2 = new List<Vector2f> (slice);

				Polygon polyA = new Polygon (polygon.pointsList);
				Polygon polyB = new Polygon (slice);
				Polygon polyC = new Polygon (slice2);

				// Get First Point - NOT FINISHED WITH INTERSECTION
				int Add;
				List<Vector2f> list;
				List<Pair2f> iterateList = Pair2f.GetList (holeA.pointsList);

				Add = 0;
				list = new List<Vector2f> ();
				foreach (Pair2f pair in iterateList) {
					List<Vector2f> intersect = MathHelper.GetListLineIntersectSlice (pair, slice);
					if (intersect.Count > 0)
						Add += intersect.Count;

					if (Add == 1)
						list.Add (pair.B);
				}

				if (list.Count > 0) {
					if (Vector2f.Distance (list.First (), slice.First ()) < Vector2f.Distance (list.First (), slice.Last ()))
						slice.Reverse ();

					polyB.AddPoints (list);
				}

				Add = 0;
				list = new List<Vector2f> ();
				foreach (Pair2f pair in iterateList) {
					List<Vector2f> intersect = MathHelper.GetListLineIntersectSlice (pair, slice2);
					if (intersect.Count > 0)
						Add += intersect.Count;

					if (Add == 2)
						list.Add (pair.B);
				}

				foreach (Pair2f pair in iterateList) {
					List<Vector2f> intersect = MathHelper.GetListLineIntersectSlice (pair, slice2);
					if (intersect.Count > 0)
						Add += intersect.Count;

					if (Add == 2)
						list.Add (pair.B);
				}

				if (list.Count > 0) {
					if (Vector2f.Distance (list.First (), slice2.First ()) < Vector2f.Distance (list.First (), slice2.Last ()))
						slice2.Reverse ();

					polyC.AddPoints (list); 
				}

				if (polyB.GetArea () > polyC.GetArea ()) {
					Polygon swap = polyB;
					polyB = polyC;
					polyC = swap;
				}

				// Add holes to new created polygon
				foreach (Polygon poly in polygon.holesList)
					if (poly != holeA && polyB.PolyInPoly(poly) == true)
						polyB.AddHole (poly);

				if (Slicer2D.complexSliceType == Slicer2D.SliceType.FillSlicedHole)
					result.AddPolygon (polyB);

				polyA.AddHole (polyC);

				// Adds polygons if they are not in the hole
				foreach (Polygon poly in polygon.holesList) // Check for errors?
					if (poly != holeA && polyC.PolyInPoly(poly) == false)
						polyA.AddHole (poly);

				result.AddPolygon (polyA);
				return(result);
			} else if (holePoly != null) {
				Polygon polyA = new Polygon ();
				Polygon polyB = new Polygon (holePoly.pointsList);
				polyB.pointsList.Reverse ();

				List<Vector2f> pointsA = VectorList2f.GetListStartingIntersectSlice (polygon.pointsList, slice);
				List<Vector2f> pointsB = VectorList2f.GetListStartingIntersectSlice (polyB.pointsList, slice);

				if (pointsA.Count < 1)
					Debug.LogError ("Slicer2D: " + pointsA.Count + " " + polygon.pointsList.Count);

				polyA.AddPoints (pointsA);

				if (collisionSlice.Count > 0) {
					// pointsA empty
					if (Vector2f.Distance (pointsA.Last (), collisionSlice.Last ()) < Vector2f.Distance (pointsA.Last (), collisionSlice.First ()))
						collisionSlice.Reverse ();

					polyA.AddPoints (collisionSlice);
				}

				polyA.AddPoints (pointsB);

				if (collisionSlice.Count > 0) {
					collisionSlice.Reverse ();
					polyA.AddPoints (collisionSlice);
				}

				foreach (Polygon poly in polygon.holesList) // Check for errors?
					if (poly != holePoly)
						polyA.AddHole (poly);

				result.AddPolygon (polyA);

				return(result);
			}
		}
							
		return(result);
	}

	static private Slice2D SliceWithTwoHoles(Polygon polygon, List<Vector2f> slice, List<Vector2f> collisionSlice) {
		Slice2D result = Slice2D.Create ();

		Polygon polyA = new Polygon ();
		Polygon polyB = new Polygon (polygon.pointsList);

		Polygon holeA = polygon.PointInHole (slice.First ());
		Polygon holeB = polygon.PointInHole (slice.Last ());

		if (holeA == null || holeB == null) {
			Debug.LogError ("Slicer2D: ERROR Split"); // Shouldn't really happen
			return(result);
		}

		List<Vector2f> pointsA = VectorList2f.GetListStartingIntersectSlice (holeA.pointsList, slice);
		List<Vector2f> pointsB = VectorList2f.GetListStartingIntersectSlice (holeB.pointsList, slice);

		polyA.AddPoints (pointsA);

		if (collisionSlice.Count > 0) {
			if (Vector2f.Distance (pointsA.Last (), collisionSlice.Last ()) < Vector2f.Distance (pointsA.Last (), collisionSlice.First ()))
				collisionSlice.Reverse ();

			polyA.AddPoints (collisionSlice);
		}

		polyA.AddPoints (pointsB);

		if (collisionSlice.Count > 0) {
			collisionSlice.Reverse ();

			polyA.AddPoints (collisionSlice);
		}

		foreach (Polygon poly in polygon.holesList)
			if (poly != holeA && poly != holeB)
				polyB.AddHole (poly);

		polyB.AddHole (polyA);
		result.AddPolygon (polyB);

		return(result);
	}

	static private Slice2D SliceWithoutHoles(Polygon polygon, List<Vector2f> slice, List<Vector2f> collisionSlice) {
		Slice2D result = Slice2D.Create ();

		// Simple non-hole slice
		Polygon polyA = new Polygon ();
		Polygon polyB = new Polygon ();

		Polygon currentPoly = polyA;

		foreach (Pair2f p in Pair2f.GetList(polygon.pointsList)) {
			List <Vector2f> intersections = MathHelper.GetListLineIntersectSlice (p, slice);
			Vector2f intersection = null;
			if (intersections.Count () > 0)
				intersection = intersections.First ();

			if (intersections.Count > 0) { // Only if 1 or 2
				if (intersections.Count == 2) { 
					Vector2f first = intersections.First ();
					Vector2f last = intersections.Last ();

					if (Vector2f.Distance (last, p.A) < Vector2f.Distance (first, p.A)) {
						first = intersections.Last ();
						last = intersections.First ();
					}

					currentPoly.AddPoint (first);

					if (collisionSlice.Count > 0) {
						if (Vector2f.Distance (first, collisionSlice.Last ()) < Vector2f.Distance (first, collisionSlice.First ()))
							collisionSlice.Reverse ();

						currentPoly.AddPoints (collisionSlice);
					}

					currentPoly.AddPoint (last);
					currentPoly.AddPoint (p.B);

					currentPoly = polyB;

					if (collisionSlice.Count > 0)
						currentPoly.AddPoints (collisionSlice);

					currentPoly.AddPoint (last);
					currentPoly.AddPoint (first);

					currentPoly = polyA;
				}

				if (intersections.Count == 1) {
					currentPoly.AddPoint (intersection);

					if (collisionSlice.Count > 0) {
						if (Vector2f.Distance (intersection, collisionSlice.Last ()) < Vector2f.Distance (intersection, collisionSlice.First ()))
							collisionSlice.Reverse ();

						currentPoly.AddPoints (collisionSlice);
					}
						
					currentPoly = (currentPoly == polyA) ? polyB : polyA;

					currentPoly.AddPoint (intersection);
				}
			}
			currentPoly.AddPoint (p.B);
		}

		result.AddPolygon (polyA);
		result.AddPolygon (polyB);

		foreach (Polygon poly in result.polygons)
			foreach (Polygon hole in polygon.holesList)
				if (poly.PolyInPoly (hole) == true)
					poly.AddHole (hole);	

		return(result);
	}


	static private Slice2D SlicePolygonInside(Polygon polygon, List<Vector2f> slice) // Create Polygon Inside?
	{
		Slice2D result = Slice2D.Create ();

		Polygon newPoly = new Polygon ();

		bool createPoly = false;
		foreach (Pair2f pairA in Pair2f.GetList(slice, false)) {
			foreach (Pair2f pairB in Pair2f.GetList(slice, false)) {
				Vector2f intersection = MathHelper.GetPointLineIntersectLine (pairA, pairB);
				if (intersection != null && (pairA.A != pairB.A && pairA.B != pairB.B && pairA.A != pairB.B && pairA.B != pairB.A)) {
					createPoly = !createPoly;
					newPoly.AddPoint (intersection);
				}
			}
			if (createPoly == true)
				newPoly.AddPoint (pairA.B);
		}

		bool inHoles = false;
		foreach (Polygon hole in polygon.holesList)
			if (hole.PolyInPoly (newPoly) == true)
				inHoles = true;

		if (inHoles == false && newPoly.pointsList.Count > 2 && polygon.PolyInPoly (newPoly) == true) {
			polygon.AddHole (newPoly);
			List <Polygon> polys = new List<Polygon> (polygon.holesList);
			foreach (Polygon hole in polys)
				if (newPoly.PolyInPoly (hole) == true) {
					polygon.holesList.Remove (hole);
					newPoly.AddHole (hole);
				}

			result.AddPolygon (polygon);
			if (Slicer2D.complexSliceType == Slicer2D.SliceType.FillSlicedHole)
				result.AddPolygon (newPoly);
			
			return(result);
		}

		return(result);
	}
}
