using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class Scoring : MonoBehaviour
{
    public static Scoring   instance;
   // public GameObject[]     stars;

   // private int             score;
    [SerializeField]
    Text                    scoreText;



    public float multiplier;
    public float maxMultiplier;
    public Text gameMultiplierText;

    public Text endScoreText;
    public Text highscoreText;
    public Text gameScoreGoalText;
    public Image gameTipJarFill;
    public Sprite starShine;

    
    public GameObject starFillPrefab;
    public GameObject shimmerFXPrefab;
    public GameObject fallingStarFXPrefab;
    public GameObject implosionFXPrefab;
    public GameObject lightbeamFXPrefab;
    public GameObject blindingTwinkleFXPrefab;

    public GameObject starRatingContainer;
    public int starIndex;
    public List<GameObject> stars = new List<GameObject>();
    public GameObject[] starSlots;
 

    public Sprite failImage;
    public Image levelPasserImage;
    public GameObject failPrompt, successPrompt;
    public GameObject coinPrefab;
    public GameObject targetLocation;
    //public Text performanceFactName;
    //public Text performanceFactValue;


    [SerializeField] private int score;
    public int scoreGoal = 0;
    public string scoreFormat = "N0";

    #region Animations Values
    public int fpsCount = 30;
    public float duration = 1f;
    public float starToSlotSpeed = 15f;
    public float fitStarToSlotDuration = 1f;
    public int percentageIncrementPerStar = 20;
    public float starRatingAnticipationDelay = 1.0f;
    #endregion
    private Coroutine countingCoroutine;

    public Text totalMathProblems;
    public Text totalSolvingTime;
    public Text additionSolvingTime;
    public Text additionEvaluation;
    public Text subtractionSolvingTime;
    public Text subtractionEvaluation;
    public Text multiplicationSolvingTime;
    public Text multiplicationEvaluation;
    public Text divisionSolvingTime;
    public Text divisionEvaluation;

    public GameObject continueButton;
    public GameObject quitButton;


    private void ShowResults(int p_newValue, int p_Value = 0)
    {
        if (countingCoroutine != null)
        {
            StopCoroutine(countingCoroutine);
        }
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
            highscoreText.text = score.ToString();
        }
        //temporary
        if (PlayerPrefs.GetInt("Highscore") > 0)
        {
            highscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
        }
        else
        {
            highscoreText.text = "0";
        }
        
        //Day Failed if score is less than half of score goal
        if (score < scoreGoal/2) 
        {
            endScoreText.text = score.ToString();
            levelPasserImage.sprite = failImage;
            failPrompt.SetActive(true);
            successPrompt.SetActive(false);
        }

        countingCoroutine = StartCoroutine(CountText(p_newValue, p_Value));
    }

    private IEnumerator CountText(int p_newValue, int p_Value = 0)
    {

        WaitForSeconds wait = new WaitForSeconds(1f / fpsCount);
        int previousValue = p_Value;
        int stepAmount;

        if (p_newValue - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((p_newValue - previousValue) / (fpsCount * duration));
        }
        else
        {
            stepAmount = Mathf.CeilToInt((p_newValue - previousValue) / (fpsCount * duration));

        }

        //Animated Look where numbers roll like a slot machine for awhile
        int backUpStarRatingsCounted = 0;
        if (previousValue < p_newValue)
        {
            //Back up counter

            while (previousValue < p_newValue)
            {
                previousValue += stepAmount;
                if (previousValue > p_newValue)
                {
                    previousValue = p_newValue;
                }

                //Update Text to new Value
                endScoreText.text = previousValue.ToString(scoreFormat);

                //Calcualtes how many stars did the player get
                int starRatingForScoreCounted = (int)((((float)previousValue / (float)scoreGoal) * 100));
                if (starRatingForScoreCounted % percentageIncrementPerStar == 0 && starRatingForScoreCounted != 0)
                {
                    //Makes sure that the star is within the minimum and maximum amount of stars that can be gained. (If it's more than maxStarAmount(5) stars, it'll become 5 stars)
                    starRatingForScoreCounted = Mathf.Clamp((starRatingForScoreCounted / percentageIncrementPerStar), 0, starSlots.Length);

                    //Makes sure there is only 1 copy
                    if (backUpStarRatingsCounted == starRatingForScoreCounted - 1)
                    {
                        //Do UI UX Animation for that star
                        GameObject newStarFill = CreateStarFill(starSlots[backUpStarRatingsCounted]);
                        StartCoroutine(FitStarToSlot(newStarFill, starSlots[backUpStarRatingsCounted].GetComponent<RectTransform>().sizeDelta));
                        backUpStarRatingsCounted++;
                    }
                }

                yield return wait;

            }

        }
        else if(previousValue > p_newValue)
        {
            while (previousValue > p_newValue)
            {
                previousValue += stepAmount;
                if (previousValue < p_newValue)
                {
                    previousValue = p_newValue;
                }

                //Update Text to new Value
                endScoreText.text = previousValue.ToString(scoreFormat);

                //Calcualtes how many stars did the player get
                int starRatingForScoreCounted = (int)((((float)previousValue / (float)scoreGoal) * 100));
                if (starRatingForScoreCounted % percentageIncrementPerStar == 0 && starRatingForScoreCounted != 0)
                {
                    //Makes sure that the star is within the minimum and maximum amount of stars that can be gained. (If it's more than maxStarAmount(5) stars, it'll become 5 stars)
                    starRatingForScoreCounted = Mathf.Clamp((starRatingForScoreCounted / percentageIncrementPerStar), 0, starSlots.Length);

                    //Makes sure there is only 1 copy
                    if (backUpStarRatingsCounted == starRatingForScoreCounted - 1)
                    {
                        //Do UI UX Animation for that star
                        GameObject newStarFill = CreateStarFill(starSlots[backUpStarRatingsCounted]);
                        StartCoroutine(FitStarToSlot(newStarFill, starSlots[backUpStarRatingsCounted].GetComponent<RectTransform>().sizeDelta));
                        backUpStarRatingsCounted++;
                    }



                }

                yield return wait;

            }
           
        }
        else if (previousValue == p_newValue)
        {
            endScoreText.text = previousValue.ToString(scoreFormat);
        }
        yield return new WaitForSeconds(1f);
        //Go through each star ratings again for Falling Star FX Particles
        foreach (GameObject selectedStar in stars)
        {
            //Create new Falling Star Particle FX

            GameObject spawnedFallingStarParticleFX = Instantiate(fallingStarFXPrefab, selectedStar.transform);
            spawnedFallingStarParticleFX.transform.position = selectedStar.transform.position;
            Destroy(spawnedFallingStarParticleFX, 3f);
        }

        continueButton.SetActive(true);
        quitButton.SetActive(true);

    }


    public IEnumerator FitStarToSlot(GameObject p_spawnedStarFill, Vector2 p_starSlotSize)
    {
    
        WaitForSeconds timeRate = new WaitForSeconds(1f / fpsCount);
       
        float stepAmount =starToSlotSpeed/ (fpsCount * fitStarToSlotDuration);
              
        //Create new Shimmer Particle FX
        GameObject spawnedImplosionParticleFX = Instantiate(implosionFXPrefab, starRatingContainer.transform);
        spawnedImplosionParticleFX.transform.position = p_spawnedStarFill.transform.position;
        Destroy(spawnedImplosionParticleFX, 3f);

        GameObject spawnedLightBeamParticleFX = Instantiate(lightbeamFXPrefab, starRatingContainer.transform);
        spawnedLightBeamParticleFX.transform.position = p_spawnedStarFill.transform.position;
        Destroy(spawnedLightBeamParticleFX, 3f);

        GameObject spawnedBlindingTwinkleParticleFX = Instantiate(blindingTwinkleFXPrefab, starRatingContainer.transform);
        spawnedBlindingTwinkleParticleFX.transform.position = p_spawnedStarFill.transform.position;
        Destroy(spawnedBlindingTwinkleParticleFX, 3f);

        //If spawned star's size is larger than the selected star slot's size, then shrink it and lessen transparency
        while (p_spawnedStarFill.GetComponent<RectTransform>().sizeDelta.x > p_starSlotSize.x + stepAmount)
        {
        
            //Set size of new star (smaller)
            p_spawnedStarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(p_spawnedStarFill.GetComponent<RectTransform>().sizeDelta.x - stepAmount, p_spawnedStarFill.GetComponent<RectTransform>().sizeDelta.y - stepAmount);
            //Set transparency/Opacity of new star (less transparent)
            var desiredColor = p_spawnedStarFill.GetComponent<Image>().color;
            desiredColor.a = p_spawnedStarFill.GetComponent<Image>().color.a + stepAmount* 0.01f;
            p_spawnedStarFill.GetComponent<Image>().color = desiredColor;
            yield return timeRate;


        }

        //Else if spawned star's size is smaller than the selected star slot's size, then enlargen it and make sure it's not transparent
        //Not available
        
    }
    public GameObject CreateStarFill(GameObject p_selectedStarSlot)
    {
        //Spawn new star
        GameObject spawnedStarFill = Instantiate(starFillPrefab, starRatingContainer.transform);
        spawnedStarFill.transform.position = p_selectedStarSlot.transform.position;
        stars.Add(spawnedStarFill);
        //Reference the selected star slot's size
        Vector2 selectedStarSlotSize = p_selectedStarSlot.GetComponent<RectTransform>().sizeDelta;

        //Set the spawned star's size to 3x the selected star slot's size
        spawnedStarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(selectedStarSlotSize.x*3f, selectedStarSlotSize.y*3f);
        
        //Set transparency/Opacity of new star
        var desiredColor = spawnedStarFill.GetComponent<Image>().color;
        desiredColor.a = 0f;
        spawnedStarFill.GetComponent<Image>().color = desiredColor;

        //Create new Shimmer Particle FX
        GameObject spawnedShimmerParticleFX = Instantiate(shimmerFXPrefab, starRatingContainer.transform);
        spawnedShimmerParticleFX.transform.position = p_selectedStarSlot.transform.position;
        Destroy(spawnedShimmerParticleFX, 3f);
        return spawnedStarFill;
    }
 
    public void UpdateGameScoreGoal()
    {
        gameScoreGoalText.text = scoreGoal.ToString();
    }

    public void SetScore(int p_newScore)
    {
        score = p_newScore;
        gameTipJarFill.fillAmount = (float)score/(float)scoreGoal;
    }
    public int GetScore()
    {
        return score;
    }
    
    public void ResetMultiplier()
    {
        multiplier = 1f;
        gameMultiplierText.text = multiplier.ToString() + "x";
    }

    public void ModifyMultiplier(float p_modifyingValue)
    {

        multiplier = Mathf.Clamp(multiplier + p_modifyingValue, 1, maxMultiplier);
        gameMultiplierText.text = multiplier.ToString() + "x";
        
    }
    public void addScore(int gainScore)
    {
        TextAnimaation();
        CoinActivator();
        score += gainScore;
        scoreText.text = score.ToString();
        gameTipJarFill.fillAmount = (float)score/(float)scoreGoal;
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        Scoring.instance.UpdateGameScoreGoal();
    }
    public void OnEnable()
    {
        GameManager.OnGameStart += OnGameStarted;
    }
    public void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStarted;
    }

    void OnGameStarted()
    {
        //? means null checker
        SetScore(0);
        UpdateGameScoreGoal();
        ResetMultiplier();
    }
    public void starCheck()
    {
        if(score % 300 == 0 && stars[starIndex] != null)
        {
            StarAnimation();
            stars[starIndex].transform.GetComponent<Image>().sprite = starShine;
            starIndex++;
        }
        else
        {
            Debug.Log(null);
        }

    }

    public void Results()
    {
        continueButton.SetActive(false);
        quitButton.SetActive(false);
        TransitionManager.instances.changeTransform.gameObject.SetActive(false);
        TransitionManager.instances.noteBookTransform.gameObject.SetActive(false);
        ShowResults(score);
       
        totalMathProblems.text = "Total Math Problems: " + PerformanceManager.instance.totalMathProblems.ToString();
        totalSolvingTime.text = PerformanceManager.instance.GetAverageTime(MathProblemOperator.none) + " seconds";
        additionSolvingTime.text = PerformanceManager.instance.GetAverageTime(MathProblemOperator.addition) + " seconds";
        additionEvaluation.text = "Addition: " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.addition, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.addition);
        subtractionSolvingTime.text = PerformanceManager.instance.GetAverageTime(MathProblemOperator.subtraction) + " seconds";
        subtractionEvaluation.text = "Subtraction: " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.subtraction, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.subtraction);
        multiplicationSolvingTime.text = PerformanceManager.instance.GetAverageTime(MathProblemOperator.multiplication) + " seconds";
        multiplicationEvaluation.text = "Multiplication: " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.multiplication, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.multiplication);
        divisionSolvingTime.text = PerformanceManager.instance.GetAverageTime(MathProblemOperator.division) + " seconds";
        divisionEvaluation.text = "Division: " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.division, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.division);
        PerformanceManager.instance.answeredProblemDatas.Clear();
        PerformanceManager.instance.totalMathProblems = 0;
    }

    public void StarAnimation()
    {
        for(int i = 0; i < stars.Count; i++)
        {
            stars[i].gameObject.transform.DOShakeScale(1,1,10,90,true);
        }
    }

    public void TextAnimaation() 
    {
       scoreText.gameObject.transform.DOShakeScale(1, 0.3f,10, 90, true);
    }

   public void CoinActivator()
    {
       // TransitionManager.instances.MoveTransition();
        for(int i = 0; i < ObjectPool.instances.amountToPool; i++)
        {
             ObjectPool.instances.pooledGameobjects[i].SetActive(true);
            // ObjectPool.instances.pooledGameobjects[i].
            // CoinAnimation(i);
           ObjectPool.instances.RandomPosition();
          StartCoroutine(CoinAnimation(i));

        }

    }

    IEnumerator CoinAnimation(int index)
    {

        Tween tween = ObjectPool.instances.pooledGameobjects[index].GetComponent<Transform>().DOMove(new Vector3(targetLocation.transform.position.x, targetLocation.transform.position.y, targetLocation.transform.position.z), 0.5f);
        yield return tween.WaitForCompletion();

        ObjectPool.instances.pooledGameobjects[index].SetActive(false);
        ObjectPool.instances.ResetPosition();
    }


}
