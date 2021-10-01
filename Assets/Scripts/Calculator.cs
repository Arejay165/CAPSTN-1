using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    /*public GameObject   Item1;
    public GameObject   Item2;*/

  
    // --------------------------------------------------------------------------------------------------------------------------
    [SerializeField] private int total = 0; // Total amount by this script
    [SerializeField] private int playerInputValue = 0;// a variable for amount of the  player input 
                                                      // --------------------------------------------------------------------------------------------------------------------------
    public InputField getAnswerInputField;
    public InputField finalAnswerInputField;
    public InputField changeInputField;

    private int inputVal;
    public int answerVal = 0;
    public int changeInput;

    public List<int> listOfAnswers = new List<int>();
    public List<Text> answerText = new List<Text>();
    [SerializeField]int textIndex;

    //public GameObject inputField;


    // Start is called before the first frame update
    void Start()
    {
        getAnswerInputField.Select();
        textIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
     
    }


    public void storePlayerInputAnswer(string answer)
    {

        answer = getAnswerInputField.text;
        if (int.TryParse(answer, out  inputVal))
        {
            playerInputValue = inputVal;
        }
        
       

        listOfAnswers.Add(playerInputValue);

        getAnswerInputField.text = "";
        

       // answerText[0].text = listOfAnswers[0].ToString();
        getAnswerInputField.Select();

        InitializeText(textIndex);
        textIndex++;
        
        if (GameManager.instance.testCalculator.itemUIClassList.Count == listOfAnswers.Count)
        {
            //  GetAnswer();
            finalAnswerInputField.gameObject.SetActive(true);
            getAnswerInputField.gameObject.SetActive(false);
        }
    }

    public void InitializeText(int index)
    {
        answerText[index].text = listOfAnswers[index].ToString();
    }

    public void GetAnswer(string answer)
    {

        answer = finalAnswerInputField.text;
        if (int.TryParse(answer, out answerVal))
        {
            total = answerVal;
        }
        Addition();
    }

    public void DisableInputFields()
    {
        finalAnswerInputField.gameObject.SetActive(true);
        getAnswerInputField.gameObject.SetActive(false);
    }
    void Addition()
    {
        // total = price1 + price2;
        int customerSum = 0;// GameManager.instance.customer.sumOfAllWants;

   //     Debug.Log(total);

        if(total == customerSum)
        {
            Debug.Log("Correct answer");
            finalAnswerInputField.gameObject.SetActive(false);
            changeInputField.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Wrong answer");
            ResetText();
        }
    }

   
    public void GetChange()
    {

        //if (changeInput == GameManager.instance.customer.randomExtraMoney)
        //{
        //    Debug.Log("Approriate Change");
        //    changeInputField.gameObject.SetActive(false);
        //    getAnswerInputField.gameObject.SetActive(true);

        //    ResetText();
        //}
        //else
        //{
        //    Debug.Log("Repeat this process");
        //}

    }

    public void InputChange(string answer)
    {
        answer = changeInputField.text;
        if(int.TryParse(answer, out int changeVal))
        {
            changeInput = changeVal;
        }

        GetChange();

    }

    void ResetText()
    {
    //    textIndex = 0;
        for (int i = 0; i < MathProblemManager.instance.GetGeneratedItemsWanted().Count; i++)
        {
            answerText[i].text = "";
        }
       
        finalAnswerInputField.gameObject.SetActive(false);
        getAnswerInputField.gameObject.SetActive(true);


        listOfAnswers.Clear();

        total = 0;
        textIndex = 0;
        //  InitializeText(textIndex);
    }

}
