using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Destruction2DPolygonControllerObject : Destruction2DControllerObject {
	public bool mouseDown = false;
	public Polygon2D.PolygonType polygonType = Polygon2D.PolygonType.Circle;
	Polygon2D slicePolygon = null;
	public float polygonSize = 5f;
	public int polygonEdgeCount = 15;

	public bool Update(Vector2D pos) {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		float newPolygonSize = polygonSize + scroll;
		if (newPolygonSize > 0.05f) {
			polygonSize = newPolygonSize;
		}

		mouseDown = true;

		if (Input.GetMouseButtonDown (0)) {
			Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
			slicePolygon = Polygon2D.Create (polygonType, polygonSize);

			Polygon2D polygon = new Polygon2D();
			polygon.pointsList = new List<Vector2D>(slicePolygon.pointsList);
			polygon.ToOffsetItself(pos);

			EraseBrush EraseBrush = new EraseBrush(null, polygon);
		
			Destruction2D.DestroyByPolygonAll(EraseBrush, destructionLayer);

			return(true);
		} else {
			return(false);
		}
	}
	
	public void Draw(Transform transform, Vector2 pos) {
		Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
		slicePolygon = Polygon2D.Create (polygonType, polygonSize);

		slicePolygon.ToOffsetItself(new Vector2D(pos));

		visuals.GenerateComplexMesh(slicePolygon.pointsList, transform);

		visuals.Draw();
	}
}
