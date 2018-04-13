using System.Collections.Generic;
using UnityEngine;

public class Max2D {
	private static Material lineMaterial;
	private static Material lineEndMaterial;
	private static Material defaultMaterial; 

	private static float lineWidth = 0.2f;
	private static bool smooth = false;
	private static bool setBorder = false;
	private static Color setColor = Color.white;

	static public void SetBorder(bool border)
	{
		setBorder = border;
	}

	static public void SetSmooth(bool _smooth)
	{
		smooth = _smooth;
	}

	public static void SetLineWidth (float size)
	{
		lineWidth = Mathf.Max(.01f, size / 5f);
	}

	private static void Check()
	{
		if (lineMaterial == null || lineEndMaterial == null) {
			lineMaterial = new Material (Shader.Find ("Legacy Shaders/Transparent/VertexLit"));
			lineMaterial.mainTexture = Resources.Load ("Textures/LineTexture") as Texture;
			lineEndMaterial = new Material (Shader.Find ("Legacy Shaders/Transparent/VertexLit"));
			lineEndMaterial.mainTexture = Resources.Load ("Textures/LineEndTexture") as Texture;
			lineEndMaterial.mainTextureScale = new Vector2 (0.5f, 1f);
			lineEndMaterial.mainTextureOffset = new Vector2 (-0.5f, 0f);
			lineMaterial.SetInt("_ZWrite", 0);
			//lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			lineEndMaterial.SetInt("_ZWrite", 0);
		}
		if (defaultMaterial == null) {
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			defaultMaterial = new Material(shader);
			defaultMaterial.hideFlags = HideFlags.HideAndDontSave;
			defaultMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			defaultMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			defaultMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			defaultMaterial.SetInt("_ZWrite", 0);
		}
	}

	static public void SetColor(Color color)
	{
		Check ();
		lineMaterial.SetColor ("_Emission", color);
		lineEndMaterial.SetColor ("_Emission", color);
		setColor = color;
	}

	static public void DrawMesh(Mesh mesh, Transform transform, Vector2f offset, float z = 0f)
	{
		if (mesh == null)
			return;
		
		GL.PushMatrix ();
		defaultMaterial.SetPass (0); 
		GL.Begin(GL.TRIANGLES);

		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i <  mesh.triangles.GetLength (0); i++ ) {
			list.Add (transform.TransformPoint(mesh.vertices [mesh.triangles [i]]));
			if (list.Count > 2) {
				DrawTriangle_Matrix (list [0].x, list [0].y, list [1].x, list [1].y, list [2].x, list [2].y, offset, z);
				list.Clear ();
			}
		}

		GL.End ();
		GL.PopMatrix ();
	}

	static public void DrawTriangle(Vector2f p0, Vector2f p1, Vector2f p2, Vector2f offset, float z = 0f)
	{
		DrawTrianglef (p0.GetX (), p0.GetY (), p1.GetX (), p1.GetY (), p2.GetX (), p2.GetY (), offset, z);
	}

	static public void DrawSquare(Vector2f p, float size, float z = 0f)
	{
		Vector2f p0 = new Vector2f (p.GetX () - size, p.GetY () - size);
		Vector2f p1 = new Vector2f (p.GetX () + size, p.GetY () - size);
		Vector2f p2 = new Vector2f (p.GetX () + size, p.GetY () + size);
		Vector2f p3 = new Vector2f (p.GetX () - size, p.GetY () + size);

		DrawTriangle (p0, p1, p2, new Vector2f(0, 0), z);
		DrawTriangle (p2, p3, p0, new Vector2f(0, 0), z);
	}

	static public void DrawImage(Material material, Vector2f pos, Vector2f size, float z = 0f)
	{
		GL.PushMatrix ();
		material.SetPass (0); 
		GL.Begin (GL.QUADS);

		GL.TexCoord2 (0, 0);
		GL.Vertex3 (pos.GetX() - size.GetX(), pos.GetY() - size.GetY(), z);
		GL.TexCoord2 (0, 1);
		GL.Vertex3 (pos.GetX() - size.GetX(), pos.GetY() + size.GetY(), z);
		GL.TexCoord2 (1, 1);
		GL.Vertex3 (pos.GetX() + size.GetX(), pos.GetY() + size.GetY(), z);
		GL.TexCoord2 (1, 0);
		GL.Vertex3 (pos.GetX() + size.GetX(), pos.GetY() - size.GetY(), z);

		GL.End ();
		GL.PopMatrix ();
	}

	static public void DrawLineSquare(Vector2f p, float size, float z = 0f)
	{
		DrawLineRectf (p.GetX() - size / 2f, p.GetY() - size / 2f, size, size, z);
	}

	static public void DrawLine(Vector2f p0, Vector2f p1, float z = 0f)
	{
		if (setBorder == true) {
			Color tmcColor = setColor;
			float tmpWidth = lineWidth;
			SetColor(Color.black);
			lineWidth = tmpWidth * 2f;
			DrawLinef (p0.GetX (), p0.GetY (), p1.GetX (), p1.GetY (), z);
			SetColor(tmcColor);
			lineWidth = tmpWidth;
			DrawLinef (p0.GetX (), p0.GetY (), p1.GetX (), p1.GetY (), z);
			lineWidth = tmpWidth;
		} else {
			DrawLinef(p0.GetX (), p0.GetY (), p1.GetX (), p1.GetY (), z);
		}
	}
		
	static public void DrawLinef(float x0, float y0, float x1, float y1, float z = 0f)
	{
		Check ();

		if (smooth == true)
			DrawSmoothLine (new Pair2f (new Vector2f (x0, y0), new Vector2f (x1, y1)), z);
		else {
			GL.PushMatrix();
			defaultMaterial.SetPass(0);
			GL.Begin(GL.LINES);
			GL.Color(setColor);

			DrawLine_Matrix (x0, y0, x1, y1, z);

			GL.End();
			GL.PopMatrix();
		}
	}
		
	static public void DrawTrianglef(float x0, float y0, float x1, float y1, float x2, float y2, Vector2f offset, float z = 0f)
	{
		GL.PushMatrix();
		defaultMaterial.SetPass(0);
		GL.Begin(GL.TRIANGLES);
		GL.Color(setColor);

		DrawTriangle_Matrix (x0, y0, x1, y1, x2, y2, offset, z);

		GL.End();
		GL.PopMatrix();
	}

	static public void DrawLineRectf(float x, float y, float w, float h, float z = 0f)
	{
		if (smooth) {
			if (setBorder == true) {
				Color tmcColor = setColor;
				float tmpWidth = lineWidth;

				SetColor (Color.black);
				lineWidth = tmpWidth * 2f;

				GL.PushMatrix ();
				lineMaterial.SetPass (0);
				GL.Begin (GL.QUADS);

				DrawLine_Smooth_Matrix (new Pair2f (new Vector2f (x, y), new Vector2f (x + w, y)), z);
				DrawLine_Smooth_Matrix (new Pair2f (new Vector2f (x, y), new Vector2f (x, y + h)), z);
				DrawLine_Smooth_Matrix (new Pair2f (new Vector2f (x + w, y), new Vector2f (x + w, y + h)), z);
				DrawLine_Smooth_Matrix (new Pair2f (new Vector2f (x, y + h), new Vector2f (x + w, y + h)), z);

				GL.End ();
				GL.PopMatrix ();

				GL.PushMatrix ();
				lineEndMaterial.SetPass (0);
				GL.Begin (GL.QUADS);

				DrawLineEnd_Smooth_Matrix (new Pair2f (new Vector2f (x, y), new Vector2f (x + w, y)), z);
				DrawLineEnd_Smooth_Matrix (new Pair2f (new Vector2f (x, y), new Vector2f (x, y + h)), z);
				DrawLineEnd_Smooth_Matrix (new Pair2f (new Vector2f (x + w, y), new Vector2f (x + w, y + h)), z);
				DrawLineEnd_Smooth_Matrix (new Pair2f (new Vector2f (x, y + h), new Vector2f (x + w, y + h)), z);

				GL.End ();
				GL.PopMatrix ();

				SetColor (tmcColor);
				lineWidth = tmpWidth;
			}

			float tmpLine = lineWidth;
			lineWidth = tmpLine * 1f;

			GL.PushMatrix();
			SetColor (setColor);
			lineMaterial.SetPass(0);
			GL.Begin(GL.QUADS);

			DrawLine_Smooth_Matrix (new Pair2f(new Vector2f(x, y), new Vector2f(x + w, y)), z);
			DrawLine_Smooth_Matrix (new Pair2f(new Vector2f(x, y), new Vector2f(x, y + h)), z);
			DrawLine_Smooth_Matrix (new Pair2f(new Vector2f(x + w, y), new Vector2f(x + w, y+ h)), z);
			DrawLine_Smooth_Matrix (new Pair2f(new Vector2f(x, y + h), new Vector2f(x + w, y+ h)), z);

			GL.End();
			GL.PopMatrix();

			lineWidth = tmpLine;

		} else {
			DrawLine (new Vector2f (x, y), new Vector2f (x + w, y), z);
			DrawLine (new Vector2f (x + w, y), new Vector2f (x + w, y + h), z);
			DrawLine (new Vector2f (x + w, y + h),	new Vector2f (x, y + h), z);
			DrawLine (new Vector2f (x, y + h), new Vector2f (x, y), z);
		}
	}

	static public void DrawSlice(List< Vector2f> slice, float z = 0f)
	{
		foreach (Pair2f p in Pair2f.GetList(slice, false)) 
			DrawLine (p.A, p.B, z);
	}

	static public void DrawPolygonList(List<Polygon> polyList, float z = 0f)
	{
		foreach (Polygon p in polyList)
			DrawPolygon (p, z);
	}

	static public void DrawStrippedLine(List<Vector2f> pointsList, float minVertsDistance, float z = 0f, bool full = false, Vector2f offset = null)
	{
		if (offset == null)
			offset = new Vector2f (0, 0);

		Vector2f vA = null, vB = null;

		if (setBorder == true) {
			Color tmcColor = setColor;
			float tmpWidth = lineWidth;

			GL.PushMatrix();
			SetColor (Color.black);
			lineMaterial.SetPass(0);
			GL.Begin(GL.QUADS);

			lineWidth = 2f * tmpWidth;

			foreach (Pair2f id in Pair2f.GetList(pointsList, full)) {
				vA = new Vector2f (id.A.Get () + offset.Get());
				vB = new Vector2f (id.B.Get () + offset.Get());

				vA.Push (Vector2f.Atan2 (id.A, id.B), -minVertsDistance / 4);
				vB.Push (Vector2f.Atan2 (id.A, id.B), minVertsDistance / 4);

				DrawLine_Smooth_Matrix (new Pair2f(vA, vB), z);
			}

			GL.End();
			GL.PopMatrix();

			GL.PushMatrix();
			SetColor (Color.black);
			lineEndMaterial.SetPass(0);
			GL.Begin(GL.QUADS);

			lineWidth = 2f * tmpWidth;

			foreach (Pair2f id in Pair2f.GetList(pointsList, full)) {
				vA = new Vector2f (id.A.Get () + offset.Get());
				vB = new Vector2f (id.B.Get () + offset.Get());

				vA.Push (Vector2f.Atan2 (id.A, id.B), -minVertsDistance / 4);
				vB.Push (Vector2f.Atan2 (id.A, id.B), minVertsDistance / 4);

				DrawLineEnd_Smooth_Matrix (new Pair2f(vA, vB), z);
			}

			GL.End();
			GL.PopMatrix();

			SetColor (tmcColor);
			lineWidth = tmpWidth;
		}

		GL.PushMatrix();
		lineMaterial.SetPass(0);
		GL.Begin(GL.QUADS);

		foreach (Pair2f id in Pair2f.GetList(pointsList, full)) {
			vA = new Vector2f (id.A.Get () + offset.Get());
			vB = new Vector2f (id.B.Get () + offset.Get());

			vA.Push (Vector2f.Atan2 (id.A, id.B), -minVertsDistance / 4);
			vB.Push (Vector2f.Atan2 (id.A, id.B), minVertsDistance / 4);

			DrawLine_Smooth_Matrix (new Pair2f(vA, vB), z);
		}

		GL.End();
		GL.PopMatrix();
	}

	static public void DrawSmoothLine(Pair2f pair, float z = 0f)
	{
		GL.PushMatrix();
		lineMaterial.SetPass(0);
		GL.Begin(GL.QUADS);

		DrawLine_Smooth_Matrix (pair, z);

		GL.End();
		GL.PopMatrix();
	}

	static public void DrawPolygon(Polygon poly, float z = 0f)
	{
		Check ();

		if (smooth) {
			GL.PushMatrix ();
			lineMaterial.SetPass(0);
			GL.Begin(GL.QUADS);

			DrawSlice_Smooth_Matrix (poly.pointsList, z);

			GL.End();
			GL.PopMatrix();

			GL.PushMatrix ();
			lineEndMaterial.SetPass(0);
			GL.Begin(GL.QUADS);

			foreach (Pair2f p in Pair2f.GetList(poly.pointsList))
				DrawLineEnd_Smooth_Matrix (p, z);
			
			GL.End();
			GL.PopMatrix();

		} else {
			GL.PushMatrix();
			defaultMaterial.SetPass(0);
			GL.Begin(GL.LINES);
			GL.Color(setColor);

			DrawSlice_Matrix (poly.pointsList, z);

			GL.End ();
			GL.PopMatrix();
		}

		foreach (Polygon p in poly.holesList)
			DrawPolygon (p, z);
	}

	private static void DrawSlice_Smooth_Matrix(List<Vector2f> list, float z)
	{
		foreach (Pair2f p in Pair2f.GetList(list))
			DrawLine_Smooth_Matrix (p, z);
		
	}

	private static void DrawSlice_Matrix(List<Vector2f> list, float z)
	{
		foreach (Pair2f p in Pair2f.GetList(list)) {
			GL.Vertex3(p.A.GetX(), p.A.GetY(), z);
			GL.Vertex3(p.B.GetX(), p.B.GetY(), z);
		}
	}

	private static void DrawTriangle_Matrix(float x0, float y0, float x1, float y1, float x2, float y2, Vector2f offset, float z = 0f)
	{
		GL.Vertex3(x0 + offset.GetX(), y0 + offset.GetY(), z);
		GL.Vertex3(x1 + offset.GetX(), y1 + offset.GetY(), z);
		GL.Vertex3(x2 + offset.GetX(), y2 + offset.GetY(), z);
	}

	private static void DrawLine_Matrix(float x0, float y0, float x1, float y1, float z = 0f)
	{
		GL.Vertex3(x0, y0, z);
		GL.Vertex3(x1, y1, z);
	}

	private static void DrawLine_Smooth_Matrix(Pair2f pair, float z = 0f)
	{
		float size = lineWidth;
		float pi2 = Mathf.PI / 2;

		float rot = Vector2f.Atan2 (pair.A, pair.B);

		Vector2f A1 = new Vector2f (pair.A);
		Vector2f A2 = new Vector2f (pair.A);
		Vector2f B1 = new Vector2f (pair.B);
		Vector2f B2 = new Vector2f (pair.B);

		A1.Push (rot + pi2, size);
		A2.Push (rot - pi2, size);
		B1.Push (rot + pi2, size);
		B2.Push (rot - pi2, size);

		GL.TexCoord2(0, 0);
		GL.Vertex3(B1.GetX(), B1.GetY(), z);
		GL.TexCoord2(1, 0);
		GL.Vertex3(A1.GetX(), A1.GetY(), z);
		GL.TexCoord2(1, 1);
		GL.Vertex3(A2.GetX(), A2.GetY(), z);
		GL.TexCoord2(0, 1);
		GL.Vertex3(B2.GetX(), B2.GetY(), z);
	}

	private static void DrawLineEnd_Smooth_Matrix(Pair2f pair, float z = 0f)
	{
		float size = lineWidth;
		float pi2 = Mathf.PI / 2;
		float pi = Mathf.PI;

		float rot = Vector2f.Atan2 (pair.A, pair.B);

		Vector2f A1 = new Vector2f (pair.A);
		Vector2f A2 = new Vector2f (pair.A);
		Vector2f A3 = new Vector2f (pair.A);
		Vector2f A4 = new Vector2f (pair.A);

		Vector2f B1 = new Vector2f (pair.B);
		Vector2f B2 = new Vector2f (pair.B);
		Vector2f B3 = new Vector2f (pair.B);
		Vector2f B4 = new Vector2f (pair.B);

		A1.Push (rot + pi2, size);
		A2.Push (rot - pi2, size);

		A3.Push (rot + pi2, size);
		A4.Push (rot - pi2, size);
		A3.Push (rot + pi, -size);
		A4.Push (rot + pi, -size);

		B1.Push (rot + pi2, size);
		B2.Push (rot - pi2, size);

		B3.Push (rot + pi2, size);
		B4.Push (rot - pi2, size);
		B3.Push (rot + pi, size);
		B4.Push (rot + pi , size);

		GL.TexCoord2(0, 0);
		GL.Vertex3(A3.GetX(), A3.GetY(), z);
		GL.TexCoord2(0, 1);
		GL.Vertex3(A4.GetX(), A4.GetY(), z);
		GL.TexCoord2(1, 1);
		GL.Vertex3(A2.GetX(), A2.GetY(), z);
		GL.TexCoord2(1, 0);
		GL.Vertex3(A1.GetX(), A1.GetY(), z);
		GL.TexCoord2(0, 0);
		GL.Vertex3(B4.GetX(), B4.GetY(), z);
		GL.TexCoord2(0, 1);
		GL.Vertex3(B3.GetX(), B3.GetY(), z);
		GL.TexCoord2(1, 1);
		GL.Vertex3(B1.GetX(), B1.GetY(), z);
		GL.TexCoord2(1, 0);
		GL.Vertex3(B2.GetX(), B2.GetY(), z);
	}
}