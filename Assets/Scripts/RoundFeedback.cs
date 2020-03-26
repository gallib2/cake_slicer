using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundFeedback : MonoBehaviour
{
    [SerializeField]
    private SoundEffectNames soundToPlayOnStart;

    public SoundEffectNames SoundToPlayOnStart
    {
        get { return soundToPlayOnStart; }
        set { soundToPlayOnStart = value; }
    }

    //[SerializeField]
    //private SoundManager soundManager;


    // Start is called before the first frame update
    void Start()
    {
        //soundManager = gameObject.AddComponent<SoundManager>();

        //soundManager.PlaySoundEffect(soundToPlayOnStart);
    }


}
