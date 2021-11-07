using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

[System.Serializable]
public class ItemUIClass
{
    public bool isCorrect;
    public Sprite icon;
    public string name;
    public float price;
    public float quantity;
    public float totalPriceAnswer;
}
public class TestCalculator : MonoBehaviour
{
    [Header("Stats")]
    public int answerAttempts;
    public int perfectAttempts;
    public List<ItemUIClass> itemUIClassList = new List<ItemUIClass>(); // Datas for the Item UIs that will be in the answer pad
    public List<float> itemsAnswer = new List<float>();
    public List<float> changeAnswers = new List<float>();

    [SerializeField] List<TMP_InputField> answerFields = new List<TMP_InputField>();

    [SerializeField] TMP_InputField totalPriceAnswerField, changeAnswerField;
    [SerializeField] GameObject customerPaidTitle;
    [SerializeField] Text customerPaidText,changeText;
    [SerializeField] float totalPriceCorrectAnswer, changeCorrectAnswer;
    bool totalPriceIsCorrect, changeIsCorrect;

    [SerializeField] GameObject itemDisplay;
    [SerializeField] Transform displayPanel;

    string text = "";
    string validCharacters = "0123456789";

    public float randomExtraMoney;
    public bool isCountingTime;
    public float timeSpent;
    public int index;
    ItemUIClass CreateItemUI(Item p_item)
    {
        
        ItemUIClass newItemUIClass = new ItemUIClass();
        newItemUIClass.isCorrect = false;
        newItemUIClass.icon = p_item.itemSprite;
        newItemUIClass.name = p_item.itemName;
        newItemUIClass.price = p_item.price;
        newItemUIClass.quantity = p_item.quantity;
        newItemUIClass.totalPriceAnswer = newItemUIClass.quantity * newItemUIClass.price;
        itemsAnswer.Add(newItemUIClass.totalPriceAnswer);
        
        return newItemUIClass;
    }
    private void Update()
    {
        if (isCountingTime)
        {
            timeSpent += Time.deltaTime;

        }
        
    }
    //private void Start()
    //{
    //    StackDuplicateItems();
    //}

    private void OnEnable()
    {

        ////Register OnGameStart Event in GameManager
        //GameManager.OnGameStart += OnGameStarted;
        //foreach (InputField selectedAnswerField in answerFields)
        //{
        //    Destroy(selectedAnswerField.gameObject);
        //}
        //clears ui list
        
        totalPriceAnswerField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };
        changeAnswerField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };

        foreach (Transform child in displayPanel.transform)
        {

            child.gameObject.SetActive(false);
            Destroy(child.gameObject);

        }
        //clears answerfields
        if (answerFields.Count > 0)
        {
            foreach (TMP_InputField selectedItemUI in answerFields)
            {
                Destroy(selectedItemUI.transform.parent.gameObject);
            }
            answerFields.Clear();
        }
    

        itemUIClassList.Clear();
        itemsAnswer.Clear();
        changeAnswers.Clear();
        totalPriceCorrectAnswer = 0;
        changeCorrectAnswer = 0;
        isCountingTime = false;
        timeSpent = 0;

        totalPriceAnswerField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
        changeAnswerField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
        //resets item input field index counter to 0
        index = 0;

        //clears change UI
        changeAnswerField.text = "";
        changeAnswerField.gameObject.SetActive(false);
        changeText.gameObject.SetActive(false);

        //clear total price UI
        totalPriceAnswerField.text = "";



        totalPriceAnswerField.enabled = false;
        changeAnswerField.gameObject.SetActive(false);
        changeText.gameObject.SetActive(false);
        customerPaidText.gameObject.SetActive(false);
        customerPaidTitle.gameObject.SetActive(false);
        StackDuplicateItems();

        

        if(answerFields.Count > 0)
        {
            answerFields[0].Select();
            answerFields[0].GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
        }
        isCountingTime = true;

        if (TutorialManager.instance.enabled == true)
        {
            TutorialManager.instance.text.text = "Multiply Quantity to item's price";
            TutorialManager.instance.dialogueBox.anchoredPosition = TutorialManager.instance.convoTransform[2].anchoredPosition;
            UIManager.instance.inGameUI.SetActive(false);
            TutorialManager.instance.tutorial.SetActive(true);
            TutorialManager.instance.nextButton.SetActive(false);
            TutorialManager.instance.tutorial.GetComponent<Image>().raycastTarget = false;
            TutorialManager.instance.ItemMasksActivator(0);

        }
        else
        {
          //  Debug.Log("Null");
        }

        AudioManager.instance.playSound(9);
    }

    private void OnDisable()
    {
        //clears answerfields
        if (answerFields.Count > 0)
        {
            foreach (TMP_InputField selectedItemUI in answerFields)
            {
                Destroy(selectedItemUI.transform.parent.gameObject);
            }
            answerFields.Clear();
        }
        foreach (TMP_InputField selectedAnswerField in answerFields)
        {
            selectedAnswerField.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };
        }
        totalPriceAnswerField.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };
        changeAnswerField.onValidateInput -= delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };
       
        GameManager.OnGameStart -= OnGameStarted;

        AudioManager.instance.playSound(10);

    }

    public void OnGameStarted()
    {
        
        StopAllCoroutines();
     

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

    //private void OnDisable()
    //{
    //    itemUIClassList.Clear();
    //    totalPriceCorrectAnswer = 0;
    //    changeCorrectAnswer = 0;
    //    Destroy(GameManager.instance.customer.gameObject);
    //    GameManager.instance.customerSpawner.SpawnCustomer();
    //}
    public void StackDuplicateItems()
    {
        Customer customer = GameManager.instance.customer;
        answerAttempts = 0;
        perfectAttempts = 0;
       
        for (int x = 0; x < MathProblemManager.instance.GetGeneratedItemsWanted().Count; x++)
        {
            bool uniqueItem = true;
           
            for (int i = 0; i < itemUIClassList.Count; i++)
            {
               // Debug.Log("Customer item check count: " + customer.itemCheck.Count + " Item UI Count : " + itemUIClassList.Count + "Customer Item Check x " + customer.itemCheck[x].itemName);
                if (itemUIClassList[i].name == MathProblemManager.instance.GetItemInGeneratedItemsWanted(x).itemName)//It's not a unique item (There is a duplicate copy already in the list)
                {
                   // Debug.Log(" Item UI Count : " + itemUIClassList.Count + "Customer Item Check x " + customer.itemCheck[x].itemName);
                    uniqueItem = false;
                    itemUIClassList[i].quantity += MathProblemManager.instance.GetItemInGeneratedItemsWanted(x).quantity;
                    itemUIClassList[i].totalPriceAnswer = itemUIClassList[i].quantity * itemUIClassList[i].price;
                    //  return;
                    break;
                }
            }
            if (uniqueItem == true)//If it's a unique item (No duplicates yet in the list)
            {
                itemUIClassList.Add(CreateItemUI(MathProblemManager.instance.GetItemInGeneratedItemsWanted(x)));
                perfectAttempts++;
            }
            uniqueItem = true;
        }

      
        for (int i = 0; i < itemUIClassList.Count; i++)
        {
            totalPriceCorrectAnswer += itemUIClassList[i].totalPriceAnswer;
        }

        perfectAttempts += 2;

        if(TutorialManager.instance.enabled == false)
        {
            randomExtraMoney = totalPriceCorrectAnswer + UnityEngine.Random.Range(0, 100);
        }
        else
        {
            randomExtraMoney = totalPriceCorrectAnswer + 0;
        }
      
        changeCorrectAnswer = randomExtraMoney - totalPriceCorrectAnswer;
        
        DisplayItemOrders();
        customerPaidText.text = randomExtraMoney.ToString();
        


    }

    public int IdentifyAnswerfieldIndex(string p_playerInput)
    {

        int itemOrderIndex = -1;
       
        //Finding which inputfield's text has a matching string with the parameter
        for (int i = 0; i < answerFields.Count; i++)
        {
            if (answerFields[i].text == p_playerInput)
            {
                itemOrderIndex = i;
                return i;
            }
        }
        return -1;
    }

    //public void InputfieldSelected(InputField p_selectedInputField)
    //{
    //    p_selectedInputField.gameObject.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
    //}
    //public void InputFieldDeselected(InputField p_selectedInputField)
    //{
    //    p_selectedInputField.gameObject.GetComponent<Image>().color = Color.white;
    //}

    private IEnumerator WaitForInputActivation(TMP_InputField p_selectedInputField)
    {

        yield return new WaitForEndOfFrame();
        p_selectedInputField.enabled = true;
        p_selectedInputField.Select();
        p_selectedInputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

    }

    public void OnPriceCorrect()
    {
        //Change select to next input field if correct


        index++;
        if (index < answerFields.Count)
        {
            //   Debug.Log("List still has inputfield");
            //answerFields[index].Select();
            StartCoroutine(WaitForInputActivation(answerFields[index]));
            //answerFields[index].GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

            if (TutorialManager.instance.enabled == true)
                TutorialManager.instance.ItemMasksActivator(index);
        }
        else
        {
            //Debug.Log("All correct answer");
            SpawnAnswerField();
        }
     

    }

    public void OnPriceWrong()
    {


        Scoring.instance.ModifyMultiplier(-1f);
    }

    public void OnPriceInputted(string p_playerInputString)
    {
        if (gameObject.activeSelf)
        {

            int itemOrderIndex = IdentifyAnswerfieldIndex(p_playerInputString); //Finding which inputfield is being used to write
            float playerInputValue = -1;

            if (float.TryParse(p_playerInputString, out float inputVal)) // convert string to float
            {
                playerInputValue = inputVal;
            }


          
            if (playerInputValue != -1) // If input is valid (any number)
            {
                //If it matches, it is correct
                if (playerInputValue == itemUIClassList[itemOrderIndex].totalPriceAnswer)
                {
                   // Debug.Log("Correct");
                    RecordAnswerResult(itemUIClassList[itemOrderIndex].totalPriceAnswer, MathProblemOperator.multiplication, true);
                    //add bonus mood
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    mc.IncreaseCurrentMoodAmount( mc.correctBonusTime*4);

                    StartCoroutine(CorrectInputted(answerFields[itemOrderIndex], itemUIClassList[itemOrderIndex].isCorrect, OnPriceCorrect));

                  

                }
                //If it doesnt match its wrong
                else
                {
                    Debug.Log("Wrong");
                    RecordAnswerResult(itemUIClassList[itemOrderIndex].totalPriceAnswer, MathProblemOperator.multiplication, false);
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    mc.DeductCurrentMoodAmount(mc.penaltyTime);
                    StartCoroutine(WrongInputted(answerFields[itemOrderIndex],OnPriceWrong));
                  

                }
                answerAttempts++;
            }
            else //If input is invalid (not a number)
            {
             //   Debug.Log("Invalid Input, retry again");
               // StartCoroutine(WrongInputted(answerFields[itemOrderIndex]));



            }
            InitializedInputField();
        }
    }

    public void InitializedInputField()
    {
        for(int i = 0; i < answerFields.Count; i++)
        {
            
            if(i == index)
            {
                answerFields[i].enabled = true;
            }
            else
            {
                answerFields[i].enabled = false;
            }
        }
    }

    public void SpawnAnswerField()
    {
        if(answerFields.Count == index)
        {
            if (TutorialManager.instance.enabled == true)
                TutorialManager.instance.ItemMasksActivator(3);

            totalPriceAnswerField.enabled = true;
            
           //totalPriceAnswerField.Select();
            StartCoroutine(WaitForInputActivation(totalPriceAnswerField));
            //totalPriceAnswerField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

            if (TutorialManager.instance.enabled == true)
            {
                TutorialManager.instance.text.text = "Add all the totals";

            }
        }
    }

    IEnumerator CorrectInputted(TMP_InputField p_inputField, bool p_correct, Action p_postFunction = null)
    {

        p_inputField.gameObject.GetComponent<Image>().color = new Color(0f, 255f, 0f);
        AudioManager.instance.playSound(0);
        yield return new WaitForSeconds(0.25f);
        p_correct = true;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(680f, 0);
        if (p_postFunction != null)
        {
            p_postFunction.Invoke();
        }
       
    }

    IEnumerator WrongInputted(TMP_InputField p_inputField, Action p_postFunction)
    {

        PlayerManager.instance.Shake(gameObject,0.25f, 3.5f, 1.5f);
    
        int blinkCount = 0;
        while (blinkCount < 3)
        {
            p_inputField.gameObject.GetComponent<Image>().color = new Color(255f, 0f, 0f);
            yield return new WaitForSeconds(0.1f);
            p_inputField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);

            yield return new WaitForSeconds(0.05f);
            blinkCount++;
        }
        if (p_postFunction != null)
        {
            p_postFunction.Invoke();
        }
        AudioManager.instance.playSound(1);
        p_inputField.text = "";
        p_inputField.ActivateInputField();
        p_inputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(680f, 0f);

    }
    //public void RecordAnswerResult(int p_index, bool p_isCorrect)
    //{
    //    AnsweredProblemData newAnswer = new AnsweredProblemData();
    //    newAnswer.operatingNumbers.Add(itemUIClassList[p_index].price);
    //    newAnswer.operatingNumbers.Add(itemUIClassList[p_index].quantity);
    //    newAnswer.answer = itemUIClassList[p_index].totalPriceAnswer;
    //    newAnswer.mathOperator = MathProblemOperator.multiplication;
    //    newAnswer.isCorrect = p_isCorrect;
    //    newAnswer.timeSpent = timeSpent;
    //    timeSpent = 0f;
    //    PerformanceManager.instance.answeredProblemDatas.Add(newAnswer);
    //}
    public void RecordAnswerResult( float p_answer, MathProblemOperator p_mathOperator, bool p_isCorrect)
    {
        AnsweredProblemData newAnswer = new AnsweredProblemData();
       

        newAnswer.answer = p_answer;
        newAnswer.mathOperator = p_mathOperator;
        newAnswer.isCorrect = p_isCorrect;
        newAnswer.timeSpent = timeSpent;
        timeSpent = 0;
        PerformanceManager.instance.answeredProblemDatas.Add(newAnswer);


    }
    //public void RecordAnswerResult(List<float> p_numbers, float p_answer, MathProblemOperator p_mathOperator, bool p_isCorrect)
    //{
    //    AnsweredProblemData newAnswer = new AnsweredProblemData();
    //    foreach (float selectedNumber in p_numbers)
    //    {
    //        newAnswer.operatingNumbers.Add(selectedNumber);
    //    }

    //    newAnswer.answer = p_answer;
    //    newAnswer.mathOperator = p_mathOperator;
    //    newAnswer.isCorrect = p_isCorrect;
    //    newAnswer.timeSpent = timeSpent;
    //    timeSpent = 0;
    //    PerformanceManager.instance.answeredProblemDatas.Add(newAnswer);


    //}
    public void ShowChangeText()
    {
        customerPaidTitle.gameObject.SetActive(true);
        customerPaidText.gameObject.SetActive(true);
        changeText.gameObject.SetActive(true);
        changeAnswerField.gameObject.SetActive(true);
        StartCoroutine(WaitForInputActivation(changeAnswerField));
        //changeAnswerField.Select();
        //changeAnswerField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

        if (TutorialManager.instance.enabled == true )
        {
            TutorialManager.instance.text.text = "Subtract Total Price to Customer Paid";

        }
    }
    public void OnTotalPriceCorrect()
    {
        if (TutorialManager.instance.enabled == true)
            TutorialManager.instance.ItemMasksActivator(4);


        changeAnswers.Add(randomExtraMoney);
        changeAnswers.Add(totalPriceCorrectAnswer);
    }

    public void OnTotalPriceWrong()
    {
        Scoring.instance.ResetMultiplier();
    }
    public void OnTotalPriceInputted()
    {
        if (gameObject.activeSelf)
        {
            string playerInputString = totalPriceAnswerField.text;

            float playerInputValue = -1;


            if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
            {

                playerInputValue = inputVal;


            }
            if (playerInputValue != -1)
            {
                if (playerInputValue == totalPriceCorrectAnswer) // Answer is correct
                {
                    ShowChangeText();
                    RecordAnswerResult(totalPriceCorrectAnswer, MathProblemOperator.addition, true);
                    StartCoroutine(CorrectInputted(totalPriceAnswerField, totalPriceIsCorrect, OnTotalPriceCorrect));

                   
                }
                else // Answer is wrong
                {
                    Debug.Log("RIGHT ANSWER IS : " + totalPriceCorrectAnswer);
                    RecordAnswerResult(totalPriceCorrectAnswer, MathProblemOperator.addition, false);
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    mc.DeductCurrentMoodAmount(mc.penaltyTime);
                    StartCoroutine(WrongInputted(totalPriceAnswerField,OnTotalPriceWrong));
                
                }
                answerAttempts++;
            }
            else
            {
                Debug.Log("Invalid Input, retry again");
//                StartCoroutine(WrongInputted(totalPriceAnswerField));


            }
        }
    }

    public void OnChangeCorrect()
    {

       // changeAnswerField.DeactivateInputField();
        OrderSheetFinish();
        index = 0;
    }

    public void OnChangeWrong()
    {

        Scoring.instance.ResetMultiplier();
    }
    public void OnChangeInputted()
    {
        if (gameObject.activeSelf)
        {
            string playerInputString = changeAnswerField.text;
            float playerInputValue = -1;



            if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
            {
                playerInputValue = inputVal;

            }
            if (playerInputValue != -1)
            {
                answerAttempts++;
                if (playerInputValue == changeCorrectAnswer)
                {
                    RecordAnswerResult(changeCorrectAnswer, MathProblemOperator.subtraction, true);

                    //add bonus mood time
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    mc.IncreaseCurrentMoodAmount( mc.correctBonusTime*4);
                    //changeAnswerField.DeactivateInputField();
                    StartCoroutine(CorrectInputted(changeAnswerField, changeIsCorrect, OnChangeCorrect));
                    


                }
                else// Answer is wrong
                {

                    RecordAnswerResult(changeCorrectAnswer, MathProblemOperator.subtraction, false);
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    mc.DeductCurrentMoodAmount(mc.penaltyTime);
                    StartCoroutine(WrongInputted(changeAnswerField, OnChangeWrong));
                  
                }

            }
            else
            {
                Debug.Log("Invalid Input, retry again");

            //    StartCoroutine(WrongInputted(changeAnswerField));


            }

        }
    }
    public void OrderSheetFinish()
    {

      
        Scoring.instance.addScore((int) (100*Scoring.instance.multiplier));
        if (answerAttempts == perfectAttempts)
        {
            Scoring.instance.ModifyMultiplier(1f);
        }

       
        StartCoroutine(SheetCompleted(changeAnswerField));
    

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
   //     yield return new WaitForSeconds(0.5f);

        //awards score
        TransitionManager.instances.MoveTransition(new Vector2(680f, 1387.0f), 0.5f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, false);
        //   TransitionManager.instances.MoveTransition(new Vector2(507f, 1387.0f), 0.5f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, false);
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

            AudioManager.instance.playSound(11);
        }
     

        if (!TutorialManager.instance.canTutorial)
        {
            //  GameManager.instance.customerSpawner.SpawnCustomer(); //No waiting time
             GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
             
        }
        else
        {
            TutorialManager.instance.ActivateTutorialUI();
            TutorialManager.instance.itemMask.SetActive(false);
        }
     
    }
    public void DisplayItemOrders()
    {

        for (int i = 0; i < itemUIClassList.Count; i++)
        {

            GameObject order = Instantiate(itemDisplay);
            //Setting Image
            order.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = itemUIClassList[i].icon;
            order.transform.GetChild(0).gameObject.GetComponent<Image>().preserveAspect = true;
            //Setting Price Text
            order.transform.GetChild(1).gameObject.GetComponent<Text>().text = itemUIClassList[i].quantity.ToString() + "x";
            //Setting Quantity Text
            order.transform.GetChild(2).gameObject.GetComponent<Text>().text = "P" + itemUIClassList[i].price.ToString();

            //Adding to answerField List
            order.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().onEndEdit.AddListener(OnPriceInputted);
    
            order.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().enabled = true;
            order.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().onValidateInput -= delegate (string input, int charIndex, char addedChar) { return MyValidate(validCharacters, addedChar); };

            answerFields.Add(order.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>());
            answerFields[i].characterLimit = 5; //can only reach 10000
            order.transform.SetParent(displayPanel);
            order.transform.localScale = new Vector3(1f, 1f, 1f);

            
        }


    }

}
