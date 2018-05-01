using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slicer2D : MonoBehaviour {
	public enum SlicingLayer {Layer1 = 0, Layer2 = 1, Layer3 = 2, Layer4 = 3, Layer5 = 4, Layer6 = 5, Layer7 = 6, Layer8 = 7}; 	// Add Default Layer?
	public enum SliceType {Regular, SliceHole, FillSlicedHole};
	public enum TextureType {Sprite, Mesh, None};

	public static SliceType complexSliceType = SliceType.Regular;
	public static int explosionPieces = 15; // In Use?

	[Tooltip("Type of texture to generate")]
	public TextureType textureType = TextureType.Sprite;

	public SlicingLayer slicingLayer = SlicingLayer.Layer1;

	private Polygon.ColliderType colliderType;

	public bool slicingLimit = false;
	public int sliceCounter = 0;
	public int maxSlices = 10;

	public Material material;

	// Event Handling
	public delegate bool Slice2DEvent(Slice2D slice);
	public delegate void Slice2DResultEvent(List<GameObject> gList); // Why It Doesn't Return Slice2D?

	private event Slice2DEvent sliceEvent;
	private event Slice2DResultEvent sliceResultEvent;

	public void AddEvent(Slice2DEvent e) { sliceEvent += e; }
	public void AddResultEvent(Slice2DResultEvent e) { sliceResultEvent += e; }

	static private List<Slicer2D> slicer2DList = new List<Slicer2D>();

	public Polygon.ColliderType GetColliderType()
	{
		return(colliderType);
	}
		
	public int GetLayerID() { return((int)slicingLayer); }

	// Update loop enables ".enabled" component field	
	void Update() {}
	void OnEnable() { slicer2DList.Add (this);}
	void OnDisable() { slicer2DList.Remove (this);}

	static public List<Slicer2D> GetList()
	{
		return(new List<Slicer2D>(slicer2DList));
	}

	void Start()
	{
		Initialize ();
	}

	// Check Before Each Function - Then This Could Be Private
	public void Initialize() {
		colliderType = Polygon.GetColliderType (gameObject);

		List<Polygon> result = Polygon.GetListFromCollider (gameObject);

		// Split collider if there are more polygons than 1
		if (result.Count > 1)
			PerformResult(result);

		switch (textureType) {
		case TextureType.Mesh:
			PolygonGenerator2D.GenerateMesh (gameObject, Polygon.CreateFromCollider(gameObject), new Vector2 (1, 1));
			MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
			meshRenderer.material = material;

			break;

		case TextureType.Sprite:
			if (GetComponent<SpriteRenderer> () != null) 
				gameObject.AddComponent<SpriteMesh2D> ();

			break;

		default:
			break;
		}
	}
		
	public Slice2D LinearSlice(Pair2f slice) {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) {
			Slice2D sliceResult = Slicer2D.LinearSlice (colliderPolygon, slice);
			sliceResult.AddGameObjects (PerformResult (sliceResult.polygons));
			
			return(sliceResult);
		}
			
		return(Slice2D.Create ());
	}
		
	public Slice2D ComplexSlice(List<Vector2f> slice) {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) {
			Slice2D sliceResult = Slicer2D.ComplexSlice (colliderPolygon, slice);
			sliceResult.AddGameObjects (PerformResult (sliceResult.polygons));

			return(sliceResult);
		}
		
		return(Slice2D.Create ());
	}

	public Slice2D PointSlice(Vector2f point, float rotation) {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) {
			Slice2D sliceResult = Slicer2D.PointSlice (colliderPolygon, point, rotation);
			sliceResult.AddGameObjects (PerformResult (sliceResult.polygons));
			
			return(sliceResult);
		}

		return(Slice2D.Create ());
	}

	public Slice2D PolygonSlice(Polygon slice, Polygon sliceDestroy, Polygon slicePolygonDestroy) {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) {
			Slice2D sliceResult = Slicer2D.PolygonSlice (colliderPolygon, slice);

			if (sliceResult.polygons.Count > 0) { //  || ComplexSlicer.success == true !!!!!!!!!!!!!!!!!!!
				if (slicePolygonDestroy != null)
					foreach (Polygon p in new List<Polygon>(sliceResult.polygons))
						if (sliceDestroy.PolyInPoly (p) == true)
							sliceResult.RemovePolygon (p);

				if (sliceResult.polygons.Count > 0) {
					// Check If Slice Result Is Correct
					sliceResult.AddGameObjects (PerformResult (sliceResult.polygons));

				} else if (slicePolygonDestroy != null)
					Destroy (gameObject);
	
				return(sliceResult);
			}
		}

		return(Slice2D.Create ());
	}

	public Slice2D ExplodingSlice(Vector2f point) {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) {
			Slice2D sliceResult = Slicer2D.ExplodingSlice (colliderPolygon, point);
			sliceResult.AddGameObjects (PerformResult (sliceResult.polygons));
			
			return(sliceResult);
		}

		return(Slice2D.Create ());
	}

	public Slice2D Explode() {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) {
			Slice2D sliceResult = Slicer2D.Explode (colliderPolygon);
			sliceResult.AddGameObjects (PerformResult (sliceResult.polygons));
			
			return(sliceResult);
		}

		return(Slice2D.Create ());
	}

	// Does not return GameObjects
	public Slice2D PolygonSlice2(Polygon slice) {
		Polygon colliderPolygon = GetPolygonToSlice ();
		if (colliderPolygon != null) 
			return( Slicer2D.PolygonSlice (colliderPolygon, slice));

		return(Slice2D.Create ());
	}

	public List<GameObject> PerformResult(List<Polygon> result)
	{
		List<GameObject> resultGameObjects = new List<GameObject> ();

		if (result.Count < 1)
			return(resultGameObjects);

		if (sliceEvent != null) {
			Slice2D slice = Slice2D.Create ();
			slice.polygons = result;
			if (sliceEvent (slice) == false)
				return(resultGameObjects);
		}
				
		Destroy (gameObject);

		int name_id = 1;
		//float originMass = Polygon.CreateFromCollider (gameObject).GetArea();

		foreach (Polygon id in result) {
			GameObject gObject = Instantiate (gameObject);

			resultGameObjects.Add (gObject);

			Component[] components = GetComponents<Component> ();
			foreach (Component c in components) 
				if (c.GetType () == typeof(Rigidbody2D)) {
					Rigidbody2D originalRigidBody = (Rigidbody2D)c;
					Rigidbody2D newRigidBody = gObject.GetComponent<Rigidbody2D> ();
					newRigidBody.velocity = originalRigidBody.velocity;
					newRigidBody.angularVelocity = originalRigidBody.angularVelocity;

					//newRigidBody.mass = originalRigidBody.mass * (id.ToLocalSpace(g.transform).GetArea () / originMass);
				}

			foreach (Behaviour childCompnent in gObject.GetComponentsInChildren<Behaviour>()) // Wrong
				childCompnent.enabled = true;

			Slicer2D slicer = gObject.GetComponent<Slicer2D> ();
			slicer.sliceCounter = sliceCounter + 1;
			slicer.maxSlices = maxSlices;
            //オブジェクトの名前を変更
			gObject.name = name + " (" + name_id + ")";
			gObject.transform.parent = transform.parent;
			gObject.transform.position = transform.position;
			gObject.transform.rotation = transform.rotation;
            gObject.AddComponent<Pauser>().Pause();
            GameObject.Find("ShotLens").SendMessage("ItemListAdd",gObject);  
		
			switch (textureType) {
				case TextureType.Sprite:
					if (gameObject.GetComponent<SpriteRenderer> () != null && gObject.GetComponent<SpriteMesh2D> () == null)
						gObject.AddComponent<SpriteMesh2D> ();

					break;

				case TextureType.Mesh:
				case TextureType.None:
				default:
					break;
			}
		
			switch (colliderType){
				case Polygon.ColliderType.Box:
					Destroy (gObject.GetComponent<BoxCollider2D> ());
					break;
				case Polygon.ColliderType.Circle:
					Destroy(gObject.GetComponent<CircleCollider2D>());
					break;
				case Polygon.ColliderType.Capsule:
					Destroy(gObject.GetComponent<CapsuleCollider2D>());
					break;
				default:
					break;
			}

			PolygonGenerator2D.GenerateCollider (gObject, id.ToLocalSpace (gObject.transform));

			name_id += 1;
		}

		if ((resultGameObjects.Count > 0) && (sliceResultEvent != null))
			sliceResultEvent (resultGameObjects);

		return(resultGameObjects);
	}

	public bool MatchLayers(Slice2DLayer sliceLayer)
	{
		return((sliceLayer == null || sliceLayer.GetLayerType() == Slice2DLayer.Type.All) || sliceLayer.GetLayerState(GetLayerID ()));
	}
		
	static public Slice2D LinearSlice(Polygon polygon, Pair2f slice) { return(LinearSlicer.Slice (polygon, slice)); }
	static public Slice2D PointSlice(Polygon polygon, Vector2f point, float rotation) { return(LinearSlicerExtended.SliceFromPoint (polygon, point, rotation)); }
	static public Slice2D ComplexSlice(Polygon polygon, List<Vector2f> slice) { return(ComplexSlicer.Slice (polygon, slice)); }
	static public Slice2D PolygonSlice(Polygon polygon, Polygon polygonB) { return(ComplexSlicerExtended.Slice (polygon, polygonB)); }
	static public Slice2D ExplodingSlice(Polygon polygon, Vector2f point) { return(LinearSlicerExtended.PointExplode (polygon, point)); }
	static public Slice2D Explode(Polygon polygon) { return(LinearSlicerExtended.Explode (polygon)); }
	static public Polygon CreatorSlice(List<Vector2f> slice) { return(ComplexSlicerExtended.CreateSlice (slice)); }

	static public List<Slice2D> LinearSliceAll(Pair2f slice, Slice2DLayer layer)
	{
		List<Slice2D> result = new List<Slice2D> ();
		foreach (Slicer2D id in GetList ())
		if (id.MatchLayers (layer)) 
		{
				Slice2D sliceResult = id.LinearSlice (slice);
				if (sliceResult.gameObjects.Count > 0) 
					result.Add (sliceResult);
		}

		return(result);
	}

	static public List<Slice2D> PointSliceAll(Vector2f slice, float rotation, Slice2DLayer layer)
	{
		List<Slice2D> result = new List<Slice2D> ();
		foreach (Slicer2D id in GetList())
			if (id.MatchLayers (layer)) 
			{
				Slice2D sliceResult = id.PointSlice (slice, rotation);
				if (sliceResult.gameObjects.Count > 0) 
					result.Add (sliceResult);
			}

		return(result);
	}

	static public List<Slice2D> ComplexSliceAll(List<Vector2f> slice, Slice2DLayer layer)
	{
		List<Slice2D> result = new List<Slice2D> ();
		foreach (Slicer2D id in GetList())
			if (id.MatchLayers (layer)) 
			{
				Slice2D sliceResult = id.ComplexSlice (slice);
				if (sliceResult.gameObjects.Count > 0) 
					result.Add (sliceResult);
			}
				
		return(result);
	}

	// Shouldn't Have Position
	static public List<Slice2D> PolygonSliceAll(Vector2f position, Polygon slicePolygon, Polygon slicePolygonDestroy, Slice2DLayer layer)
	{
		Polygon sliceDestroy = null;
		Polygon slice = slicePolygon.ToOffset (position);

		if (slicePolygonDestroy != null) 
			sliceDestroy = slicePolygonDestroy.ToOffset (position);

		List<Slice2D> result = new List<Slice2D> ();
		foreach (Slicer2D id in GetList())
			if (id.MatchLayers (layer)) 
				result.Add (id.PolygonSlice (slice, slicePolygon, sliceDestroy));
		
		return(result);
	}
	
	static public List<Slice2D> ExplodingSliceAll(Vector2f point, Slice2DLayer layer)
	{
		List<Slice2D> result = new List<Slice2D> ();
		foreach (Slicer2D id in GetList())
			if (id.MatchLayers (layer)) 
			{
				Slice2D sliceResult = id.ExplodingSlice (point);
				if (sliceResult.gameObjects.Count > 0)
					result.Add (sliceResult);
			}

		return(result);
	}

	static public List<Slice2D> ExplodeAll(Slice2DLayer layer)
	{
		List<Slice2D> result = new List<Slice2D> ();
		foreach (Slicer2D id in GetList())
			if (id.MatchLayers (layer)) 
			{
				Slice2D sliceResult = id.Explode ();
				if (sliceResult.gameObjects.Count > 0)
					result.Add (sliceResult);
			}

		return(result);
	}
		
	private Polygon GetPolygonToSlice()
	{
		if (sliceCounter >= maxSlices && slicingLimit)
			return(null);

		return(Polygon.CreateFromCollider (gameObject, colliderType).ToWorldSpace (gameObject.transform));
	}
}
