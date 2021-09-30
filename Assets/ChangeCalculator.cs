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

    public int answerAttempts;
    public int perfectAttempts;
    private void OnEnable()
    {
       
        GameManager.instance.orderSheetShowing = true;
        
        cash = GameManager.instance.customer.itemsWanted[0];
        numeratorText.text = cash.numValue.ToString();
        denominatorText.text = cash.price.ToString();
        isCountingTime = true;
        perfectAttempts = 1;

        StartCoroutine(InputFieldSelect());
        changeInputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
        Debug.Log("Enable Change Calculator");
    }

    private void OnDisable()
    {
        billCounter.isChangeUIActive = false;
        changeInputField.text = "";
       
        // changeInputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
        Debug.Log("Disable Change Calculator");
    }
    private void Update()
    {
        if (isCountingTime)
        {
            timeSpent += Time.deltaTime;
  
        }

    }
    
    IEnumerator InputFieldSelect()
    {
        yield return 0;
        changeInputField.Select();
        changeInputField.ActivateInputField();


    }

    IEnumerator WrongInputted(InputField p_inputField)
    {
        p_inputField.gameObject.GetComponent<Image>().color = new Color(255f, 0f, 0f);
        yield return new WaitForSeconds(0.1f);
        p_inputField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);

        yield return new WaitForSeconds(0.05f);

        p_inputField.gameObject.GetComponent<Image>().color = new Color(255f, 0f, 0f);
        yield return new WaitForSeconds(0.1f);
        p_inputField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);

        yield return new WaitForSeconds(0.05f);

        p_inputField.gameObject.GetComponent<Image>().color = new Color(255f, 0f, 0f);
        yield return new WaitForSeconds(0.1f);
        p_inputField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);

        yield return new WaitForSeconds(0.05f);
        //yield return new WaitForSeconds(1f);

        p_inputField.text = "";
        p_inputField.Select();
        p_inputField.ActivateInputField();
        p_inputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

    }

    public void OnPriceInputted()
    {
        changeInputField.Select();

        string playerInputString = changeInputField.text;

        float playerInputValue = -1;

        if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
        {

            playerInputValue = inputVal;
        }
        if (playerInputValue != -1)
        {

            if (playerInputValue == cash.denValue)
            {
                //Answer is correct
                ChangeOrderFinish();
                answerAttempts++;
                if (answerAttempts == perfectAttempts)
                {
                    Scoring.instance.ModifyMultiplier(1f);
                }
                
                Scoring.instance.addScore((int) (100 * Scoring.instance.multiplier));
                RecordAnswerResult(MathProblemOperator.division, true);
                Debug.Log("Is Correct");
                
            }
            else
            {
                Debug.Log("Isincorrect");
                RecordAnswerResult(MathProblemOperator.division, false);
                changeInputField.text = "";
                StartCoroutine(WrongInputted(changeInputField));
                Scoring.instance.ModifyMultiplier(-1f);
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
        TransitionManager.instances.MoveTransition(new Vector2(-523f, 1386f), 1f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, false);
        if (GameManager.instance.customer)
        {
            Destroy(GameManager.instance.customer.gameObject);
        }
        GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
        GameManager.instance.orderSheetShowing = false;
    }
}
