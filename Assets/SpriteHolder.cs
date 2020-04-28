using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    [SerializeField]private Sprite[] stars;
    [SerializeField]private Sprite neutralStarsPanel;
    [SerializeField]private Sprite firstTryStarsPanel;

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

    public static Sprite FirstTryStarsPanel
    {
        get
        {
            return instance.firstTryStarsPanel;
        }
    }
    public static Sprite NeutralStarsPanel
    {
        get
        {
            return instance.neutralStarsPanel;
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
        if (numberOfStars<0||numberOfStars > instance.stars.Length - 1)
        {
            Debug.LogError("Recieved an ilegal number of stars!");
            return null;

        }
        return instance.stars[numberOfStars];
    }

}
