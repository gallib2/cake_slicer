using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbsEffect : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem particleSystem;
    [SerializeField]
    private float signtificantDistance;
    [SerializeField]
    private float emissionRateNormaliser;
    private Vector3 oldMousePosition;
    private Vector3 newMousePosition;
    private float distanceBetweenOldAndNewMousePositions;
    //public static bool isSlicing;

    void Update()
    {
        newMousePosition = Input.mousePosition;
        distanceBetweenOldAndNewMousePositions= Vector3.Distance(newMousePosition, oldMousePosition) / Time.deltaTime;
        if ((HoleCutController.isSlicing || SpriteSlicer.isSlicing) && distanceBetweenOldAndNewMousePositions > signtificantDistance)
        {
            transform.position = Camera.main.ScreenToWorldPoint(newMousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            particleSystem.enableEmission = true;
            particleSystem.emissionRate = distanceBetweenOldAndNewMousePositions * emissionRateNormaliser;


        }
        else
        {
            particleSystem.enableEmission = false;
        }
        oldMousePosition = newMousePosition;
    }

    
}
