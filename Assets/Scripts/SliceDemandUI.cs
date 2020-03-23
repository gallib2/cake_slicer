using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceDemandUI : MonoBehaviour
{
    private float originalX;
    private float XDestination;
    [SerializeField]
    private float XSpaceBetweenFractions;
    [SerializeField]
    private float speed;

    public void ChangeDestination(int numberOfElementsToShow)
    {
        XDestination = originalX + ((numberOfElementsToShow - 1) * XSpaceBetweenFractions);

    }

    private void Start()
    {
        originalX = transform.localPosition.x;
    }
 
    void Update()
    {
        if (transform.localPosition.x != XDestination)
        {
            float currentStep = speed * Time.deltaTime;
            if (Mathf.Abs(XDestination - transform.localPosition.x) > currentStep)
            {
                Vector3 newPosition = new Vector3
                    ((transform.localPosition.x + currentStep * (transform.localPosition.x > XDestination ? -1 : 1)), transform.localPosition.y);
                // Vector3.MoveTowards(transform.position, new Vector3(XDestination, transform.position.y), speed*Time.deltaTime);
                transform.localPosition = newPosition;
            }
            else
            {
                transform.localPosition = new Vector3(XDestination, transform.localPosition.y);
            }
        }

    }
}
