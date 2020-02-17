using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundFeedback : MonoBehaviour
{
    [SerializeField]
    private SoundEffectNames soundToPlayOnStart;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySoundEffect(soundToPlayOnStart);
    }


}
