using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlicesManager : MonoBehaviour
{
    public delegate void ScoreChange(int score,ScoreData.ScoreLevel scoreLevel);
    public delegate void BadSliceHandler(bool isTooManySlices);

    public static event ScoreChange OnScoreChange;
    public static event BadSliceHandler OnBadSlice;



    public Animator timerAnimation;
    public Slider sliderTimer;
    [SerializeField]
    private Image sliderImage;
    //public int currentLevel;
    bool test = true;

    [SerializeField]
    private float initialLevelTime = 20;
    private float timeLeft;

    List<double> slicesSizeList;
    public int currentCakeIndex = 0;
    
    //private LevelManager levelManager;
    [SerializeField]
    public Level currentLevel;
    public static event Action OnGoalChange;
    public static int goal;
    [SerializeField]
    private float criticalTime=5f;
    private int minmumSize;
    public ParticleSystem particlesEndLevel;

    TimerHelper timer;
    float timerRequired = 1f;
    bool toStopTimer = false;

    private double originalSize = 0;
    private int sliced = 0;

    //public GameObject cake;
    //public GameObject[] cakes;
    public GameObject gameOverScreenPrefub;

    [SerializeField]
    private GameObject sliceableObjects;

    public int slicesCount = 0;
    [SerializeField]
    private Text cakesLeftText;

    //private float timerOpp;
    static public SlicesManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
      
        //sliceableObjects = GameObject.FindGameObjectWithTag("SliceableObjects");
        //goal = GameManager.currentGoal;
        currentCakeIndex = -1;
        NextRound();

        //BaseTime = sliderTimer.value;
        /* BaseTimerMax = sliderTimer.value;
         BaseTimerMaxInitial = sliderTimer.value;*/
        timer = TimerHelper.Create();
        timeLeft = initialLevelTime;
    }

    private void TimerGraphicsUpdate()
    {
        sliderTimer.value = (timeLeft/initialLevelTime);//This should work if slider max value's 1 and min value's 0 
        Color sliderColour;
        if (sliderTimer.value > 0.5f)
        {
            sliderColour = Color.Lerp(Color.yellow, Color.green, sliderTimer.value-0.5f);
        }
        else
        {
            sliderColour = Color.Lerp(Color.red, Color.yellow, sliderTimer.value + 0.5f);
        }
      
        sliderImage.color = sliderColour;
        //  sliderTimer.value =(int)timer.Get();
        if (timeLeft <= criticalTime )
        {
            timerAnimation.SetBool("isCritical2", true);
        }
        bool isAnimOn = timerAnimation.GetBool("isCritical2");
        if (isAnimOn && timeLeft > criticalTime)
        {
            timerAnimation.SetBool("isCritical2", false);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("LevelMax" + currentLevel.MaximumScore());
            }
            //Debug.Log("timerOpp: " +timerOpp);     
            //timerOpp = BaseTime - timer.Get();
            timeLeft -= Time.deltaTime;// timer.Get();
            TimerGraphicsUpdate();
            // Mathf.Clamp(timerOpp, 0, 8);
            if (!toStopTimer && timeLeft < 0)
            {
                //   Debug.Log("times up! ");
                GameOver();
                toStopTimer = true;
            }
            else
            {
                CheckSlices();
            }
        }    
    }

    private void CheckSlices()
    {
        slicesCount = sliceableObjects.GetComponentsInChildren<Transform>().Length - 1;
        if (slicesCount == goal)
        {
            Debug.Log("slicesCount: " + slicesCount);
            slicesSizeList = GetSlicesSizesList();

            CalculateNewScore();
            // TODO :note add a small delay after the cake is cut 

            NextRound();

            GameManager.NextLevel();
        }
        else if (slicesCount > goal)
        {
            Debug.Log("slicesCount: " + slicesCount);
            Debug.Log("BadSlice slicesCount > goal");

            NextRound();

            GameManager.NextLevel();

            BadSlice(true);
            //OnBadSlice?.Invoke();
        }
    }

    private void BadSlice(bool toManySlices)
    {
        OnBadSlice?.Invoke(toManySlices);
    }

    private void CalculateNewScore()
    {
        bool isHaveScoreLevel = false;
        ScoreData.ScoreLevel playerScoreLevel = ScoreData.ScoreLevel.Regular;

        ScoreData.ScoreLevel[] ScoreLevelArr = (ScoreData.ScoreLevel[])Enum.GetValues(typeof(ScoreData.ScoreLevel));

        // we need to run on the array from the biggest value to the lower.
        foreach (ScoreData.ScoreLevel scoreLevelEnum in ScoreLevelArr)
        {
            if (scoreLevelEnum != ScoreData.ScoreLevel.Regular)
            {
                int scoreLevel = (int)scoreLevelEnum;
                isHaveScoreLevel = CheckScoreLevel(scoreLevel);

                //Debug.Log("scoreLevel " + scoreLevel); Debug.Log("isHaveScoreLevel " + isHaveScoreLevel); Debug.Log("scoreLevelEnum " + scoreLevelEnum);
                if (isHaveScoreLevel)
                {
                    playerScoreLevel = scoreLevelEnum;
                    break;
                }
            }
        }
        Debug.Log(playerScoreLevel.ToString());
        int scoreToAdd = (int)Enum.Parse(typeof(ScoreData.ScorePointsByLevel), playerScoreLevel.ToString());
        scoreToAdd = 
           (int)((double)scoreToAdd * (ScoreData.NumberOfSlicesScoreNormaliser *(currentLevel.Cakes[currentCakeIndex].numberOfSlices)));
        OnScoreChange?.Invoke(scoreToAdd, playerScoreLevel);
    }

    private bool CheckScoreLevel(int scoreLevel)
    {
        double sliceSizeSupposedToBe = originalSize / goal;
        //Debug.Log("sliceSizeSupposedToBe " + sliceSizeSupposedToBe);
        //Debug.Log("originalSize " + originalSize);  //Debug.Log("scoreLevel " + scoreLevel);

        return slicesSizeList.Any(currSize => {
            double sliceSizeSupposedToBeInPercentage = (sliceSizeSupposedToBe / originalSize) * 100;
            double currSizePercentage = (currSize / originalSize) * 100;
            Debug.Log("sliceSizeSupposedToBeInPercentage: " + sliceSizeSupposedToBeInPercentage); Debug.Log("currSizePercentage " + currSizePercentage);

            double difference = Mathf.Abs((float)(sliceSizeSupposedToBeInPercentage - currSizePercentage));
            Debug.Log("difference: " + difference);
            return difference <= scoreLevel;
        });
    }

    private void GameOver()
    {
        DestroyAllLeftPieces();
        //Instantiate(gameOverScreenPrefub);
        GameManager.GameOver();
        //SceneManager.LoadScene(2);
    }

    void DestroyAllLeftPieces()
    {
        foreach (Transform item in sliceableObjects.transform)
        {
            Destroy(item.gameObject);
        }
    }

    bool IsAllSlicesAreAlmostEqual()
    {
        bool isAlmostEqual = false;
        slicesSizeList = GetSlicesSizesList();
        int sliceSizeSupposedToBe = (int)originalSize / goal;

        minmumSize = sliceSizeSupposedToBe / 4;

        isAlmostEqual = !slicesSizeList.Any(currSize => currSize < minmumSize);

        return isAlmostEqual;
    }

    List<double> GetSlicesSizesList()
    {
        slicesSizeList = new List<double>();

        foreach (Slicer2D slicer in Slicer2D.GetList())
        {
            Polygon2D poly = slicer.GetPolygon().ToWorldSpace(slicer.transform);

            originalSize = slicer.GetComponent<DemoSlicer2DInspectorTracker>().originalSize;
            int currentSizeInt = Mathf.FloorToInt((float)poly.GetArea()); //(int)poly.GetArea();

            //  Debug.Log("current size : " + currentSizeInt);
            slicesSizeList.Add(currentSizeInt);

            sliced = slicer.sliceCounter;
        }

        return slicesSizeList;
    }
     
    private void NextRound()
    {
        toStopTimer = false;
        DestroyAllLeftPieces();
        currentCakeIndex++;
        if (currentCakeIndex < currentLevel.Cakes.Length)
        {
            goal = currentLevel.Cakes[currentCakeIndex].numberOfSlices;
            OnGoalChange.Invoke();
            Cake newCake = currentLevel.Cakes[currentCakeIndex];
            GameObject cake = newCake.cakePrefab;
            Instantiate(cake, sliceableObjects.transform, true); // create new cake
            cakesLeftText.text =
                (currentLevel.Cakes.Length - currentCakeIndex).ToString();
        }
        else
        {
            cakesLeftText.text = "0";
            toStopTimer = true;
            GameOver();
        }
        SoundManager.instance.PlaySoundEffect(SoundEffectNames.NEXT_LEVEL);
        particlesEndLevel.Play();
    }
}