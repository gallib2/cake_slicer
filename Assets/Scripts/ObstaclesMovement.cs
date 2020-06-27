﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesMovement : MonoBehaviour
{
    public float speed = 0.5f;
    //PolygonCollider2D parentCollider;
    private Bounds bounds;// Removed the ref to the collider cause it might get destroyed
    Vector2 targetPoint;
   [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        changeTarget();
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        bool arriveTarget = IsArriveTarget(transform.position, targetPoint);

        if(arriveTarget)
        {
            changeTarget();
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, step);


    }

    private bool IsArriveTarget(Vector2 position, Vector2 target)
    {
        bool isXEqual = Mathf.Approximately(position.x, target.x);
        bool isYEqual = Mathf.Approximately(position.y, target.y);

        return isXEqual && isYEqual;
    }

    private void changeTarget()
    {
        bounds = transform.parent.GetComponent<PolygonCollider2D>().bounds;
        targetPoint = RandomPointInBounds(bounds);
        spriteRenderer.flipX = (transform.position.x < targetPoint.x);
    }

    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
        );
    }

}
