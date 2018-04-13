using System.Collections.Generic;
using UnityEngine;

public class Slice2D {
	public List<Vector2f> collisions = new List<Vector2f>();
	public List<Polygon> polygons = new List<Polygon>();
	public List<GameObject> gameObjects = new List<GameObject>();

	public void AddCollision(Vector2f point)
	{
		collisions.Add (point);
	}

	// Private
	private void AddGameObject(GameObject gameObject)
	{
		gameObjects.Add (gameObject);
	}

	public void AddGameObjects(List<GameObject> gameObjects)
	{
		foreach (GameObject gameObject in gameObjects)
			AddGameObject (gameObject);
	}

	public void AddPolygon(Polygon polygon)
	{
		polygons.Add (polygon);
	}

	public void RemovePolygon(Polygon polygon)
	{
		polygons.Remove (polygon);
	}

	public static Slice2D Create()
	{
		return(new Slice2D ());
	}
}
