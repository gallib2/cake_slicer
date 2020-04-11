﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Destruction2DVisualsMesh {
	const float pi = Mathf.PI;
	const float pi2 = pi / 2;
	const float uv0 = 1f / 32;
	const float uv1 = 1f - uv0;

	static Vector2D vA = Vector2D.Zero(), vB = Vector2D.Zero();
	
	static public Mesh GenerateComplexMesh(List<Vector2D> complexSlicerPointsList, Transform transform, float lineWidth, float minVertexDistance, float zPosition, float squareSize, float lineEndWidth, float vertexSpace) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		Pair2D pair = Pair2D.Zero();
		for(int i = 0; i < complexSlicerPointsList.Count; i++) {
			pair.A = complexSlicerPointsList[i];
			pair.B = complexSlicerPointsList[(i + 1) % complexSlicerPointsList.Count];
		
			vA.x = pair.A.x;
			vA.y = pair.A.y;

			vB.x = pair.B.x;
			vB.y = pair.B.y;

			vA.Push (Vector2D.Atan2 (pair.A, pair.B), -minVertexDistance * vertexSpace);
			vB.Push (Vector2D.Atan2 (pair.A, pair.B), minVertexDistance * vertexSpace);

			trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(vA, vB), transform, lineWidth, zPosition));
		}

		return(Max2DMesh.Export(trianglesList));
	}


	static public Mesh GenerateLinearCutMesh(Pair2D linearPair, float cutSize, Transform transform, float lineWidth, float zPosition) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		LinearCut linearCutLine = LinearCut.Create(linearPair, cutSize);
		foreach(Pair2D pair in Pair2D.GetList(linearCutLine.GetPointsList(), true)) {
			trianglesList.Add(Max2DMesh.CreateLine(pair, transform, lineWidth, zPosition));
		}

		return(Max2DMesh.Export(trianglesList));
	}

	static public Mesh GenerateComplexCutMesh(List<Vector2D> complexSlicerPointsList, float cutSize, Transform transform, float lineWidth, float zPosition) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		ComplexCut complexCutLine = ComplexCut.Create(complexSlicerPointsList, cutSize);
		foreach(Pair2D pair in Pair2D.GetList(complexCutLine.GetPointsList(), true)) {
			trianglesList.Add(Max2DMesh.CreateLine(pair, transform, lineWidth, zPosition));
		}

		return(Max2DMesh.Export(trianglesList));
	}

	static public Mesh GeneratePolygonMesh(Vector2D pos, Polygon2D.PolygonType polygonType, float polygonSize, float minVertexDistance, Transform transform, float lineWidth, float zPosition) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		Polygon2D slicePolygon = Polygon2D.Create (polygonType, polygonSize);
		slicePolygon.ToOffsetItself(pos);

		Vector2D vA, vB;
		foreach(Pair2D pair in Pair2D.GetList(slicePolygon.pointsList, true)) {
			vA = new Vector2D (pair.A);
			vB = new Vector2D (pair.B);

			vA.Push (Vector2D.Atan2 (pair.A, pair.B), -minVertexDistance / 5);
			vB.Push (Vector2D.Atan2 (pair.A, pair.B), minVertexDistance / 5);

			trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(vA, vB), transform, lineWidth, zPosition));
		}

		return(Max2DMesh.Export(trianglesList));
	}

	static public Mesh GeneratePolygon2DMesh(Transform transform, Polygon2D polygon, float lineOffset, float lineWidth, bool connectedLine) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		Polygon2D poly = polygon;

		foreach(Pair2D p in Pair2D.GetList(poly.pointsList, connectedLine)) {
			trianglesList.Add(Max2DMesh.CreateLine(p, transform, lineWidth, lineOffset));
		}

		foreach(Polygon2D hole in poly.holesList) {
			foreach(Pair2D p in Pair2D.GetList(hole.pointsList, connectedLine)) {
				trianglesList.Add(Max2DMesh.CreateLine(p, transform, lineWidth, lineOffset));
			}
		}
		
		return(Max2DMesh.Export(trianglesList));
	}

	static public Mesh GenerateLinearMesh(Pair2D linearPair, Transform transform, float lineWidth, float zPosition, float squareSize, float lineEndWidth) {
		List<Mesh2DSubmesh> trianglesList = new List<Mesh2DSubmesh>();

		float size = squareSize;

		trianglesList.Add(Max2DMesh.CreateLine(linearPair, transform, lineWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.A.x - size, linearPair.A.y - size), new Vector2D(linearPair.A.x + size, linearPair.A.y - size)), transform, lineEndWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.A.x - size, linearPair.A.y - size), new Vector2D(linearPair.A.x - size, linearPair.A.y + size)), transform, lineEndWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.A.x + size, linearPair.A.y + size), new Vector2D(linearPair.A.x + size, linearPair.A.y - size)), transform, lineEndWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.A.x + size, linearPair.A.y + size), new Vector2D(linearPair.A.x - size, linearPair.A.y + size)), transform, lineEndWidth, zPosition));
	
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.B.x - size, linearPair.B.y - size), new Vector2D(linearPair.B.x + size, linearPair.B.y - size)), transform, lineEndWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.B.x - size, linearPair.B.y - size), new Vector2D(linearPair.B.x - size, linearPair.B.y + size)), transform, lineEndWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.B.x + size, linearPair.B.y + size), new Vector2D(linearPair.B.x + size, linearPair.B.y - size)), transform, lineEndWidth, zPosition));
		trianglesList.Add(Max2DMesh.CreateLine(new Pair2D(new Vector2D(linearPair.B.x + size, linearPair.B.y + size), new Vector2D(linearPair.B.x - size, linearPair.B.y + size)), transform, lineEndWidth, zPosition));
	
		return(Max2DMesh.Export(trianglesList));
	}
}
