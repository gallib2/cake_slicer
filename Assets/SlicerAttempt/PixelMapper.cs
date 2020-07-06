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
        private static PixelMap[] staticPixelMaps;

        public static PixelMap GetPixelMap(int index)
        {
            if(staticPixelMaps==null||index < 1 || index>= staticPixelMaps.Length )
            {
                return null;
            }
            else
            {
                return staticPixelMaps[index];
            }
        }

        private void Awake()
        {
            if (!initialised)
            {
                List<string> textureNames = new List<string>();
                for (int i = 1; i < pixelMaps.Length; i++)
                {
                    if (pixelMaps[i] != null)
                    {
                        string textureName = pixelMaps[i].GetTextureName();
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

        private const float ALPHA_THRESHOLD = 0.15f;

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
                        (texture.GetPixel(x, y).a > ALPHA_THRESHOLD ? PixelState.OPAQUE_UNTOUCHED : PixelState.TRANSPARENT);
                }
            }
        }

        public string GetTextureName()
        {      
            return ( texture == null ? null : texture.name);
        }


        public static PixelMap GetEmergencyPixelMap(Texture2D texture)
        {
            Debug.LogWarning("Generating emergency pixel map.");
            PixelMap pixelMap = new PixelMap(texture);
            pixelMap.Initialise();
            return pixelMap;
        }
        private PixelMap (Texture2D texture)
        {
            this.texture = texture;
            outlineColour1 = Color.black;
            outlineColour2 = Color.white;
        }
    }
}
public enum PixelState : byte
{
    UNKNOWN = 0, TRANSPARENT = 1, OPAQUE_UNTOUCHED = 2, OPAQUE_TOUCHED = 3,
}