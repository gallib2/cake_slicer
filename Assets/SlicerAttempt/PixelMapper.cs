using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelMapping
{
    public class PixelMapper : MonoBehaviour
    {
        public static PixelMapper instance;
        public PixelMap[] pixelMaps;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
          
            for (int i = 1; i < pixelMaps.Length; i++)
            {
                if(pixelMaps[i] != null)
                {
                    if (pixelMaps[i].pixelStates == null)
                    {
                        pixelMaps[i].Initialise();
                        Debug.Log("Initialising pixel map!");
                    }
                }

            }
        }
    }

    [Serializable]
    public class PixelMap
    {
        [SerializeField] private Texture2D texture;
        [HideInInspector] public PixelState[,] pixelStates;
        public Color outlineColour1;
        public Color outlineColour2;

        public void Initialise()
        {
            outlineColour1.a = 1;
            outlineColour2.a = 1;

            int width = texture.width;
            int height = texture.height;
            pixelStates = new PixelState[width,height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y  < height; y++)
                {
                    pixelStates[x, y] =
                        (texture.GetPixel(x, y).a > 0.5f ? PixelState.OPAQUE : PixelState.TRANSPARENT);
                }
            }
        }

    }

    public enum PixelState:byte
    {
        TRANSPARENT=0, OPAQUE = 1,
    }
}
