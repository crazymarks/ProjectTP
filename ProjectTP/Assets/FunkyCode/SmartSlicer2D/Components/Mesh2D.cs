using UnityEngine;

public class Mesh2D : MonoBehaviour {
	// Optionable material
	public Material material;

	public string sortingLayerName; 
	public int sortingLayerID;
	public int sortingOrder;

	void Start () {
		// Generate Mesh from collider
		PolygonGenerator2D.GenerateMesh(gameObject, Polygon.CreateFromCollider (gameObject), Vector2.zero);

		// Setting Mesh material
		if (material != null) {
			MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
			meshRenderer.material = material;
		
			meshRenderer.sortingLayerName = sortingLayerName;
			meshRenderer.sortingLayerID = sortingLayerID;
			meshRenderer.sortingOrder = sortingOrder;
		}
	}
}
