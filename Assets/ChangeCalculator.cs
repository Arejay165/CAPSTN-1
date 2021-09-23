using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public Text numeratorText;
    public Text denominatorText;
    public BillCounter billCounter;
    public InputField changeInputField;
    public Item cash;
    public bool isCountingTime;
    public float timeSpent;

    private void OnEnable()
    {
        GameManager.instance.orderSheetShowing = true;
        
        cash = GameManager.instance.customer.itemsWanted[0];
        numeratorText.text = cash.numValue.ToString();
        denominatorText.text = cash.denValue.ToString();
        isCountingTime = true;
    }

    private void OnDisable()
    {
        billCounter.isChangeUIActive = false;
        changeInputField.text = "";
        changeInputField.Select();
      
    }
    private void Update()
    {
        if (isCountingTime)
        {
            timeSpent += Time.deltaTime;
  
        }

    }
    
    public void OnPriceInputted()
    {

        string playerInputString = changeInputField.text;

        float playerInputValue = -1;

        if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
        {

            playerInputValue = inputVal;
        }
        if (playerInputValue != -1)
        {
            if (playerInputValue == cash.price)
            {
                //Answer is correct
                ChangeOrderFinish();
                Scoring.instance.addScore(100);
                RecordAnswerResult(MathProblemOperator.division, true);
                Debug.Log("Is Correct");
            }
            else
            {
                Debug.Log("Isincorrect");
                RecordAnswerResult(MathProblemOperator.division, false);
                changeInputField.text = "";
                changeInputField.Select();
            }
        }
     }

    public void RecordAnswerResult(MathProblemOperator p_mathOperator, bool p_isCorrect)
    {
        AnsweredProblemData newAnswer = new AnsweredProblemData();


        newAnswer.mathOperator = p_mathOperator;
        newAnswer.isCorrect = p_isCorrect;
        newAnswer.timeSpent = timeSpent;
        timeSpent = 0;
        PerformanceManager.instance.answeredProblemDatas.Add(newAnswer);


    }

    public void ChangeOrderFinish()
    {
        TransitionManager.instances.MoveTransition(new Vector2(523f, 1386f), 1f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, false);
        if (GameManager.instance.customer)
        {
            Destroy(GameManager.instance.customer.gameObject);
        }
        GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
    }
}
