using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlicesManager : MonoBehaviour
{
    public delegate void ScoreChange(int score, ScoreData.ScoreLevel scoreLevel);
    public delegate void BadSliceHandler(bool isTooManySlices);

    public static event Action OnGoalChange;
    public static event ScoreChange OnScoreChange;
    public static event BadSliceHandler OnBadSlice;
    public static event Action OnGameOver;
   // public int obstaclesLayer = 9;
    public LayerMask obstacleLayerMask;

    List<double> slicesSizeList;
    public int currentCakeIndex = 0;

    [SerializeField]
    public Level currentLevel;
    public static int goal;
    [SerializeField]
    private float criticalTime = 5f;
    private int minmumSize;
    public ParticleSystem particlesEndLevel;

    private double originalSize = 0;

    [SerializeField]
    private GameObject sliceableObjects;
    [SerializeField]
    private GameObject obstacleObjects;

    public int slicesCount = 0;
    [SerializeField]
    private Text cakesLeftText;
    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private Timer timer;
    [SerializeField]
    private bool perfectSlicing = false;

    private void Awake()
    {
        currentLevel = LevelsManager.CurrentLevel;
        GameManager.OnLevelInitialised += InitialiseLevel;
    }

    private void OnDisable()
    {
        GameManager.OnLevelInitialised -= InitialiseLevel;
    }

    private void InitialiseLevel()
    {
        currentCakeIndex = -1;
        NextRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isGameOver)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D colliderAtfingerPosition = Physics2D.OverlapPoint(fingerPosition, obstacleLayerMask);
                if (colliderAtfingerPosition != null)
                {
                    if (colliderAtfingerPosition.gameObject.GetComponent<Obstacle>())
                    {
                        //  Debug.Log("Obstacle!!!!1111");
                        Slicer2DController.ClearPoints();
                        Handheld.Vibrate();
                        NextRound();
                        return;
                    }
                }
            }

            CheckSlices();
            if (timer.ToStopTimer && !GameManager.isGameOver)//the reason !GameManager.isGameOver is checked is that CheckSlices() can lead to a GameOver()
            {
                GameOver();
            }
        }

    }

    private void CheckSlices()
    {
        slicesCount = sliceableObjects.GetComponentsInChildren<Transform>().Length - 1;
        if (slicesCount == goal)
        {
            Debug.Log("slicesCount: " + slicesCount+ "goal: " + goal );
            slicesSizeList = GetSlicesSizesList();

            CalculateNewScore();
            // TODO :note add a small delay after the cake is cut 

            NextRound();

        }
        else if (slicesCount > goal)
        {
            Debug.Log("BadSlice slicesCount > goal => slicesCount: " + slicesCount);

            NextRound();

            BadSlice(true);
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

                if (isHaveScoreLevel)
                {
                    playerScoreLevel = scoreLevelEnum;
                    break;
                }
            }
        }

        if (perfectSlicing)
        {
            playerScoreLevel = ScoreData.ScoreLevel.Awesome;
            Debug.Log("PERFECT!");
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

        return slicesSizeList.Any(currSize => {
            double sliceSizeSupposedToBeInPercentage = (sliceSizeSupposedToBe / originalSize) * 100;
            double currSizePercentage = (currSize / originalSize) * 100;
            //Debug.Log("sliceSizeSupposedToBeInPercentage: " + sliceSizeSupposedToBeInPercentage); Debug.Log("currSizePercentage " + currSizePercentage);

            double difference = Mathf.Abs((float)(sliceSizeSupposedToBeInPercentage - currSizePercentage));
            //Debug.Log("difference: " + difference);
            return difference <= scoreLevel;
        });
    }

    private void GameOver()
    {
        DestroyAllLeftPieces();
        OnGameOver?.Invoke();
    }

    void DestroyAllLeftPieces()
    {
        foreach (Transform item in sliceableObjects.transform)
        {
            //Debug.Log("DestroyAllLeftPieces... " + item);
            //DestroyImmediate(item.gameObject);
            Destroy(item.gameObject);
        }

        foreach (Transform item in obstacleObjects.transform)
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
        }

        return slicesSizeList;
    }
     
    private void NextRound()
    {
        Debug.Log("--------------------- in NextRound!!!");
        timer.ToStopTimer = false;
        DestroyAllLeftPieces();
        currentCakeIndex++;
        if (currentCakeIndex < currentLevel.Cakes.Length)
        {
            goal = currentLevel.Cakes[currentCakeIndex].numberOfSlices;
            OnGoalChange.Invoke();
            Cake newCake = currentLevel.Cakes[currentCakeIndex];
            GameObject cakeGameObject = Instantiate(newCake.cakePrefab, sliceableObjects.transform, true); // create new cake
            Obstacle[] obstacles = cakeGameObject.GetComponentsInChildren<Obstacle>();
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].transform.parent = obstacleObjects.transform;
            }
            cakesLeftText.text =
                (currentLevel.Cakes.Length - currentCakeIndex).ToString();
        }
        else
        {
            cakesLeftText.text = "0";
            timer.ToStopTimer = true;
            GameOver();
        }
        soundManager.PlaySoundEffect(SoundEffectNames.NEXT_LEVEL);
        particlesEndLevel.Play();
    }
}