using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbsEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private ParticleSystemRenderer particleSystemRenderer;
   [SerializeField] private float signtificantDistance;
    [SerializeField]
    private float emissionRateNormaliser;
    private Vector3 oldTouchPosition;
    private Vector3 newTouchPosition;
    private float distanceBetweenOldAndNewMousePositions;
    [SerializeField] private Material standardMaterial;
    [SerializeField] private Material goldenKnifeMaterial;
    private Color standardColour1;
    private Color standardColour2;
    [SerializeField] private Color goldenKnifeColour1;
    [SerializeField] private Color goldenKnifeColour2;
    [SerializeField] private float standardSize1;
    [SerializeField] private float standardSize2;
    [SerializeField] private float goldenKnifeSize1;
    [SerializeField] private float goldenKnifeSize2;
    //public static bool isSlicing;

    private void OnEnable()
    {
        PowerUps.OnGoldenKnifeActivated += SwitchToGoldenKnifeMode;
        PowerUps.OnGoldenKnifeDeactivated += SwitchToStandardMode;
    }

    private void OnDisable()
    {
        PowerUps.OnGoldenKnifeActivated -= SwitchToGoldenKnifeMode;
        PowerUps.OnGoldenKnifeDeactivated -= SwitchToStandardMode;
    }

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

        /*if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchToGoldenKnifeMode();
        }*/
    }
    
    public void ChangeStandardColours(SpriteSliceable spriteSliceable)//TODO: chenge into an event
    {
        PixelMapping.PixelMap pixelMap = PixelMapping.PixelMapper.GetPixelMap(spriteSliceable.pixelMapIndex);
        if(pixelMap != null)
        {
            standardColour1 = pixelMap.outlineColour1;
            standardColour2 = pixelMap.outlineColour2;
        }
        else
        {
            standardColour1 = Color.black;
            standardColour2 = Color.white;
            Debug.LogWarning("No pixel map found.");
        }

        if (!PowerUps.GoldenKnifeIsActive)
        {
            SwitchToStandardMode();
        }

       /* particleSystem.main = mainModule;
        ParticleSystem.MinMaxGradient colours = new ParticleSystem.MinMaxGradient(spriteSliceable.outlineColour1, spriteSliceable.outlineColour2);

        particleSystem.main.startColor = colours;*/
    }

    private void SwitchToGoldenKnifeMode()
    {
        particleSystemRenderer.material = goldenKnifeMaterial;
        ParticleSystem.MainModule mainModule = particleSystem.main;//How could a struct influence the particle system????
        mainModule.startColor = new ParticleSystem.MinMaxGradient(goldenKnifeColour1, goldenKnifeColour2);
        mainModule.startSize = new ParticleSystem.MinMaxCurve(goldenKnifeSize1, goldenKnifeSize2);

    }

    private void SwitchToStandardMode()
    {
        particleSystemRenderer.material = standardMaterial;
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(standardColour1, standardColour2);
        mainModule.startSize = new ParticleSystem.MinMaxCurve(standardSize1, standardSize2);

    }
}
