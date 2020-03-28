using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Destruction2DPolygonBrushControllerObject : Destruction2DControllerObject {
	public Polygon2D.PolygonType polygonType = Polygon2D.PolygonType.Circle;
	public int polygonEdgeCount = 15;
	public float polygonSize = 5f;
	Polygon2D slicePolygon = null;

	EraseBrush eraseBrush = new EraseBrush(null, null);
	
	public void Initialize() {
		Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
		slicePolygon = Polygon2D.Create (polygonType, polygonSize);

		eraseBrush.SetBrush(slicePolygon);
	}
	
	public void Update(Vector2D pos) {
		if (Input.GetMouseButton (0)) {
			eraseBrush.SetPosition(pos);

			Destruction2D.DestroyByPolygonAll(eraseBrush, destructionLayer);
		}
	}
	
	public void Draw(Transform transform, Vector2 pos) {
		eraseBrush.SetPosition(new Vector2D(pos));

		if (eraseBrush.GetWorldShape() != null) {
			visuals.GenerateComplexMesh(eraseBrush.GetWorldShape().pointsList, transform);

			visuals.Draw();
		}
	}
}