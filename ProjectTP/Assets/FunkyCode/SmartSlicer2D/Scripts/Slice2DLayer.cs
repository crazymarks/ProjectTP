using System.Collections.Generic;
using UnityEngine;

public class Slice2DLayer {
	public enum Type {All, Selected};

	private Type layer = Slice2DLayer.Type.All;
	private bool[] layers = new bool[10];

	static public Slice2DLayer Create()
	{
		return(new Slice2DLayer());
	}

	public void SetLayerType(Type type)
	{
		layer = type;
	}

	public void SetLayer(int id, bool value)
	{
		layers [id] = value;
	}

	public void DisableLayers()
	{
		layers = new bool[10];
	}

	public Type GetLayerType()
	{
		return(layer);
	}

	public bool GetLayerState(int id)
	{
		return(layers [id]);
	}

}
