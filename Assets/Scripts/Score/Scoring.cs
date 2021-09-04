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
    public Text performanceFactName;
    public Text performanceFactValue;
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
        PerformanceManager.instance.ChoosePerformanceFact();
        PerformanceManager.instance.answeredProblemDatas.Clear();
    }
}
