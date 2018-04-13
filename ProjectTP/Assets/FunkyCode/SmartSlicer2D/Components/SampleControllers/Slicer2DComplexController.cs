using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Slicer2DComplexController : MonoBehaviour {
	// Physics Force
	public bool addForce = true;
	public float addForceAmount = 5f;

	// Controller Visuals
	public bool drawSlicer = true;
	public float lineWidth = 1.0f;
	public float zPosition = 0f;
	public Color slicerColor = Color.black;

	// Mouse Events
	private static List<List<Vector2f>> complexEvents = new List<List<Vector2f>>();
	private static List<Vector2f> complexPairs = new List<Vector2f>();
	private float minVertsDistance = 1f;

	private bool mouseDown = false;

	// Complex Slice Type
	public Slicer2D.SliceType complexSliceType = Slicer2D.SliceType.SliceHole;

	public void OnRenderObject() {
		if (drawSlicer == false)
			return;

		if (mouseDown) {
			Max2D.SetBorder(true);
			Max2D.SetSmooth(true);

			Max2D.SetLineWidth (lineWidth * .5f);
			Max2D.SetColor (slicerColor);

			if (complexPairs.Count > 0) {
				Max2D.DrawStrippedLine (complexPairs, minVertsDistance, zPosition);
				Max2D.DrawLineSquare (complexPairs.Last(), 0.5f, zPosition);
				Max2D.DrawLineSquare (complexPairs.First (), 0.5f, zPosition);
			}
		}
	}

	public void LateUpdate()
	{
		complexEvents.Clear ();

		// Checking mouse press and release events to get linear slices based on input
		Vector2f pos = new Vector2f (Camera.main.ScreenToWorldPoint (Input.mousePosition));

		if (Input.GetMouseButtonDown (0)) {
			complexPairs.Clear ();
			complexPairs.Add (pos);
		}

		if (Input.GetMouseButton (0)) {
			if (complexPairs.Count == 0 || (Vector2f.Distance (pos, complexPairs.Last ()) > minVertsDistance)) 
				complexPairs.Add (pos);

			mouseDown = true;
		}

		if (mouseDown == true && Input.GetMouseButton (0) == false) {
			mouseDown = false;
			Slicer2D.complexSliceType = complexSliceType;
			ComplexSlice (complexPairs);
			complexEvents.Add (complexPairs);
		}
	}

	private void ComplexSlice(List <Vector2f> slice)
	{
		List<Slice2D> results = Slicer2D.ComplexSliceAll (slice, null);
		if (addForce == true)
			foreach (Slice2D id in results)
				foreach (GameObject gameObject in id.gameObjects) {
					Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
					if (rigidBody2D)
						foreach (Pair2f p in Pair2f.GetList(id.collisions)) {
							float sliceRotation = Vector2f.Atan2 (p.B, p.A);
							rigidBody2D.AddForceAtPosition (new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount, Mathf.Sin (sliceRotation) * addForceAmount), (p.A.Get () + p.B.Get ()) / 2f);
						}
				}
	}
}