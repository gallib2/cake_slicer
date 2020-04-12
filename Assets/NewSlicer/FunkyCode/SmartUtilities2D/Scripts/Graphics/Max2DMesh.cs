using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mesh2DSubmesh {
	public Vector2[] uv;
	public Vector3[] vertices;

	public Mesh2DSubmesh(int size) {
		uv = new Vector2[size];
		vertices = new Vector3[size];
	}
}

public class Max2DMesh {
	const float pi = Mathf.PI;
	const float pi2 = pi / 2;
	const float uv0 = 1f / 32;
	const float uv1 = 1f - uv0;
	
	static Vector2D A1 = Vector2D.Zero();
	static Vector2D A2 = Vector2D.Zero();
	static Vector2D B1 = Vector2D.Zero();
	static Vector2D B2 = Vector2D.Zero();

	static Vector2D A3 = Vector2D.Zero();
	static Vector2D A4 = Vector2D.Zero();

	static public void Draw(Mesh mesh, Transform transform, Material material) {
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		Vector3 scale = transform.lossyScale;
		Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

		Graphics.DrawMesh(mesh, matrix, material, 0);
	}

	static public void Draw(Mesh mesh, Material material) {
		Vector3 position = Vector3.zero;
		Quaternion rotation = Quaternion.Euler(0, 0, 0);
		Vector3 scale = new Vector3(1, 1, 1);
		Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

		Graphics.DrawMesh(mesh, matrix, material, 0);
	}

	static public Mesh2DSubmesh CreateLine(Pair2D pair, float lineWidth, float z = 0f) {
		Mesh2DSubmesh result = new Mesh2DSubmesh(18);

		float xuv0 = 0;
		float xuv1 = 1f;

		float yuv0 = 0;
		float yuv1 = 1f;

		float size = lineWidth / 6;
		float rot = (float)Vector2D.Atan2 (pair.A, pair.B);

		A1.x = pair.A.x;
		A1.y = pair.A.y;

		A2.x = pair.A.x;
		A2.y = pair.A.y;

		B1.x = pair.B.x;
		B1.y = pair.B.y;

		B2.x = pair.B.x;
		B2.y = pair.B.y;

		A1.Push (rot + pi2, size);
		A2.Push (rot - pi2, size);
		B1.Push (rot + pi2, size);
		B2.Push (rot - pi2, size);

		result.vertices[0] = new Vector3((float)B1.x, (float)B1.y, z);
		result.vertices[1] = new Vector3((float)A1.x, (float)A1.y, z);
		result.vertices[2] = new Vector3((float)A2.x, (float)A2.y, z);
		
		result.vertices[3] = new Vector3((float)A2.x, (float)A2.y, z);
		result.vertices[4] = new Vector3((float)B2.x, (float)B2.y, z);
		result.vertices[5] = new Vector3((float)B1.x, (float)B1.y, z);

		result.uv[0] = new Vector2(xuv1 / 3, yuv1); 
		result.uv[1] = new Vector2(1 - xuv1 / 3, yuv1);
		result.uv[2] = new Vector2(1 - xuv1 / 3, yuv0);
		
		result.uv[3] = new Vector2(1 - xuv1 / 3, yuv0);
		result.uv[4] = new Vector2(yuv1 / 3, xuv0);
		result.uv[5] = new Vector2(xuv1 / 3, yuv1);

		A3.x = A1.x;
		A3.y = A1.y;

		A4.x = A1.x;
		A4.y = A1.y;
	
		A3.Push (rot - pi2, size);

		A3.x = A1.x;
		A3.y = A1.y;
		
		A4.x = A2.x;
		A4.y = A2.y;


		A1.Push (rot, size);
		A2.Push (rot, size);

		result.vertices[6] = new Vector3((float)A3.x, (float)A3.y, z);
		result.vertices[7] = new Vector3((float)A1.x, (float)A1.y, z);
		result.vertices[8] = new Vector3((float)A2.x, (float)A2.y, z);
		
		result.vertices[9] = new Vector3((float)A2.x, (float)A2.y, z);
		result.vertices[10] = new Vector3((float)A4.x, (float)A4.y, z);
		result.vertices[11] = new Vector3((float)A3.x, (float)A3.y, z);
		
		result.uv[6] = new Vector2(xuv1 / 3, yuv1); 
		result.uv[7] = new Vector2(xuv0, yuv1);
		result.uv[8] = new Vector2(xuv0, yuv0);

	 	result.uv[9] = new Vector2(xuv0, yuv0);
		result.uv[10] = new Vector2(yuv1 / 3, xuv0);
		result.uv[11] = new Vector2(xuv1 / 3, yuv1);

		A1.x = B1.x;
		A1.y = B1.y;

		A2.x = B2.x;
		A2.y = B2.y;

		B1.Push (rot - Mathf.PI, size);
		B2.Push (rot - Mathf.PI, size);
		
		result.vertices[12] = new Vector3((float)B1.x, (float)B1.y, z);
		result.vertices[13] = new Vector3((float)A1.x, (float)A1.y, z);
		result.vertices[14] = new Vector3((float)A2.x, (float)A2.y, z);

		result.vertices[15] = new Vector3((float)A2.x, (float)A2.y, z);
		result.vertices[16] = new Vector3((float)B2.x, (float)B2.y, z);
		result.vertices[17] = new Vector3((float)B1.x, (float)B1.y, z);

		result.uv[12] = new Vector2(xuv0, yuv1); 
		result.uv[13] = new Vector2(xuv1 / 3, yuv1);
		result.uv[14] = new Vector2(xuv1 / 3, yuv0);

		result.uv[15] = new Vector2(xuv1 / 3, yuv0);
		result.uv[16] = new Vector2(yuv0, xuv0);
		result.uv[17] = new Vector2(xuv0, yuv1);
		
		return(result);
	}
	static public Mesh2DSubmesh CreateLine(Pair2D pair, Transform transform, float lineWidth, float z = 0f) {
		Mesh2DSubmesh result = new Mesh2DSubmesh(18);

		float xuv0 = 0; //1f / 128;
		float xuv1 = 1f - xuv0;
		float yuv0 = 0; //1f / 192;
		float yuv1 = 1f - xuv0;

		float size = lineWidth / 6;
		float rot = (float)Vector2D.Atan2 (pair.A, pair.B);

		A1.x = pair.A.x;
		A1.y = pair.A.y;

		A2.x = pair.A.x;
		A2.y = pair.A.y;

		B1.x = pair.B.x;
		B1.y = pair.B.y;

		B2.x = pair.B.x;
		B2.y = pair.B.y;

		Vector2 scale = new Vector2(1f / transform.localScale.x, 1f / transform.localScale.y);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);
		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		result.vertices[0] = new Vector3((float)B1.x, (float)B1.y, z);
		result.vertices[1] = new Vector3((float)A1.x, (float)A1.y, z);
		result.vertices[2] = new Vector3((float)A2.x, (float)A2.y, z);
		
		result.vertices[3] = new Vector3((float)A2.x, (float)A2.y, z);
		result.vertices[4] = new Vector3((float)B2.x, (float)B2.y, z);
		result.vertices[5] = new Vector3((float)B1.x, (float)B1.y, z);

		result.uv[0] = new Vector2(xuv1 / 3, yuv1); 
		result.uv[1] = new Vector2(1 - xuv1 / 3, yuv1);
		result.uv[2] = new Vector2(1 - xuv1 / 3, yuv0);
		
		result.uv[3] = new Vector2(1 - xuv1 / 3, yuv0);
		result.uv[4] = new Vector2(yuv1 / 3, xuv0);
		result.uv[5] = new Vector2(xuv1 / 3, yuv1);

		A3.x = A1.x;
		A3.y = A1.y;

		A4.x = A1.x;
		A4.y = A1.y;
	
		A3.Push (rot - pi2, size, scale);

		A3.x = A1.x;
		A3.y = A1.y;
		
		A4.x = A2.x;
		A4.y = A2.y;

		A1.Push (rot, size, scale);
		A2.Push (rot, size, scale);

		result.vertices[6] = new Vector3((float)A3.x, (float)A3.y, z);
		result.vertices[7] = new Vector3((float)A1.x, (float)A1.y, z);
		result.vertices[8] = new Vector3((float)A2.x, (float)A2.y, z);
		
		result.vertices[9] = new Vector3((float)A2.x, (float)A2.y, z);
		result.vertices[10] = new Vector3((float)A4.x, (float)A4.y, z);
		result.vertices[11] = new Vector3((float)A3.x, (float)A3.y, z);
		
		result.uv[6] = new Vector2(xuv1 / 3, yuv1); 
		result.uv[7] = new Vector2(xuv0, yuv1);
		result.uv[8] = new Vector2(xuv0, yuv0);

	 	result.uv[9] = new Vector2(xuv0, yuv0);
		result.uv[10] = new Vector2(yuv1 / 3, xuv0);
		result.uv[11] = new Vector2(xuv1 / 3, yuv1);

		A1.x = B1.x;
		A1.y = B1.y;

		A2.x = B2.x;
		A2.y = B2.y;
		
		B1.Push (rot - Mathf.PI, size, scale);
		B2.Push (rot - Mathf.PI, size, scale);
		
		result.vertices[12] = new Vector3((float)B1.x, (float)B1.y, z);
		result.vertices[13] = new Vector3((float)A1.x, (float)A1.y, z);
		result.vertices[14] = new Vector3((float)A2.x, (float)A2.y, z);

		result.vertices[15] = new Vector3((float)A2.x, (float)A2.y, z);
		result.vertices[16] = new Vector3((float)B2.x, (float)B2.y, z);
		result.vertices[17] = new Vector3((float)B1.x, (float)B1.y, z);

		result.uv[12] = new Vector2(xuv0, yuv1); 
		result.uv[13] = new Vector2(xuv1 / 3, yuv1);
		result.uv[14] = new Vector2(xuv1 / 3, yuv0);

		result.uv[15] = new Vector2(xuv1 / 3, yuv0);
		result.uv[16] = new Vector2(yuv0, xuv0);
		result.uv[17] = new Vector2(xuv0, yuv1);
		
		return(result);
	}

	static public Mesh Export(List<Mesh2DSubmesh> trianglesList) {
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uv = new List<Vector2>();
		List<int> triangles = new List<int>();

		int count = 0;
		foreach(Mesh2DSubmesh triangle in trianglesList) {
			foreach(Vector3 v in triangle.vertices) {
				vertices.Add(v);
			}
			foreach(Vector2 u in triangle.uv) {
				uv.Add(u);
			}
			
			int iCount = triangle.vertices.Length;
			for(int i = 0; i < iCount; i++) {
				triangles.Add(count + i);
			}
			
			count += iCount;
		}

		mesh.vertices = vertices.ToArray();
		mesh.uv = uv.ToArray();
		mesh.triangles = triangles.ToArray();

		return(mesh);
	}

	static public Mesh GeneratePolygon2DMesh(Transform transform, Polygon2D polygon, float lineOffset, float lineWidth, bool connectedLine) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		Polygon2D poly = polygon;

		foreach(Pair2D p in Pair2D.GetList(poly.pointsList, connectedLine)) {
			trianglesList.Add(CreateLine(p, transform, lineWidth, lineOffset));
		}

		foreach(Polygon2D hole in poly.holesList) {
			foreach(Pair2D p in Pair2D.GetList(hole.pointsList, connectedLine)) {
				trianglesList.Add(CreateLine(p, transform, lineWidth, lineOffset));
			}
		}
		
		return(Export(trianglesList));
	}

	static public Mesh GeneratePolygon2DMeshNew(Transform transform, Polygon2D polygon, float lineOffset, float lineWidth, bool connectedLine) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		Polygon2D poly = polygon;

		foreach(Pair2D p in Pair2D.GetList(poly.pointsList, connectedLine)) {
			trianglesList.Add(CreateLine(p, transform, lineWidth, lineOffset));
		}

		foreach(Polygon2D hole in poly.holesList) {
			foreach(Pair2D p in Pair2D.GetList(hole.pointsList, connectedLine)) {
				trianglesList.Add(CreateLine(p, transform, lineWidth, lineOffset));
			}
		}
		
		return(Export(trianglesList));
	}

	public class Legacy {

		static public Mesh2DSubmesh CreateBox(float size) {
			Mesh2DSubmesh result = new Mesh2DSubmesh(6);

			result.vertices[0] = new Vector3(-size, -size, 0);
			result.vertices[1] = new Vector3(size, -size, 0);
			result.vertices[2] = new Vector3(size, size, 0);

			result.vertices[3] = new Vector3(size, size, 0);
			result.vertices[4] = new Vector3(-size, size, 0);
			result.vertices[5] = new Vector3(-size, -size, 0);
			
			result.uv[0] = new Vector2(uv0, uv0);
			result.uv[1] = new Vector2(uv1, uv0);
			result.uv[2] = new Vector2(uv1, uv1);

			result.uv[3] = new Vector2(uv1, uv1);
			result.uv[4] = new Vector2(uv1, uv0);
			result.uv[5] = new Vector2(uv0, uv0);
			
			return(result);
		}
	}
}
/* 
	
	}*/


/* 
	static public Mesh2DSubmesh CreateLine(Pair2D pair, Transform transform, float lineWidth, float z = 0f) {
		Mesh2DSubmesh result = new Mesh2DSubmesh();

		float size = lineWidth / 6;
		float rot = (float)Vector2D.Atan2 (pair.A, pair.B);

		Vector2D A1 = new Vector2D (pair.A);
		Vector2D A2 = new Vector2D (pair.A);
		Vector2D B1 = new Vector2D (pair.B);
		Vector2D B2 = new Vector2D (pair.B);

		Vector2 scale = new Vector2(1f / transform.localScale.x, 1f / transform.localScale.y);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);
		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		result.uv.Add(new Vector2(0.5f + uv0, 0));
		result.vertices.Add(new Vector3((float)B1.x, (float)B1.y, z));
		result.uv.Add(new Vector2(uv1, 0));
		result.vertices.Add(new Vector3((float)A1.x, (float)A1.y, z));
		result.uv.Add(new Vector2(uv1, 1));
		result.vertices.Add(new Vector3((float)A2.x, (float)A2.y, z));
		result.uv.Add(new Vector2(0.5f + uv0, 1));
		result.vertices.Add(new Vector3((float)B2.x, (float)B2.y, z));
	
		A1 = new Vector2D (pair.A);
		A2 = new Vector2D (pair.A);
		Vector2D A3 = new Vector2D (pair.A);
		Vector2D A4 = new Vector2D (pair.A);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);

		A3.Push (rot + pi2, size, scale);
		A4.Push (rot - pi2, size, scale);
		A3.Push (rot + pi, -size, scale);
		A4.Push (rot + pi, -size, scale);

		result.uv.Add(new Vector2(uv0, 0));
		result.vertices.Add(new Vector3((float)A3.x, (float)A3.y, z));
		result.uv.Add(new Vector2(uv0, 1));
		result.vertices.Add(new Vector3((float)A4.x, (float)A4.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 1));
		result.vertices.Add(new Vector3((float)A2.x, (float)A2.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 0));
		result.vertices.Add(new Vector3((float)A1.x, (float)A1.y, z));

		B1 = new Vector2D (pair.B);
		B2 = new Vector2D (pair.B);
		Vector2D B3 = new Vector2D (pair.B);
		Vector2D B4 = new Vector2D (pair.B);

		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		B3.Push (rot + pi2, size, scale);
		B4.Push (rot - pi2, size, scale);
		B3.Push (rot + pi, size, scale);
		B4.Push (rot + pi , size, scale);

		result.uv.Add(new Vector2(uv0, 0));
		result.vertices.Add(new Vector3((float)B4.x, (float)B4.y, z));
		result.uv.Add(new Vector2(uv0, 1));
		result.vertices.Add(new Vector3((float)B3.x, (float)B3.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 1));
		result.vertices.Add(new Vector3((float)B1.x, (float)B1.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 0));
		result.vertices.Add(new Vector3((float)B2.x, (float)B2.y, z));

		return(result);
	}
*/