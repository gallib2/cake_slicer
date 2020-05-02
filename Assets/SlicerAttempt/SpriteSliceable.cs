﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSliceable : MonoBehaviour
{
    /*[HideInInspector]*/ public SpriteRenderer spriteRenderer;
    [HideInInspector] public BoxCollider2D boxCollider;
    [HideInInspector] public PolygonCollider2D polygonCollider;
    private List<Vector2> physicsShape = new List<Vector2>();
    bool initialised = false;
    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        if (!initialised)
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            }
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        initialised = true;
    }

    public PolygonCollider2D GetNewPolygonCollider()
    {
        if (polygonCollider != null)
        {
            Destroy(polygonCollider);
        }
        //polygonCollider.pat
       return (polygonCollider = gameObject.AddComponent<PolygonCollider2D>());

    }

    /*public PolygonCollider2D GetUpdatedPolygonCollider()
    {
        //TODO: LateUpdate?

        if (polygonCollider == null)
        {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
        physicsShape.Clear();
        spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);
        polygonCollider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
        polygonCollider.SetPath(0, physicsShape);
        return polygonCollider;
    }*/
}

/*public class ColliderCreator : MonoBehaviour
{
    void Start()
    {
        // Stop if no mesh filter exists or there's already a collider
        if (GetComponent<PolygonCollider2D>() || GetComponent<SpriteRenderer>() == null)
        {
            return;
        }

        // Get triangles and vertices from mesh
        ushort[] triangles = GetComponent<SpriteRenderer>().sprite.triangles;
        Vector2[] vertices = GetComponent<SpriteRenderer>().sprite.vertices;

        // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            for (int e = 0; e < 3; e++)
            {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                if (edges.ContainsKey(edge))
                {
                    edges.Remove(edge);
                }
                else
                {
                    edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                }
            }
        }

        // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> edge in edges.Values)
        {
            if (lookup.ContainsKey(edge.Key) == false)
            {
                lookup.Add(edge.Key, edge.Value);
            }
        }

        // Create empty polygon collider
        PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.pathCount = 0;

        // Loop through edge vertices in order
        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while (true)
        {

            // Add vertex to collider path
            colliderPath.Add(vertices[nextVert]);

            // Get next vertex
            nextVert = lookup[nextVert];

            // Store highest vertex (to know what shape to move to next)
            if (nextVert > highestVert)
            {
                highestVert = nextVert;
            }

            // Shape complete
            if (nextVert == startVert)
            {

                // Add path to polygon collider
                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPath.ToArray());
                colliderPath.Clear();

                // Go to next shape if one exists
                if (lookup.ContainsKey(highestVert + 1))
                {

                    // Set starting and next vertices
                    startVert = highestVert + 1;
                    nextVert = startVert;

                    // Continue to next loop
                    continue;
                }

                // No more verts
                break;
            }
        }
    }
}
*/