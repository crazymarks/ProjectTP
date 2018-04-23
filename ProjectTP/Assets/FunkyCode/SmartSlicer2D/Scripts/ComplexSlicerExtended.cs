using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ComplexSlicerExtended {
	
	// Can Be Used In Advanced SliceInside
	static public Polygon CreateSlice(List<Vector2f> slice) 
	{
		if (slice.Count () > 2 && MathHelper.SliceIntersectItself(slice) == false)
			return(new Polygon (slice));

		return(null);
	}

	// Polygon Slice - TODO: Return No Polygon if it's eaten by polygon slice
	static public Slice2D Slice(Polygon polygon, Polygon polygonSlice)
	{
		Slice2D result = Slice2D.Create ();
		Slicer2D.SliceType tempSliceType = Slicer2D.complexSliceType;
		Slicer2D.complexSliceType = Slicer2D.SliceType.SliceHole;

		polygonSlice.Normalize ();
		polygon.Normalize ();

		// Eat a polygon completely
		// Complex Slicer does not register slice in this case
		if (polygonSlice.PolyInPoly (polygon) == true) {
			result.AddPolygon (polygon);
			return(result);
		}

		if (polygon.PolyInPoly (polygonSlice) == true) {
			polygon.AddHole (polygonSlice);
			result.AddPolygon (polygon);
			return(result);
		}

		// Act as Regular Slice
		Vector2f startPoint = null;

		foreach (Vector2f id in polygonSlice.pointsList)
			if (polygon.PointInPoly (id) == false) {
				startPoint = id;
				break;
			}


		if (startPoint == null) {
			Debug.LogError ("Slicer2D: Starting Point Error In PolygonSlice");
			return(result);
		}

		polygonSlice.pointsList = VectorList2f.GetListStartingPoint (polygonSlice.pointsList, startPoint);
		polygonSlice.AddPoint (startPoint);

		//List<Vector2f> s = new List<Vector2f> ();
		//foreach (Pair2f pair in Pair2f.GetList(polygonSlice.pointsList, false)) {
		//	List<Vector2f> stackList = polygon.GetListSliceIntersectPoly(pair);
		//	stackList = VectorList2f.GetListSortedToPoint (stackList, pair.A);
		//	s.Add (pair.A);

			//foreach (Vector2f id in stackList) 
			//	s.Add (id);
		//}

		//polygonSlice.pointsList = s;

		// Not necessary
		if (polygon.SliceIntersectPoly (polygonSlice.pointsList) == false)
			return(result);
		
		result = ComplexSlicer.Slice (polygon, new List<Vector2f> (polygonSlice.pointsList));


        //if (result.polygons.Count < 1)
            //debug用　消しても大丈夫そう
          //  Debug.LogError ("Slicer2D: Returns Empty Polygon Slice");

		Slicer2D.complexSliceType = tempSliceType;

		return(result);
	}
}