using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbsEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private float signtificantDistance;
    [SerializeField]
    private float emissionRateNormaliser;
    private Vector3 oldTouchPosition;
    private Vector3 newTouchPosition;
    private float distanceBetweenOldAndNewMousePositions;
    //public static bool isSlicing;

    void Update()
    {

        //newMousePosition = Input.mousePosition;
        newTouchPosition = InputManager.GetTouchPosition();
        distanceBetweenOldAndNewMousePositions = Vector3.Distance(newTouchPosition, oldTouchPosition) / Time.deltaTime;
        if ((/*HoleCutController.isSlicing ||*/ SpriteSlicer.isSlicing) && distanceBetweenOldAndNewMousePositions > signtificantDistance)
        {
            if (InputManager.GetTouch())//TODO: we might be able to remove this line, it is intended for testing
            {
                transform.position = Camera.main.ScreenToWorldPoint(newTouchPosition);
                transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            }

            particleSystem.enableEmission = true;
            particleSystem.emissionRate = distanceBetweenOldAndNewMousePositions * emissionRateNormaliser;

        }
        else
        {
            particleSystem.enableEmission = false;
        }
        oldTouchPosition = newTouchPosition;
    }
    
    public void ChangeColours(SpriteSliceable spriteSliceable)//TODO: chenge into an event
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(spriteSliceable.outlineColour1, spriteSliceable.outlineColour2);
       /* particleSystem.main = mainModule;
        ParticleSystem.MinMaxGradient colours = new ParticleSystem.MinMaxGradient(spriteSliceable.outlineColour1, spriteSliceable.outlineColour2);

        particleSystem.main.startColor = colours;*/
    }
}
