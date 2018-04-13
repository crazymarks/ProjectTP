using System.Collections.Generic;
using UnityEngine;

public class Demo6BombUpdate : MonoBehaviour {
	private float timer = 0;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > 5.0)
			Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.name.Contains ("Terrain")) {
			Vector2f pos = new Vector2f (transform.position);

			Polygon.defaultCircleVerticesCount = 15;;
			Polygon slicePolygon = Polygon.Create (Polygon.PolygonType.Circle, 2f);

			Polygon slicePolygonDestroy = null;
			Polygon sliceDestroy = null;

			slicePolygonDestroy = Polygon.Create (Polygon.PolygonType.Circle, 2.5f);
			sliceDestroy = new Polygon ();

			foreach (Vector2f id in slicePolygonDestroy.pointsList) 
				sliceDestroy.AddPoint (new Vector2f (id.Get () + pos.Get ()));

			Polygon slice = new Polygon ();
			foreach (Vector2f id in slicePolygon.pointsList) 
				slice.AddPoint (new Vector2f (id.Get () + pos.Get ()));

			foreach (Slicer2D id in Slicer2D.GetList()) {
				Slice2D result = id.PolygonSlice2 (slice); // Why not use Slice All?
				if (result.polygons.Count > 0) {
					foreach (Polygon p in new List<Polygon>(result.polygons))
						if (sliceDestroy.PolyInPoly (p) == true)
							result.polygons.Remove (p);

					if (result.polygons.Count > 0)
						id.PerformResult (result.polygons);
					else
						Destroy (id.gameObject);
				}
			}
			Destroy (gameObject);

			Polygon.defaultCircleVerticesCount = 25;
		}
	}
}
