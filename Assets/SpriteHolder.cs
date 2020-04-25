using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{

    public Sprite[] stars;
    public static SpriteHolder instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogWarning("Tried to create more than one singleton");
        }
    }
    public static Sprite GetStarsSprite(int numberOfStars)
    {
        return instance.stars[numberOfStars];
    }

}
