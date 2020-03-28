using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Destruction2DLinearCutControllerObject : Destruction2DControllerObject {
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
			linearPair.A.Set (pos);
		}

		if (Input.GetMouseButton (0)) {
			linearPair.B.Set (pos);
			mouseDown = true;
		}

		if (mouseDown == true && Input.GetMouseButton (0) == false) {
			mouseDown = false;

			Destruction2D.DestroyByLinearCutAll(linearCutLine, destructionLayer);
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
