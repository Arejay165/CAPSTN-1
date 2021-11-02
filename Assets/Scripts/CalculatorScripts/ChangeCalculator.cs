using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ChangeCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public Text numeratorText;
    public Text denominatorText;
    public BillCounter billCounter;
 
    public TMP_InputField tmpChangeInputField;
   
    string text = "";
    string validCharacters = "0123456789";

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
        int divisor;// use to multiply to the quotient to always be whole number 
        int quotient; // possible answers 
        int dividend;// determine the dividend

        if (TutorialManager.instance.enabled == false)
        {
             divisor = Random.Range(2, 51); // use to multiply to the quotient to always be whole number 
             quotient = Random.Range(1, 20); // possible answers 
             dividend = divisor * quotient; // determine the dividend
        }
        else
        {
            divisor = /*Random.Range(2, 11)*/ 2; // use to multiply to the quotient to always be whole number 
            quotient = /*Random.Range(1, 21)*/ 10; // possible answers 
            dividend = divisor * quotient; // determine the dividend
        }

        tmpChangeInputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };

        MathProblemManager.instance.cash.numValue = dividend;//Random.Range(60, 500);
        MathProblemManager.instance.cash.denValue = divisor;//cash.numValue / Random.Range(2, 50);
        MathProblemManager.instance.cash.price = quotient;//cash.numValue / cash.denValue;
        tmpChangeInputField.characterLimit = 5; //can only reach 10000
        tmpChangeInputField.Select();
    
        numeratorText.text = MathProblemManager.instance.cash.numValue.ToString();
        denominatorText.text = MathProblemManager.instance.cash.price.ToString();
        isCountingTime = true;
        perfectAttempts = 1;
        StartCoroutine(InputFieldSelect());
        tmpChangeInputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

      //  Debug.Log("Enable Change Calculator");

        if (TutorialManager.instance.enabled == true )
        {
            TutorialManager.instance.text.text = "Divide the upper value to the lower value";
            TutorialManager.instance.dialogueBox.anchoredPosition = TutorialManager.instance.convoTransform[2].anchoredPosition;
            UIManager.instance.inGameUI.SetActive(false);
            TutorialManager.instance.tutorial.SetActive(true);
            TutorialManager.instance.nextButton.SetActive(false);
            TutorialManager.instance.tutorial.GetComponent<Image>().raycastTarget = false;
            TutorialManager.instance.itemMask.SetActive(true);
            TutorialManager.instance.ItemMasksActivator(5);
        }
        else
        {
          //  Debug.Log("is null");
        }

        AudioManager.instance.playSound(9);
    }

    private void OnDisable()
    {
        tmpChangeInputField.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };
        tmpChangeInputField.text = "";
        tmpChangeInputField.Select();

        AudioManager.instance.playSound(10);
    }

    private char MyValidate(string validCharacters, char charToValidate)
    {
        //Checks if a dollar sign is entered....
        if (validCharacters.IndexOf(charToValidate) != -1)
        {
            //valid characters
            return charToValidate;
        }
        else
        {
            // ... if it is change it to an empty character.
            charToValidate = '\0';
        }
        return charToValidate;
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
        yield return new WaitForEndOfFrame();
        tmpChangeInputField.enabled = true;
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
                   // Debug.Log("Is Correct");


                }
                else
                {
                    //Debug.Log("Isincorrect");
                    StartCoroutine(WrongInputted(tmpChangeInputField));
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    mc.DeductCurrentMoodAmount(mc.penaltyTime);
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

        TransitionManager.instances.MoveTransition(new Vector2(680f, 1387f), 0.5f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, false);
        //Customer despawn
        if (GameManager.instance.customer)
        {
            //Disable customer bubble
            GameManager.instance.customer.panel.gameObject.SetActive(false);

            //Disable customer mood bar
            GameManager.instance.customer.moodPanel.SetActive(false);
            //animation
            DOTween.Sequence().Append(GameManager.instance.customer.gameObject.transform.DOMove(GameManager.instance.customerSpawner.outShopPoint.position, 0.5f, false));
            Destroy(GameManager.instance.customer.gameObject,1.5f);
            GameManager.instance.customer = null;
        }

        ///    GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());

        if (!TutorialManager.instance.canTutorial)
        {
            GameManager.instance.customerSpawner.SpawnCustomer(); //No waiting time 
        }
        else
        {

            TutorialManager.instance.ActivateTutorialUI();
            TutorialManager.instance.customerCounter = 1;
            TutorialManager.instance.tutorial.SetActive(true);
            TutorialManager.instance.nextButton.SetActive(true);
            TutorialManager.instance.itemMask.SetActive(false);
        }
     
    }
}
