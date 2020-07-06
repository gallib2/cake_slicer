﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class SpriteSlicer : MonoBehaviour
{

    [SerializeField] private float drawRate = 0.012f;
    [SerializeField] private LayerMask cakesLayerMask;
    private Vector2Int lastHitPixel;
    private SpriteSliceable sliceableBeingSliced;
    private float boundsMinX;
    private float boundsMinY;
    private float normalisedMaxX;
    private float normalisedMaxY;
    private Texture2D currentTexture;
    private Texture2D dynamicTexture;
    //private bool appliedTextureThisFrame;
   // private Texture2D lowerDynamicTexture;//TODO: continue..
   // private Texture2D upperDynamicTexture;

    private int textureWidth;
    private int textureHeight;

    [SerializeField] private int eraseBrushSize = 7;
    [SerializeField] private int interpolationDistance;
    [Header("Outline")]
    [SerializeField] private int outlineThickness = 5;
    [SerializeField] private bool randomiseOutlineShape = false;
    [SerializeField] private bool generateOutlineColoursProcedurally = false;
    [SerializeField] private Texture2D outlineTexture;
    private float[,] outlineColoursBlackAndWhite;
    private PixelMapping.PixelMap currentPixelMap;

    [SerializeField] private Color goldenKnifeOutlineColour1;
    [SerializeField] private Color goldenKnifeOutlineColour2;

    private PixelState[,] pixelsStatesMap;
    private const int PIXEL_STATES_MAP_SIZE = 600;

    private int halfOfEraseBrushSize;
    private bool changedSinceLastCheck;
    [SerializeField] private float SliceChecksSpeed = 0.2f;
    [SerializeField] private bool shouldCheckSlicesRegularly;
    [SerializeField] private bool shouldCheckWhenEnteringAndExiting;
    [SerializeField] private double negligiblePolygonSliceSize = 0.01;
    [SerializeField] private bool usePixelMaps;
    //[SerializeField] private bool UsePrealculatedHoleShape = true;
    [SerializeField] private bool useInHouseLerp = true;
    [SerializeField] private bool useFloodFill = true;


    public static bool isSlicing;
    private int slicesCount;
    public int SlicesCount
    {
        get
        {
            return slicesCount;
        }
    }

    private void Start()
    {
        if (eraseBrushSize % 2 != 0)
        {
            eraseBrushSize++;
        }
        halfOfEraseBrushSize = eraseBrushSize / 2;

        if(holeShape == null)
        {
            InitialiseHoleShape();
        }

        CalculateSlicesRegularly();
        // InvokeRepeating("CalculateSlices", 2f,0.12f);
    }

    /* public void Restart()
     {
         sliceableBeingSliced = null;
         slicesCount = 0;
         lastHitPixel = Vector2Int.zero;
     }*/

    /*private void Update()
    {
        /*if (Input.GetMouseButtonUp(0))
        {
           // lastHitPixel = Vector2Int.zero;
            CalculateSlices();
        }
    }*/

    public void SetNewSliceable(SpriteSliceable newSliceable )
    {
        //if (sliceableBeingSliced == null)
        slicesCount = 0;
        lastHitPixel = Vector2Int.zero;

        // Collider2D colliderAtPoint = Physics2D.OverlapPoint(mousePoint);
        // if (colliderAtPoint != null && colliderAtPoint.GetComponent<SpriteSliceable>() != null)
        {
            //TODO: check if some of these are obselete!
            sliceableBeingSliced = newSliceable;//colliderAtPoint.GetComponent<SpriteSliceable>();
            sliceableBeingSliced.Initialise();
            currentTexture = sliceableBeingSliced.spriteRenderer.sprite.texture;
            dynamicTexture = new Texture2D(currentTexture.width, currentTexture.height);
            dynamicTexture.filterMode = currentTexture.filterMode;
            dynamicTexture.SetPixels(currentTexture.GetPixels());
            dynamicTexture.Apply();
            textureWidth = dynamicTexture.width;
            textureHeight = dynamicTexture.height;
            BoxCollider2D boxCollider = sliceableBeingSliced.boxCollider;//TODO we migh be able to get rid of this
            boundsMinX = sliceableBeingSliced.boxCollider.bounds.min.x;
            normalisedMaxX = sliceableBeingSliced.boxCollider.bounds.max.x - boundsMinX;
            boundsMinY = sliceableBeingSliced.boxCollider.bounds.min.y;
            normalisedMaxY = sliceableBeingSliced.boxCollider.bounds.max.y - boundsMinY;
            boxCollider.enabled = false;
           /* lowerDynamicTexture = new Texture2D(currentTexture.width, currentTexture.height/2);
            lowerDynamicTexture.filterMode = currentTexture.filterMode;
            lowerDynamicTexture.SetPixels(currentTexture.GetPixels(0,0, lowerDynamicTexture.width, lowerDynamicTexture.height));*/
            Sprite currentSprite = sliceableBeingSliced.spriteRenderer.sprite;
            Sprite newSprite = Sprite.Create
                (dynamicTexture, currentSprite.rect, new Vector2(0.5f, 0.5f), currentSprite.pixelsPerUnit);//, 1, SpriteMeshType.FullRect, currentSprite.border);
            sliceableBeingSliced.spriteRenderer.sprite = newSprite;

            PixelMapping.PixelMap map = PixelMapping.PixelMapper.GetPixelMap(sliceableBeingSliced.pixelMapIndex);
            if (map==null)
            {
                currentPixelMap = PixelMapping.PixelMap.GetEmergencyPixelMap(dynamicTexture);
            }
            else
            {
                currentPixelMap = map;
            }
            GenerateOutlineColoursMap(currentPixelMap.outlineColour1, currentPixelMap.outlineColour2);//TODO:Move and change name
            ResetPixelsStates();
            GenerateFloodFillMap();

        }
    }

    private void GenerateOutlineColoursMap(Color colour1, Color colour2)
    {
        if(outlineColoursBlackAndWhite == null)
        {
            outlineColoursBlackAndWhite = new float[outlineTexture.width, outlineTexture.height];
            for (int x = 0; x < outlineColoursBlackAndWhite.GetLength(0); x++)
            {
                for (int y = 0; y < outlineColoursBlackAndWhite.GetLength(1); y++)
                {
                    outlineColoursBlackAndWhite[x, y] = outlineTexture.GetPixel(x, y).r;
                }
            }
        }
       /* int width = outlineColours.GetLength(0);
        int height = outlineColours.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                outlineColours[x, y] = colour1;
               // outlineColours[x,y] = Color.Lerp(colour1, colour2, outlineColoursBlackAndWhite[x, y]); 
            }
        }*/
    }

    private void ResetPixelsStates()
    {
        if (pixelsStatesMap == null)
        {
            pixelsStatesMap = new PixelState[PIXEL_STATES_MAP_SIZE, PIXEL_STATES_MAP_SIZE];
        }

        int pixelMapWidth = pixelsStatesMap.GetLength(0);
        int pixelMapHeight = pixelsStatesMap.GetLength(1);
        int currentMapWidth = currentPixelMap.pixelStates.GetLength(0);
        int currentMapHeight = currentPixelMap.pixelStates.GetLength(1);
        for (int x = 0; x < pixelMapWidth; x++)
        {
            for (int y = 0; y < pixelMapHeight; y++)
            {
                //pixelsStatesMap[x, y] = PixelState.UNKNOWN;
                if(x >= currentMapWidth || y >= currentMapHeight)
                {
                    pixelsStatesMap[x, y] = PixelState.TRANSPARENT;
                }
                else
                {
                    pixelsStatesMap[x, y] = currentPixelMap.pixelStates[x, y];
                }
            }
        }
    }

    //Bookeeping:
    private bool overlappedColliderThisFrame;
    private bool overlappedColliderPreviousFrame;
    private float timeOnLastDraw;

    private void Update()
    {

        isSlicing = false;
        //appliedTextureThisFrame = false;
        if (GameManager.GameIsPaused)
        {
            return;
        }

        if (!SlicesManager.allowToSlice)
        {
            //skipedFrame = false;
            return;
        }

        if (InputManager.GetTouchUp())
        {
            ThingsToDoOnTouchEnded();
        }
       /* else*/ if (InputManager.GetTouch())
        {
            ThingsToDoWhenTouching(InputManager.GetTouchPosition());
        }

       /* if (GameManager.testingOnPersonalComputer)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ThingsToDoOnTouchEnded();
            }
            if (Input.GetMouseButton(0))
            {
                ThingsToDoWhenTouching(Input.mousePosition);
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                //  if (Input.GetMouseButtonUp(0))
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ThingsToDoOnTouchEnded();
                }

                ThingsToDoWhenTouching(Input.GetTouch(0).position);
            }
            else
            {
                lastHitPixel = Vector2Int.zero;
            }
        }*/


      /*  if (Input.GetKeyDown(KeyCode.F))
        {
            floodFillTexture = currentTexture;
            FloodFillNumberOfSlices();
        }*/
    }

    private void ThingsToDoOnTouchEnded()
    {
        //if (!appliedTextureThisFrame)
        {
            currentTexture.Apply();//TODO: avoid
        }
        CalculateSlices();
        lastHitPixel = Vector2Int.zero;
    }

    private void ThingsToDoWhenTouching( Vector2 touchPosition)
    {
         Vector2 mousePoint = Camera.main.ScreenToWorldPoint(touchPosition);
         Collider2D colliderAtMousePoint = (Physics2D.OverlapPoint(mousePoint, cakesLayerMask));
         overlappedColliderThisFrame = (colliderAtMousePoint != null && colliderAtMousePoint is PolygonCollider2D);//TODO: improve, get rid of collider?

         if (sliceableBeingSliced == null)
         {
             return;
         }
         currentTexture = sliceableBeingSliced.spriteRenderer.sprite.texture;
         float normalisedHitPointX = mousePoint.x - boundsMinX;
         float fractionX = normalisedHitPointX / normalisedMaxX;

         float normalisedHitPointY = mousePoint.y - boundsMinY;
         float fractionY = normalisedHitPointY / normalisedMaxY;

         int hitPixelX = Mathf.RoundToInt((float)textureWidth * fractionX);
         int hitPixelY = Mathf.RoundToInt((float)textureHeight * fractionY);
         Vector2Int newHitPixel = new Vector2Int(hitPixelX, hitPixelY);
         bool textureChanged = false;

         //Texture Creation:
         if (MakeCircleHole(hitPixelX, hitPixelY))
         {
             textureChanged = true;
         }

         if (lastHitPixel != Vector2Int.zero)
         {
             int distance = Mathf.RoundToInt(Vector2.Distance(newHitPixel, lastHitPixel));
             //Debug.Log("distance" + distance);
             if (distance > interpolationDistance)
             {
                 for (int multiplierA = interpolationDistance; multiplierA < distance; multiplierA += interpolationDistance)
                 {
                     int multiplierB = distance - multiplierA;
                     Vector2Int InterpolatedPoint = new Vector2Int(
                         Mathf.RoundToInt((float)((newHitPixel.x * multiplierA) + (lastHitPixel.x * multiplierB)) / (float)distance),
                         Mathf.RoundToInt((float)((newHitPixel.y * multiplierA) + (lastHitPixel.y * multiplierB)) / (float)distance));
                     if (MakeCircleHole(InterpolatedPoint.x, InterpolatedPoint.y))
                     {
                         textureChanged = true;
                     }
                 }
                 //Vector2Int InterpolatedPoint = new Vector2Int((newHitPixel.x + lastHitPixel.x) / 2, (newHitPixel.y + lastHitPixel.y) / 2);
             }
         }

         if (textureChanged)
         {
            if (Time.time - timeOnLastDraw > drawRate)
            {
                 currentTexture.Apply();
                 timeOnLastDraw = Time.time;
            }
         }
         lastHitPixel = newHitPixel;
         if (overlappedColliderThisFrame)
         {
             isSlicing = true;
         }

         if (overlappedColliderPreviousFrame && !overlappedColliderThisFrame)
         {
             currentTexture.Apply();
             if (shouldCheckWhenEnteringAndExiting)
             {
                 CalculateSlices();

             }
         }

         overlappedColliderPreviousFrame = overlappedColliderThisFrame;
        
        /*else
        {
            lastHitPixel = Vector2Int.zero;
        }*/
    }

    private bool MakeCircleHole(int x, int y)
    {
       // if (UsePrealculatedHoleShape)
        {
            return MakeCircleHoleFromShape(x, y);
        }
        /*else
        {
            return MakeCircleHoleByDistanceCalculations(x, y);
        }*/
    }


    private bool MakeCircleHoleFromShape(int x, int y)
    {
        if (textureWidth != currentTexture.width || textureHeight != currentTexture.height)
        {
            Debug.LogError("texture dimentions are incorrect!");
            textureWidth = currentTexture.width;
            textureHeight = currentTexture.height;
        }

        bool changed = false;

        bool goldenKnifeIsActive = PowerUps.GoldenKnifeIsActive;

        for (int i = 0; i < holeShape.Length; i++)
        {
            ref ShapePixel shapePixel = ref holeShape[i];
            int textureX = shapePixel.x + x;
            int textureY = shapePixel.y + y;

            if (textureX >= 0 && textureY >= 0 &&
                textureX < textureWidth && textureY < textureHeight)
            {
                PixelState texturePixelState = pixelsStatesMap[textureX, textureY];
                if (texturePixelState == PixelState.UNKNOWN)
                {
                    if (usePixelMaps)
                    {
                        // Profiler.BeginSample("PixelHunting");
                        if (currentPixelMap.pixelStates[textureX, textureY] == PixelState.TRANSPARENT)
                        {
                            texturePixelState = pixelsStatesMap[textureX, textureY] = PixelState.TRANSPARENT;
                        }
                        //  Profiler.EndSample();

                    }
                    else
                    {
                        //  Profiler.BeginSample("PixelHunting");

                        if (currentTexture.GetPixel(textureX, textureY).a < 0.15f)
                        {
                            texturePixelState = pixelsStatesMap[textureX, textureY] = PixelState.TRANSPARENT;
                        }
                        //Profiler.EndSample();

                    }

                }
                if (texturePixelState != PixelState.TRANSPARENT && texturePixelState != shapePixel.state)
                {
                    if (shapePixel.state == PixelState.OPAQUE_TOUCHED)
                    {
                        //Outline Generation:
                        Color outlineColour;
                         if (!goldenKnifeIsActive)
                         {      
                            if (useInHouseLerp)
                            {
                                Profiler.BeginSample("In House Lerp");
                                float normaliser = outlineColoursBlackAndWhite[textureX, textureY];
                                Color colour1 = currentPixelMap.outlineColour1;
                                Color colour2 = currentPixelMap.outlineColour2;

                                //Wow, no need to check max and min..
                                outlineColour.r =
                                   (normaliser * (colour1.r - colour2.r)) + colour2.r;          
                                outlineColour.g =
                                   (normaliser * (colour1.g - colour2.g)) + colour2.g;
                                outlineColour.b =
                                   (normaliser * (colour1.b - colour2.b)) + colour2.b;

                                outlineColour.a = 1;

                                Profiler.EndSample();  //Why is this slower than unity's function and why does it give dark results??
                            }
                            else
                            {
                                outlineColour = Color.LerpUnclamped
                                    (currentPixelMap.outlineColour1, currentPixelMap.outlineColour2, outlineColoursBlackAndWhite[textureX, textureY]);
                            }
                         }
                         else
                         {
                             outlineColour = Color.LerpUnclamped
                                 (goldenKnifeOutlineColour1, goldenKnifeOutlineColour2, outlineColoursBlackAndWhite[textureX, textureY]);
                         }
                         currentTexture.SetPixel(textureX, textureY, outlineColour);
                         pixelsStatesMap[textureX, textureY] = PixelState.OPAQUE_TOUCHED;
                    }
                    else
                    {
                        currentTexture.SetPixel(textureX, textureY, Color.clear);
                        pixelsStatesMap[textureX, textureY] = PixelState.TRANSPARENT;
                    }
                    changed = true;
                }
            }   
        }
        
        if (changed)
        {
            changedSinceLastCheck = true;
            return true;
        }
        return false;

    }

    private void CalculateSlices()
    {
        if (changedSinceLastCheck)
        {
            Debug.Log("CalculateSlices");

            if (useFloodFill)
            {
                CalculateSlicesByFloodFill();
            }
            else 
            {
                CalculateSlicesByPolygonCalculations();
            }

            changedSinceLastCheck = false;

        }

        //Invoke("CalculateSlices", SliceChecksSpeed);
    }

    private void CalculateSlicesByPolygonCalculations()
    {
        List<Polygon2D> polygons = Polygon2DList.CreateFromPolygonCollider(sliceableBeingSliced.GetNewPolygonCollider());
        if (polygons.Count > 1)//Optimisation..
        {
            //Debug.Log("polygons.Count :" + polygons.Count);
            /*for (int i = 0; i < polygons.Count;)//TODO: must be inefficient
            {
                Debug.Log("Area:" + polygons[i].GetArea());
                i++;
            }*/
            for (int i = 0; i < polygons.Count;)//TODO: must be inefficient//TODO: add this feature to floodfill system
            {
                if (polygons[i].GetArea() < negligiblePolygonSliceSize)
                {
                    polygons.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        slicesCount = polygons.Count;
    }

    private void CalculateSlicesRegularly()
    {
        if (shouldCheckSlicesRegularly)
        {
            CalculateSlices();
        }
        Invoke("CalculateSlicesRegularly", SliceChecksSpeed);
    }


    public List<double> SlicesSizesInDoubles()
    {
        return (useFloodFill ?
            floodFillSlicesSizes : SlicesSizesInDoublesByPolygonCalculations());
    }

    public List<double> SlicesSizesInDoublesByPolygonCalculations()
    {
        List<double> slicesSizesInDoubles = new List<double>();

        /*foreach (Destruction2D slicer in Destruction2D.GetList())
        {
            //Polygon2D poly = slicer.GetPolygon().ToWorldSpace(slicer.transform);
            Polygon2D poly = slicer.GetBoundPolygon().ToWorldSpace(slicer.transform);
            double size = poly.GetArea(); //(int)poly.GetArea();
            //  Debug.Log("current size : " + currentSizeInt);
            slicesSizesInDoubles.Add(size);
        }*/
        List<Polygon2D> polygons = Polygon2DList.CreateFromPolygonCollider(sliceableBeingSliced.polygonCollider);
        for (int i = 0; i < polygons.Count; i++)
        {
            double size = polygons[i].GetArea();
            Debug.Log("size " + size);
            if (size > negligiblePolygonSliceSize)
            {
                slicesSizesInDoubles.Add(polygons[i].GetArea());
            }
        }

        return slicesSizesInDoubles;
    }

    private struct ShapePixel
    {
        public PixelState state;
        public int x;
        public int y;
        public ShapePixel(PixelState state, int x, int y)
        {                                  
            this.state = state;
            this.x = x;
            this.y = y;
        }
    }

    private static ShapePixel[] holeShape;

    private void InitialiseHoleShape()
    {
        int radius = halfOfEraseBrushSize;
        int expandedRadius = radius + outlineThickness;
        Vector2 centre = new Vector2(0, 0);
        List<ShapePixel> pixelStates = new List<ShapePixel>();

        for (int ix = - expandedRadius; ix < expandedRadius; ix++)
        {
            for (int iy = - expandedRadius; iy < expandedRadius; iy++)
            {
                float distance = Vector2.Distance(new Vector2(ix, iy), centre);
                if (distance <= expandedRadius)
                {
                    if (distance > radius)
                    {
                        pixelStates.Add(new ShapePixel( PixelState.OPAQUE_TOUCHED,ix,iy));
                    }
                    else
                    {
                        pixelStates.Add(new ShapePixel(PixelState.TRANSPARENT, ix, iy));
                    }
                }
            }
        }

        holeShape = pixelStates.ToArray();
    }

    #region Flood Fill:
    [Header("Flood Fill")]

    [SerializeField] private int floodFillScale = 8;
    [SerializeField] private bool _debugFloodFillOutput = false;
    private bool DebugFloodFillOutput
    {
        get { return (_debugFloodFillOutput && (SystemInfo.deviceType != DeviceType.Handheld)); }
    }
    public Texture2D floodFillOutputTexture;
    [SerializeField] private double negligibleFloodFillSliceSize = 4;

    private byte[,] floodFillMap;
    public List<double> floodFillSlicesSizes;
    private const byte UNMARKED_SOLID = 1;
    private const byte TRANSPARENT = 0;
    const double FLOOD_FILL_PIXEL_SIZE = 1;

    private List<FillUnit> fills = new List<FillUnit>();
    private List<FillUnit> fillsNext = new List<FillUnit>();


    public void CalculateSlicesByFloodFill()
    {
        UpdateFloodFillMap();
        //StartCoroutine( CalculateFill());
        CalculateFill();
        for (int i = 0; i < floodFillSlicesSizes.Count;)
        {
            if (floodFillSlicesSizes[i] < negligibleFloodFillSliceSize)
            {
                Debug.Log("Removed slice of size " + floodFillSlicesSizes[i]);
                floodFillSlicesSizes.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        slicesCount = floodFillSlicesSizes.Count;

    }

    private void GenerateFloodFillMap()
    {

        Debug_InitTexture();

        if (floodFillMap == null)
        {
            floodFillMap = new byte[PIXEL_STATES_MAP_SIZE / floodFillScale, PIXEL_STATES_MAP_SIZE / floodFillScale];
        }

        int currentMapWidth = currentPixelMap.pixelStates.GetLength(0);
        int currentMapHeight = currentPixelMap.pixelStates.GetLength(1);
        int width = currentMapWidth / floodFillScale;
        int height = currentMapHeight / floodFillScale;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                floodFillMap[x, y] =
                     (pixelsStatesMap[x * floodFillScale, y * floodFillScale] == PixelState.TRANSPARENT 
                      ? TRANSPARENT : UNMARKED_SOLID);
                // Color colour = value == TRANSPARENT ? Color.black : Color.white;
                //outputTexture2D.SetPixel(px, py, colour);
            }
        }
        // outputTexture2D.Apply();
    }

    private void UpdateFloodFillMap()
    {
        Debug_InitTexture();

        int currentMapWidth = currentPixelMap.pixelStates.GetLength(0);
        int currentMapHeight = currentPixelMap.pixelStates.GetLength(1);
        int width = currentMapWidth / floodFillScale;
        int height = currentMapHeight / floodFillScale;

        for (int x = 0; x < width; x ++)
        {
            for (int y = 0; y < height; y ++)
            {
                //if(floodFillMap[x, y] != TRANSPARENT)
                {
                    floodFillMap[x, y] =
                        (pixelsStatesMap[x * floodFillScale, y * floodFillScale] == PixelState.TRANSPARENT 
                        ? TRANSPARENT : UNMARKED_SOLID);
                }
                // Color colour = value == TRANSPARENT ? Color.black : Color.white;
                //outputTexture2D.SetPixel(px, py, colour);
            }
        }
        // outputTexture2D.Apply();
    }

    private void Debug_InitTexture()
    {
        if (DebugFloodFillOutput)
        {
            if (floodFillOutputTexture == null)
            {
                floodFillOutputTexture = new Texture2D
                    (PIXEL_STATES_MAP_SIZE / floodFillScale, PIXEL_STATES_MAP_SIZE / floodFillScale, TextureFormat.RGBA32, false);
            }
            else
            {
                Color32 resetColor = new Color32(0, 0, 0, 0);
                Color32[] resetColorArray = floodFillOutputTexture.GetPixels32();
                for (int i = 0; i < resetColorArray.Length; i++)
                {
                    resetColorArray[i] = resetColor;
                }
                floodFillOutputTexture.SetPixels32(resetColorArray);
            }
        }
    }

    public struct FillUnit
    {
        public byte x;
        public byte y;
        public byte id;

        public FillUnit(byte px, byte py, byte pId)
        {
            x = px;
            y = py;
            id = pId;
        }
    }

    private void CalculateFill()
    {
        int width = currentPixelMap.pixelStates.GetLength(0) / floodFillScale;
        int height = currentPixelMap.pixelStates.GetLength(1) / floodFillScale;
       // Debug.Log("width" + width);
       // Debug.Log("height" + height);
       // int count = 0;
        //int loopCount = 0;
        byte currentID = UNMARKED_SOLID;
        int currentSliceSizeIndex = 0;

        fills.Clear();
        fillsNext.Clear();
        floodFillSlicesSizes.Clear();

        Color currentColour = new Color();
        bool shouldDebugFloodFillOutput = DebugFloodFillOutput;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (floodFillMap[x, y] == UNMARKED_SOLID)
                {
                    currentID += 1;
                    currentSliceSizeIndex = currentID - 2;
                    floodFillSlicesSizes.Add(0);
                    floodFillMap[x, y] = currentID;
                    fills.Add(new FillUnit((byte)x, (byte)y, currentID));
                    currentColour = new Color
                        (UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                }

                while (fills.Count > 0)
                {
                   // yield return new WaitForSeconds(0.004f);
                    //loopCount += 1;
                    foreach (FillUnit fill in fills)
                    {
                       // yield return new WaitForSeconds(0.002f);

                        byte fx = fill.x;
                        byte fy = fill.y;

                        if (shouldDebugFloodFillOutput)
                        {
                            floodFillOutputTexture.SetPixel(fx, fy, currentColour);
                           // floodFillOutputTexture.Apply();
                        }

                        //floodFillMap[fx, fy] = currentID;
                        // Debug.Log("currentID:" + currentID);
                        floodFillSlicesSizes[currentSliceSizeIndex] += FLOOD_FILL_PIXEL_SIZE;//TODO: bad writing

                        //count++;

                        /* if (fy <= 0 || fx <= 0 || fx >= width - 1 || fy >= height - 1)//TODO: find out what this is all about
                         {
                             floodFillMap[fx, fy] = TRANSPARENT;
                             continue;
                         }*/

                        int xStart = (fx == 0 ? 0 : -1);
                        int yStart = (fy == 0 ? 0 : -1);
                        int xEnd = (fx == width -1 ? 0 : 1);
                        int yEnd = (fy == height - 1 ? 0 : 1);
                        for (int ix = xStart; ix <= xEnd; ix++)
                        {
                            for (int iy = yStart; iy <= yEnd; iy++)
                            {
                                int nx = fx + ix;
                                int ny = fy + iy;
                               /* if (nx<0||ny<0|| nx >= width|| ny >= height)
                                {
                                    continue;
                                }*/
                                if (floodFillMap[nx, ny] == UNMARKED_SOLID)
                                {
                                    floodFillMap[nx, ny] = currentID;
                                    fillsNext.Add(new FillUnit((byte)nx, (byte)ny, currentID));
                                }
                            }
                        }
                    }

                    // Debug.Log("fills" + fills.Count);
                    // Debug.Log("fillsNext" + fillsNext.Count);

                    List<FillUnit> swap = fills;
                    swap.Clear();
                    fills = fillsNext;
                    fillsNext = swap;
                }
            }
        }
        
        // For visual represantion / Only Testing Purposes
        if (DebugFloodFillOutput)
        {
            floodFillOutputTexture.Apply();
        }

        //textInput.text = width + "x" + height + " : Count: " + count + " | Loop Count: " + loopCount + " | Pieces: " + currentID;
    }

    #endregion
}


