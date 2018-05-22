using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Representation of 2D points
/// </summary>
public class Vector2f {
	private Vector2 vector;

	/// <summary>
	/// Representation of 2D points
	/// </summary>
	public Vector2f(float x, float y)
	{
		vector = new Vector2 (x, y);
	}

	/// <summary>
	/// Representation of 2D points
	/// </summary>
	public Vector2f(Vector2f point)
	{
		vector = new Vector2(point.GetX(), point.GetY());
	}
		
	/// <summary>
	/// Representation of 2D points
	/// </summary>
	public Vector2f(Vector2 point)
	{
		vector = new Vector2(point.x, point.y);
	}
		
	/// <summary>
	/// Returns 2D Vector
	/// </summary>
	public Vector2 Get()
	{
		return(vector);
	}

	/// <summary>
	/// Returns x component of 2D vector object
	/// </summary>
	public float GetX()
	{
		return(vector.x);
	}

	/// <summary>
	/// Returns y component of 2D vector object
	/// </summary>
	public float GetY()
	{
		return(vector.y);
	}
		
	/// <summary>
	/// Set x and y components of an existing 2D vector
	/// </summary>
	public void Set(float x, float y)
	{
		vector.Set (x, y);
	}

	/// <summary>
	/// Set x and y components of an existing 2D vector
	/// </summary>
	public void Set (Vector2f point)
	{
		vector.Set (point.GetX(), point.GetY());
	}

	/// <summary>
	/// Push 2D vector coordinates by given rotation and distance 
	/// </summary>
	public void Push(float rot, float distance)
	{
		Inc(Mathf.Cos(rot) * distance, Mathf.Sin(rot) * distance);
	}

	/// <summary>
	/// Increase 2D vector coordinates by given x and y coordinates
	/// </summary>
	public void Inc (float x, float y)
	{
		Inc (new Vector2f (x, y));
	}

	/// <summary>
	/// Decrease 2D vector coordinates by given x and y coordinates
	/// </summary>
	public void Dec (float x, float y)
	{
		Dec (new Vector2f (x, y));
	}

	/// <summary>
	/// Increase 2D vector coordinates by given 2D vector
	/// </summary>
	public void Inc (Vector2f point)
	{
		vector += point.vector;
	}

	/// <summary>
	/// Decrease 2D vector coordinates by given 2D vector
	/// </summary>
	public void Dec (Vector2f point)
	{
		vector -= point.vector;
	}

	/// <summary>
	/// Distance between given 2D vectors
	/// </summary>
	public static float Distance(Vector2f a, Vector2f b)
	{
		return(Vector2.Distance(a.Get(), b.Get()));
	}

	/// <summary>
	/// Angle between two given 2D coordinates
	/// </summary>
	public static float Atan2(Vector2f a, Vector2f b)
	{
		return(Mathf.Atan2 (a.GetY() - b.GetY(), a.GetX() - b.GetX()));
	}
}
