using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBuffer : BufferBase {

	void Initialize() {
		Transform transform = renderCamera.transform;
		
		DrawSelf ();

		DrawModifiers(transform);

		GenerateMeshes(transform);

		EraseBrushes(transform);

		SaveRenderTextureToSprite();
		
		destructible.initialized = true;
	}

	void Update() {
		if (destructible.initialized == false) {
			Initialize();
			return;
		}

		if (destructible.eraseEvents.Count > 0 || destructible.modifiersAdded == true) {
			Transform transform = renderCamera.transform;

			DrawSelf ();

			DrawModifiers(transform);

			GenerateMeshes(transform);

			EraseBrushes(transform);

			SaveRenderTextureToSprite();

			destructible.eraseEvents.Clear();
			destructible.modifiersAdded = false;
		}
	} 

	public void OnRenderObject() {
		Update();

		destructible.UpdateCollider();
	}

	public void DrawModifiers(Transform transform) {
		foreach(DestructionModifier modifier in destructible.modifiers) {
				Vector3 pos = (Vector3)modifier.position;

				float ratioX = (destructible.transform.localScale.x / destructible.transform.localScale.y);

				Vector2 size = modifier.size;

				pos.x *= transform.localScale.x;
				pos.y *= transform.localScale.y * ratioX;

				size.y *= ratioX;
			
				Vector2 scale = new Vector2(destructible.transform.localScale.x, destructible.transform.localScale.y);
				 
				Max2D.DrawImage(transform, modifier.material, pos, size, modifier.rotation, 0.3f, new Vector2D(scale));
			}
	}

	public void GenerateMeshes(Transform transform) {
		// New Event Meshes That Erase Generation
		foreach(DestructionEvent e in destructible.eraseEvents) {
			EraseMesh mesh = e.eraseBrush.GetMesh(transform, destructible.transform);
			destructible.EraseBrushes.Add(mesh);
		}
	}

	public void EraseBrushes(Transform transform) {
		// Erase Mesh
		GetEraseMaterial().SetPass(0);

		foreach(EraseMesh mesh in destructible.EraseBrushes) {
			float ratioX = destructible.transform.localScale.x / destructible.transform.localScale.y;
			Vector2 scale = destructible.transform.localScale;
			scale.y *= ratioX;

			Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f), Quaternion.Euler(0, 0, 0), new Vector3(scale.x, scale.y, 0));

			Graphics.DrawMeshNow(mesh.mesh, matrix);
		}
	}
}
