using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction2DControllerObject {
	public Destruction2DVisuals visuals = new Destruction2DVisuals();
	public Destruction2DLayer destructionLayer = Destruction2DLayer.Create();

	public void SetController(Destruction2DVisuals visualsSettings, Destruction2DLayer layerObject) {
		visuals = visualsSettings;
		destructionLayer = layerObject;
	}
}
