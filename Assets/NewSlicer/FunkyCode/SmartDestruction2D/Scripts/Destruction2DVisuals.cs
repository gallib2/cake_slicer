using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Destruction2DVisuals {
	public bool drawVisuals = true;
	public float visualScale = 1f;
	public float lineWidth = 1.0f;
	public float lineEndWidth = 1.0f;
	public float zPosition = 0f;
	public Color destructionColor = Color.white;
	public bool lineBorder = true;
	public float lineEndSize = 0.5f;
	public float vertexSpace = 0.25f;
	public float borderScale = 2f;
	public float minVertexDistance = 1f;

	public int sortingOrder;
	public string sortingLayerName;

	// Mesh & Material
	private List<Mesh> mesh = new List<Mesh>();
	private List<Mesh> meshBorder = new List<Mesh>();

	private Material lineMaterial;
	private Material lineMaterialBorder;

	public List<RendererObject> rendererObjects = new List<RendererObject>();
	private GameObject gameObject;

	public void SetGameObject(GameObject setGameObject) {
		gameObject = setGameObject;
	}


	public void GenerateComplexMesh(List<Vector2D> points, Transform transform) {
		meshBorder.Add(Destruction2DVisualsMesh.GenerateComplexMesh(points, transform, lineWidth * visualScale * borderScale, minVertexDistance, zPosition - 0.001f, lineEndSize * visualScale,  lineEndWidth * visualScale * borderScale, vertexSpace));
		mesh.Add(Destruction2DVisualsMesh.GenerateComplexMesh(points, transform, lineWidth * visualScale, minVertexDistance, zPosition - 0.002f, lineEndSize * visualScale, lineEndWidth * visualScale, vertexSpace));
	}

	public void GeneratePolygonMesh(Vector2D pos, Polygon2D.PolygonType polygonType, float polygonSize, Transform transform) {
		meshBorder.Add(Destruction2DVisualsMesh.GeneratePolygonMesh(pos, polygonType, polygonSize * visualScale, minVertexDistance, transform, lineWidth * visualScale * borderScale, zPosition - 0.001f));
		mesh.Add(Destruction2DVisualsMesh.GeneratePolygonMesh(pos, polygonType, polygonSize * visualScale, minVertexDistance, transform, lineWidth * visualScale, zPosition - 0.002f));
	}
	
	public void GenerateLinearMesh(Pair2D linearPair, Transform transform) {
		meshBorder.Add(Destruction2DVisualsMesh.GenerateLinearMesh(linearPair, transform, lineWidth * visualScale * borderScale, zPosition - 0.001f, lineEndSize * visualScale, lineEndWidth * visualScale * borderScale));
		mesh.Add(Destruction2DVisualsMesh.GenerateLinearMesh(linearPair, transform, lineWidth * visualScale, zPosition - 0.002f, lineEndSize * visualScale, lineEndWidth * visualScale));
	}

	public void GenerateLinearCutMesh(Pair2D linearPair, float cutSize, Transform transform) {
		meshBorder.Add(Destruction2DVisualsMesh.GenerateLinearCutMesh(linearPair, cutSize * visualScale, transform, lineWidth * visualScale * borderScale, zPosition - 0.001f));
		mesh.Add(Destruction2DVisualsMesh.GenerateLinearCutMesh(linearPair, cutSize * visualScale, transform, lineWidth * visualScale, zPosition - 0.002f));
	}

	public void GenerateComplexCutMesh(List<Vector2D> pointsList, float cutSize, Transform transform) {
		meshBorder.Add(Destruction2DVisualsMesh.GenerateComplexCutMesh(pointsList, cutSize * visualScale, transform, lineWidth * visualScale * borderScale, zPosition - 0.001f));
		mesh.Add(Destruction2DVisualsMesh.GenerateComplexCutMesh(pointsList, cutSize * visualScale, transform, lineWidth * visualScale, zPosition - 0.002f));
	}

	public void Initialize() {
		Max2D.Check();

		lineMaterial = new Material(Max2D.lineMaterial);
		lineMaterialBorder = new Material(Max2D.lineMaterial);
	}

	public Material GetBorderMaterial() {
		return(lineMaterial);
	}

	public Material GetFillMaterial() {
		return(lineMaterialBorder);
	}

	public void Draw() {
		if (lineBorder && meshBorder.Count > 0) {
			if (meshBorder.Count > 0) {
				foreach(Mesh m in meshBorder) {
					RendererObject renderObject = GetFreeRenderObject();
					renderObject.drawn = true;
					renderObject.meshRenderer.sharedMaterial = GetBorderMaterial();
					renderObject.meshFilter.sharedMesh = m;
				}
			}
		}

		if (mesh.Count > 0) {
			foreach(Mesh m in mesh) {
				RendererObject renderObject = GetFreeRenderObject();
				renderObject.drawn = true;
				renderObject.meshRenderer.sharedMaterial = GetFillMaterial();
				renderObject.meshFilter.sharedMesh = m;
			}
		}
	}

	public void Clear() {
		foreach(RendererObject renderObject in rendererObjects) {
			renderObject.drawn = false;
		}


		if (meshBorder.Count > 0) {
			foreach(Mesh m in meshBorder) {
				UnityEngine.Object.DestroyImmediate(m);
			}
			meshBorder.Clear();
		}
		if (mesh.Count > 0) {
			foreach(Mesh m in mesh) {
				UnityEngine.Object.DestroyImmediate(m);
			}
			mesh.Clear();
		}
	}

	public RendererObject GetFreeRenderObject() {
		foreach(RendererObject renderObject in rendererObjects) {
			if (renderObject.drawn == false) {
				return(renderObject);
			}
		}
		
		RendererObject newRenderGameObject = new RendererObject(gameObject);
		
		newRenderGameObject.meshRenderer.sortingOrder = sortingOrder;
		newRenderGameObject.meshRenderer.sortingLayerName = sortingLayerName;

		rendererObjects.Add(newRenderGameObject);

		return(newRenderGameObject);
	}

	public class RendererObject {
		public MeshRenderer meshRenderer = new MeshRenderer();
		public MeshFilter meshFilter = new MeshFilter();
		public bool drawn = false;

		public RendererObject(GameObject gameObject) {
			GameObject newRenderGameObject = new GameObject("Slicer Visuals Render Object");
			newRenderGameObject.transform.parent = gameObject.transform;

			meshRenderer = newRenderGameObject.AddComponent<MeshRenderer>();
			meshFilter = newRenderGameObject.AddComponent<MeshFilter>();
		}
	}
}
