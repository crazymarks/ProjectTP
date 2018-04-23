using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public enum SliceType { Polygon };
    public enum SliceRotation { Random, Vertical, Horizontal }
    public enum CreateType { Slice, PolygonType }

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

    public static Test2 instance;
    private bool mouseDown = false;

    public void Awake()
    {
        instance = this;
    }


    //マウスの位置を獲得だけだ
    public void LateUpdate()
    {
        Vector2f pos = new Vector2f(0f,0f);
        UpdatePolygon(pos);
    }
    public static Vector2f GetMousePosition()
    {
        return (new Vector2f(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void UpdatePolygon(Vector2f pos)
    {
        mouseDown = true;

        if (Input.GetButtonDown("Trace"))
            PolygonSlice(pos);
    }


    private void PolygonSlice(Vector2f pos)
    {
        Polygon newPolygon = new Polygon();
        //この後点を追加する

        Polygon slicePolygonDestroy = null;
        if (polygonDestroy == true)
            slicePolygonDestroy = Polygon.Create(polygonType, polygonSize * 1.1f);

        Slicer2D.PolygonSliceAll(pos, Polygon.Create(polygonType, polygonSize), slicePolygonDestroy, sliceLayer);
    }
}