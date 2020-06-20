using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    [SerializeField]private Sprite[] stars;
    [SerializeField]private Sprite neutralButtonSprite;
    [SerializeField]private Sprite firstTryButtonSprite;

    private static SpriteHolder instance;

    private void Awake()
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

    public static Sprite FirstTryButtonSprite
    {
        get
        {
            return instance.firstTryButtonSprite;
        }
    }
    public static Sprite NeutralButtonSprite
    {
        get
        {
            return instance.neutralButtonSprite;
        }
    }

    public static Sprite GetStarsSprite(int numberOfStars)
    {
        if(instance== null)
        {
            Debug.LogError("instance is null!");
            return null;
        }
        if (instance.stars == null)
        {
            Debug.LogError("instance.stars is null!");
            return null;

        }
        if (numberOfStars < 0 || numberOfStars > instance.stars.Length - 1)
        {
            Debug.LogError("Recieved an ilegal number of stars!");
            return null;

        }
        return instance.stars[numberOfStars];
    }

}
