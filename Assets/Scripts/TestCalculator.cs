using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [SerializeField] List<InputField> answerFields = new List<InputField>();

    [SerializeField] InputField totalPriceAnswerField, changeAnswerField;
    [SerializeField] GameObject customerPaidTitle;
    [SerializeField] Text customerPaidText,changeText;
    [SerializeField] float totalPriceCorrectAnswer, changeCorrectAnswer;
    bool totalPriceIsCorrect, changeIsCorrect;

    [SerializeField] GameObject itemDisplay;
    [SerializeField] Transform displayPanel;

 
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
        StackDuplicateItems();
        GameManager.instance.orderSheetShowing = true;
        totalPriceAnswerField.enabled = false;
        changeAnswerField.gameObject.SetActive(false);
        changeText.gameObject.SetActive(false);
        customerPaidText.gameObject.SetActive(false);
        customerPaidTitle.gameObject.SetActive(false);
        if(answerFields.Count > 0)
        {
            answerFields[0].Select();
            answerFields[0].GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
        }
        isCountingTime = true;
    }

    private void OnDisable()
    {
        itemUIClassList.Clear();
        itemsAnswer.Clear();
        changeAnswers.Clear();
        totalPriceCorrectAnswer = 0;
        changeCorrectAnswer = 0;
        isCountingTime = false;
        timeSpent = 0;
        totalPriceAnswerField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
        changeAnswerField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
       
        
 
        GameManager.instance.orderSheetShowing = false;
 
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
        for (int x = 0; x < customer.itemInCart.Count; x++)
        {
            bool uniqueItem = true;
           
            for (int i = 0; i < itemUIClassList.Count; i++)
            {
               // Debug.Log("Customer item check count: " + customer.itemCheck.Count + " Item UI Count : " + itemUIClassList.Count + "Customer Item Check x " + customer.itemCheck[x].itemName);
                if (itemUIClassList[i].name == customer.itemInCart[x].itemName)//It's not a unique item (There is a duplicate copy already in the list)
                {
                   // Debug.Log(" Item UI Count : " + itemUIClassList.Count + "Customer Item Check x " + customer.itemCheck[x].itemName);
                    uniqueItem = false;
                    itemUIClassList[i].quantity += customer.itemInCart[x].quantity;
                    itemUIClassList[i].totalPriceAnswer = itemUIClassList[i].quantity * itemUIClassList[i].price;
                    //  return;
                    break;
                }
            }
            if (uniqueItem == true)//If it's a unique item (No duplicates yet in the list)
            {
                itemUIClassList.Add(CreateItemUI(customer.itemInCart[x]));
                perfectAttempts++;
            }
            uniqueItem = true;
        }

      
        for (int i = 0; i < itemUIClassList.Count; i++)
        {
            totalPriceCorrectAnswer += itemUIClassList[i].totalPriceAnswer;
        }

        perfectAttempts += 2;

        GameManager.instance.customer.randomExtraMoney = totalPriceCorrectAnswer + Random.Range(0, 20);
        changeCorrectAnswer = GameManager.instance.customer.randomExtraMoney - totalPriceCorrectAnswer;
        
        DisplayItemOrders();
        customerPaidText.text = GameManager.instance.customer.randomExtraMoney.ToString();
        

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
    public void OnPriceInputted(string p_playerInputString)
    {
        int itemOrderIndex = IdentifyAnswerfieldIndex(p_playerInputString); //Finding which inputfield is being used to write
        float playerInputValue = -1;
        
        if (float.TryParse(p_playerInputString, out float inputVal)) // convert string to float
        {
            playerInputValue = inputVal;
        }
        


        if (playerInputValue != -1 ) // If input is valid (any number)
        {
            //If it matches, it is correct
            if (playerInputValue == itemUIClassList[itemOrderIndex].totalPriceAnswer)
            {
                Debug.Log("Correct");

                StartCoroutine(CorrectInputted(answerFields[itemOrderIndex], itemUIClassList[itemOrderIndex].isCorrect));

                //Change select to next input field if correct
                RecordAnswerResult(itemOrderIndex, true);
                index++;
                if (index < answerFields.Count) 
                {
                    Debug.Log("List still has inputfield");
                    answerFields[index].Select();
                    answerFields[index].GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
                }
                else
                {
                    Debug.Log("All correct answer");
                    SpawnAnswerField();
                }
                answerAttempts++;
               
            }
            //If it doesnt match its wrong
            else
            {
                Debug.Log("Wrong");
                StartCoroutine(WrongInputted(answerFields[itemOrderIndex]));
                RecordAnswerResult(itemOrderIndex, false);
               
                Scoring.instance.ModifyMultiplier(-1f);
            }
        }
        else //If input is invalid (not a number)
        {
            Debug.Log("Invalid Input, retry again");
            StartCoroutine(WrongInputted(answerFields[itemOrderIndex]));
           

            
        }
    }

    public void SpawnAnswerField()
    {
        if(answerFields.Count == index)
        {
           totalPriceAnswerField.enabled = true;
            
           totalPriceAnswerField.Select();
            totalPriceAnswerField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

        }
    }

    IEnumerator CorrectInputted(InputField p_inputField, bool p_correct)
    {

        p_inputField.gameObject.GetComponent<Image>().color = new Color(0f, 255f, 0f);
      
        yield return new WaitForSeconds(0.25f);
        p_correct = true;
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
        p_inputField.ActivateInputField();
        p_inputField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);

    }
    public void RecordAnswerResult(int p_index, bool p_isCorrect)
    {
        AnsweredProblemData newAnswer = new AnsweredProblemData();
        newAnswer.operatingNumbers.Add(itemUIClassList[p_index].price);
        newAnswer.operatingNumbers.Add(itemUIClassList[p_index].quantity);
        newAnswer.answer = itemUIClassList[p_index].totalPriceAnswer;
        newAnswer.mathOperator = MathProblemOperator.multiplication;
        newAnswer.isCorrect = p_isCorrect;
        newAnswer.timeSpent = timeSpent;
        timeSpent = 0f;
        PerformanceManager.instance.answeredProblemDatas.Add(newAnswer);
    }

    public void RecordAnswerResult(List<float> p_numbers, float p_answer, MathProblemOperator p_mathOperator, bool p_isCorrect)
    {
        AnsweredProblemData newAnswer = new AnsweredProblemData();
        foreach (float selectedNumber in p_numbers)
        {
            newAnswer.operatingNumbers.Add(selectedNumber);
        }
       
        newAnswer.answer = p_answer;
        newAnswer.mathOperator = p_mathOperator;
        newAnswer.isCorrect = p_isCorrect;
        newAnswer.timeSpent = timeSpent;
        timeSpent = 0;
        PerformanceManager.instance.answeredProblemDatas.Add(newAnswer);
       
        
    }
    public void ShowChangeText()
    {
        customerPaidTitle.gameObject.SetActive(true);
        customerPaidText.gameObject.SetActive(true);
        changeText.gameObject.SetActive(true);
        changeAnswerField.gameObject.SetActive(true);
        changeAnswerField.Select();
        changeAnswerField.GetComponent<Image>().color = new Color(0.0f, 0.6f, 0.9f);
    }

    public void OnTotalPriceInputted()
    {
       
        string playerInputString = totalPriceAnswerField.text;
     
        float playerInputValue = -1;

       
        if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
        {
            
            playerInputValue = inputVal;
        

        }
        if (playerInputValue != -1)
        {
            if ( playerInputValue == totalPriceCorrectAnswer) // Answer is correct
            {
                ShowChangeText();
                StartCoroutine(CorrectInputted(totalPriceAnswerField, totalPriceIsCorrect));
                RecordAnswerResult(itemsAnswer, totalPriceCorrectAnswer, MathProblemOperator.addition, true);

               
                changeAnswers.Add(GameManager.instance.customer.randomExtraMoney);
                changeAnswers.Add(totalPriceCorrectAnswer);
            }
            else // Answer is wrong
            {
                Debug.Log("RIGHT ANSWER IS : " + totalPriceCorrectAnswer);
                StartCoroutine(WrongInputted(totalPriceAnswerField));
                RecordAnswerResult(itemsAnswer, totalPriceCorrectAnswer, MathProblemOperator.addition, false);
                Scoring.instance.ModifyMultiplier(-1f);
            }
            answerAttempts++;
        }
        else
        {
            Debug.Log("Invalid Input, retry again");
            StartCoroutine(WrongInputted(totalPriceAnswerField));
            

        }

    }

    public void OnChangeInputted()
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
                StartCoroutine(CorrectInputted(changeAnswerField, changeIsCorrect));
                RecordAnswerResult(changeAnswers, changeCorrectAnswer, MathProblemOperator.subtraction, true);

                //awards score
                for (int i = 0; i < answerFields.Count;)
                {
                    InputField currentlySelectedItemUI = answerFields[0];
                    currentlySelectedItemUI.Select();

                    answerFields.RemoveAt(0);
                    changeAnswerField.text = "";
                    changeAnswerField.gameObject.SetActive(false);
                    changeText.gameObject.SetActive(false);
                    totalPriceAnswerField.text = "";
                   
                    Destroy(currentlySelectedItemUI.gameObject.transform.parent.gameObject);
                }
                
                OrderSheetFinish();
                index = 0;
            }
            else// Answer is wrong
            {


                StartCoroutine(WrongInputted(changeAnswerField));
                RecordAnswerResult(changeAnswers, changeCorrectAnswer, MathProblemOperator.subtraction, false);
                Scoring.instance.ModifyMultiplier(-1f);
            }
            
        }
        else
        {
            Debug.Log("Invalid Input, retry again");
            
            StartCoroutine(WrongInputted(changeAnswerField));
            

        }
    }
    public void OrderSheetFinish()
    {
        Debug.Log("PPPPPPPPPPPPPPPPPPPPPPPP: " + answerAttempts + " [] " + perfectAttempts);
        if (answerAttempts == perfectAttempts)
        {
            Scoring.instance.ModifyMultiplier(1f);
        }
        
        Scoring.instance.addScore((int) (100*Scoring.instance.multiplier));

        //  UIManager.instance.inGameUI.GetComponent<InGameUI>().scoring.gameObject.GetComponent<Text>().text = "Score: " + GameManager.instance.score.ToString(); //Very temporary until restructured codes
        
        TransitionManager.instances.MoveTransition(new Vector2(507f, 1387.0f), 1f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, false);
        if (GameManager.instance.customer)
        {
            Destroy(GameManager.instance.customer.gameObject);
        }
        GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
        //      gameObject.transform.root.gameObject.SetActive(false);

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
            order.transform.GetChild(3).gameObject.GetComponent<InputField>().onEndEdit.AddListener(OnPriceInputted);
            answerFields.Add(order.transform.GetChild(3).gameObject.GetComponent<InputField>());
            
            order.transform.SetParent(displayPanel);
            order.transform.localScale = new Vector3(1f, 1f, 1f);

        }


    }

}
