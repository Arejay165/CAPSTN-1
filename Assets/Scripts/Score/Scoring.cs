using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public static Scoring   instance;
    public GameObject[]     Stars;
    [SerializeField] 
    private int             score;
    [SerializeField]
    Text                    scoreUI;
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

    public int GetScore()
    {
        return score;
    }

    public void addScore(int gainScore)
    {
        score += gainScore;
        scoreUI.text = score.ToString();
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
        if(score % 300 == 0 && Stars[starIndex] != null)
        {
            Stars[starIndex].transform.GetComponent<Image>().sprite = starShine;
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
}
