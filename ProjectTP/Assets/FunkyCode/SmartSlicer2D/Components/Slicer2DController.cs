using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

// Controller
public class Slicer2DController : MonoBehaviour {
	public enum SliceType {Linear, Complex, Point, Polygon, Explode, Create};
	public enum SliceRotation {Random, Vertical, Horizontal}
	public enum CreateType {Slice, PolygonType}

	public bool addForce = true;
	public float addForceAmount = 5f;

	[Tooltip("Slice type represents algorithm complexity")]
	public SliceType sliceType = SliceType.Complex;
	public Slice2DLayer sliceLayer = Slice2DLayer.Create();

	public Polygon slicePolygon = Polygon.Create (Polygon.PolygonType.Pentagon);

	[Tooltip("Minimum distance between points (SliceType: Complex")]
	private float minVertsDistance = 1f;

	// Polygon Destroyer type settings
	public Polygon.PolygonType polygonType = Polygon.PolygonType.Circle;
	public float polygonSize = 1;
	public bool polygonDestroy = false;

	// Polygon Creator
	public Material material;
	public CreateType createType = CreateType.Slice;

	// Complex Slicer
	public Slicer2D.SliceType complexSliceType = Slicer2D.SliceType.SliceHole;

	// Slicer Visuals
	public bool drawSlicer = true;
	public float lineWidth = 1.0f;
	public float zPosition = 0f;
	public Color slicerColor = Color.black;

	// Point Slicer
	public SliceRotation sliceRotation = SliceRotation.Random;

	// Events Handler
	private static List<List<Vector2f>> complexEvents = new List<List<Vector2f>>();
	private static List<Pair2f> linearEvents = new List<Pair2f>();

	// Events Input Handler
	private static List<Vector2f> complexPairs = new List<Vector2f>();
	private static Pair2f linearPair = Pair2f.Zero();

	public static Slicer2DController instance;
	private bool mouseDown = false;

	public static Color[] slicerColors = {Color.black, Color.green, Color.yellow , Color.red, new Color(1f, 0.25f, 0.125f)};

	public void Awake()
	{
		instance = this;
	}

	public static Vector2f GetMousePosition()
	{
		//Vector3 pos = Input.mousePosition; //pos.z = Camera.main.transform.position.z;
		return(new Vector2f (Camera.main.ScreenToWorldPoint (Input.mousePosition)));
	}

	public void SetSliceType(int type)
	{
		sliceType = (SliceType)type;
	}

	public void SetLayerType(int type)
	{
		if (type == 0) 
			sliceLayer.SetLayerType((Slice2DLayer.Type)0);
		else {
			sliceLayer.SetLayerType((Slice2DLayer.Type)1);
			sliceLayer.DisableLayers ();
			sliceLayer.SetLayer (type - 1, true);
		}
	}

	public void SetSlicerColor(int colorInt)
	{
		slicerColor = slicerColors [colorInt];
	}

	public void OnRenderObject() {
		Vector2f pos = GetMousePosition ();

		if (drawSlicer == false)
			return;

		Max2D.SetBorder (true);
		Max2D.SetSmooth(true);
		Max2D.SetLineWidth (lineWidth * .5f);

		if (mouseDown) {
			Max2D.SetColor (slicerColor);

			switch (sliceType) {
				case SliceType.Complex:
					if (complexPairs.Count > 0) {
						Max2D.DrawStrippedLine (complexPairs, minVertsDistance, zPosition);
						Max2D.DrawLineSquare (complexPairs.Last(), 0.5f, zPosition);
						Max2D.DrawLineSquare (complexPairs.First (), 0.5f, zPosition);
					}
					break;

				case SliceType.Create:
					if (createType == CreateType.Slice) {
						if (complexPairs.Count > 0) {
							Max2D.DrawStrippedLine (complexPairs, minVertsDistance, zPosition, true);
							Max2D.DrawLineSquare (complexPairs.Last(), 0.5f, zPosition);
							Max2D.DrawLineSquare (complexPairs.First (), 0.5f, zPosition);
						}
					} else {
						Max2D.DrawStrippedLine (Polygon.Create(polygonType, polygonSize).pointsList, minVertsDistance, zPosition, true, pos);
					}
					break;
				
				case SliceType.Linear:
					Max2D.DrawLine (linearPair.A, linearPair.B, zPosition);
					Max2D.DrawLineSquare (linearPair.A, 0.5f, zPosition);
					Max2D.DrawLineSquare (linearPair.B, 0.5f, zPosition);
					break;

				case SliceType.Point:
					break;

				case SliceType.Explode:
					break;

				case SliceType.Polygon:
					slicePolygon = Polygon.Create (polygonType, polygonSize);
					Max2D.DrawStrippedLine (slicePolygon.pointsList, minVertsDistance, zPosition, false, pos);
					break;
				
				default:
					break; 
			}
		}
	}

	public void LateUpdate()
	{
		Vector2f pos = GetMousePosition ();

		complexEvents.Clear ();
		linearEvents.Clear ();

		switch (sliceType) {	
			case SliceType.Linear:
				UpdateLinear (pos);
				break;

			case SliceType.Complex:
				UpdateComplex (pos);
				break;

			case SliceType.Point:
				UpdatePoint (pos);
				break;

			case SliceType.Explode:			
				UpdateExplode (pos);
				break;

			case SliceType.Create:
				UpdateCreate (pos);
				break;

			case SliceType.Polygon:
				UpdatePolygon (pos);
				break;

			default:
				break; 
		}
	}
		
	private void UpdateLinear(Vector2f pos)
	{
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

	private void UpdateComplex(Vector2f pos)
	{
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

	private void UpdatePoint(Vector2f pos)
	{
		if (Input.GetMouseButtonDown (0)) 
			PointSlice(pos);
	}

	private void UpdatePolygon(Vector2f pos)
	{
		mouseDown = true;

		if (Input.GetMouseButtonDown (0))
			PolygonSlice (pos);
	}

	private void UpdateExplode(Vector2f pos)
	{
		if (Input.GetMouseButtonDown (0))
			ExplodingSlice(pos);
	}

	private void UpdateCreate(Vector2f pos)
	{
		if (Input.GetMouseButtonDown (0)) {
			complexPairs.Clear ();
			complexPairs.Add (pos);
		}

		if (createType == CreateType.Slice) {
			if (Input.GetMouseButton (0)) {
				if (complexPairs.Count == 0 || (Vector2f.Distance (pos, complexPairs.Last ()) > minVertsDistance))
					complexPairs.Add (pos);

				mouseDown = true;
			}

			if (mouseDown == true && Input.GetMouseButton (0) == false) {
				mouseDown = false;
				CreatorSlice (complexPairs);
			}
		} else {
			mouseDown = true;
			if (Input.GetMouseButtonDown (0))
				PolygonCreator (pos);
		}
	}

	private void LinearSlice(Pair2f slice)
	{
		List<Slice2D> results = Slicer2D.LinearSliceAll (slice, sliceLayer);

		if (addForce == true) {
			float sliceRotation = Vector2f.Atan2 (slice.B, slice.A);

			foreach (Slice2D id in results) {
				foreach (GameObject gameObject in id.gameObjects) {
					Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
					if (rigidBody2D)
						foreach (Vector2f p in id.collisions) {
							Vector2 force = new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount, Mathf.Sin (sliceRotation) * addForceAmount);
							rigidBody2D.AddForceAtPosition (force, p.Get ());
						}
				}
			}
		}
	}

	private void ComplexSlice(List <Vector2f> slice)
	{
		List<Slice2D> results = Slicer2D.ComplexSliceAll (slice, sliceLayer);
		if (addForce == true)
			foreach (Slice2D id in results)
				foreach (GameObject gameObject in id.gameObjects) {
					Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
					if (rigidBody2D) {
						List<Pair2f> list = Pair2f.GetList (id.collisions);
						float forceVal = 1.0f / list.Count;
						foreach (Pair2f p in list) {
							float sliceRotation = -Vector2f.Atan2 (p.B, p.A);
							Vector2 force = new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount, Mathf.Sin (sliceRotation) * addForceAmount);
							rigidBody2D.AddForceAtPosition (forceVal * force, (p.A.Get () + p.B.Get ()) / 2f);
						}
					}
				}
	}

	private void PointSlice(Vector2f pos)
	{
		float rotation = 0;

		switch (sliceRotation) {	
			case SliceRotation.Random:
				rotation = UnityEngine.Random.Range (0, Mathf.PI * 2);
				break;

			case SliceRotation.Vertical:
				rotation = Mathf.PI / 2f;
				break;

			case SliceRotation.Horizontal:
				rotation = Mathf.PI;
				break;
		}

		Slicer2D.PointSliceAll (pos, rotation, sliceLayer);
	}
		
	private void PolygonSlice(Vector2f pos)
	{
		Polygon slicePolygonDestroy = null;
		if (polygonDestroy == true)
			slicePolygonDestroy = Polygon.Create (polygonType, polygonSize * 1.1f);

		Slicer2D.PolygonSliceAll(pos, Polygon.Create (polygonType, polygonSize), slicePolygonDestroy, sliceLayer);
	}

	private void ExplodingSlice(Vector2f pos)
	{
		List<Slice2D> results =	Slicer2D.ExplodingSliceAll (pos, sliceLayer);
		if (addForce == true)
			foreach (Slice2D id in results)
				foreach (GameObject gameObject in id.gameObjects) {
					Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
					if (rigidBody2D) {
						float sliceRotation = Vector2f.Atan2 (pos, new Vector2f (gameObject.transform.position));
						Rect rect = Polygon.CreateFromCollider (gameObject).GetBounds ();
						rigidBody2D.AddForceAtPosition (new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount / 10f, Mathf.Sin (sliceRotation) * addForceAmount/ 10f), rect.center);
					}
				}
	}

	private void ExplodeAll()
	{
		List<Slice2D> results =	Slicer2D.ExplodeAll (sliceLayer);
		if (addForce == true)
			foreach (Slice2D id in results)
				foreach (GameObject gameObject in id.gameObjects) {
					Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
					if (rigidBody2D) {
						float sliceRotation = Vector2f.Atan2 (new Vector2f(0, 0), new Vector2f (gameObject.transform.position));
						Rect rect = Polygon.CreateFromCollider (gameObject).GetBounds ();
						rigidBody2D.AddForceAtPosition (new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount / 10f, Mathf.Sin (sliceRotation) * addForceAmount/ 10f), rect.center);
					}
				}
	}

	private void CreatorSlice(List <Vector2f> slice)
	{
		Polygon newPolygon = Slicer2D.CreatorSlice (slice);
		if (newPolygon != null) 
			CreatePolygon (newPolygon);
	}

	private void PolygonCreator(Vector2f pos)
	{
		Polygon newPolygon = Polygon.Create (polygonType, polygonSize);
		newPolygon = newPolygon.ToOffset (pos);
		CreatePolygon (newPolygon);
	}

	private void CreatePolygon(Polygon newPolygon)
	{
		GameObject newGameObject = new GameObject ();
		newGameObject.AddComponent<Rigidbody2D> ();
		newGameObject.transform.parent = transform;
		newGameObject.AddComponent<ColliderLineRenderer2D> ().color = Color.black;
		PolygonGenerator2D.GenerateCollider (newGameObject, newPolygon);

		Slicer2D smartSlicer = newGameObject.AddComponent<Slicer2D> ();
		smartSlicer.textureType = Slicer2D.TextureType.Mesh;
		smartSlicer.material = material;

		PolygonGenerator2D.GenerateMesh (newGameObject, newPolygon, new Vector2 (1, 1));
	}

	static public List<List<Vector2f>> GetComplexEvents(){ return(complexEvents); }
	static public List<Pair2f> GetLinearEvents() { return(linearEvents); }
}