using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesMovement : MonoBehaviour
{
    public float speed = 0.5f;
    PolygonCollider2D poly;
    private bool changeDirection = true;
    private bool toMoveHorizontal;

    void Start()
    {
        toMoveHorizontal = Random.value < 0.5;
    }

    void Update()
    {
        poly = transform.parent.GetComponent<PolygonCollider2D>();
        // NOTE - The obstacle z position have to be the same as the parent (for the bounds.Contains function)
        bool isPositionInsideParentBounds = poly.bounds.Contains(transform.localPosition);
        if (!isPositionInsideParentBounds)
        {
            changeDirection = !changeDirection;
            //direction *= -1;

        }

        if (changeDirection)
        {
            if(toMoveHorizontal)
            {
                MoveHorizontal(1);
            }
            else
            {
                MoveVertical(1);
            }
        }
        else
        {
            if (toMoveHorizontal)
            {
                MoveHorizontal(-1);
            }
            else
            {
                MoveVertical(-1);
            }
        }
    }

    private void MoveHorizontal(int direction)
    {
        transform.position += direction * transform.right * speed * Time.deltaTime;
    }

    private void MoveVertical(int direction)
    {
        transform.position += direction * transform.up * speed * Time.deltaTime;
    }
}
