using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelMapping
{
    public class PixelMapper : MonoBehaviour
    {
       // public static PixelMapper instance;
        private static bool initialised = false;
        public PixelMap[] pixelMaps;
        public static PixelMap[] staticPixelMaps;
        private void Awake()
        {
            if (!initialised)
            {
                List<string> textureNames = new List<string>();
                for (int i = 1; i < pixelMaps.Length; i++)
                {
                    if (pixelMaps[i] != null)
                    {
                        string textureName = pixelMaps[i].GetTexturwName();
                        if (textureName != null)
                        {
                            for (int j = 0; j < textureNames.Count; j++)
                            {
                                if(string.Equals(textureName,textureNames[j]))
                                {
                                    Debug.LogWarning("Duplicate found.");
                                }
                            }
                            textureNames.Add(textureName);

                        }
                        if (pixelMaps[i].pixelStates == null)
                        {
                            pixelMaps[i].Initialise();
                            Debug.Log("Initialising pixel map!");
                        }
                    }

                }
                staticPixelMaps = pixelMaps;
                initialised = true;
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
            if (texture == null)
            {
                Debug.LogWarning("PixelMap texture is null.");
                return;
            }
            else if (pixelStates != null)
            {
                Debug.LogError("pixelStates had been initialised somehow !");
                return;
            }
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
                        (texture.GetPixel(x, y).a > 0.5f ? PixelState.OPAQUE_UNTOUCHED : PixelState.TRANSPARENT);
                }
            }
        }

        public string GetTexturwName()
        {      
            return ( texture == null ? null : texture.name);
        }
    }
}
public enum PixelState : byte
{
    UNKNOWN = 0, TRANSPARENT = 1, OPAQUE_UNTOUCHED = 2, OPAQUE_TOUCHED = 3,
}