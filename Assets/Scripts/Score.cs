using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int initialScore = 0;
    public static int score = 0;
    //public int regularScoreToAdd = 10;

    public Text scoreText;
    [SerializeField]
    private Slider scoreSlider;
    public GameObject[] floatingTextPrefubs;
    [SerializeField]
    public GameObject[] negativeFeedbackPrefubs;
    GameObject floatingText;
    [SerializeField]
    private UIStar starPrefab;
    private UIStar[] stars;
    public static bool[] hasStarAt;

    private void OnEnable()
    {
        GameManager.OnNextLevel += NextLevel;
        GameManager.OnGameOver += GameOver;
        SlicesManager.OnScoreChange += ScoreChanged;
        SlicesManager.OnBadSlice += BadSlice;
    }

    private void OnDisable()
    {
        GameManager.OnNextLevel -= NextLevel;
        GameManager.OnGameOver -= GameOver;
        SlicesManager.OnScoreChange -= ScoreChanged;
        SlicesManager.OnBadSlice -= BadSlice;
    }

    void Awake()
    {
        score = 0;
    }

    private void Start()
    {
        score = 0;
        hasStarAt = new bool[SlicesManager.instance.currentLevel.StarRequirements.Length];
        CreateUIStarsBar();
    }

    private void CreateUIStarsBar()
    {
        RectTransform scoreSliderRectTransform = scoreSlider.GetComponent(typeof(RectTransform)) as RectTransform;
        stars = new UIStar[SlicesManager.instance.currentLevel.StarRequirements.Length];
        for (int i = 0; i < stars.Length; i++)
        {
            float xPosition =
                 ((float)SlicesManager.instance.currentLevel.StarRequirements[i] * scoreSliderRectTransform.rect.width)
                 - (scoreSliderRectTransform.rect.width / 2);
            UIStar newStar = Instantiate(starPrefab);
            newStar.transform.parent = scoreSlider.transform;
            newStar.rectTransform.localPosition = new Vector2(xPosition, 0);
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
        if(floatingTextPrefubs[index] && scoreLevel != ScoreData.ScoreLevel.Regular)
        {
            ShowFloatingText(scoreLevel, floatingTextPrefubs[index]);
        }

    }

    private void BadSlice(bool isTooManySlices)
    {
        int tooManySlicesIndex = 3;

        int index = isTooManySlices ? tooManySlicesIndex : Random.Range(0, negativeFeedbackPrefubs.Length - 1);

        if (negativeFeedbackPrefubs[index])
        {
            ShowFloatingText(ScoreData.ScoreLevel.Regular, negativeFeedbackPrefubs[index]);
        }
    }

    private void ShowFloatingText(ScoreData.ScoreLevel scoreLevel, GameObject floatingTextPrefub)
    {
        floatingText = Instantiate(floatingTextPrefub);
    }

    private void NextLevel()
    {
        int newScore = score;// + regularScoreToAdd;

        //Destroy(floatingText);

        SetScore(newScore);
    }

    private void GameOver()
    {
        Highscores.AddNewHighScore(GameManager.playerName, score);
        //SetScore(initialScore);
    }

    private void SetScore(int scoreToSet)
    {
        score = scoreToSet;
        scoreText.text = score.ToString();
        Level currentLevel = SlicesManager.instance.currentLevel;
        double ScoreDividedByMaxScore = ((double)score / currentLevel.MaximumScore());
        scoreSlider.value = (float)ScoreDividedByMaxScore;
        for (int i = 0; i < currentLevel.StarRequirements.Length; i++)
        {
            if(ScoreDividedByMaxScore > currentLevel.StarRequirements[i] && !hasStarAt[i])
            {
                hasStarAt[i] = true;
                stars[i].FillStar();
            }
        }

    }
}
