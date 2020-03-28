using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Destruction2DModifierControllerObject : Destruction2DControllerObject {
	public bool mouseDown = false;
	public bool enableModdifier = false;
	public Vector2 modifierSize = new Vector2(1, 1);
	public float modifierRotation = 0;
	public bool randomRotation = true;
	public int modifierID = 0;
	public bool randomModifierID = false;
	
	public Texture2D[] modifierTextures = new Texture2D[1];

	public void Update(Vector2D pos) {
		float scroll = Input.GetAxis("Mouse ScrollWheel");	
		float newModifierSizeX = modifierSize.x + scroll * 2;
		if (newModifierSizeX > 0.05f) {
			modifierSize.x = newModifierSizeX;
		}
		float newModifierSizeY = modifierSize.y + scroll * 2;
		if (newModifierSizeY > 0.05f) {
			modifierSize.y = newModifierSizeY;
		}

		mouseDown = true;

		if (Input.GetMouseButtonDown (0)) {
			Spawn(pos);
		}
	}

	public void Spawn(Vector2D pos) {
		if (modifierTextures.Length > 0) { 
			Destruction2D.AddModifierAll(modifierTextures[modifierID], pos.ToVector2(), modifierSize, modifierRotation, destructionLayer);

			if (randomRotation) {
				modifierRotation = Random.Range(0, 360);
			}

			if (randomModifierID) {
				modifierID = Random.Range(0, modifierTextures.Length - 1);
			}
		}
	}
	
	public void OnRender(Vector2 pos) {
		if (modifierID < modifierTextures.Length) {
			Material material = new Material (Shader.Find ("SmartDestruction2D/ModifierShader")); 
			material.mainTexture = modifierTextures[modifierID];

			Max2D.DrawImage(material, pos, modifierSize, modifierRotation, visuals.zPosition);
		}
	}
}
