using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCutController : MonoBehaviour {
    public Destruction2DVisuals visuals = new Destruction2DVisuals();

    EraseBrush eraseBrushStart = new EraseBrush(null, null);
    EraseBrush eraseBrushMiddle = new EraseBrush(null, null);

    private Vector2D oldPosition = null;

    public float size = 1;

    public int circleVerticesCount = 15;

    public void Initialize() {
		Polygon2D.defaultCircleVerticesCount = circleVerticesCount;
		Polygon2D circlePolygon = Polygon2D.Create (Polygon2D.PolygonType.Octagon, size);

		eraseBrushStart.SetBrush(circlePolygon.Copy());
	}

    void Start() {
        visuals.Initialize();

        visuals.SetGameObject(gameObject);

        Initialize();
    }

    void Update() {
        Vector2 pos = GetMousePosition();

        if (Input.GetMouseButtonDown(0)) {
            oldPosition = new Vector2D(pos);
        }

        if (Input.GetMouseButton (0)) {
			eraseBrushStart.SetPosition(new Vector2D(pos));

			Destruction2D.DestroyByPolygonAll(eraseBrushStart);

            if (oldPosition != null) {
                if (UpdateMiddleEraseMesh()) {
                    Destruction2D.DestroyByPolygonAll(eraseBrushMiddle);

                    oldPosition = new Vector2D(pos);
                }
            }
		} else {
            oldPosition = new Vector2D(pos);
        }

        Draw(transform, pos);
    }

    public void Draw(Transform transform, Vector2 pos) {
        eraseBrushStart.SetPosition(new Vector2D(pos));

		if (eraseBrushStart.GetWorldShape() != null) {
            visuals.Clear();
        
			visuals.GenerateComplexMesh(eraseBrushStart.GetWorldShape().pointsList, transform);

            if (oldPosition != null) {
                if (UpdateMiddleEraseMesh()) {
                    visuals.GenerateComplexMesh(eraseBrushMiddle.GetWorldShape().pointsList, transform);
                }

            } else {
                oldPosition = new Vector2D(pos);
            }   
                    
			visuals.Draw();
		}
	}

    bool UpdateMiddleEraseMesh() {
        if (oldPosition == null) {
            return(false);
        }

        Vector2D pos = new Vector2D(GetMousePosition());
        Vector2D oldPos = new Vector2D(oldPosition);

        if (Vector2D.Distance(pos, oldPos) < 1) {
            return(false);
        }

        Polygon2D middlePolygon = new Polygon2D();

        double rotation = Vector2D.Atan2(pos, oldPos) - Mathf.PI / 2;

        Vector2D push = Vector2D.Zero();
        push.Push(rotation, size);

        middlePolygon.AddPoint(pos + push);
        middlePolygon.AddPoint(pos - push);
        middlePolygon.AddPoint(oldPos - push);
        middlePolygon.AddPoint(oldPos + push);

        eraseBrushMiddle = new EraseBrush(null, middlePolygon);

        return(true);
    }

    public static Vector2 GetMousePosition() {
		return(Camera.main.ScreenToWorldPoint (Input.mousePosition));
	}
}
