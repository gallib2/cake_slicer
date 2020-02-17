using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum SoundEffectNames
{
   NEXT_LEVEL, LOSE, YAY_PositiveFB, YAY2_PositiveFB, Amaizing_PositiveFB, Delicious_PositiveFB, WOW_PositiveFB, Tritone_Horn_Fail, Audiance_Reaction_Fail
}
[Serializable]
public class SoundEffect
{
    public SoundEffectNames Name;
    public AudioClip[] Clips;
    public AudioSource Source;
    public float Volume = 1;
}
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private SoundEffect[] soundEffects;
    static public SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            GameManager.OnLose += OnGameEnd;
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnGameEnd()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffect(SoundEffectNames soundEffectName)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].Name == soundEffectName)
            {
                soundEffects[i].Source.clip = soundEffects[i].Clips[UnityEngine.Random.Range(0, soundEffects[i].Clips.Length)];
                soundEffects[i].Source.volume = soundEffects[i].Volume;
                soundEffects[i].Source.Play();
                return;
            }
           
        }
        Debug.Log("Sound effect not found!");
    }
}
