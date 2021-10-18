using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public Text numeratorText;
    public Text denominatorText;
    public BillCounter billCounter;
    public InputField changeInputField;
    public TMP_InputField tmpChangeInputField;
   
    public bool isCountingTime;
    public float timeSpent;

    public int answerAttempts;
    public int perfectAttempts;
    private void OnEnable()
    {
        //Test for whole number answer for Division 
        //dividend / divisor = quotient (answer of player)
        // Reverse the division to multiplication
        // divisor * quotient = dividend
        int divisor = Random.Range(2, 51); // use to multiply to the quotient to always be whole number 
        int quotient = Random.Range(1, 50); // possible answers 
        int dividend = divisor * quotient; // determine the dividend
                                           

        MathProblemManager.instance.cash.numValue = dividend;//Random.Range(60, 500);
        MathProblemManager.instance.cash.denValue = divisor;//cash.numValue / Random.Range(2, 50);
        MathProblemManager.instance.cash.price = quotient;//cash.numValue / cash.denValue;

        tmpChangeInputField.Select();
    
        numeratorText.text = MathProblemManager.instance.cash.numValue.ToString();
        denominatorText.text = MathProblemManager.instance.cash.price.ToString();
        isCountingTime = true;
        perfectAttempts = 1;
        StartCoroutine(InputFieldSelect());
        tmpChangeInputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

        Debug.Log("Enable Change Calculator");

        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.screenText.text = "Divide the upper value to the lower value";

        }
    }

    private void OnDisable()
    {

        tmpChangeInputField.text = "";
        tmpChangeInputField.Select();
   

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
        tmpChangeInputField.Select();
        tmpChangeInputField.ActivateInputField();


    }




    public void OnPriceInputted()
    {
        if (gameObject.activeSelf)
        {
            tmpChangeInputField.Select();
            string playerInputString = tmpChangeInputField.text;

            float playerInputValue = -1;

            if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
            {

                playerInputValue = inputVal;
            }
            if (playerInputValue != -1)
            {

                if (playerInputValue == MathProblemManager.instance.cash.denValue)
                {
                    //Answer is correct
                    StartCoroutine(SheetCompleted(tmpChangeInputField));

                    answerAttempts++;
                 

                    Scoring.instance.addScore((int)(100 * Scoring.instance.multiplier));
                    if (answerAttempts == perfectAttempts)
                    {
                        Scoring.instance.ModifyMultiplier(1f);
                    }
                    RecordAnswerResult(MathProblemOperator.division, true);

                    AudioManager.instance.playSound(0);
                    Debug.Log("Is Correct");


                }
                else
                {
                    //Debug.Log("Isincorrect");
                    StartCoroutine(WrongInputted(tmpChangeInputField));
                    RecordAnswerResult(MathProblemOperator.division, false);
                    Scoring.instance.ModifyMultiplier(-1f);
                }


            }
        }
     }

    IEnumerator SheetCompleted(TMP_InputField p_inputField)
    {
        int blinkCount = 0;
        while (blinkCount < 3)
        {
            p_inputField.gameObject.GetComponent<Image>().color = new Color(0f, 255f, 0f);
            yield return new WaitForSeconds(0.15f);
            p_inputField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
            yield return new WaitForSeconds(0.075f);
            blinkCount++;
        }
        p_inputField.gameObject.GetComponent<Image>().color = new Color(0f, 255f, 0f);
       // yield return new WaitForSeconds(0.5f);
        ChangeOrderFinish();
    }

    IEnumerator WrongInputted(TMP_InputField p_inputField)
    {
        //PlayerManager.instance.Shake(Camera.main.gameObject,0.15f, 0.05f, 0.25f);
        AudioManager.instance.playSound(1);
        PlayerManager.instance.Shake(gameObject, 0.2f, 3.5f, 1.5f);
        int blinkCount = 0;
        while (blinkCount < 3)
        {
            p_inputField.gameObject.GetComponent<Image>().color = new Color(255f, 0f, 0f);
            yield return new WaitForSeconds(0.1f);
            p_inputField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);

            yield return new WaitForSeconds(0.05f);
            blinkCount++;
        }
        p_inputField.text = "";
     
        tmpChangeInputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
        p_inputField.Select();
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

        TransitionManager.instances.MoveTransition(new Vector2(-523f, 1386f), 0.5f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, false);
        if (GameManager.instance.customer)
        {
            Destroy(GameManager.instance.customer.gameObject);
        }

        ///    GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());

        if (!TutorialManager.instance.canTutorial)
        {
            GameManager.instance.customerSpawner.SpawnCustomer(); //No waiting time 
        }
        else
        {

            TutorialManager.instance.ActivateTutorialUI();

            TutorialManager.instance.playTutorial = false;
            TutorialManager.instance.customerCounter = 1;
        }
     
    }
}