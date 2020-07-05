using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSliceable : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public BoxCollider2D boxCollider;//TODO: get rid of me
    [HideInInspector] public PolygonCollider2D polygonCollider;
    //private List<Vector2> physicsShape = new List<Vector2>();
    private bool initialised = false;

    public int pixelMapIndex;

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

            GetNewPolygonCollider();
        }
        initialised = true;
    }

    public PolygonCollider2D GetNewPolygonCollider()
    {
        Debug.Log("GetNewPolygonCollider");
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

