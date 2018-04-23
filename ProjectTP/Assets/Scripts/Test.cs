using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

// Controller
public class Test : MonoBehaviour{
    public enum SliceType {  Polygon };
    public enum SliceRotation { Random, Vertical, Horizontal }
    public enum CreateType { Slice, PolygonType }

    [Tooltip("Slice type represents algorithm complexity")]
    public SliceType sliceType = SliceType.Polygon;
    public Slice2DLayer sliceLayer = Slice2DLayer.Create();

    public Polygon slicePolygon = Polygon.Create(Polygon.PolygonType.Pentagon);

    // Polygon Destroyer type settings
    public Polygon.PolygonType polygonType = Polygon.PolygonType.Circle;
    public float polygonSize = 1;
    public bool polygonDestroy = false;

    // Polygon Creator
    public CreateType createType = CreateType.Slice;

    // Slicer Visuals
    public bool drawSlicer = true;
    public float lineWidth = 1.0f;
    public float zPosition = 0f;
    public Color slicerColor = Color.black;

    public static Test instance;
    private bool mouseDown = false;

    public void Awake()
    {
        instance = this;
    }

    public static Vector2f GetMousePosition()
    {
        return (new Vector2f(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    public void SetLayerType(int type)
    {
        if (type == 0)
            sliceLayer.SetLayerType((Slice2DLayer.Type)0);
        else
        {
            sliceLayer.SetLayerType((Slice2DLayer.Type)1);
            sliceLayer.DisableLayers();
            sliceLayer.SetLayer(type - 1, true);
        }
    }

    //枠を表示する
    public void OnRenderObject()
    {
        Vector2f pos = GetMousePosition();
        Max2D.SetBorder(true);
        Max2D.SetSmooth(true);
        Max2D.SetLineWidth(lineWidth * .5f);
        if (true)
        {
            Max2D.SetColor(slicerColor);     
                    slicePolygon = Polygon.Create(polygonType, polygonSize);       
        }
    }
    //マウスの位置を獲得だけだ
   public void LateUpdate()
    {
        Vector2f pos = GetMousePosition();
         UpdatePolygon(pos);        
    }

    private void UpdatePolygon(Vector2f pos)
    {
        mouseDown = true;

        if (Input.GetMouseButtonDown(0))
            PolygonSlice(pos);
    }


    private void PolygonSlice(Vector2f pos)
    {
        Polygon slicePolygonDestroy = null;
        if (polygonDestroy == true)
            slicePolygonDestroy = Polygon.Create(polygonType, polygonSize * 1.1f);

        Slicer2D.PolygonSliceAll(pos, Polygon.Create(polygonType, polygonSize), slicePolygonDestroy, sliceLayer);
    }


    private void PolygonCreator(Vector2f pos)
    {
        Polygon newPolygon = Polygon.Create(polygonType, polygonSize);
        newPolygon = newPolygon.ToOffset(pos);
        CreatePolygon(newPolygon);
    }

    private void CreatePolygon(Polygon newPolygon)
    {
        GameObject newGameObject = new GameObject();
        newGameObject.AddComponent<Rigidbody2D>();
        newGameObject.transform.parent = transform;
        newGameObject.AddComponent<ColliderLineRenderer2D>().color = Color.black;
        PolygonGenerator2D.GenerateCollider(newGameObject, newPolygon);

        Slicer2D smartSlicer = newGameObject.AddComponent<Slicer2D>();
        smartSlicer.textureType = Slicer2D.TextureType.Mesh;

        PolygonGenerator2D.GenerateMesh(newGameObject, newPolygon, new Vector2(1, 1));
    }
}