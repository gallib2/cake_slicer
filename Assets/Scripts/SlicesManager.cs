using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlicesManager : MonoBehaviour
{
    public delegate void ScoreChange(int score, ScoreLevel scoreLevel);
    public delegate void BadSliceHandler(bool isTooManySlices);

    public static event ScoreChange OnScoreChange;
    public static event BadSliceHandler OnBadSlice;

    public AudioSource audioSource;
    public AudioClip nextLevelSound;

    public Animator timerAnimation;

    public int currentLevel;
    bool test = true;

    public float timerPenelty;
    public float timerReward;
    public float BaseTime = 20;
    private bool badSliceOccur = false;
    [SerializeField]
    private float BaseTimerMax = 20;
    private float BaseTimerMaxInitial;

    List<int> slicesSizeList;
    // public AudioSource m_MyAudioSource;
    public Slider sliderTimer;
    public int goal;
    private int minmumSize;
    public ParticleSystem particlesEndLevel;

    public Text timerText;

    TimerHelper timer;
    float timerRequired = 1f;
    bool toStopTimer = false;

    private double originalSize = 0;
    private int sliced = 0;

    //public GameObject cake;
    public GameObject[] cakes;
    public GameObject gameOverScreenPrefub;

    GameObject sliceableObjects;

    public int slicesCount = 0;

    private float timerOpp;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // m_MyAudioSource = GetComponent<AudioSource>();
        // m_MyAudioSource.Play();
        sliceableObjects = GameObject.FindGameObjectWithTag("SliceableObjects");
        goal = GameManager.currentGoal;

        BaseTime = sliderTimer.value;
        BaseTimerMax = sliderTimer.value;
        BaseTimerMaxInitial = sliderTimer.value;

        timer = TimerHelper.Create();
    }

    private void OnEnable()
    {
        GameManager.OnNextLevel += NextLevel;
    }

    private void OnDisable()
    {
        GameManager.OnNextLevel -= NextLevel;
    }

    // Update is called once per frame
    void Update()
    {

        timerOpp = BaseTime - timer.Get();
        // Mathf.Clamp(timerOpp, 0, 8);
        sliderTimer.value = timerOpp; //(int)timer.Get();

        if (sliderTimer.value <= 5)
        {
            timerAnimation.SetBool("isCritical2", true);
        }

        bool isAnimOn = timerAnimation.GetBool("isCritical2");
        if (isAnimOn && sliderTimer.value > 5)
        {
            timerAnimation.SetBool("isCritical2", false);
        }

        if (!toStopTimer && timerOpp < 0)
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

    void CheckSlices()
    {
        slicesCount = sliceableObjects.GetComponentsInChildren<Transform>().Length - 1;

        goal = GameManager.currentGoal;

        if (slicesCount == goal)
        {
            Debug.Log("slicesCount= " + slicesCount);
            bool isAllSlicesEqual = IsAllSlicesAreAlmostEqual();

            if (isAllSlicesEqual)
            {

                CalculateNewScore();

                //note add a small delay after the cake is cut 
                DestroyAllLeftPieces();

                GoodSlice();

                GameObject cake = GetRandomCake();
                Instantiate(cake, sliceableObjects.transform, true); // create new cake

                GameManager.NextLevel();
                //NextLevel();
            }
            else if (!isAllSlicesEqual && !GameManager.isGameOver)
            {
                DestroyAllLeftPieces();

                GameObject cake = GetRandomCake();
                Instantiate(cake, sliceableObjects.transform, true); // create new cake

                GameManager.NextLevel();
               // NextLevel();

                Debug.Log("BadSlice");
                BadSlice(false);

                // dont end the game when 
                //GameOver();
            }
        }

        else if (slicesCount > goal)
        {
            Debug.Log("BadSlice slicesCount > goal");
            DestroyAllLeftPieces();

            GameObject cake = GetRandomCake();
            Instantiate(cake, sliceableObjects.transform, true); // create new cake

            GameManager.NextLevel();
            NextLevel();

            BadSlice(true);
            //OnBadSlice?.Invoke();

            //if(test)
            //{
            //    test = false;

            //    OnBadSlice?.Invoke();
            //    GameManager.NextLevel();
            //    NextLevel();
            //}
            ////BadSlice();
            /// 
            /// 
            //GameOver();

        }
    }


    private void BadSlice(bool toManySlices)
    {
        badSliceOccur = true;
        timerAnimation.SetBool("isCritical", true);
        //BaseTime -= timerPenelty;
        BaseTime = sliderTimer.value;// BaseTimerMax;
        OnBadSlice?.Invoke(toManySlices);
    }

    private void GoodSlice()
    {
        //BaseTime += timerReward;
        BaseTime = sliderTimer.value + BaseTimerMaxInitial / 4; // sliderTimer.value + sliderTimer.value / 2;
        if (BaseTime > BaseTimerMaxInitial)
            BaseTime = BaseTimerMaxInitial;
    }


    private void CalculateNewScore()
    {
        bool isHaveScoreLevel = false;
        ScoreLevel playerScoreLevel = ScoreLevel.Regular;

        ScoreLevel[] ScoreLevelArr = (ScoreLevel[])Enum.GetValues(typeof(ScoreLevel));

        // we need to run on the array from the biggest value to the lower.
        foreach (ScoreLevel scoreLevelEnum in ScoreLevelArr)
        {
            if (scoreLevelEnum != ScoreLevel.Regular)
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

        int scoreToAdd = (int)Enum.Parse(typeof(ScorePointsByLevel), playerScoreLevel.ToString());

        OnScoreChange?.Invoke(scoreToAdd, playerScoreLevel);
    }

    private bool CheckScoreLevel(int scoreLevel)
    {
        int sliceSizeSupposedToBe = (int)originalSize / goal;
        //  Debug.Log("originalSize " + originalSize); Debug.Log("sliceSizeSupposedToBe " + sliceSizeSupposedToBe); Debug.Log("scoreLevel " + scoreLevel);

        return slicesSizeList.Any(currSize => {
            double sliceSizeSupposedToBeInPercentage = (sliceSizeSupposedToBe / originalSize) * 100;
            double currSizePercentage = ((double)currSize / originalSize) * 100;
            // Debug.Log("sliceSizeSupposedToBeInPercentage: " + sliceSizeSupposedToBeInPercentage); Debug.Log("currSizePercentage " + currSizePercentage);

            int difference = Mathf.Abs((int)sliceSizeSupposedToBeInPercentage - (int)currSizePercentage);
            return difference <= scoreLevel;
        });
    }

    void NextLevel()
    {
        //prevLevel = currentLevel;
        test = true;
        audioSource.PlayOneShot(nextLevelSound);
        currentLevel++;
        if (BaseTimerMax > 2)
        {
            BaseTimerMax--;
        }
        if(currentLevel % 5 == 0)
        {
            BaseTime = sliderTimer.value - 2;
        }
        particlesEndLevel.Play();
        toStopTimer = false;
        if (!badSliceOccur)
        {
            timer = TimerHelper.Create();
        }

        badSliceOccur = false;
    }


    void GameOver()
    {
        BaseTimerMax = 20;
        BaseTime = BaseTimerMax;
        // m_MyAudioSource.Stop();
        toStopTimer = true;
        DestroyAllLeftPieces();
        //Instantiate(gameOverScreenPrefub);
        GameManager.GameOver();
        SceneManager.LoadScene(2);
        //GameObject cake = GetRandomCake();
        //Instantiate(cake, sliceableObjects.transform, true);

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

    //private List<int> GetSlicesPercentageList(int sliceSizeSupposedToBe)
    //{
    //    List<int> slicesPercentageList = new List<int>();

    //    foreach (var currentSliceSize in slicesSizeList)
    //    {
    //        int percentage = (currentSliceSize * 100) / sliceSizeSupposedToBe;
    //        slicesPercentageList.Add(percentage);
    //    }

    //    return slicesPercentageList;

    //    //slicesSizeList.Any(currSize => {
    //    //int percentage = (currSize * 100) / sliceSizeSupposedToBe;

    //    //    return currSize < minmumSize;
    //    //});

    //}

    List<int> GetSlicesSizesList()
    {
        slicesSizeList = new List<int>();

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

    GameObject GetRandomCake()
    {
        int maxIndex = cakes.Length;
        int index = UnityEngine.Random.Range(0, maxIndex);

        return cakes[index];
    }
}