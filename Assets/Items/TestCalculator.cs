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
    public int quantity;
    public float totalPriceAnswer;
}
public class TestCalculator : MonoBehaviour
{
    [Header("Stats")]
    public List<ItemUIClass> itemUIClassList = new List<ItemUIClass>(); // Datas for the Item UIs that will be in the answer pad
    [SerializeField] List<InputField> answerFields = new List<InputField>();

    [SerializeField] InputField totalPriceAnswerField, changeAnswerField;
    [SerializeField] Text customerPaidText,changeText;
    [SerializeField] float totalPriceCorrectAnswer, changeCorrectAnswer;
    bool totalPriceIsCorrect, changeIsCorrect;

    [SerializeField] GameObject itemDisplay;
    [SerializeField] Transform displayPanel;

    public List<ItemSpawner> clickableItems;

    public int index;
    ItemUIClass CreateItemUI(Item p_item)
    {
        
        ItemUIClass newItemUIClass = new ItemUIClass();
        newItemUIClass.isCorrect = false;
        newItemUIClass.icon = p_item.itemSprite;
        newItemUIClass.name = p_item.itemName;
        newItemUIClass.price = p_item.price;
        newItemUIClass.quantity = 1;
        newItemUIClass.totalPriceAnswer = newItemUIClass.quantity * newItemUIClass.price;
        return newItemUIClass;
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
    }

    private void OnDisable()
    {
        itemUIClassList.Clear();
        totalPriceCorrectAnswer = 0;
        changeCorrectAnswer = 0;
        totalPriceAnswerField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
        changeAnswerField.gameObject.GetComponent<Image>().color = new Color(233f, 231f, 214f);
        if (GameManager.instance.customer)
        {
            Destroy(GameManager.instance.customer.gameObject);
        }
        
        GameManager.instance.customerSpawner.SpawnCustomer();
        GameManager.instance.orderSheetShowing = false;
        foreach (ItemSpawner item in clickableItems)
        {
            item.canSpawn = true;

        }
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
                    itemUIClassList[i].quantity++;
                    itemUIClassList[i].totalPriceAnswer = itemUIClassList[i].quantity * itemUIClassList[i].price;
                    //  return;
                    break;
                }
            }
            if (uniqueItem == true)//If it's a unique item (No duplicates yet in the list)
            {
                itemUIClassList.Add(CreateItemUI(customer.itemInCart[x]));
            }
            uniqueItem = true;
        }

      
        for (int i = 0; i < itemUIClassList.Count; i++)
        {
            totalPriceCorrectAnswer += itemUIClassList[i].totalPriceAnswer;
        }
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
                index++;
                SpawnAnswerField();
            }
            //If it doesnt match its wrong
            else
            {
                Debug.Log("Wrong");
                StartCoroutine(WrongInputted(answerFields[itemOrderIndex]));
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
        p_inputField.Select();
    }

    public void ShowChangeText()
    {
        changeText.gameObject.SetActive(true);
        changeAnswerField.gameObject.SetActive(true);
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

            }
            else // Answer is wrong
            {
                Debug.Log("RIGHT ANSWER IS : " + totalPriceCorrectAnswer);
                StartCoroutine(WrongInputted(totalPriceAnswerField));

            }
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
            if (playerInputValue == changeCorrectAnswer)
            {
                StartCoroutine(CorrectInputted(changeAnswerField, changeIsCorrect));
                //awards score
                for (int i = 0; i < answerFields.Count;)
                {
                    InputField currentlySelectedItemUI = answerFields[0];

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
        GameManager.instance.score+= 100;
      //  UIManager.instance.inGameUI.GetComponent<InGameUI>().scoring.gameObject.GetComponent<Text>().text = "Score: " + GameManager.instance.score.ToString(); //Very temporary until restructured codes
        TransitionManager.instances.MoveTransition(new Vector2(-743f, 1387.0f), 1f, TransitionManager.instances.noteBookTransform, gameObject.transform.parent.gameObject, false);
   
  //      gameObject.transform.root.gameObject.SetActive(false);
       
    }

    public void DisplayItemOrders()
    {
;
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

        foreach (ItemSpawner item in clickableItems)
        {
            item.canSpawn = false;

        }
    }

}
