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


    public int round = 0;
    public float multiplier;
    public float maxMultiplier;
    public Text gameMultiplierText;
    [SerializeField] private int scoreToNextGoal = 0;

    public Text gameDayText;
    public TextMeshProUGUI resultDayText;
    public Text endScoreText;
   // public Text highscoreText;
    public TextMeshProUGUI briefingDayText;
    public TextMeshProUGUI briefingScoreGoalText;
    public Text gameScoreGoalText;
    public Image gameTipJarFill;
    public Sprite starShine;

    //public GameObject jarStarFillPrefab;
    
    public GameObject starFillPrefab;
    public GameObject shimmerFXPrefab;
    public GameObject fallingStarFXPrefab;
    public GameObject implosionFXPrefab;
    public GameObject lightbeamFXPrefab;
    public GameObject blindingTwinkleFXPrefab;
    public List<ParticleSystem> confettis = new List<ParticleSystem>();
    public GameObject gameStarRatingContainer;
    public GameObject starRatingContainer;
    public int starIndex;
    public List<GameObject> gameStars;
    public List<GameObject> resultStars = new List<GameObject>();

    int gameStarSlotIndex = 0;
    public GameObject[] gameStarSlots;
    public GameObject[] resultStarSlots;


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

   
    public Text totalMathProblemsValue;
   
    public Text totalSolvingTimeValue;
    public Text additionSolvingTime;
    public Text additionEvaluation;
    public Text subtractionSolvingTime;
    public Text subtractionEvaluation;
    public Text multiplicationSolvingTime;
    public Text multiplicationEvaluation;
    public Text divisionSolvingTime;
    public Text divisionEvaluation;

    public GameObject continueButton;
    public GameObject restartButton;
    public GameObject quitButton;

    public GameObject enterHighscoreUI;
    public GameObject newHighscoreUI;

    public HighscoreTable hsTable;

    public GameObject scoreFloater;
    Vector3 defaultScoreFloaterPos;
    public int resultsNextStar;

    public TextMeshProUGUI minGoalValue;
   
    public void ShowBriefing()
    {
        briefingDayText.text = (round+1).ToString();
        scoreGoal = 1000 + ((round-1) * (250));
        briefingScoreGoalText.text = scoreGoal.ToString();
        resultsNextStar = scoreGoal / 5;
        minGoalValue.text = resultsNextStar.ToString();
        score = 0;
        UpdateGameScoreGoal();
    }
    private void ShowResults(int p_newValue, int p_Value = 0)
    {
        if (countingCoroutine != null)
        {
            StopCoroutine(countingCoroutine);
        }


        
        //Day Failed if score is less than half of score goal
        //if (score < scoreGoal/2) 
        //{
        //    endScoreText.text = score.ToString();
        //    levelPasserImage.sprite = failImage;
        //    failPrompt.SetActive(true);
        //    successPrompt.SetActive(false);
        //}

        countingCoroutine = StartCoroutine(CountText(p_newValue, p_Value));
    }

    private IEnumerator CountText(int p_newValue, int p_Value = 0)
    {
        resultsNextStar = scoreGoal / 5;

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
              
               
                if (previousValue >= resultsNextStar)
                {
                    //Makes sure that the star is within the minimum and maximum amount of stars that can be gained. (If it's more than maxStarAmount(5) stars, it'll become 5 stars)
                    if (backUpStarRatingsCounted < 5)
                    {
                        
                        //Do UI UX Animation for that star
                        GameObject newStarFill = CreateStarFill(resultStarSlots[backUpStarRatingsCounted]);
                        StartCoroutine(FitStarToSlot(newStarFill, resultStarSlots[backUpStarRatingsCounted].GetComponent<RectTransform>().sizeDelta));
                        backUpStarRatingsCounted++;
                        resultsNextStar += scoreGoal / 5;
                        
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
                    starRatingForScoreCounted = Mathf.Clamp((starRatingForScoreCounted / percentageIncrementPerStar), 0, resultStarSlots.Length);

                    //Makes sure there is only 1 copy
                    if (backUpStarRatingsCounted == starRatingForScoreCounted - 1)
                    {
                        //Do UI UX Animation for that star
                        GameObject newStarFill = CreateStarFill(resultStarSlots[backUpStarRatingsCounted]);
                        StartCoroutine(FitStarToSlot(newStarFill, resultStarSlots[backUpStarRatingsCounted].GetComponent<RectTransform>().sizeDelta));
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
        foreach (GameObject selectedStar in resultStars)
        {
            //Create new Falling Star Particle FX

            GameObject spawnedFallingStarParticleFX = Instantiate(fallingStarFXPrefab, selectedStar.transform);
            spawnedFallingStarParticleFX.transform.position = selectedStar.transform.position;
            Destroy(spawnedFallingStarParticleFX, 3f);
           
            
           
        }

        //If passing
        if (resultStars.Count >= 3)
        {
            AudioManager.instance.playSound(4);
            //If perfect, do confetti
            if (resultStars.Count >= 4)
            {
                foreach (ParticleSystem selectedConfetti in confettis)
                {
                    selectedConfetti.Play();
                }
            }

            //HIGHSCORE RECORDING
            string jsonString = PlayerPrefs.GetString("highscoreTable");
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
            //if it's not 0
            if (score > 0)
            {
               
                //check if there are highscores
                //if there is high scores
                if (highscores != null)
                {

                    bool nameExists = false;
                    // find if player name already exists by loop through all highscores
                    foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
                    {
                        //if player name is found
                        if (selectedHighscoreEntry.name == PlayerManager.instance.playerName)
                        {
                            nameExists = true;
                            //compare if current score is higher than old score
                            if (score > selectedHighscoreEntry.score)
                            {
                                //replace high score of same name
                                hsTable.ReplaceHighscoreEntry(score, PlayerManager.instance.playerName, true);
                                StartCoroutine(NewHighscore());
                            }
                            else
                            {
                                StartCoroutine(ShowPerformanceStats());
                            }
                        }


                    }
                    //else if player name is not found
                    if (!nameExists)
                    {
                        //check if it hasnt reached max limit of high scores
                        //if it is max limit for high scores already, replace
                        if (highscores.highscoreEntryList.Count >= 10)
                        {
                            //replace highscore of different name
                            hsTable.ReplaceHighscoreEntry(score, PlayerManager.instance.playerName, false);
                            StartCoroutine(NewHighscore());
                        }
                        //else if it isnt max limit for highscores yet, add
                        else if (highscores.highscoreEntryList.Count < 10)
                        {
                            // add
                            hsTable.AddHighscoreEntry(score, PlayerManager.instance.playerName);
                            StartCoroutine(NewHighscore());
                        }

                    }

                }
                //ELSE IF THERE IS NO HIGHSCORES YET
                else
                {
                    // add
                    hsTable.AddHighscoreEntry(score, PlayerManager.instance.playerName);
                    StartCoroutine(NewHighscore());

                }


                //if (hsTable.highscoresList.Count < 10)
                //{
                //    StartCoroutine(NewHighscore());
                //    starRatingContainer.gameObject.GetComponent<Canvas>().sortingOrder = -10;
                //}
                //else
                //{
                //    if (score > hsTable.highscoresList[hsTable.highscoresList.Count - 1])
                //    {
                //        starRatingContainer.gameObject.GetComponent<Canvas>().sortingOrder = -10;
                //        StartCoroutine(NewHighscore());


                //    }
                //    else
                //    {
                //        StartCoroutine(ShowPerformanceStats());
                //    }
                //}
            }
        }

        //else if fail
        else
        {
            failPrompt.SetActive(true);
            AudioManager.instance.playSound(5);
            StartCoroutine(ShowPerformanceStats());
        }


    }
    IEnumerator NewHighscore()
    {
        starRatingContainer.gameObject.GetComponent<Canvas>().sortingOrder = 1;
        newHighscoreUI.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        newHighscoreUI.SetActive(false);
        //enterHighscoreUI.SetActive(true);
        
        StartCoroutine(ShowPerformanceStats());
    }

   

    public void ShowPerformance(Text p_perfromanceTitle, string p_performanceValue)
    {
        p_perfromanceTitle.gameObject.SetActive(true);
        p_perfromanceTitle.text = p_performanceValue.ToString();
    }
    public IEnumerator ShowPerformanceStats()
    {

        starRatingContainer.gameObject.GetComponent<Canvas>().sortingOrder = 110;

        int totalCorrectAnswers = 0;
        int totalMathProblems = 0;
        //Correctly Answered Math Problems
        for (int i = 0; i < 5; i++)
        {
            string currentCorrectAnswersForOperatorString = PerformanceManager.instance.GetOperatorCount((MathProblemOperator)i, true);
            int addCorrectAnswerAmount = 0;
            if (int.TryParse(currentCorrectAnswersForOperatorString, out int currentCorrectAnswersForOperatorInt)) // convert string to float
            {
                addCorrectAnswerAmount = currentCorrectAnswersForOperatorInt;
            }

            totalCorrectAnswers += addCorrectAnswerAmount;
            totalMathProblems += addCorrectAnswerAmount;
        }

        //Wrongly Answered Math Problems
        for (int i = 0; i < 5; i++)
        {
            string currentWrongAnswersForOperatorString = PerformanceManager.instance.GetOperatorCount((MathProblemOperator)i, false);
            int addWrongAnswerAmount= 0;
            if (int.TryParse(currentWrongAnswersForOperatorString, out int currentWrongAnswersForOperatorInt)) // convert string to float
            {
                addWrongAnswerAmount = currentWrongAnswersForOperatorInt;
            }

            totalMathProblems += addWrongAnswerAmount;
        }


        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.playMusic(2);

        //Addition
        ShowPerformance(additionSolvingTime, PerformanceManager.instance.GetAverageTime(MathProblemOperator.addition) + " seconds");
        ShowPerformance(additionEvaluation, PerformanceManager.instance.GetOperatorCount(MathProblemOperator.addition, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.addition));
        yield return new WaitForSeconds(0.5f);

        //Subtraction
        ShowPerformance(subtractionSolvingTime, PerformanceManager.instance.GetAverageTime(MathProblemOperator.subtraction) + " seconds");
        ShowPerformance(subtractionEvaluation, PerformanceManager.instance.GetOperatorCount(MathProblemOperator.subtraction, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.subtraction));
        yield return new WaitForSeconds(0.5f);

        //Multiplication
        ShowPerformance(multiplicationSolvingTime, PerformanceManager.instance.GetAverageTime(MathProblemOperator.multiplication) + " seconds");
        ShowPerformance(multiplicationEvaluation, PerformanceManager.instance.GetOperatorCount(MathProblemOperator.multiplication, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.multiplication));
        yield return new WaitForSeconds(0.5f);

        //Division
        ShowPerformance(divisionSolvingTime, PerformanceManager.instance.GetAverageTime(MathProblemOperator.division) + " seconds");
        ShowPerformance(divisionEvaluation, PerformanceManager.instance.GetOperatorCount(MathProblemOperator.division, true) + " / " + PerformanceManager.instance.GetOperatorCount(MathProblemOperator.division));
        yield return new WaitForSeconds(0.5f);

        //Overall
        ShowPerformance(totalMathProblemsValue, totalCorrectAnswers.ToString() + " / " + totalMathProblems.ToString());
        ShowPerformance(totalSolvingTimeValue, PerformanceManager.instance.GetAverageTime(MathProblemOperator.none) + " seconds");
        yield return new WaitForSeconds(0.5f);

        //if passed
        if (resultStars.Count >= 3)
        {
          
            continueButton.SetActive(true);
            round++;
            

        }
        else
        {
            //play timeline (cutscene bankrupted)
            restartButton.SetActive(true);
            
        }
        quitButton.SetActive(true);

    }
    public IEnumerator FitStarToSlot(GameObject p_spawnedStarFill, Vector2 p_starSlotSize)
    {
    
        WaitForSeconds timeRate = new WaitForSeconds(1f / fpsCount);
       
        float stepAmount =starToSlotSpeed/ (fpsCount * fitStarToSlotDuration);

 

  

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
    public GameObject CreateGameStarFill(GameObject p_selectedStarSlot)
    {
        //Spawn new star
        Debug.Log("Spawn Star");
        GameObject spawnedStarFill = Instantiate(starFillPrefab, gameStarRatingContainer.transform);
        spawnedStarFill.transform.position = p_selectedStarSlot.transform.position;
        //Set size of new star (smaller)
        
        gameStars.Add(spawnedStarFill);
        return spawnedStarFill;
    }

    public GameObject CreateStarFill(GameObject p_selectedStarSlot)
    {
        //Spawn new star
        GameObject spawnedStarFill = Instantiate(starFillPrefab, starRatingContainer.transform);
        spawnedStarFill.transform.position = p_selectedStarSlot.transform.position;
        resultStars.Add(spawnedStarFill);
        //Reference the selected star slot's size
        Vector2 selectedStarSlotSize = p_selectedStarSlot.GetComponent<RectTransform>().sizeDelta;

        //Set the spawned star's size to 3x the selected star slot's size
        spawnedStarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(selectedStarSlotSize.x*3f, selectedStarSlotSize.y*3f);
        
        //Set transparency/Opacity of new star
        var desiredColor = spawnedStarFill.GetComponent<Image>().color;
        desiredColor.a = 0f;
        spawnedStarFill.GetComponent<Image>().color = desiredColor;


        //Create new Implosion Particle FX
        GameObject spawnedImplosionParticleFX = Instantiate(implosionFXPrefab, starRatingContainer.transform);
        spawnedImplosionParticleFX.transform.position = p_selectedStarSlot.transform.position;
        Destroy(spawnedImplosionParticleFX, 3f);
       


        //Create new Shimmer Particle FX
        GameObject spawnedShimmerParticleFX = Instantiate(shimmerFXPrefab, starRatingContainer.transform);
        spawnedShimmerParticleFX.transform.position = p_selectedStarSlot.transform.position;
        Destroy(spawnedShimmerParticleFX, 3f);

        GameObject spawnedLightBeamParticleFX = Instantiate(lightbeamFXPrefab, starRatingContainer.transform);
        spawnedLightBeamParticleFX.transform.position = p_selectedStarSlot.transform.position;
        Destroy(spawnedLightBeamParticleFX, 3f);

        GameObject spawnedBlindingTwinkleParticleFX = Instantiate(blindingTwinkleFXPrefab, starRatingContainer.transform);
        spawnedBlindingTwinkleParticleFX.transform.position = p_selectedStarSlot.transform.position;
        Destroy(spawnedBlindingTwinkleParticleFX, 3f);
        return spawnedStarFill;
    }

    
    public void UpdateGameScoreGoal()
    {
        int currentScaledScore = score;


        currentScaledScore = score - (scoreGoal / 5 * gameStarSlotIndex);



        int scoreToNextGoal = (scoreGoal / 5) - (currentScaledScore);
        while (scoreToNextGoal < 1)
        {
            scoreToNextGoal += (scoreGoal / 5);

            if (gameStarSlotIndex < 5)
            {
               
                GameObject newStarFill = CreateGameStarFill(gameStarSlots[gameStarSlotIndex]);
                newStarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(gameStarSlots[gameStarSlotIndex].GetComponent<RectTransform>().sizeDelta.x, gameStarSlots[gameStarSlotIndex].GetComponent<RectTransform>().sizeDelta.y);
                gameStarSlotIndex++;


            }
        }

        gameScoreGoalText.text = scoreToNextGoal.ToString();

    }

    public void SetScore(int p_newScore)
    {
        score = p_newScore;
        gameTipJarFill.fillAmount = (float)score/(float)scoreGoal;
        
        scoreText.text = score.ToString();
        
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
        CoinActivator(gainScore);
        
        score += gainScore;
        scoreText.text = score.ToString();
        gameTipJarFill.fillAmount = (float)score/(float)scoreGoal;
        UpdateGameScoreGoal();
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        defaultScoreFloaterPos = scoreFloater.transform.position;

        
    }
    public void OnEnable()
    {
       
        GameManager.OnGameStart += OnGameStarted;
        GameManager.OnGameEnd += OnGameEnded;
    }
    public void OnDisable()
    {
        
        GameManager.OnGameStart -= OnGameStarted;
        GameManager.OnGameEnd -= OnGameEnded;
    }

    void OnGameStarted()
    {
        failPrompt.SetActive(false);
        restartButton.SetActive(false);
        continueButton.SetActive(false);
        //Delete all stars made
        if (resultStars.Count > 0)
        {
            foreach (GameObject selectedStar in resultStars)
            {
                
                Destroy(selectedStar);
            }
            resultStars.Clear();
        }

        if (gameStars.Count > 0)
        {
            foreach (GameObject selectedStar in gameStars)
            {

                Destroy(selectedStar);
            }
            gameStars.Clear();
        }
        gameStarSlotIndex = 0;
        SetScore(0); //TEMP
        starRatingContainer.gameObject.GetComponent<Canvas>().sortingOrder = 110;

        //round's day
        resultDayText.text = (round + 1).ToString();

        //score floater
        scoreFloater.SetActive(false);
        scoreFloater.transform.position = defaultScoreFloaterPos;
        scoreFloater.GetComponent<Text>().color = new Color(scoreFloater.GetComponent<Text>().color.r, scoreFloater.GetComponent<Text>().color.g, scoreFloater.GetComponent<Text>().color.b, 0f);
        gameDayText.text = "Day: " + (round + 1).ToString();
        //scoreGoal = 1000 + ((round - 1) * (250)); //LAT

       

        ResetMultiplier();
       
    }

    void OnGameEnded()
    {
        PerformanceManager.instance.answeredProblemDatas.Clear();
        

    }
    public void starCheck()
    {
        if(score % 300 == 0 && resultStars[starIndex] != null)
        {
            StarAnimation();
            resultStars[starIndex].transform.GetComponent<Image>().sprite = starShine;
            starIndex++;
        }
        else
        {
            Debug.Log(null);
        }

    }

    public void Results()
    {
     
        totalMathProblemsValue.gameObject.SetActive(false);
  
        totalSolvingTimeValue.gameObject.SetActive(false);
        additionSolvingTime.gameObject.SetActive(false);
        additionEvaluation.gameObject.SetActive(false);
        subtractionSolvingTime.gameObject.SetActive(false);
        subtractionEvaluation.gameObject.SetActive(false);
        multiplicationSolvingTime.gameObject.SetActive(false);
        multiplicationEvaluation.gameObject.SetActive(false);
        divisionSolvingTime.gameObject.SetActive(false);
        divisionEvaluation.gameObject.SetActive(false);
        resultDayText.text = (round+1).ToString();
        continueButton.SetActive(false);
        quitButton.SetActive(false);
        ShowResults(score);
        TransitionManager.instances.changeTransform.gameObject.SetActive(false);
        TransitionManager.instances.noteBookTransform.gameObject.SetActive(false);
       
       
      

    }

    public void StarAnimation()
    {
        for(int i = 0; i < resultStars.Count; i++)
        {
            resultStars[i].gameObject.transform.DOShakeScale(1,1,10,90,true);
        }
    }

    public void TextAnimaation() 
    {
       scoreText.gameObject.transform.DOShakeScale(1, 0.3f,10, 90, true);
    }

   public void CoinActivator(int p_gainedScore)
    {
       // TransitionManager.instances.MoveTransition();
        for(int i = 0; i < ObjectPool.instances.amountToPool; i++)
        {
             ObjectPool.instances.pooledGameobjects[i].SetActive(true);
            // ObjectPool.instances.pooledGameobjects[i].
            // CoinAnimation(i);
           ObjectPool.instances.RandomPosition();
          StartCoroutine(CoinAnimation(i,p_gainedScore));

        }

    }

    IEnumerator ScoreFloating(float p_duration, int p_gainedScore)
    {
        Sequence sequence = DOTween.Sequence();
        scoreFloater.SetActive(true);
        
        scoreFloater.GetComponent<Text>().text = "+ " + p_gainedScore;
        //scoreFloater.SetActive(true);
        //Move
        sequence.Append(scoreFloater.transform.DOMove(new Vector3(scoreFloater.transform.position.x, scoreFloater.transform.position.y + 58f, scoreFloater.transform.position.z), p_duration + 0.5f));//(modifiedScale, sizeTweenSpeed).SetEase(Ease.Linear)).Append(timeValueUI.transform.DOScale(OriginalScale, 0.2f).SetEase(Ease.Linear));

        //FadeIn
        sequence.Append(scoreFloater.GetComponent<Text>().DOFade(0.0f, p_duration * 0.05f));
        sequence.Play();
        yield return new WaitForSeconds(p_duration *0.5f);
        scoreFloater.GetComponent<Text>().DOFade(1.0f, p_duration * 0.45f);
        

        yield return new WaitForSeconds(p_duration * 0.65f);
        //FadeOut
        
        scoreFloater.SetActive(false);
    }

    IEnumerator CoinAnimation(int index, int p_gainedScore)
    {

        Tween tween = ObjectPool.instances.pooledGameobjects[index].GetComponent<Transform>().DOMove(new Vector3(targetLocation.transform.position.x, targetLocation.transform.position.y, targetLocation.transform.position.z), 0.2f);
        yield return tween.WaitForCompletion();

        ObjectPool.instances.pooledGameobjects[index].SetActive(false);
        ObjectPool.instances.ResetPosition();
        StartCoroutine(ScoreFloating(2f,p_gainedScore));
        // yield return null;
    }


}

//public void HighscoreNameEntered(string p_entered)
//{


//    hsTable.AddHighscoreEntry(score,p_entered);
//    string jsonString = PlayerPrefs.GetString("highscoreTable");
//    Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

//    if (highscores != null)
//    {

//        //Sort
//        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
//        {
//            for (int ii = i + 1; ii < highscores.highscoreEntryList.Count; ii++)
//            {
//                if (highscores.highscoreEntryList[ii].score > highscores.highscoreEntryList[i].score)
//                {
//                    //Swap
//                    HighscoreEntry temporaryHighscoreEntry = highscores.highscoreEntryList[i];
//                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[ii];
//                    highscores.highscoreEntryList[ii] = temporaryHighscoreEntry;
//                    //hsTable.highscoresList[ii] = highscores.highscoreEntryList[ii].score;
//                    //hsTable.highscoresList[i] = highscores.highscoreEntryList[i].score;
//                }
//            }
//        }

//        hsTable.highscoreEntryTransformList = new List<Transform>();
//        foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
//        {
//            hsTable.CreateHighscoreEntryTransform(selectedHighscoreEntry, hsTable.entryContainer, hsTable.highscoreEntryTransformList);

//        }
//    }
//    enterHighscoreUI.SetActive(false);

//}