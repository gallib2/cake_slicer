using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int initialScore = 0;
    public static int score = 0;
    //public int regularScoreToAdd = 10;

    AudioSource audioSource;

    public Text scoreText;
    [SerializeField]
    private Slider scoreSlider;
    public GameObject[] floatingTextPrefubs;
    public AudioClip[] positiveSound;
    public GameObject[] negativeFeedbackPrefubs;
    public AudioClip[] negativeSound;
    GameObject floatingText;
    [SerializeField]
    private UIStar starPrefab;
    private UIStar[] stars;
    private bool[] hasStar;

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
        audioSource = GetComponent<AudioSource>();
        RectTransform scoreSliderRectTransform = scoreSlider.GetComponent(typeof(RectTransform)) as RectTransform;
        stars = new UIStar[SlicesManager.instance.currentLevel.StarRequirements.Length];
        hasStar = new bool[stars.Length];
        for (int i = 0; i < stars.Length; i++)
        {
            hasStar[i] = false;
           float xPosition=
                ((float)SlicesManager.instance.currentLevel.StarRequirements[i]* scoreSliderRectTransform.rect.width)
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
       // Debug.Log("Score: " + score);
    }

    private void ScoreChanged(int scoreToAdd,ScoreData.ScoreLevel scoreLevel)
    {
        int newScore = score + scoreToAdd;
        int index = Random.Range(0, floatingTextPrefubs.Length);

        SetScore(newScore);
        if(floatingTextPrefubs[index] && scoreLevel != ScoreData.ScoreLevel.Regular)
        {
            ShowFloatingText(scoreLevel, floatingTextPrefubs[index]);
            audioSource.PlayOneShot(positiveSound[index]);
        }

    }

    private void BadSlice(bool isTooManySlices)
    {
        int tooManySlicesIndex = 3;

        int index = isTooManySlices ? tooManySlicesIndex : Random.Range(0, negativeFeedbackPrefubs.Length - 1);

        if (negativeFeedbackPrefubs[index])
        {
            ShowFloatingText(ScoreData.ScoreLevel.Regular, negativeFeedbackPrefubs[index]);
            audioSource.PlayOneShot(negativeSound[index]);
        }
    }

    private void ShowFloatingText(ScoreData.ScoreLevel scoreLevel, GameObject floatingTextPrefub)
    {
        floatingText = Instantiate(floatingTextPrefub);
        //TextMesh floatingTextMesh = floatingText.GetComponent<TextMesh>();
        //floatingTextMesh.text = scoreLevel.ToString();
        //floatingTextMesh.color
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
            if(ScoreDividedByMaxScore > currentLevel.StarRequirements[i] && !hasStar[i])
            {
                // Debug.Log("I have star number " + i);
                hasStar[i] = true;
                stars[i].FillStar();
            }
        }

    }
}
