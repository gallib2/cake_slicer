using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSlicer : MonoBehaviour
{
    private Vector2Int lastHitPixel;
    private static Vector2 lastMousePointInsideCake;
    private SpriteSliceable sliceableBeingSliced;
    private float boundsMinX;
    private float boundsMinY;
    private float normalisedMaxX;
    private float normalisedMaxY;
    private Texture2D currentTexture;
    private Texture2D dynamicTexture;
    private Texture2D lowerDynamicTexture;
    private Texture2D upperDynamicTexture;

    private int textureWidth;
    private int textureHeight;

    [SerializeField] private int eraseBrushSize = 7;
    [SerializeField] private int interpolationDistance;
    private int halfOfEraseBrushSize;
    private Color[] clearColours;
    private bool changedSinceLastCheck;
    [SerializeField] private float SliceChecksSpeed = 0.2f;
    [SerializeField] private bool shouldCheckSlicesRegularly;
    [SerializeField] private double negligibleSliceSize = 0.01;
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
        clearColours = new Color[eraseBrushSize * eraseBrushSize];
        for (int i = 0; i < clearColours.Length; i++)
        {
            clearColours[i] = Color.clear;
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

    public static Vector2 GetLastMousePosition()
    {
        return lastMousePointInsideCake;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lastHitPixel = Vector2Int.zero;
            CalculateSlices();
        }
    }

    public void SetNewSliceable(SpriteSliceable newSliceable )
    {
        //if (sliceableBeingSliced == null)
        slicesCount = 0;
        lastHitPixel = Vector2Int.zero;

        // Collider2D colliderAtPoint = Physics2D.OverlapPoint(mousePoint);
        // if (colliderAtPoint != null && colliderAtPoint.GetComponent<SpriteSliceable>() != null)
        {
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

            lowerDynamicTexture = new Texture2D(currentTexture.width, currentTexture.height/2);
            lowerDynamicTexture.filterMode = currentTexture.filterMode;
            lowerDynamicTexture.SetPixels(currentTexture.GetPixels(0,0, lowerDynamicTexture.width, lowerDynamicTexture.height));



            Sprite currentSprite = sliceableBeingSliced.spriteRenderer.sprite;
            Sprite newSprite = Sprite.Create
                (dynamicTexture, currentSprite.rect, new Vector2(0.5f, 0.5f), currentSprite.pixelsPerUnit);//, 1, SpriteMeshType.FullRect, currentSprite.border);
            sliceableBeingSliced.spriteRenderer.sprite = newSprite;
        }
    }

    private bool overlappedColliderThisFrame;
    private bool overlappedColliderPreviousFrame;

    private void FixedUpdate()
    {
        isSlicing = false;
        if (GameManager.GameIsPaused)
        {
            return;
        }

        if (!SlicesManager.allowToSlice)
        {
            //skipedFrame = false;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            overlappedColliderThisFrame = (Physics2D.OverlapPoint(mousePoint) != null);

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

            /*currentTexture.SetPixel(hitPixelX, hitPixelY, Color.clear);
            currentTexture.SetPixel(hitPixelX + 1, hitPixelY, Color.clear);
            currentTexture.SetPixel(hitPixelX- 1, hitPixelY, Color.clear);
            currentTexture.SetPixel(hitPixelX, hitPixelY + 1, Color.clear);
            currentTexture.SetPixel(hitPixelX, hitPixelY - 1, Color.clear);*/
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
                        if( MakeCircleHole(InterpolatedPoint.x, InterpolatedPoint.y))
                        {
                            textureChanged = true;
                        }
                        /* currentTexture.SetPixel(InterpolatedPoint.x, InterpolatedPoint.y, Color.clear);
  currentTexture.SetPixel(InterpolatedPoint.x + 1, InterpolatedPoint.y, Color.clear);
  currentTexture.SetPixel(InterpolatedPoint.x - 1, InterpolatedPoint.y, Color.clear);
  currentTexture.SetPixel(InterpolatedPoint.x, InterpolatedPoint.y + 1, Color.clear);
  currentTexture.SetPixel(InterpolatedPoint.x, InterpolatedPoint.y - 1, Color.clear);*/
                    }
                    //Vector2Int InterpolatedPoint = new Vector2Int((newHitPixel.x + lastHitPixel.x) / 2, (newHitPixel.y + lastHitPixel.y) / 2);
                }
            }

            if (textureChanged)
            {
                currentTexture.Apply();
            }
            lastHitPixel = newHitPixel;
            if (overlappedColliderThisFrame)
            {
                isSlicing = true;
            }

            if (overlappedColliderPreviousFrame)
            {
                lastMousePointInsideCake = mousePoint;
                if (!overlappedColliderThisFrame)
                {
                    Debug.Log("CalculateSlices");
                    CalculateSlices();
                }

            }

            overlappedColliderPreviousFrame = overlappedColliderThisFrame;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            floodFillTexture = currentTexture;
            FloodFillNumberOfSlices();
        }
    }

    private bool MakeSquraeHole(int x, int y)
    {
        if(textureWidth!= currentTexture.width|| textureHeight != currentTexture.height)
        {
            Debug.LogError("texture dimentions are incorrect!");
            textureWidth = currentTexture.width;
            textureHeight = currentTexture.height;
        }
        x = x - halfOfEraseBrushSize;
        y = y - halfOfEraseBrushSize;
        int holeWidth = eraseBrushSize;
        int holeHeight = eraseBrushSize;
        bool solidPixelFound = false;

        // Color[] coloursInTexture = currentTexture.GetPixels(x, y, eraseBrushSize, eraseBrushSize);//TODO: Optimise
        /*for (int i = 0; i < coloursInTexture.Length; i++)
        {
            if( coloursInTexture[i]!= Color.clear)
            {
                emptyPixelFound = true;
                 break;
            }
        }*/
        if (x < 0)
        {
            //Debug.Log("x:" + x);
            holeWidth += x;
            // Debug.Log("x:"+x);
            if (holeWidth <= 0)
            {
                return false;
            }
            else
            {
                x = 0;
            }
        }
        else
        {
            int textureWidthMinusHoleX = textureWidth - (x + holeWidth);
            if (textureWidthMinusHoleX < 0)
            {
                holeWidth += textureWidthMinusHoleX;
                if (holeWidth <= 0)
                {
                    return false;
                }
            }
        }

        if (y < 0)
        {
            //Debug.Log("x:" + x);
            holeHeight += y;
            // Debug.Log("x:"+x);
            if (holeHeight <= 0)
            {
                return false;
            }
            else
            {
                y = 0;
            }
        }
        else
        {
            int textureHeightMinusHoleY = textureHeight - (y + holeHeight);
            if (textureHeightMinusHoleY < 0)
            {
                holeHeight += textureHeightMinusHoleY;
                if (holeHeight <= 0)
                {
                    return false;
                }
            }
        }


       /* for (int ix = x; ix < holeWidth+x; ix++)//TODO: should this be before or after bounds checks?
        {
            for (int iy = y; iy < holeHeight+y; iy++)
            {
                if (currentTexture.GetPixel(ix, iy) != Color.clear)
                {
                    Debug.Log("emptyPixelFound:"+ currentTexture.GetPixel(ix, iy));
                    Debug.Log("x:" + ix+"y:" + iy);
                    Debug.Log("width:" + textureWidth + "height:" + textureHeight);

                    currentTexture.SetPixel(ix, iy, Color.magenta);
                    solidPixelFound = true;
                    return true;
                    break;
                }
            }
        }*/
       // if (solidPixelFound)
        {
            changedSinceLastCheck = true;
            //x = Mathf.Clamp()
            currentTexture.SetPixels
                (x, y, holeWidth, holeHeight, clearColours);

            return true;
        }
       // return solidPixelFound;

    }

    private bool MakeCircleHole(int x, int y)
    {
        if (textureWidth != currentTexture.width || textureHeight != currentTexture.height)
        {
            Debug.LogError("texture dimentions are incorrect!");
            textureWidth = currentTexture.width;
            textureHeight = currentTexture.height;
        }
       /* x = x - halfOfEraseBrushSize;
        y = y - halfOfEraseBrushSize;*/
       // int holeWidth = eraseBrushSize;
       // int holeHeight = eraseBrushSize;
       // bool solidPixelFound = false;

        // Color[] coloursInTexture = currentTexture.GetPixels(x, y, eraseBrushSize, eraseBrushSize);//TODO: Optimise
        /*for (int i = 0; i < coloursInTexture.Length; i++)
        {
            if( coloursInTexture[i]!= Color.clear)
            {
                emptyPixelFound = true;
                 break;
            }
        }*/

        /* for (int ix = x; ix < holeWidth+x; ix++)//TODO: should this be before or after bounds checks?
         {
             for (int iy = y; iy < holeHeight+y; iy++)
             {
                 if (currentTexture.GetPixel(ix, iy) != Color.clear)
                 {
                     Debug.Log("emptyPixelFound:"+ currentTexture.GetPixel(ix, iy));
                     Debug.Log("x:" + ix+"y:" + iy);
                     Debug.Log("width:" + textureWidth + "height:" + textureHeight);

                     currentTexture.SetPixel(ix, iy, Color.magenta);
                     solidPixelFound = true;
                     return true;
                     break;
                 }
             }
         }*/
        // if (solidPixelFound)
        bool changed = false;

        //x = Mathf.Clamp()
        // currentTexture.SetPixels (x, y, holeWidth, holeHeight, clearColours);
        int radius = halfOfEraseBrushSize;
        Vector2 centre = new Vector2(x, y);
        if (!(centre.x + radius < 0 || centre.x - radius > textureWidth ||
           centre.y + radius < 0 || centre.y - radius > textureHeight))
        {
            for (int ix = x - radius; ix < x + radius; ix++)
            {
                for (int iy = y - radius; iy < y + radius; iy++)
                {
                    if (ix == centre.x || iy == centre.y || Vector2.Distance(new Vector2(ix, iy), centre) <= radius)
                    {
                        if (ix > -1 && ix < textureWidth &&
                            iy > -1 && iy < textureHeight)
                        {
                            currentTexture.SetPixel(ix, iy, Color.clear);
                            changed = true;
                        }
                    }
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
            List<Polygon2D> polygons = Polygon2DList.CreateFromPolygonCollider(sliceableBeingSliced.GetNewPolygonCollider());
            if (polygons.Count > 1)//Optimisation..
            {
                //Debug.Log("polygons.Count :" + polygons.Count);
                /*for (int i = 0; i < polygons.Count;)//TODO: must be inefficient
                {
                    Debug.Log("Area:" + polygons[i].GetArea());
                    i++;
                }*/
                for (int i = 0; i < polygons.Count;)//TODO: must be inefficient
                {
                    if (polygons[i].GetArea() < negligibleSliceSize)
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
        changedSinceLastCheck = false;
        //Invoke("CalculateSlices", SliceChecksSpeed);
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
            if (size > negligibleSliceSize)
            {
                slicesSizesInDoubles.Add(polygons[i].GetArea());
            }
        }

        return slicesSizesInDoubles;
    }

    #region Flood Fill:
    Texture2D floodFillTexture;
    
    private void FloodFillNumberOfSlices(/*Texture2D texture*/)
    {
        /* Debug.Log(floodFillTexture.GetPixel(32, 32));
         floodFillTexture.SetPixel(32,32,  Color.black);
         Debug.Log(floodFillTexture.GetPixel(32, 32));*/
        //return;
        // byte[,] signatures = new byte[floodFillTexture.width, floodFillTexture.height];
        Color targetColour = Color.black; ;//= new Color(UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255));
        int random = UnityEngine.Random.Range(0, 5);
        switch (random)
        {
            case 0:
                targetColour = Color.black; break;
            case 1:
                targetColour = Color.white; break;
            case 2:
                targetColour = Color.green; break;
            case 3:
                targetColour = Color.red; break;
            case 4:
                targetColour = Color.blue; break;
        }
        List<Color> previousColours = new List<Color>();
        previousColours.Add(targetColour);
        floodFillFuncs.Clear();

        // byte sig = 1;
        for (int x = 0; x < floodFillTexture.width; x++)
        {
            for (int y = 0; y < floodFillTexture.height; y++)
            {
                Color thisColour = floodFillTexture.GetPixel(x, y);

                if (thisColour != Color.clear && thisColour != targetColour)
                {
                    bool shouldContinue = false;
                    for (int i = 0; i < previousColours.Count; i++)
                    {
                        if(thisColour == previousColours[i])
                        {
                            shouldContinue = true;
                            break;
                        }
                    }
                    if (shouldContinue)
                    {
                        continue;
                    }
                    FloodFill((byte)x, (byte)y, targetColour);
                    int newCalls = 0;
                    while (floodFillFuncs.Count > 0)
                    {
                        int oldCalls = floodFillFuncs.Count;
                        for (int i = 0; i < oldCalls; i++)
                        {
                            floodFillFuncs[i]();
                        }
                        newCalls = floodFillFuncs.Count - oldCalls;
                       if ( oldCalls  > 0)
                       {
                            floodFillFuncs.RemoveRange(0, oldCalls);
                       }
                       else
                       {
                            floodFillFuncs.Clear();
                           break;
                       }

                    }
                   // return;
                    //break;
                    //StartCoroutine( FloodFillCR(x, y, targetColour));
                    //return;
                  //  previousColours.Add(targetColour);
                   //targetColour = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
                 //  previousColours.Add(targetColour);

                    /* if (signatures[x, y] == 0)
                     {
                         int ix = x;
                         int iy = y;

                         if(texture.GetPixel(ix+1, iy) != Color.clear)
                         {
                             signatures[ix + 1, iy] = sig;
                             ix++;

                         }

                         while (texture.GetPixel(ix+1, iy) != Color.clear)
                         {
                             signatures[ix+1, iy] = sig;
                             ix++;

                         }
                         while
                     }*/
                }
            }
        }

        floodFillTexture.Apply();
    }

    /*IEnumerator FloodFillCR(int x, int y, Color target)
    {
        Color thisColour = floodFillTexture.GetPixel(x, y);
        if (thisColour != target && thisColour != Color.clear)
        {
            //Debug.Log(thisColour.ToString() + "," + target.ToString());
            floodFillTexture.SetPixel(x, y, target);
            floodFillTexture.Apply();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(FloodFillCR(x + 1, y, target));
            yield return new WaitForSeconds(0.1f);

            StartCoroutine(FloodFillCR(x - 1, y, target));
            yield return new WaitForSeconds(0.1f);

            StartCoroutine(FloodFillCR(x, y + 1, target));
            yield return new WaitForSeconds(0.1f);

            StartCoroutine(FloodFillCR(x, y - 1, target));
            yield return new WaitForSeconds(0.1f);


        }
    }*/
    //public delegate void FloodFillDelegate(byte x, byte y, Color target);
    public List<Action> floodFillFuncs = new List<Action>();

    private void FloodFill(byte x, byte y,  Color target)
    {
       // Color thisColour = floodFillTexture.GetPixel(x, y);
       // if (thisColour!= target && thisColour != Color.clear)
        {
            //Debug.Log(thisColour.ToString() + "," + target.ToString());
            //floodFillTexture.SetPixel(x, y, target);

            Color neighbourColour;
            neighbourColour = floodFillTexture.GetPixel(x + 1, y);
            if(neighbourColour != target && neighbourColour != Color.clear)
            {
                //floodFillFuncs.Add(new FloodFillDelegate (FloodFill((byte)(x + 1), y, ref target)));
                floodFillTexture.SetPixel(x+1, y, target);
                floodFillFuncs.Add(delegate () { FloodFill((byte)(x + 1), y,  target); });
            }
            neighbourColour = floodFillTexture.GetPixel(x - 1, y);
            if (neighbourColour != target && neighbourColour != Color.clear)
            {
                floodFillTexture.SetPixel(x - 1, y, target);

                floodFillFuncs.Add(delegate () { FloodFill((byte)(x - 1), y, target); });
            }
            neighbourColour = floodFillTexture.GetPixel(x, y+1);
            if (neighbourColour != target && neighbourColour != Color.clear)
            {
                floodFillTexture.SetPixel(x , y + 1, target);

                floodFillFuncs.Add(delegate () { FloodFill(x, (byte)(y + 1), target); });
            }
            neighbourColour = floodFillTexture.GetPixel(x , y - 1);
             if (neighbourColour != target && neighbourColour != Color.clear)
             {
                floodFillTexture.SetPixel(x, y - 1, target);

                floodFillFuncs.Add(delegate () { FloodFill(x, (byte)(y - 1), target); });

             }

        }        
    }

    #endregion
}


