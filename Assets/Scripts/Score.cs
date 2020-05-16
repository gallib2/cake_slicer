using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Score : MonoBehaviour
{
    //public int initialScore = 0;
    public static int score = 0;
    

    public Text scoreText;
    [SerializeField]
    private Image scoreSliderFill;
    public GameObject[] floatingTextPrefubs;
    [SerializeField]
    public GameObject[] negativeFeedbackPrefubs;
    GameObject floatingText;
    [SerializeField]
    private ScoreFeedback scoreFeedback;
    [SerializeField]
    private ScoreFeedbackSprite[] scoreFeedbackSprites;
    [SerializeField]
    private UIStar UIStarPrefab;
    private UIStar[] UIStars;
    [SerializeField] private float StarYOffset = 54f;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private BoxCollider2D scoreFeedbacksBorders;


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
        CurrentStars = 0;
        CreateUIStarsBar();
        SetScore(0);
    }

    private void CreateUIStarsBar()
    {
        if (LevelsManager.CurrentLevel == null)
        {
            return;
        }
        if (UIStars != null)
        {
            //TODO: this part can be avoided by reusing stars that had been created, (We're better off not creating and destroying objects willy-nilly )
            for (int i = 0; i < UIStars.Length; i++)
            {
                if(UIStars[i] != null)
                {
                    GameObject.Destroy(UIStars[i].gameObject);
                }
            }
        }

        RectTransform scoreSliderRectTransform = scoreSliderFill.GetComponent(typeof(RectTransform)) as RectTransform;
        UIStars = new UIStar[LevelsManager.CurrentLevel.StarRequirements.Length];
        //stars = new UIStar[3];
        for (int i = 0; i < UIStars.Length; i++)
        {
            float xPosition =
                 ((float)LevelsManager.CurrentLevel.StarRequirements[i] * scoreSliderRectTransform.rect.width)
                 - (scoreSliderRectTransform.rect.width / 2);
            UIStar newStar = Instantiate(UIStarPrefab);
            newStar.transform.SetParent(scoreSliderFill.transform);
            //newStar.transform.parent = scoreSlider.transform;
            newStar.rectTransform.localPosition = new Vector2(xPosition, 0+StarYOffset);
            newStar.rectTransform.localScale = new Vector3(1,1,1);
            newStar.gameObject.SetActive(true);// the objects are not active when instantiated for some reason...
            UIStars[i] = newStar;
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

    private void ScoreChanged(int bonuslessScoreToAdd, int bonus, ScoreData.ScoreLevel scoreLevel)
    {
        int newScore = score + bonuslessScoreToAdd + bonus;
        int index = UnityEngine.Random.Range(0, floatingTextPrefubs.Length);

        SetScore(newScore);
        if(scoreLevel == ScoreData.ScoreLevel.Regular)
        {
            BadSlice(false);
        }
        else if(floatingTextPrefubs[index])
        {
            ShowFloatingText(scoreLevel, floatingTextPrefubs[index]);
        }
        CreateScoreFeedback(bonuslessScoreToAdd, bonus, scoreLevel);

    }

    private void BadSlice(bool isTooManySlices)
    {
        int tooManySlicesIndex = 1;

        int index = isTooManySlices ? tooManySlicesIndex : UnityEngine.Random.Range(0, negativeFeedbackPrefubs.Length - 1);

        if (negativeFeedbackPrefubs[index])
        {
            ShowFloatingText(ScoreData.ScoreLevel.Regular, negativeFeedbackPrefubs[index]);
        }
    }

    private void ShowFloatingText(ScoreData.ScoreLevel scoreLevel, GameObject floatingTextPrefub)
    {
        return;
        floatingText = Instantiate(floatingTextPrefub);
        RoundFeedback feedbackGameObject = floatingText.GetComponent<RoundFeedback>();
        soundManager.PlaySoundEffect(feedbackGameObject.SoundToPlayOnStart);
    }

    private void CreateScoreFeedback(int bonuslessScore, int bonus, ScoreData.ScoreLevel scoreLevel)
    {

        Vector3 feedbackPosition = Camera.main.ScreenToWorldPoint(InputManager.GetTouchPosition());
        feedbackPosition.x = Mathf.Clamp(feedbackPosition.x, scoreFeedbacksBorders.bounds.min.x, scoreFeedbacksBorders.bounds.max.x);
        feedbackPosition.y = Mathf.Clamp(feedbackPosition.y, scoreFeedbacksBorders.bounds.min.y, scoreFeedbacksBorders.bounds.max.y);

        ScoreFeedback newScoreFeedback = Instantiate
            (scoreFeedback, feedbackPosition, Quaternion.identity);
        //Making sure Z is zero so that the camera actually desplays the damn thing
        Sprite feedbackSprite=null;
        for (int i = 0; i < scoreFeedbackSprites.Length; i++)
        {
            if(scoreLevel== scoreFeedbackSprites[i].scoreLevel)
            {
                feedbackSprite = scoreFeedbackSprites[i].sprite;
            }
        }
        newScoreFeedback.transform.position = new Vector3(newScoreFeedback.transform.position.x, newScoreFeedback.transform.position.y, 0);
        newScoreFeedback.ScoreFeedbackConstructor(bonuslessScore,bonus, feedbackSprite);
    }

    //private void GameOver(){
    //    Highscores.AddNewHighScore(GameManager.playerName, score);
    //    //SetScore(initialScore);}

    private void SetScore(int scoreToSet)
    {
        if (LevelsManager.CurrentLevel == null)
        {
            return;
        }
        score = scoreToSet;
        scoreText.text = score.ToString();
        Level currentLevel = LevelsManager.CurrentLevel;
        int levelMaxScore = currentLevel.MaximumScoreWithouPowerUps();
        double ScoreDividedByMaxScore = ((double)score / levelMaxScore);
        scoreSliderFill.fillAmount = (float)ScoreDividedByMaxScore;
        for (int i = 0; i < currentLevel.StarRequirements.Length; i++)
        {
            bool isAlreadyHasStar = (CurrentStars >= i + 1);//CurrentStars == i + 1;
            bool shouldGetStar = (ScoreDividedByMaxScore >= currentLevel.StarRequirements[i]);
            if (shouldGetStar && !isAlreadyHasStar)
            {
                CurrentStars++;
                UIStars[i].FillStar();
            }
        }
        if (score > levelMaxScore)
        {
            Debug.LogError("Score is somehow larger than the level's maximum score!");
        }
    }
}

[Serializable]
public class ScoreFeedbackSprite
{
    public ScoreData.ScoreLevel scoreLevel;
    public Sprite sprite;
}