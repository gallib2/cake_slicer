using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlicesManager : MonoBehaviour
{
    public delegate void ScoreChange(int bonuslessScoreToAdd,int bonus, ScoreData.ScoreLevel scoreLevel);
    public delegate void BadSliceHandler(bool isTooManySlices);

    public static event Action OnGoalChange;
    public static event ScoreChange OnScoreChange;
    public static event BadSliceHandler OnBadSlice;
    public static event Action OnGameOver;
   // public int obstaclesLayer = 9;
    public LayerMask obstacleLayerMask;

    //List<double> slicesSizeList;
    public int currentCakeIndex = 0;

    [SerializeField]
    public Level currentLevel;
    private static int slicesToSlice;
    [SerializeField]
    private float criticalTime = 5f;
    private int minmumSize;
    public ParticleSystem particlesEndLevel;

    //private double originalSize = 0;

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
    [SerializeField]
    private Text sliceDemandText;
    [SerializeField]
    private FractionUI[] fractionUIS;
    [SerializeField]
    private SliceDemandUI sliceDemandUI;

    [SerializeField]
    private double negligibleSliceSize = 0.01;

    [SerializeField]
    private Animator[] swipedDownObjects;

    public static bool allowToSlice;
    private Obstacle[] obstacles;
    private List<Obstacle> candleObstacles;

    private int comboCounter = 0;

    private void Awake()
    {
        if (LevelsManager.CurrentLevel != null)
        {
            currentLevel = LevelsManager.CurrentLevel;
        }
        GameManager.OnLevelInitialised += InitialiseLevel;
    }

    private void OnDisable()
    {
        GameManager.OnLevelInitialised -= InitialiseLevel;
    }

    private void InitialiseLevel()
    {
        comboCounter = 0;
        currentLevel.IsLegitimate();
        currentCakeIndex = -1;
        NextRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsPaused)
        {
            return;
        }

        if (!GameManager.isGameOver)
        {
            if (Input.GetMouseButton(0))
            {
                bool isHaveDecorators = obstacles.Length > 0;
                Collider2D collider = isHaveDecorators ? CheckClicksByLayer(obstacleLayerMask) : null;
                Obstacle decorator = collider ? collider.gameObject.GetComponent<Obstacle>() : null;
                if (decorator != null)
                {
                    if (decorator.Type == ObstacleType.CHERRY)
                    {
                        //  Debug.Log("Obstacle!!!!1111");
                        //Slicer2DController.ClearPoints();//TODO: Fix
                        Handheld.Vibrate();
                        BadSlice();
                        NextRound();
                        return;
                    }
                    else if (decorator.Type == ObstacleType.CANDLE)
                    {
                        Debug.Log(" decorator:  caandlleeeee ");

                        candleObstacles.RemoveAt(0);
                        Destroy(collider.gameObject);
                        return;
                    }
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                if (candleObstacles.Count == 0)
                {
                    allowToSlice = true;
                }
            }

            CheckSlices();//TODO: We don't have to do this every frame.
            if (timer.ToStopTimer && !GameManager.isGameOver)//the reason !GameManager.isGameOver is checked is that CheckSlices() can lead to a GameOver()
            {
                GameOver();
            }
        }

    }

    private Collider2D CheckClicksByLayer(int layer)
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D colliderAtfingerPosition = Physics2D.OverlapPoint(fingerPosition, layer);
            if (colliderAtfingerPosition != null)
            {
                return colliderAtfingerPosition;
            }
        }

        return null;
    }

    private void CheckSlices()
    {
        //slicesCount = sliceableObjects.GetComponentsInChildren<Transform>().Length - 1;//TODO: this comes out wrong alot
        //if (false)
        {
            List<Polygon2D> polygons = Polygon2DList.CreateFromGameObject(sliceableObjects.transform.GetChild(0).gameObject,Polygon2D.ColliderType.Polygon);
            if (polygons.Count > 1)//Optimisation..
            {
                //Debug.Log("polygons.Count > 1 ");
                for (int i = 0; i < polygons.Count;)//TODO: must be inefficient
                {
                    if (polygons[i].GetArea() < negligibleSliceSize)
                    {
                        polygons.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
                slicesCount = polygons.Count;

                if (slicesCount == slicesToSlice)
                {
                    Debug.Log("slicesCount: " + slicesCount + "goal: " + slicesToSlice);
                    //slicesSizeList = GetSlicesSizesList();

                    CalculateNewScore();
                    // TODO :note add a small delay after the cake is cut 

                    NextRound();

                }
                else if (slicesCount > slicesToSlice)
                {
                    Debug.Log("BadSlice slicesCount > goal => slicesCount: " + slicesCount);

                    NextRound();

                    BadSlice(true);
                }
            }
        }
       

    }

    private void BadSlice(bool toManySlices = false)
    {
        comboCounter = 0;
        OnBadSlice?.Invoke(toManySlices);
    }

    private void CalculateNewScore()
    {
        //bool isHaveScoreLevel = false;
        ScoreData.ScoreLevel playerScoreLevel = ScoreData.ScoreLevel.Regular;

        if (perfectSlicing)
        {
            playerScoreLevel = ScoreData.ScoreLevel.Awesome;
            Debug.Log("PERFECT!");
        }
        else
        {
            
            //We get a list of the real sizes of all slices, determine their overall size and then modify the list so that it will contain the percentage rather than real size
            List<double> slicesInPercentage = SlicesSizesInDoubles();
            double overallSize = 0;

            // get overall size
            for (int i = 0; i < slicesInPercentage.Count; i++)
            {
                overallSize += slicesInPercentage[i];
            }
            // calculate the (new) percetage 
            for (int i = 0; i < slicesInPercentage.Count; i++)
            {
                slicesInPercentage[i] = ((slicesInPercentage[i] / overallSize) * 100);
                Debug.Log("slicesInPercentage[i] = " + (slicesInPercentage[i]));
            }

            List<double> differences = new List<double>();
            // if fractions
            bool isFractions = (currentLevel.Cakes[currentCakeIndex].fractions != null && currentLevel.Cakes[currentCakeIndex].fractions.Length > 0);
            if (isFractions)
            {
                Fraction[] fractions = currentLevel.Cakes[currentCakeIndex].fractions;
                List<double> slicesSupposedToBeInPercentage = new List<double>();
                for (int i = 0; i < fractions.Length; i++)
                {
                    double sliceSupposedToBeInPercentage = 
                        ((double)fractions[i].numerator / (double)fractions[i].denominator) * 100;
                    Debug.Log("sliceSupposedToBeInPercentage = " + sliceSupposedToBeInPercentage);
                    slicesSupposedToBeInPercentage.Add(sliceSupposedToBeInPercentage);
                }
                //List<ScoreData.ScoreLevel> scoreLevels = new List<ScoreData.ScoreLevel>();

                foreach (double sliceSupposedToBeInPercentage in slicesSupposedToBeInPercentage)
                {
                    double difference;
                    double smallestDifference = 100;
                    int smallestDifferenceIndex = 0;
                    for (int i = 0; i < slicesInPercentage.Count; i++)
                    {
                        difference = Math.Abs(slicesInPercentage[i] - sliceSupposedToBeInPercentage);
                        if (difference < smallestDifference)
                        {
                            smallestDifference = difference;
                            smallestDifferenceIndex = i;
                        }
                    }
                    Debug.Log("smallestDifference = " + smallestDifference);
                    slicesInPercentage.RemoveAt(smallestDifferenceIndex);
                    differences.Add(smallestDifference);
                    //ScoreData.ScoreLevel sliceScoreLevel = ScoreData.ScoreLevel.Regular;
                }
            }
            else
            {
                double sliceSupposedToBeInPercentage = ((1 / (double)currentLevel.Cakes[currentCakeIndex].numberOfSlices) * 100);
                Debug.Log("sliceSupposedToBeInPercentage = " + sliceSupposedToBeInPercentage);
                for (int i = 0; i < slicesInPercentage.Count; i++)
                {
                    differences.Add (Math.Abs(slicesInPercentage[i] - sliceSupposedToBeInPercentage));
                }
            }

            double differencesAverage = 0;
            for (int i = 0; i < differences.Count; i++)
            {
                differencesAverage += differences[i];
            }
            differencesAverage /= differences.Count;
            Debug.Log("differencesAverage = " + differencesAverage);

            ScoreData.ScoreLevel[] ScoreLevelArr = (ScoreData.ScoreLevel[])Enum.GetValues(typeof(ScoreData.ScoreLevel));
            foreach (ScoreData.ScoreLevel possibleScoreLevel in ScoreLevelArr)
            {
                if (possibleScoreLevel != ScoreData.ScoreLevel.Regular)//might not be nesssry
                {
                    if(differencesAverage <= (double)possibleScoreLevel)
                    {
                        playerScoreLevel = possibleScoreLevel;
                        break;
                    }
                }
            }
                //Phew....
        }

        Debug.Log(playerScoreLevel.ToString());
        int bonuslessScoreToAdd = (int)Enum.Parse(typeof(ScoreData.ScorePointsByLevel), playerScoreLevel.ToString());
        bonuslessScoreToAdd = (int)((double)bonuslessScoreToAdd * (ScoreData.NumberOfSlicesScoreNormaliser * slicesToSlice));
        int bonus = 0;
        if (playerScoreLevel == ScoreData.ScoreLevel.Awesome)
        {
            bonus = comboCounter * ScoreData.COMBO_MULTIPLIER;
            comboCounter++;
        }
        else
        {
            comboCounter = 0;
        }
        OnScoreChange?.Invoke(bonuslessScoreToAdd,bonus, playerScoreLevel);
        //comboCounter = (playerScoreLevel == ScoreData.ScoreLevel.Awesome ? (comboCounter + 1) : 0);
        Debug.Log("comboCounter = " + comboCounter);
        Debug.Log("scoreToAdd = " + bonuslessScoreToAdd+bonus);
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
            Destroy(item.gameObject);
        }

        foreach (Transform item in obstacleObjects.transform)
        {
            Destroy(item.gameObject);
        }
    }

    List<double> SlicesSizesInDoubles()
    {
        List<double> slicesSizesInDoubles = new List<double>();

        //foreach (Slicer2D slicer in Slicer2D.GetList())
        /*foreach (Destruction2D slicer in Destruction2D.GetList())
        {
            //Polygon2D poly = slicer.GetPolygon().ToWorldSpace(slicer.transform);
            Polygon2D poly = slicer.GetBoundPolygon().ToWorldSpace(slicer.transform);
            double size = poly.GetArea(); //(int)poly.GetArea();
            //  Debug.Log("current size : " + currentSizeInt);
            slicesSizesInDoubles.Add(size);
        }*/
        List<Polygon2D> polygons = Polygon2DList.CreateFromGameObject(sliceableObjects.transform.GetChild(0).gameObject,Polygon2D.ColliderType.Polygon);
        for (int i = 0; i < polygons.Count; i++)
        {
            //Debug.Log("polygon " + i + "area: " + polygons[i].GetArea());
            double size = polygons[i].GetArea();
            Debug.Log("size " + size);
            if (size > negligibleSliceSize)
            {
                slicesSizesInDoubles.Add(polygons[i].GetArea());
            }
        }

        return slicesSizesInDoubles;
    }

    private void NextRound()
    {
        Debug.Log("--------------------- in NextRound!!!");

        timer.ToStopTimer = false;
        DestroyAllLeftPieces();
        currentCakeIndex++;
        if (currentCakeIndex < currentLevel.Cakes.Length)
        {
            for (int i = 0; i < swipedDownObjects.Length; i++)
            {
                swipedDownObjects[i].SetTrigger("SwipeDown");
            }
            slicesToSlice = currentLevel.Cakes[currentCakeIndex].SlicesToSlice();
            // OnGoalChange.Invoke();

            Cake newCake = currentLevel.Cakes[currentCakeIndex];
            GameObject cakeGameObject = Instantiate(newCake.cakePrefab, sliceableObjects.transform, true); // create new cake
            obstacles = cakeGameObject.GetComponentsInChildren<Obstacle>();
            candleObstacles = obstacles.Where(dec => dec.Type == ObstacleType.CANDLE).ToList();
            allowToSlice = candleObstacles.Count == 0;

            cakesLeftText.text =
                (currentLevel.Cakes.Length - currentCakeIndex).ToString();
            UpdateSliceDemandGraphics();
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

    private void UpdateSliceDemandGraphics()
    {
        bool hasFractions = (currentLevel.Cakes[currentCakeIndex].HasFractions());
        if (hasFractions)
        {
            sliceDemandText.gameObject.SetActive(false);
            Fraction[] fractions = currentLevel.Cakes[currentCakeIndex].fractions;
            for (int i = 0; i < fractionUIS.Length; i++)
            {
                if (i < fractions.Length)
                {
                    fractionUIS[i].gameObject.SetActive(true);
                    fractionUIS[i].ChangeText(fractions[i]);
                }
                else
                {
                    fractionUIS[i].gameObject.SetActive(false);
                }
                /*if (i > 0)
                {
                    sliceDemandText.text += ",";
                }
                sliceDemandText.text += fractions[i].numerator + "/" + fractions[i].denominator;*/
            }
            sliceDemandUI.ChangeDestination(fractions.Length);
        }
        else
        {
            for (int i = 0; i < fractionUIS.Length; i++)
            {
                fractionUIS[i].gameObject.SetActive(false);
            }
            sliceDemandUI.ChangeDestination(1);
            sliceDemandText.gameObject.SetActive(true);
            sliceDemandText.text = slicesToSlice.ToString();
        }
       
    }
}