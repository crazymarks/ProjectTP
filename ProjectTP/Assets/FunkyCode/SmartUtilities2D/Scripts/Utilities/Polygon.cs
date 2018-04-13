using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Polygon  {
	public enum ColliderType {Polygon, Box, Circle, Capsule, None}
	public enum PolygonType {Rectangle, Circle, Pentagon, Hexagon};
	static public int defaultCircleVerticesCount = 25;

	public List<Vector2f> pointsList = new List<Vector2f>();
	public List<Polygon> holesList = new List<Polygon>();

	public void AddPoint(Vector2f point) { pointsList.Add (point); }
	public void AddPoint(Vector2 point) { pointsList.Add (new Vector2f(point)); }
	public void AddPoint(float pointX, float pointY) { pointsList.Add (new Vector2f(pointX, pointY)); }
	public void AddPoints(List<Vector2f> points) { 
		foreach (Vector2f point in points)
			AddPoint (point);
	}

	public Polygon() {}
	public Polygon(List<Vector2f> polygonPointsList) {
		pointsList = polygonPointsList; //new List<Vector2f>(
	}
	public Polygon(Polygon polygon) {
		pointsList = polygon.pointsList;
		holesList = polygon.holesList;
	}

	public void AddHole(Polygon poly)
	{
		holesList.Add (poly);
	}
		
	public bool PointInPoly(Vector2f point)
	{
		if (PointInHole (point) != null)
			return(false);
		
		return(MathHelper.PointInPoly(point, pointsList));
	}

	public bool PolyInPoly(Polygon poly) // Not Finished?
	{
		foreach (Polygon p in holesList)
			if (MathHelper.PolyIntersectPoly (poly.pointsList, p.pointsList) == true)
				return(false);
		
		return(MathHelper.PolyInPoly(pointsList, poly.pointsList));
	}

	public Polygon PointInHole(Vector2f point)
	{
		foreach (Polygon p in holesList)
			if (p.PointInPoly (point) == true)
				return(p);

		return(null);
	}

	public Polygon ToLocalSpace(Transform transform)
	{
		Polygon newPolygon = new Polygon ();
		foreach (Vector2f id in pointsList)
			newPolygon.AddPoint (transform.InverseTransformPoint (id.Get()));

		foreach (Polygon p in holesList) 
			newPolygon.AddHole (p.ToLocalSpace (transform));

		return(newPolygon);
	}

	public Polygon ToWorldSpace(Transform transform)
	{
		Polygon newPolygon = new Polygon ();
		foreach (Vector2f id in pointsList)
			newPolygon.AddPoint (transform.TransformPoint (id.Get()));

		foreach (Polygon p in holesList) 
			newPolygon.AddHole (p.ToWorldSpace (transform));

		return(newPolygon);
	}

	public Polygon ToOffset(Vector2f pos)
	{
		Polygon newPolygon = new Polygon (pointsList);
		foreach (Vector2f p in newPolygon.pointsList)
			p.Inc (pos);

		foreach (Polygon p in holesList) 
			newPolygon.AddHole (p.ToOffset(pos));

		return(newPolygon);
	}
		
	public bool IsClockwise()
	{
		double sum = 0;
		foreach (Pair2f id in Pair2f.GetList(pointsList))
			sum += (id.B.GetX() - id.A.GetX()) * (id.B.GetY() + id.A.GetY());

		return(sum > 0);
	}

	public void Normalize()
	{
		if (IsClockwise () == false)
			pointsList.Reverse ();

		foreach (Polygon p in holesList)
			p.Normalize ();
	}

	public float GetArea()
	{
		float area = 0f;
		foreach (Pair2f id in Pair2f.GetList(pointsList))
			area += ((id.B.GetX() - id.A.GetX()) * (id.B.GetY() + id.A.GetY())) / 2.0f;

		foreach (Polygon p in holesList)
			area -= p.GetArea ();

		return(Mathf.Abs(area)); 
	}

	public Rect GetBounds()
	{
		return(MathHelper.GetListBounds(pointsList)); 
	}

	public List<Polygon> LineIntersectHoles(Pair2f pair)
	{
		List<Polygon> resultList = new List<Polygon> ();
		foreach (Polygon poly in holesList)
			if (MathHelper.LineIntersectPoly(pair, poly.pointsList) == true)
				resultList.Add (poly);

		return(resultList);
	}

	public bool SliceIntersectPoly(List <Vector2f> slice)
	{
		if (MathHelper.SliceIntersectPoly (slice, pointsList))
			return(true);
		
		foreach (Polygon poly in holesList)
			if (MathHelper.SliceIntersectPoly (slice, poly.pointsList))
				return(true);

		return(false);
	}
		
	public List<Polygon> SliceIntersectHoles(List <Vector2f> slice)
	{
		List<Polygon> resultList = new List<Polygon> ();
		foreach (Polygon poly in holesList)
			if (MathHelper.SliceIntersectPoly(slice, poly.pointsList) == true)
				resultList.Add (poly);

		return(resultList);
	}

	public List<Vector2f> GetListSliceIntersectPoly(Pair2f slice)
	{
		List<Vector2f> intersections = MathHelper.GetListPolyIntersectSlice(pointsList, slice);

		foreach (Polygon poly in holesList) 
			foreach (Vector2f p in MathHelper.GetListPolyIntersectSlice(poly.pointsList, slice))
				intersections.Add (p);
		
		return(intersections);
	}


	public static ColliderType GetColliderType(GameObject gameObject)
	{
		PolygonCollider2D polygonCollider2D = gameObject.GetComponent<PolygonCollider2D> ();
		if (polygonCollider2D != null) 
			return(ColliderType.Polygon);

		BoxCollider2D boxCollider2D = gameObject.GetComponent<BoxCollider2D> ();
		if (boxCollider2D != null) 
			return(ColliderType.Box);

		CircleCollider2D circleCollider2D = gameObject.GetComponent<CircleCollider2D> ();
		if (circleCollider2D != null) 
			return(ColliderType.Circle);

		CapsuleCollider2D capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D> ();
		if (capsuleCollider2D != null) 
			return(ColliderType.Capsule);

		return(ColliderType.None);
	}

	// Slower CreateFromCollider
	public static Polygon CreateFromCollider(GameObject gameObject)
	{
		ColliderType colliderType = GetColliderType(gameObject);
		switch (colliderType) {
		case ColliderType.Polygon:
			return (CreateFromPolygonCollider(gameObject.GetComponent<PolygonCollider2D> ()));
		case ColliderType.Box:
			return(CreateFromBoxCollider (gameObject.GetComponent<BoxCollider2D> ()));
		case ColliderType.Circle:
			return(CreateFromCircleCollider (gameObject.GetComponent<CircleCollider2D> ()));
		case ColliderType.Capsule:
			return(CreateFromCapsuleCollider (gameObject.GetComponent<CapsuleCollider2D> ()));
		default:
			break;
		}
		return(null);
	}

	// Faster CreateFromCollider
	public static Polygon CreateFromCollider(GameObject gameObject, ColliderType colliderType)
	{
		switch (colliderType) {
		case ColliderType.Polygon:
			return (CreateFromPolygonCollider(gameObject.GetComponent<PolygonCollider2D> ()));
		case ColliderType.Box:
			return(CreateFromBoxCollider (gameObject.GetComponent<BoxCollider2D> ()));
		case ColliderType.Circle:
			return(CreateFromCircleCollider (gameObject.GetComponent<CircleCollider2D> ()));
		case ColliderType.Capsule:
			return(CreateFromCapsuleCollider (gameObject.GetComponent<CapsuleCollider2D> ()));
		default:
			break;
		}
		return(null);
	}

	static private Polygon CreateFromPolygonCollider(PolygonCollider2D polygonCollider)
	{
		Polygon newPolygon = new Polygon ();
		if (polygonCollider != null) {
			foreach (Vector2 p in polygonCollider.GetPath (0))
				newPolygon.AddPoint (p + polygonCollider.offset);

			for (int i = 1; i < polygonCollider.pathCount; i++) {
				Polygon hole = new Polygon ();
				foreach (Vector2 p in polygonCollider.GetPath (i))
					hole.AddPoint (p + polygonCollider.offset);

				if (newPolygon.PolyInPoly (hole) == true)
					newPolygon.AddHole (hole);
				else
					Debug.LogError ("Path is not a hole");
			}
		}
		return(newPolygon);
	}

	static private Polygon CreateFromCircleCollider(CircleCollider2D circleCollider, int pointsCount = -1)
	{
		if (pointsCount < 1)
			pointsCount = defaultCircleVerticesCount;

		Polygon newPolygon = new Polygon ();

		float size = circleCollider.radius;
		float i = 0;

		while (i < 360) {
			newPolygon.AddPoint (new Vector2(Mathf.Cos (i * Mathf.Deg2Rad) * size, Mathf.Sin (i * Mathf.Deg2Rad) * size) + circleCollider.offset);
			i += 360f / (float)pointsCount;
		}

		return(newPolygon);
	}

	static private Polygon CreateFromBoxCollider(BoxCollider2D boxCollider)
	{
		Polygon newPolygon = new Polygon ();

		Vector2 size = new Vector2(boxCollider.size.x / 2, boxCollider.size.y / 2);

		newPolygon.AddPoint (new Vector2(-size.x, -size.y) + boxCollider.offset);
		newPolygon.AddPoint (new Vector2(-size.x, size.y) + boxCollider.offset);
		newPolygon.AddPoint (new Vector2(size.x, size.y) + boxCollider.offset);
		newPolygon.AddPoint (new Vector2(size.x, -size.y) + boxCollider.offset);

		return(newPolygon);
	}

	static private Polygon CreateFromCapsuleCollider(CapsuleCollider2D capsuleCollider, int pointsCount = -1)
	{
		if (pointsCount < 1)
			pointsCount = defaultCircleVerticesCount;

		Polygon newPolygon = new Polygon ();

		Vector2 size = new Vector2(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2);
		float offset = 0;
		float i = 0;

		switch (capsuleCollider.direction) {
		case CapsuleDirection2D.Vertical:
			float sizeXY = (capsuleCollider.transform.localScale.x / capsuleCollider.transform.localScale.y);
			size.x *= sizeXY;
			i = 0;

			if (capsuleCollider.size.x < capsuleCollider.size.y) 
				offset = (capsuleCollider.size.y - capsuleCollider.size.x) / 2;

			while (i < 180) {
				Vector2 v = new Vector2 (Mathf.Cos (i * Mathf.Deg2Rad) * size.x, offset + Mathf.Sin (i * Mathf.Deg2Rad) * size.x);
				newPolygon.AddPoint (v + capsuleCollider.offset);
				i += 360f / (float)pointsCount;
			}

			while (i < 360) {
				Vector2 v = new Vector2 (Mathf.Cos (i * Mathf.Deg2Rad) * size.x, -offset + Mathf.Sin (i * Mathf.Deg2Rad) * size.x);
				newPolygon.AddPoint (v + capsuleCollider.offset);
				i += 360f / (float)pointsCount;
			}
			break;

		case CapsuleDirection2D.Horizontal:
			float sizeYX = (capsuleCollider.transform.localScale.y / capsuleCollider.transform.localScale.x);
			size.x *= sizeYX; // not size.y?
			i = -90;

			if (capsuleCollider.size.y < capsuleCollider.size.x) 
				offset = (capsuleCollider.size.x - capsuleCollider.size.y) / 2;

			while (i < 90) {
				Vector2 v = new Vector2 ( offset + Mathf.Cos (i * Mathf.Deg2Rad) * size.y,Mathf.Sin (i * Mathf.Deg2Rad) * size.y);
				newPolygon.AddPoint (v + capsuleCollider.offset);
				i += 360f / (float)pointsCount;
			}

			while (i < 270) {
				Vector2 v = new Vector2 (-offset + Mathf.Cos (i * Mathf.Deg2Rad) * size.y, Mathf.Sin (i * Mathf.Deg2Rad) * size.y);
				newPolygon.AddPoint (v + capsuleCollider.offset);
				i += 360f / (float)pointsCount;
			}
			break;
		}

		return(newPolygon);
	}

	// Capsule Missing
	static public Polygon Create(PolygonType type, float size = 1f)
	{
		Polygon newPolygon = new Polygon ();

		switch (type) {

		case PolygonType.Pentagon:
			newPolygon.AddPoint (0f * size, 1f * size);
			newPolygon.AddPoint (-0.9510565f * size, 0.309017f * size);
			newPolygon.AddPoint (-0.5877852f * size, -0.8090171f * size);
			newPolygon.AddPoint (0.5877854f * size, -0.8090169f * size);
			newPolygon.AddPoint (0.9510565f * size, 0.3090171f * size);
			break;

		case PolygonType.Rectangle:
			newPolygon.AddPoint (-size, -size);
			newPolygon.AddPoint (size, -size);
			newPolygon.AddPoint (size, size);
			newPolygon.AddPoint (-size, size);
			break;

		case PolygonType.Circle:
			float i = 0;

			while (i < 360) {
				newPolygon.AddPoint (Mathf.Cos (i * Mathf.Deg2Rad) * size, Mathf.Sin (i * Mathf.Deg2Rad) * size);
				i += 360f / (float)defaultCircleVerticesCount;
			}
			break;

		case PolygonType.Hexagon:
			for (int s = 1; s < 360; s = s + 60)
				newPolygon.AddPoint (Mathf.Cos (s * Mathf.Deg2Rad) * size, Mathf.Sin (s * Mathf.Deg2Rad) * size);

			break;
		}

		return(newPolygon);
	}

	// Get List Of Polygons from Collider (Usually Used Before Creating Slicer2D Object)
	static public List<Polygon> GetListFromCollider(GameObject gameObject)
	{
		List<Polygon> result = new List<Polygon> ();
		PolygonCollider2D polygonCollider = gameObject.GetComponent<PolygonCollider2D> ();

		if (polygonCollider != null && polygonCollider.pathCount > 1) {
			Polygon newPolygon = new Polygon ();

			foreach (Vector2 p in polygonCollider.GetPath (0))
				newPolygon.AddPoint (p + polygonCollider.offset);

			result.Add (newPolygon.ToWorldSpace(gameObject.transform));

			for (int i = 1; i < polygonCollider.pathCount; i++) {
				Polygon hole = new Polygon ();
				foreach (Vector2 p in polygonCollider.GetPath (i))
					hole.AddPoint (p + polygonCollider.offset);

				if (newPolygon.PolyInPoly (hole) == false) 
					result.Add (hole.ToWorldSpace(gameObject.transform));
			}
		}
		return(result);
	}
}