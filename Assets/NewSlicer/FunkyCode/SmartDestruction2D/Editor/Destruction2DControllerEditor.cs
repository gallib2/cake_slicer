using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Destruction2DController))]
public class Destruction2DControllerEditor : Editor {
	static bool visualsFoldout = true;
	static bool foldout = true;
	static bool modifiersFoldout = true;

	override public void OnInspectorGUI() {
		Destruction2DController script = target as Destruction2DController;

		script.destructionType = (Destruction2DController.DestructionType)EditorGUILayout.EnumPopup ("Destruction Type", script.destructionType);
		script.destructionLayer.SetLayerType((Destruction2DLayerType)EditorGUILayout.EnumPopup ("Destruction Layer", script.destructionLayer.GetLayerType()));

		EditorGUI.indentLevel = EditorGUI.indentLevel + 2;

		if (script.destructionLayer.GetLayerType() == Destruction2DLayerType.Selected) {
			for (int i = 0; i < 8; i++) {
				script.destructionLayer.SetLayer(i, EditorGUILayout.Toggle ("Layer " + (i + 1), script.destructionLayer.GetLayerState(i)));
			}
		}

		EditorGUI.indentLevel = EditorGUI.indentLevel - 2;

		visualsFoldout = EditorGUILayout.Foldout(visualsFoldout, "Visuals");
		if (visualsFoldout) {
			EditorVisuals(script.visuals);
		}

		DestructionTypesUpdate (script);

		if (Destruction2DController.DestructionType.Polygon == script.destructionType || Destruction2DController.DestructionType.Modifier == script.destructionType) {

			modifiersFoldout = EditorGUILayout.Foldout(modifiersFoldout, "Mofidiers" );
			if (modifiersFoldout) {
				EditorModifier(script.modifierControllerObject);
			}
		}
	}

	void EditorLinearCut(Destruction2DLinearCutControllerObject script) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
		
		script.cutSize = EditorGUILayout.FloatField ("Linear Cut Size", script.cutSize);
		if (script.cutSize < 0.01f) {
			script.cutSize = 0.01f;
		}

		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}

	void EditorComplexCut(Destruction2DComplexCutControllerObject script) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
		
		script.cutSize = EditorGUILayout.FloatField ("Complex Cut Size", script.cutSize);
		if (script.cutSize < 0.01f) {
			script.cutSize = 0.01f;
		}

		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}

	void EditorPolygon(Destruction2DPolygonControllerObject script) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

		script.polygonType = (Polygon2D.PolygonType)EditorGUILayout.EnumPopup ("Type", script.polygonType);
		script.polygonSize = EditorGUILayout.FloatField ("Size", script.polygonSize);
		if (script.polygonType == Polygon2D.PolygonType.Circle) {
			script.polygonEdgeCount = EditorGUILayout.IntField ("Edge Count", script.polygonEdgeCount);	
		}
		
		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}

	void EditorPolygonBrush(Destruction2DPolygonBrushControllerObject script) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

		script.polygonType = (Polygon2D.PolygonType)EditorGUILayout.EnumPopup ("Type", script.polygonType);
		script.polygonSize = EditorGUILayout.FloatField ("Size", script.polygonSize);

		if (script.polygonType == Polygon2D.PolygonType.Circle) {
			script.polygonEdgeCount = EditorGUILayout.IntField ("Edge Count", script.polygonEdgeCount);	
		}
		
		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}

	void EditorComplexBrush(Destruction2DComplexBrushControllerObject script) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

		script.cutSize = EditorGUILayout.FloatField ("Complex Cut Size", script.cutSize);
		if (script.cutSize < 0.01f) {
			script.cutSize = 0.01f;
		}

		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}

	void DestructionTypesUpdate(Destruction2DController script){
		Destruction2DVisuals visuals = script.visuals;

		switch (script.destructionType) {
			case Destruction2DController.DestructionType.LinearCut:
				foldout = EditorGUILayout.Foldout(foldout, "Linear Cut" );
				if (foldout) {
					EditorLinearCut(script.linearCutControlelrObject);
				}
				break;

			case Destruction2DController.DestructionType.ComplexCut:
				foldout = EditorGUILayout.Foldout(foldout, "Complex Cut" );
				if (foldout) {
					EditorComplexCut(script.complexCutControllerObject);
					visuals.minVertexDistance = EditorGUILayout.FloatField ("Min Vertex Size", visuals.minVertexDistance);
				}
				break;

			case Destruction2DController.DestructionType.Polygon:
				foldout = EditorGUILayout.Foldout(foldout, "Polygon Cut");
				if (foldout) {
					EditorPolygon(script.polygonControllerObject);
					visuals.minVertexDistance = EditorGUILayout.FloatField ("Min Vertex Size", visuals.minVertexDistance);
				}
				break;

			case Destruction2DController.DestructionType.PolygonBrush:
				foldout = EditorGUILayout.Foldout(foldout, "Polygon Brush");
				if (foldout) {
					EditorPolygonBrush(script.polygonBrushControllerObject);
					visuals.minVertexDistance = EditorGUILayout.FloatField ("Min Vertext Size", visuals.minVertexDistance);
				}
				break;

			case Destruction2DController.DestructionType.ComplexBrush:
				foldout = EditorGUILayout.Foldout(foldout, "Complex Brush");
				if (foldout) {
					EditorComplexBrush(script.complexBrushControllerObject);
					visuals.minVertexDistance = EditorGUILayout.FloatField ("Min Vertex Size", visuals.minVertexDistance);
				}
				break;
		}
	}

	void EditorModifier(Destruction2DModifierControllerObject script) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

		script.enableModdifier = EditorGUILayout.Toggle("Enable", script.enableModdifier);
		
		script.modifierSize = EditorGUILayout.Vector2Field("Size", script.modifierSize);
		script.randomRotation = EditorGUILayout.Toggle("Random Rotation", script.randomRotation);
		if (script.randomRotation == false) {
			script.modifierRotation = EditorGUILayout.FloatField("Rotation", script.modifierRotation);
		}

		script.randomModifierID = EditorGUILayout.Toggle("Random Modifier", script.randomModifierID);
		if (script.randomModifierID == false) {
			script.modifierID = EditorGUILayout.IntField("ModifierID", script.modifierID);
		}

		if (script.modifierTextures.Length > 0) {
			SerializedProperty myIterator = serializedObject.FindProperty("modifierControllerObject.modifierTextures");

			while (true) {
				Rect myRect = GUILayoutUtility.GetRect(0f, 16f);
				bool showChildren = EditorGUI.PropertyField(myRect, myIterator);
				
				if (myIterator.NextVisible(showChildren) == false) {
					break;
				}
			}
		
			serializedObject.ApplyModifiedProperties();		
		} else {
			script.modifierTextures = new Texture2D[1];
			script.modifierTextures[0] = new Texture2D(2, 2);
		}
			
		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}

	void EditorVisuals(Destruction2DVisuals id) {
		EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
		id.drawVisuals = EditorGUILayout.Toggle ("Enable Visuals", id.drawVisuals);

		if (id.drawVisuals == true) {
			id.zPosition = EditorGUILayout.FloatField ("Z Position", id.zPosition);
			id.destructionColor = (Color)EditorGUILayout.ColorField ("Color", id.destructionColor);
			id.visualScale = EditorGUILayout.Slider("Scale", id.visualScale, 1f, 50f);
			id.lineBorder = EditorGUILayout.Toggle ("Border", id.lineBorder);
			id.borderScale = EditorGUILayout.Slider("Border Scale", id.borderScale, 1f, 5f);
			id.lineWidth = EditorGUILayout.Slider ("Width", id.lineWidth, 0.01f, 5f);
			id.lineEndWidth = EditorGUILayout.Slider ("Line End Width", id.lineEndWidth, 0.01f, 5f);
			id.minVertexDistance = EditorGUILayout.Slider("Min Vertex Distance", id.minVertexDistance, 0.1f, 5f);
		}
		
		EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	}
}