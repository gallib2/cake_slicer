using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Destruction2DComplexBrushControllerObject : Destruction2DControllerObject {
	public bool mouseDown = false;
	public Pair2D linearPair = Pair2D.Zero();
	public LinearCut linearCutLine = new LinearCut();
	public float cutSize = 0.25f;

	public void Update(Vector2D pos) {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		float newCutSize = cutSize + scroll;
		if (newCutSize > 0.05f) {
			cutSize = newCutSize;
		}

		if (Input.GetMouseButtonDown (0)) {
			linearPair.A = pos;
		}

		if (Input.GetMouseButton (0)) {
			mouseDown = true;
			Vector2D posMove = linearPair.A;

			linearPair.B = pos;

			if ((Vector2D.Distance (posMove, pos) > visuals.minVertexDistance * visuals.visualScale)) {
				linearCutLine = LinearCut.Create(linearPair, cutSize);

				Destruction2D.DestroyByLinearCutAll(linearCutLine, destructionLayer);

				linearPair.A = pos;
			}
		} else {
			mouseDown = false;
		}
	}
	
	public void Draw(Transform transform) {
		if (mouseDown) {
			linearCutLine = LinearCut.Create(linearPair, cutSize);
			visuals.GenerateLinearCutMesh(linearPair, cutSize, transform);

			visuals.Draw();
		}
	}
}
