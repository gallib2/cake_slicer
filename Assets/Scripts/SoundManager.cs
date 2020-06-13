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

[Serializable]
public class CookieScoreFeedback
{
   [Flags] public enum ScoreLevels { AWESOME = 8, GREAT = 4, OK = 2, BAD = 1};

    // public SoundEffectNames Name;
    public AudioClip[] Clips;
    public ScoreLevels scoreLevel;

    //public AudioSource Source;
    //public float Volume = 1;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private SoundEffect[] soundEffects;

    [SerializeField] private AudioSource cookiesVoice;
    [SerializeField] private CookieScoreFeedback[] cookieScoreFeedbacks;
    private CookieScoreFeedback[][] cookieScoreFeedbacksByScoreLevel;
    [SerializeField] private float cookiesPitchModifier = 0.05f;

    private void OnEnable()
    {
        GameManager.OnLose += OnGameEnd;
        GameManager.OnLevelInitialised += InitialiseLevel;
        SlicesManager.OnScoreChange += PlayCookieScoreFeedback;
    }

    private void OnDisable()
    {
        GameManager.OnLose -= OnGameEnd;
        GameManager.OnLevelInitialised -= InitialiseLevel;
        SlicesManager.OnScoreChange -= PlayCookieScoreFeedback;

    }

    private void InitialiseLevel()
    {
        musicSource.Play();
    }

    private void Start()
    {
        InitialiseCookiesVoice();
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

    private void InitialiseCookiesVoice()
    {
        cookieScoreFeedbacksByScoreLevel = new CookieScoreFeedback[4][];
        for (int i = 0; i < cookieScoreFeedbacksByScoreLevel.Length; i++)
        {
            CookieScoreFeedback.ScoreLevels correspondingLevel = 0;
            switch (i)
            {
                case 0:
                    correspondingLevel = CookieScoreFeedback.ScoreLevels.BAD; break;
                case 1:
                    correspondingLevel = CookieScoreFeedback.ScoreLevels.OK; break;
                case 2:
                    correspondingLevel = CookieScoreFeedback.ScoreLevels.GREAT; break;
                case 3:
                    correspondingLevel = CookieScoreFeedback.ScoreLevels.AWESOME; break;
            }
            List<CookieScoreFeedback> feedbacks = new List<CookieScoreFeedback>();
            for (int j = 0; j < cookieScoreFeedbacks.Length; j++)
            {
                if((cookieScoreFeedbacks[j].scoreLevel & correspondingLevel) != 0)
                {
                    feedbacks.Add(cookieScoreFeedbacks[j]);
                }
            }
            cookieScoreFeedbacksByScoreLevel[i] = feedbacks.ToArray();
        }
    }

    public void PlayCookieScoreFeedback(int bonuslessScore, int bonus, ScoreData.ScoreLevel scoreLevel)
    {
        CookieScoreFeedback[] feedbacksArray = null;
        switch (scoreLevel)
        {
            case ScoreData.ScoreLevel.Regular:
                feedbacksArray = cookieScoreFeedbacksByScoreLevel[0];break;
            case ScoreData.ScoreLevel.Nice:
                feedbacksArray = cookieScoreFeedbacksByScoreLevel[1]; break;
            case ScoreData.ScoreLevel.Great:
                feedbacksArray = cookieScoreFeedbacksByScoreLevel[2]; break;
            case ScoreData.ScoreLevel.Awesome:
                feedbacksArray = cookieScoreFeedbacksByScoreLevel[3]; break;
        }

        CookieScoreFeedback feedback = feedbacksArray[UnityEngine.Random.Range(0, feedbacksArray.Length)];
        cookiesVoice.clip = feedback.Clips[UnityEngine.Random.Range(0, feedback.Clips.Length)];
        cookiesVoice.pitch = GetRandomisedPitch(cookiesPitchModifier);
        cookiesVoice.Play();
    }

    private float GetRandomisedPitch(float randomiser)
    {
        return 1 + UnityEngine.Random.Range(-randomiser, randomiser);
    }
}
