using System.Collections.Generic;
using UnityEngine;

public class ColliderLineRenderer2D : MonoBehaviour {
	public Color color = Color.white;
	public float lineWidth = 1;
	public bool smooth = true;

	private float lineOffset = -0.001f;
	private Polygon poly = new Polygon ();

	public void Start()
	{
		poly = Polygon.CreateFromCollider (gameObject);
	}

	public void OnRenderObject()
	{
		Max2D.SetLineWidth (lineWidth);
		Max2D.SetColor (color);
		Max2D.SetSmooth (smooth);
		Max2D.SetBorder (false);
		Max2D.DrawPolygon (poly.ToWorldSpace (transform), transform.position.z + lineOffset);
	}
}
