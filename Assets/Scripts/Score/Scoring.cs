using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Scoring : MonoBehaviour
{
    public static Scoring   instance;
    public GameObject[]     stars;
    [SerializeField] 
    private int             score;
    [SerializeField]
    Text                    scoreText;
    public Sprite starShine;

    public int starIndex;
    public GameObject[] endStars;
    public Text endScoreText;
    public Sprite failImage;
    public Image levelPasserImage;
    public GameObject failPrompt, successPrompt;
    //public Text performanceFactName;
    //public Text performanceFactValue;

    public Text customersEntertained;
    public Text totalSolvingTime;
    public Text additionSolvingTime;
    public Text additionEvaluation;
    public Text subtractionSolvingTime;
    public Text subtractionEvaluation;
    public Text multiplicationSolvingTime;
    public Text multiplicationEvaluation;
    public Text divisionSolvingTime;
    public Text divisionEvaluation;

    public void SetScore(int p_newScore)
    {
        score = p_newScore;
    }
    public int GetScore()
    {
        return score;
    }

    public void addScore(int gainScore)
    {
        TextAnimaation();
        score += gainScore;
        scoreText.text = score.ToString();
      
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        GameManager.instance.score = score;
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
        if(starIndex > 0)
        {
            endScoreText.text = score.ToString();
            for (int i = 0; i < starIndex; i++)
            {
                endStars[i].transform.GetComponent<Image>().sprite = starShine;
            }
        }
        else
        {
            endScoreText.text = score.ToString();
            levelPasserImage.sprite = failImage;
            failPrompt.SetActive(true);
            successPrompt.SetActive(false);
            
        }
        //PerformanceManager.instance.ChoosePerformanceFact();
        customersEntertained.text = "Customer Entertained: " + PerformanceManager.instance.customersEntertained.ToString();
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
        PerformanceManager.instance.customersEntertained = 0;
    }

    public void StarAnimation()
    {
        for(int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.transform.DOShakeScale(1,1,10,90,true);
        }
    }

    public void TextAnimaation() 
    {
       scoreText.gameObject.transform.DOShakeScale(1, 0.3f,10, 90, true);
    }
}
