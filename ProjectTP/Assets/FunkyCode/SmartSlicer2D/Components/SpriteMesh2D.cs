using System.Collections.Generic;
using UnityEngine;

public class SpriteMesh2D : MonoBehaviour {
	// Should Be Invisible
	public Texture2D texture;
	public Color color;
	public Vector2 scale;

	private string sortingLayerName;
	private int sortingLayerID;
	private int sortingOrder;

	private bool added = false;

	void Update () {
		if (added == false) {
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
			if (spriteRenderer == null) {
				if (texture != null) {
					added = true;

					PolygonGenerator2D.GenerateMesh (gameObject, Polygon.CreateFromCollider (gameObject), scale);

					MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
					if (meshRenderer == null)
						meshRenderer = gameObject.AddComponent<MeshRenderer> ();
					
					meshRenderer.material = new Material (Shader.Find ("Sprites/Default"));
					meshRenderer.material.mainTexture = texture;
					meshRenderer.material.color = color;

					meshRenderer.sortingLayerName = sortingLayerName;
					meshRenderer.sortingLayerID = sortingLayerID;
					meshRenderer.sortingOrder = sortingOrder;
				}
			} else {
				texture = spriteRenderer.sprite.texture;
				color = spriteRenderer.color;
				scale = new Vector2(spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit, spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit);

				sortingLayerID = spriteRenderer.sortingLayerID;
				sortingLayerName = spriteRenderer.sortingLayerName;
				sortingOrder = spriteRenderer.sortingOrder;

				Destroy (spriteRenderer);
			}
		}
	}

}
