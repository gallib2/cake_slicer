using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Destruction2DController : MonoBehaviour {
	public enum DestructionType {LinearCut, ComplexCut, Polygon, Modifier, PolygonBrush, ComplexBrush}
	public static Destruction2DController instance;

	public Destruction2DVisuals visuals = new Destruction2DVisuals();

	public DestructionType destructionType = DestructionType.LinearCut;
	public Destruction2DLayer destructionLayer = Destruction2DLayer.Create();

	public Destruction2DLinearCutControllerObject linearCutControlelrObject = new Destruction2DLinearCutControllerObject();
	public Destruction2DComplexCutControllerObject complexCutControllerObject = new Destruction2DComplexCutControllerObject();

	public Destruction2DPolygonControllerObject polygonControllerObject = new Destruction2DPolygonControllerObject();

	public Destruction2DPolygonBrushControllerObject polygonBrushControllerObject = new Destruction2DPolygonBrushControllerObject();
	public Destruction2DComplexBrushControllerObject complexBrushControllerObject = new Destruction2DComplexBrushControllerObject();

	public Destruction2DModifierControllerObject modifierControllerObject = new Destruction2DModifierControllerObject();

	void Awake () {
		instance = this;
	}

	void Start() {
		visuals.Initialize();

		linearCutControlelrObject.SetController(visuals, destructionLayer);
		complexCutControllerObject.SetController(visuals, destructionLayer);

		polygonControllerObject.SetController(visuals, destructionLayer);
		
		polygonBrushControllerObject.SetController(visuals, destructionLayer);
		complexBrushControllerObject.SetController(visuals, destructionLayer);

		polygonBrushControllerObject.Initialize();
	}

	public void SetLayerType(int type) {
		if (type == 0) {
			destructionLayer.SetLayerType((Destruction2DLayerType)0);
		} else {
			destructionLayer.SetLayerType((Destruction2DLayerType)1);
			destructionLayer.DisableLayers ();
			destructionLayer.SetLayer (type - 1, true);
		}
	}

	public static Vector2 GetMousePosition() {
		return(Camera.main.ScreenToWorldPoint (Input.mousePosition));
	}

	public void LateUpdate() {
		if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		Vector2D pos = new Vector2D(GetMousePosition());

		switch(destructionType) {
			case DestructionType.LinearCut:
				linearCutControlelrObject.Update(pos);
				break;

			case DestructionType.ComplexCut:
				complexCutControllerObject.Update(pos);
				break;

			case DestructionType.Polygon:
				bool addModifier = polygonControllerObject.Update(pos);
				if (addModifier && modifierControllerObject.enableModdifier) {
					modifierControllerObject.Spawn(pos);
				}
				break;
			
			case DestructionType.PolygonBrush:
				polygonBrushControllerObject.Update(pos);
				break;

			case DestructionType.Modifier:
				modifierControllerObject.Update(pos);
				break;

			case DestructionType.ComplexBrush:
				complexBrushControllerObject.Update(pos);
				break;
		}
	}

	void Update() {
		if (visuals.drawVisuals == false) {
			return;
		}

		Vector2 pos = GetMousePosition ();

		switch(destructionType) {

			case DestructionType.LinearCut:
				linearCutControlelrObject.Draw(transform);
				break;

			case DestructionType.ComplexCut:
				complexCutControllerObject.Draw(transform);
				break;

			case DestructionType.Polygon:
				polygonControllerObject.Draw(transform, pos);
				break;

			case DestructionType.PolygonBrush:
				polygonBrushControllerObject.Draw(transform, pos);
				break;

			case DestructionType.ComplexBrush:
				complexBrushControllerObject.Draw(transform);
				
				break;
		}
	}

	public void OnRenderObject() {
		if (visuals.drawVisuals == false) {
			return;
		}

		switch(destructionType) {
			case DestructionType.Modifier:
				Vector2 pos = GetMousePosition ();
				modifierControllerObject.OnRender(pos);
				break;
		}
	}
}