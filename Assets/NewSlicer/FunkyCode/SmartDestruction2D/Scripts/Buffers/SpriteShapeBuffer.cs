using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShapeBuffer : BufferBase {

	void Initialize() {
		Transform transform = renderCamera.transform;
		
		//DrawSelf

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

		foreach(Polygon2D p in destructible.erasePolygons) {
			Vector2 scale = new Vector2(1, 1);
		
			Polygon2D polygon = p.ToScale(scale);

			Mesh mesh = polygon.CreateMesh(Vector2.zero, Vector2.zero);

			EraseMesh eraseMesh = new EraseMesh();

			eraseMesh.mesh = mesh;

			destructible.EraseBrushes.Add(eraseMesh);
		}
		
		// Not Necessary? Why?
		foreach(EraseMesh e in destructible.EraseBrushes) {
			float ratioX = destructible.transform.localScale.x / destructible.transform.localScale.y;

			Vector2 scale = destructible.transform.localScale;
			scale.y *= ratioX;
			
			Max2D.DrawMesh(GetEraseMaterial(), e.mesh, transform, Vector2D.Zero(), transform.position.z + 0.2f, new Vector2D(scale));
		}

		SaveRenderTextureToSprite();
		
		destructible.initialized = true;
	}

	void Update() {
		Transform transform = renderCamera.transform;

		if (destructible.initialized == false) {
			Initialize();
			return;
		}

		if (destructible.eraseEvents.Count > 0 || destructible.modifiersAdded == true) {
			//DrawSelf

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

			// New Event Meshes That Erase Generation
			foreach(DestructionEvent e in destructible.eraseEvents) {
				EraseMesh mesh = e.eraseBrush.GetMesh(transform, destructible.transform);

				destructible.EraseBrushes.Add(mesh);
			}

			// Erase Mesh
			foreach(EraseMesh e in destructible.EraseBrushes) {
				float ratioX = destructible.transform.localScale.x / destructible.transform.localScale.y;

				Vector2 scale = destructible.transform.localScale;
				scale.y *= ratioX;
				
				Max2D.DrawMesh(GetEraseMaterial(), e.mesh, transform, Vector2D.Zero(), transform.position.z + 0.2f, new Vector2D(scale));
			}

			SaveRenderTextureToSprite();

			destructible.eraseEvents.Clear();
			destructible.modifiersAdded = false;
		}
	} 

	public void OnRenderObject() {
		Update();

		destructible.UpdateCollider();
	}
}
