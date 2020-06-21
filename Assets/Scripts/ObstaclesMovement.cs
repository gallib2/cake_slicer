using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesMovement : MonoBehaviour
{
    public float speed = 0.5f;
    PolygonCollider2D parentCollider;
    Vector2 targetPoint;

    void Start()
    {
        parentCollider = transform.parent.GetComponent<PolygonCollider2D>();
        targetPoint = RandomPointInBounds(parentCollider.bounds);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        bool arriveTarget = IsArriveTarget(transform.position, targetPoint);

        if(arriveTarget)
        {
            parentCollider = transform.parent.GetComponent<PolygonCollider2D>();
            targetPoint = RandomPointInBounds(parentCollider.bounds);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, step);

    }

    private bool IsArriveTarget(Vector2 position, Vector2 target)
    {
        bool isXEqual = Mathf.Approximately(position.x, target.x);
        bool isYEqual = Mathf.Approximately(position.y, target.y);

        return isXEqual && isYEqual;
    }

    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
        );
    }

}
