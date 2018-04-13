using System.Collections.Generic;
using UnityEngine;

public class Slicer2DLinearController : MonoBehaviour {
	// Physics Force
	public bool addForce = true;
	public float addForceAmount = 5f;

	// Controller Visuals
	public bool drawSlicer = true;
	public float lineWidth = 1.0f;
	public float zPosition = 0f;
	public Color slicerColor = Color.black;

	// Mouse Events
	private Pair2f linearPair = Pair2f.Zero();
	private List<Pair2f> linearEvents = new List<Pair2f> ();

	private bool mouseDown = false;

	public void OnRenderObject() {
		if (drawSlicer == false)
			return;
		
		if (mouseDown) {
			Max2D.SetBorder (true);
			Max2D.SetSmooth(true);

			Max2D.SetLineWidth (lineWidth * .5f);
			Max2D.SetColor (slicerColor);

			Max2D.DrawLineSquare (linearPair.A, 0.5f, zPosition);
			Max2D.DrawLineSquare (linearPair.B, 0.5f, zPosition);
			Max2D.DrawLine (linearPair.A, linearPair.B, zPosition);
		}
	}

	public void LateUpdate()
	{
		linearEvents.Clear ();

		// Checking mouse press and release events to get linear slices based on input
		Vector2f pos = new Vector2f (Camera.main.ScreenToWorldPoint (Input.mousePosition));

		if (Input.GetMouseButtonDown (0)) 
			linearPair.A.Set (pos);
		
		if (Input.GetMouseButton (0)) {
			linearPair.B.Set (pos);
			mouseDown = true;
		}

		if (mouseDown == true && Input.GetMouseButton (0) == false) {
			mouseDown = false;
			LinearSlice (linearPair);
			linearEvents.Add (linearPair);
		}
	}

	private void LinearSlice(Pair2f slice)
	{
		List<Slice2D> results = Slicer2D.LinearSliceAll (slice, null);

		// Adding Physics Forces
		if (addForce == true) {
			float sliceRotation = Vector2f.Atan2 (slice.B, slice.A);
			foreach (Slice2D id in results)
				foreach (GameObject gameObject in id.gameObjects) {
					Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
					if (rigidBody2D)
						foreach (Vector2f p in id.collisions)
							rigidBody2D.AddForceAtPosition (new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount, Mathf.Sin (sliceRotation) * addForceAmount), p.Get ());
				}
		}
	}
}