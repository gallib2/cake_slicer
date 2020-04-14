using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseBrush {
	public Polygon2D shape_local;
	public Polygon2D shape_world;

	public Vector2D offset_local = null;
	
	public EraseMesh mesh = null;

	public EraseBrush(Polygon2D polygonLocal = null, Polygon2D polygonWorld = null, Vector2D offset = null) {
		shape_local = polygonLocal;
		shape_world = polygonWorld;
	}

	public void SetBrush(Polygon2D poly) {
		shape_local = poly;
	}

	public void SetPosition(Vector2D pos) {
		offset_local = pos;

		shape_world = null;
	}

	public Polygon2D GetWorldShape() {
		if (shape_world == null) {
			shape_world = shape_local.Copy();
			shape_world.ToOffsetItself(offset_local);
		}
		return(shape_world);
	}

	public EraseMesh GetMesh(Transform transformA, Transform transformB) {
		Polygon2D polygon = GetWorldShape().ToLocalSpace(transformA);

		polygon.ToOffsetItself(new Vector2D(transformA.position - transformB.position));
		polygon.ToRotationItself(transformB.rotation.eulerAngles.z * -Mathf.Deg2Rad);
		polygon.ToScaleItself(new Vector2(1.0f / transformB.localScale.x, 1.0f / transformB.localScale.y));

		Mesh mesh = PolygonTriangulator2D.Triangulate(polygon, Vector2.zero, Vector2.zero, PolygonTriangulator2D.Triangulation.Advanced);

		EraseMesh eraseMesh = new EraseMesh();
		eraseMesh.mesh = mesh;
		return(eraseMesh);
	}
}

public class EraseMesh {
	public Mesh mesh;

	public Vector2 position;
	public float rotation;
	public Vector2 scale;
}