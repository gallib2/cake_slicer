using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int initialScore = 0;
    public static int score = 0;

    public Text scoreText;
    [SerializeField]
    private Image scoreSliderFill;
    public GameObject[] floatingTextPrefubs;
    [SerializeField]
    public GameObject[] negativeFeedbackPrefubs;
    GameObject floatingText;
    [SerializeField]
    private UIStar starPrefab;
    private UIStar[] stars;
    [SerializeField]
    private float StarYOffset = 54f;
    [SerializeField]
    private SoundManager soundManager;

    public int CurrentStars { get; set; }


    private void OnEnable()
    {
        //GameManager.OnLose += GameOver;
        //GameManager.OnWin += GameOver;
        SlicesManager.OnScoreChange += ScoreChanged;
        SlicesManager.OnBadSlice += BadSlice;
        GameManager.OnLevelInitialised += InitialiseLevel;
    }

    private void OnDisable()
    {
        //GameManager.OnLose -= GameOver;
        //GameManager.OnWin -= GameOver;
        SlicesManager.OnScoreChange -= ScoreChanged;
        SlicesManager.OnBadSlice -= BadSlice;
        GameManager.OnLevelInitialised -= InitialiseLevel;
    }

    void Awake()
    {
        //Initialise();
    }

    private void InitialiseLevel()
    {
        score = 0;
        CreateUIStarsBar();
        SetScore(0);
    }

    private void CreateUIStarsBar()
    {
        RectTransform scoreSliderRectTransform = scoreSliderFill.GetComponent(typeof(RectTransform)) as RectTransform;
        stars = new UIStar[LevelsManager.CurrentLevel.StarRequirements.Length];
        //stars = new UIStar[3];
        for (int i = 0; i < stars.Length; i++)
        {
            float xPosition =
                 ((float)LevelsManager.CurrentLevel.StarRequirements[i] * scoreSliderRectTransform.rect.width)
                 - (scoreSliderRectTransform.rect.width / 2);
            UIStar newStar = Instantiate(starPrefab);
            newStar.transform.SetParent(scoreSliderFill.transform);
            //newStar.transform.parent = scoreSlider.transform;
            newStar.rectTransform.localPosition = new Vector2(xPosition, 0+StarYOffset);
            newStar.rectTransform.localScale = new Vector3(1,1,1);
            stars[i] = newStar;
        }
    }

    private void Update()
    {
        // if the feedback animation is over destroy
        if(floatingText && Mathf.Approximately(floatingText.transform.localScale.x, 0))
        {
            Destroy(floatingText);
        }
    }

    private void ScoreChanged(int scoreToAdd,ScoreData.ScoreLevel scoreLevel)
    {
        int newScore = score + scoreToAdd;
        int index = Random.Range(0, floatingTextPrefubs.Length);

        SetScore(newScore);
        if(scoreLevel == ScoreData.ScoreLevel.Regular)
        {
            BadSlice(false);
        }
        else if(floatingTextPrefubs[index])
        {
            ShowFloatingText(scoreLevel, floatingTextPrefubs[index]);
        }

    }

    private void BadSlice(bool isTooManySlices)
    {
        int tooManySlicesIndex = 1;

        int index = isTooManySlices ? tooManySlicesIndex : Random.Range(0, negativeFeedbackPrefubs.Length - 1);

        if (negativeFeedbackPrefubs[index])
        {
            ShowFloatingText(ScoreData.ScoreLevel.Regular, negativeFeedbackPrefubs[index]);
        }
    }

    private void ShowFloatingText(ScoreData.ScoreLevel scoreLevel, GameObject floatingTextPrefub)
    {
        floatingText = Instantiate(floatingTextPrefub);
        RoundFeedback feedbackGameObject = floatingText.GetComponent<RoundFeedback>();
        soundManager.PlaySoundEffect(feedbackGameObject.SoundToPlayOnStart);
    }

    //private void GameOver()
    //{
    //    Highscores.AddNewHighScore(GameManager.playerName, score);
    //    //SetScore(initialScore);
    //}

    private void SetScore(int scoreToSet)
    {
        score = scoreToSet;
        scoreText.text = score.ToString();
        Level currentLevel = LevelsManager.CurrentLevel;
        double ScoreDividedByMaxScore = ((double)score / currentLevel.MaximumScore());
        scoreSliderFill.fillAmount = (float)ScoreDividedByMaxScore;
        for (int i = 0; i < currentLevel.StarRequirements.Length; i++)
        {
            bool isAlreadyHasStar = CurrentStars == i + 1;
            bool shouldGetStar = ScoreDividedByMaxScore > currentLevel.StarRequirements[i];
            if (shouldGetStar && !isAlreadyHasStar)
            {
                CurrentStars++;
                stars[i].FillStar();
            }
        }

    }
}
