using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Destruction2DComplexCutControllerObject : Destruction2DControllerObject {
	public bool mouseDown = false;
	public static ComplexCut complexCutLine = new ComplexCut();
	public List<Vector2D> complexSlicerPointsList = new List<Vector2D>();
	public float cutSize = 0.25f;

	public void Update(Vector2D pos) {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		float newCutSize = cutSize + scroll;
		if (newCutSize > 0.05f) {
			cutSize = newCutSize;
		}
				
		if (Input.GetMouseButtonDown (0)) {
			complexSlicerPointsList.Clear ();

			complexSlicerPointsList.Add (pos);
			mouseDown = true;
		}

		if (complexSlicerPointsList.Count < 1) {
			return;
		}
		
		if (Input.GetMouseButton (0)) {
			Vector2D posMove = new Vector2D (complexSlicerPointsList.Last ());

			while ((Vector2D.Distance (posMove, pos) > visuals.minVertexDistance * visuals.visualScale)) {
				float direction = (float)Vector2D.Atan2 (pos, posMove);
				posMove.Push (direction, visuals.minVertexDistance * visuals.visualScale);

				complexSlicerPointsList.Add (new Vector2D (posMove));
			}
		}

		if (mouseDown == true && Input.GetMouseButton (0) == false) {
			mouseDown = false;

			Destruction2D.DestroyByComplexCutAll(complexCutLine, destructionLayer);

			complexSlicerPointsList.Clear ();
		}
	}
	
	public void Draw(Transform transform) {
		if (mouseDown) {
			complexCutLine = ComplexCut.Create(complexSlicerPointsList, cutSize);
			visuals.GenerateComplexCutMesh(complexSlicerPointsList, cutSize, transform);

			visuals.Draw();
		}
	}
}
