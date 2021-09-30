using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AnsweredProblemStack
{
    public AnsweredProblemData answeredProblemData;
    public int quantity;
    public float totalTimeSpent;
}

[System.Serializable]
public enum PerformanceFact
{
    //In General
    //Equations like 5 + 4 = 9
    mostCommonCorrectMathProblems, // 1 + 1 = 2 
    mostCommonWrongMathProblems, // 1 + 3 = 4
    mathProblemAverageTime, //how long does the player spent on each math problem
    //Student's most wrong
    mostDifficultOperator, // Addition 
    //Order sheet's overall cost
    additionCorrects,
    additionWrongs,
    additionAverageTime,
    //Change
    subtractionCorrects,
    subtractionWrongs,
    subtractionAverageTime,
    //Each item's total price
    multiplicationCorrects,
    multiplicationWrongs,
    multiplicationAverageTime,
    //Division
    divisionCorrects,
    divisionWrongs,
    divisionAverageTime,
    //Example:
    //Addition 3/4 correct (instead of having a separate counter for corrects and wrongs)
}

[System.Serializable]
public enum MathProblemOperator
{
    addition,
    subtraction,
    multiplication,
    division,
    none
}

[System.Serializable]
public class AnsweredProblemData
{
    public List<float> operatingNumbers = new List<float>();
    public float answer;
    public MathProblemOperator mathOperator;
    public bool isCorrect;
    public float timeSpent;
}
public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager instance;
    public int totalMathProblems = 0;
    public List<AnsweredProblemData> answeredProblemDatas = new List<AnsweredProblemData>();
    public int rerollAttempts = 0;
    public int maxRerollAttempts = System.Enum.GetValues(typeof(PerformanceFact)).Length;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        
    }

    #region General Functions
    public bool CheckMatchingMathProblem(AnsweredProblemData p_selectedAnsweredProblemData, AnsweredProblemData p_selectedComparisonProblemData, bool isCorrect)
    {
        if (p_selectedAnsweredProblemData.operatingNumbers.Count == p_selectedComparisonProblemData.operatingNumbers.Count)
        {
            //Addition && Multiplication
            if (p_selectedAnsweredProblemData.mathOperator == MathProblemOperator.addition || p_selectedAnsweredProblemData.mathOperator == MathProblemOperator.multiplication)
            {
                for (int i = 0; i < p_selectedAnsweredProblemData.operatingNumbers.Count;)
                {
                    for (int ii = 0; ii < p_selectedComparisonProblemData.operatingNumbers.Count;)
                    {
                        if (p_selectedAnsweredProblemData.operatingNumbers[i] == p_selectedComparisonProblemData.operatingNumbers[ii])
                        {
                            break;
                        }
                        ii++;
                        //if no matching operating number was found that means it's not a matching math problem
                        if (ii >= p_selectedComparisonProblemData.operatingNumbers.Count)
                        {
                            return false;
                        }

                    }
                    i++;

                }
            }
            //Subtraction && Division
            else if (p_selectedAnsweredProblemData.mathOperator == MathProblemOperator.subtraction || p_selectedAnsweredProblemData.mathOperator == MathProblemOperator.division)
            {
                for (int i = 0; i < p_selectedAnsweredProblemData.operatingNumbers.Count;)
                {
                    //If it does not match
                    if (p_selectedAnsweredProblemData.operatingNumbers[i] != p_selectedComparisonProblemData.operatingNumbers[i])
                    {
                        return false;
                    }
                    i++;

                }
            }
        }


        if (p_selectedAnsweredProblemData.answer == p_selectedComparisonProblemData.answer)
        {
            if (p_selectedAnsweredProblemData.isCorrect == p_selectedComparisonProblemData.isCorrect)
            {
                if (p_selectedAnsweredProblemData.isCorrect == isCorrect)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }



        return false;

    }

    public bool CheckDuplicate(List<AnsweredProblemStack> p_mostCommonMathProblems, AnsweredProblemData p_searchedAnsweredProblemData)
    {
        foreach (AnsweredProblemStack selectedCommonMathProblem in p_mostCommonMathProblems)
        {
            if (CheckMatchingMathProblem(selectedCommonMathProblem.answeredProblemData, p_searchedAnsweredProblemData, p_searchedAnsweredProblemData.isCorrect))
            {
                return true;
            }
        }
        return false;
    }

    public AnsweredProblemStack GetDuplicate(List<AnsweredProblemStack> p_mostCommonMathProblems, AnsweredProblemData p_searchedAnsweredProblemData)
    {
        foreach (AnsweredProblemStack selectedCommonMathProblem in p_mostCommonMathProblems)
        {
            if (CheckMatchingMathProblem(selectedCommonMathProblem.answeredProblemData, p_searchedAnsweredProblemData, p_searchedAnsweredProblemData.isCorrect))
            {
                return selectedCommonMathProblem;
            }
        }
        return null;
    }

    public List<List<AnsweredProblemData>> SortMathProblemsByOperator()
    {
        List<List<AnsweredProblemData>> sortedMathProblems = new List<List<AnsweredProblemData>>();
        //Check each problem data and sort them into operator lists
        foreach (AnsweredProblemData selectedAnsweredProblemData in answeredProblemDatas)
        {
            bool makeNewOperatorList = true;
            //Look For Which Operator list will it be sorted into
            for (int i = 0; i < sortedMathProblems.Count;)
            {
                makeNewOperatorList = false;
                //If matching operator found from the list, add it to the operator's list
                if (selectedAnsweredProblemData.mathOperator == sortedMathProblems[i][0].mathOperator)
                {
                    
                    //Add it into the sorted operator
                    sortedMathProblems[i].Add(selectedAnsweredProblemData);
                    break;
                }
                i++;
                if (i >= sortedMathProblems.Count)
                {
                    makeNewOperatorList = true;
                }


            }
            if (makeNewOperatorList)
            {
                //If matching operator not found from the list, add it as new operator list
                //Add new operator
                List<AnsweredProblemData> newOperatorList = new List<AnsweredProblemData>();
                newOperatorList.Add(selectedAnsweredProblemData);
                sortedMathProblems.Add(newOperatorList);
                


            }
    

        }
        return sortedMathProblems;
    }

    public List<List<AnsweredProblemData>> SortMathProblemsByOperator(bool p_isCorrect)
    {
        List<List<AnsweredProblemData>> sortedMathProblems = new List<List<AnsweredProblemData>>();
 
        //Check each problem data and sort them into operator lists
        foreach (AnsweredProblemData selectedAnsweredProblemData in answeredProblemDatas)
        {
            bool makeNewOperatorList = true;
            //Look For Which Operator list will it be sorted into
            //if (operatorIndex < sortedMathProblems.Count)
            for (int i = 0; i < sortedMathProblems.Count; )
            {
                makeNewOperatorList = false;
                //If matching operator found from the list, add it to the operator's list
                if (selectedAnsweredProblemData.mathOperator == sortedMathProblems[i][0].mathOperator)
                {
                    if (selectedAnsweredProblemData.isCorrect == p_isCorrect)
                    {
                        //Add it into the sorted operator
                        sortedMathProblems[i].Add(selectedAnsweredProblemData);
                    }
      
                    break;
                }
                i++;
                if (i >= sortedMathProblems.Count)
                {
                    makeNewOperatorList = true;
                }

               
            }
            if (makeNewOperatorList)
            {
            //If matching operator not found from the list, add it as new operator list
               
                if (selectedAnsweredProblemData.isCorrect == p_isCorrect)
                {
                    //Add new operator
                    List<AnsweredProblemData> newOperatorList = new List<AnsweredProblemData>();
                    newOperatorList.Add(selectedAnsweredProblemData);
                    sortedMathProblems.Add(newOperatorList);
                }
                    
                
            }

       
        }

        return sortedMathProblems;
    }

    public string GetOperatorCount(MathProblemOperator p_operator)
    {
        string mostOperator = "0";
        List<List<AnsweredProblemData>> mostOperatorCounts = new List<List<AnsweredProblemData>>();
        int highestQuantity = 0;
        foreach (List<AnsweredProblemData> selectedOperatorList in SortMathProblemsByOperator())
        {
            //If the currently selected answered problem stack's quantity is bigger than the recorded highest quantity
            if (selectedOperatorList[0].mathOperator == p_operator)
            {
                if (highestQuantity < selectedOperatorList.Count)
                {
                    //Remove all math problems that has the same quantity as the highest quantity
                    mostOperatorCounts.Clear();
                    //And update the highest quantity to the quantity of the currently selected answered problem stack
                    mostOperatorCounts.Add(selectedOperatorList);
                    highestQuantity = selectedOperatorList.Count;
                }
                //If the currently selected answered problem stack has the same quantity as the recorded highest quantity
                else if (highestQuantity == selectedOperatorList.Count)
                {
                    //Add the currently selected answered problem stack to the options to be chosen later
                    mostOperatorCounts.Add(selectedOperatorList);
                }
            }

        }
        int chosenIndex = 0;
        //If there is more than 1 math problem that is the most common, randomly choose one of them to be the most common
        ////(example : the quantity for math problem (1 + 1 = 2) is 2 while another math problem (3 + 1 = 4) also has a quantity of 2 
        if (mostOperatorCounts.Count > 1)
        {
            chosenIndex = Random.Range(0, mostOperatorCounts.Count);
        }
        if (mostOperatorCounts.Count > 0)
        {
            mostOperator = mostOperatorCounts[chosenIndex].Count.ToString();

            //Clear all lists in list
            foreach (List<AnsweredProblemData> selectedOperator in mostOperatorCounts)
            {
                selectedOperator.Clear();
            }
            mostOperatorCounts.Clear();
            
            return mostOperator;
        }
        else
        {
            return "0";
        }
    }
    public string GetOperatorCount(MathProblemOperator p_operator,bool p_isCorrect)
    {
        string mostOperator = "";
        List<List<AnsweredProblemData>> mostOperatorCounts = new List<List<AnsweredProblemData>>();
        int highestQuantity = 0;
        foreach (List<AnsweredProblemData> selectedOperatorList in SortMathProblemsByOperator(p_isCorrect))
        {
            //If the currently selected answered problem stack's quantity is bigger than the recorded highest quantity
            if (selectedOperatorList[0].mathOperator == p_operator)
            {
                if (highestQuantity < selectedOperatorList.Count)
                {
                    //Remove all math problems that has the same quantity as the highest quantity
                    mostOperatorCounts.Clear();
                    //And update the highest quantity to the quantity of the currently selected answered problem stack
                    mostOperatorCounts.Add(selectedOperatorList);
                    highestQuantity = selectedOperatorList.Count;
                }
                //If the currently selected answered problem stack has the same quantity as the recorded highest quantity
                else if (highestQuantity == selectedOperatorList.Count)
                {
                    //Add the currently selected answered problem stack to the options to be chosen later
                    mostOperatorCounts.Add(selectedOperatorList);
                }
            }
            
        }
        int chosenIndex = 0;
        //If there is more than 1 math problem that is the most common, randomly choose one of them to be the most common
        ////(example : the quantity for math problem (1 + 1 = 2) is 2 while another math problem (3 + 1 = 4) also has a quantity of 2 
        if (mostOperatorCounts.Count > 1)
        {
            chosenIndex = Random.Range(0, mostOperatorCounts.Count);
        }
        if (mostOperatorCounts.Count > 0)
        {
            mostOperator = mostOperatorCounts[chosenIndex].Count.ToString();

            //Clear all lists in list
            foreach (List<AnsweredProblemData> selectedOperator in mostOperatorCounts)
            {
                selectedOperator.Clear();
            }
            mostOperatorCounts.Clear();
            return mostOperator;
        }
        else
        {
            return "0";
        }    
         
    }

    public string GetMostDifficultOperator()
    {
        string mostDifficultOperator = "";
        List< List<AnsweredProblemData>> mostDifficultOperators = new List<List<AnsweredProblemData>>();
        int highestQuantity = 0;
        foreach (List < AnsweredProblemData > selectedOperatorList in SortMathProblemsByOperator(false))
        {
            //If the currently selected answered problem stack's quantity is bigger than the recorded highest quantity
            if (highestQuantity < selectedOperatorList.Count)
            {
                //Remove all math problems that has the same quantity as the highest quantity
                mostDifficultOperators.Clear();
                //And update the highest quantity to the quantity of the currently selected answered problem stack
                mostDifficultOperators.Add(selectedOperatorList);
                highestQuantity = selectedOperatorList.Count;
            }
            //If the currently selected answered problem stack has the same quantity as the recorded highest quantity
            else if (highestQuantity == selectedOperatorList.Count)
            {
                //Add the currently selected answered problem stack to the options to be chosen later
                mostDifficultOperators.Add(selectedOperatorList);
            }
        }
        int chosenIndex = 0;
        //If there is more than 1 math problem that is the most common, randomly choose one of them to be the most common
        ////(example : the quantity for math problem (1 + 1 = 2) is 2 while another math problem (3 + 1 = 4) also has a quantity of 2 
        if (mostDifficultOperators.Count > 1)
        {
            chosenIndex = Random.Range(0, mostDifficultOperators.Count);
        }
        if (mostDifficultOperators.Count > 0 )
        {
            if (mostDifficultOperators[chosenIndex][0].mathOperator == MathProblemOperator.addition)
            {
                mostDifficultOperator = "Addition";
            }
            else if (mostDifficultOperators[chosenIndex][0].mathOperator == MathProblemOperator.subtraction)
            {
                mostDifficultOperator = "Subtraction";
            }
            else if (mostDifficultOperators[chosenIndex][0].mathOperator == MathProblemOperator.multiplication)
            {
                mostDifficultOperator = "Multiplication";
            }
            else if (mostDifficultOperators[chosenIndex][0].mathOperator == MathProblemOperator.division)
            {
                mostDifficultOperator = "Division";
            }
            //Clear all lists in list
            foreach (List<AnsweredProblemData> selectedOperator in mostDifficultOperators)
            {
                selectedOperator.Clear();
            }
            mostDifficultOperators.Clear();
            return mostDifficultOperator;
        }
        else
        {
            return "";
        }
    }
    public string GetAverageTime(MathProblemOperator p_operatorFilter)
    {
        float averageTime = 0f;
        float totalTime = 0;
        int amount = 0;
        foreach (AnsweredProblemData selectedAnsweredProblemData in answeredProblemDatas)
        {
            if (p_operatorFilter == MathProblemOperator.none || p_operatorFilter == selectedAnsweredProblemData.mathOperator)
            {
                totalTime += selectedAnsweredProblemData.timeSpent;
                amount++;
            }
            
        }

        averageTime = totalTime / amount;
        
        if (float.IsNaN(averageTime))
        {
            averageTime = 0.00f;
        }
        return averageTime.ToString("F2");
    }
    public string GetMostCommonMathProblem(bool p_isCorrect)
    {
        List<AnsweredProblemStack> mostCommonMathProblems = new List<AnsweredProblemStack>();
        int highestQuantity = 0;
        //Loop through each common math problems to see which ones are the most common
        List<AnsweredProblemStack> commonMathProblems = GetCommonMathProblems(p_isCorrect);
        foreach (AnsweredProblemStack selectedAnsweredProblemStack in commonMathProblems)
        {
            //If the currently selected answered problem stack's quantity is bigger than the recorded highest quantity
            if (highestQuantity < selectedAnsweredProblemStack.quantity)
            {
                //Remove all math problems that has the same quantity as the highest quantity
                mostCommonMathProblems.Clear();
                //And update the highest quantity to the quantity of the currently selected answered problem stack
                mostCommonMathProblems.Add(selectedAnsweredProblemStack);
                highestQuantity = selectedAnsweredProblemStack.quantity;
            }
            //If the currently selected answered problem stack has the same quantity as the recorded highest quantity
            else if (highestQuantity == selectedAnsweredProblemStack.quantity)
            {
                //Add the currently selected answered problem stack to the options to be chosen later
                mostCommonMathProblems.Add(selectedAnsweredProblemStack);
            }
        }
        int chosenIndex = 0;
        //If there is more than 1 math problem that is the most common, randomly choose one of them to be the most common
        ////(example : the quantity for math problem (1 + 1 = 2) is 2 while another math problem (3 + 1 = 4) also has a quantity of 2 
        if (mostCommonMathProblems.Count > 1)
        {
            chosenIndex = Random.Range(0, mostCommonMathProblems.Count);
        }

        if (mostCommonMathProblems.Count > 0)
        {
            AnsweredProblemData chosenAnsweredProblemStack = mostCommonMathProblems[chosenIndex].answeredProblemData;
            //Clear all lists in list
            mostCommonMathProblems.Clear();
            commonMathProblems.Clear();
            return BuildMathProblemString(chosenAnsweredProblemStack);
        }
        else
        {
            return "";
        }    
    }

    public List<AnsweredProblemStack> GetCommonMathProblems(bool p_isCorrect)
    {
        List<AnsweredProblemStack> commonMathProblems = new List<AnsweredProblemStack>();
        //Check each problem data
        foreach (AnsweredProblemData selectedAnsweredProblemData in answeredProblemDatas)
        {
            if (!CheckDuplicate(commonMathProblems, selectedAnsweredProblemData))
            {
                //Check for same copies of the currently selected problem data by looping through all problem datas in list
                foreach (AnsweredProblemData selectedComparisonProblemData in answeredProblemDatas)
                {
                    //If there is an exact copy of the selected problem data, add it to the list of problem datas that was encountered more than once
                    if (CheckMatchingMathProblem(selectedAnsweredProblemData, selectedComparisonProblemData, p_isCorrect))
                    {
                        //If the list already has the copy of the math problem
                        if (CheckDuplicate(commonMathProblems, selectedComparisonProblemData))
                        {
                            AnsweredProblemStack duplicateCopy = GetDuplicate(commonMathProblems, selectedComparisonProblemData);
                            duplicateCopy.quantity++;

                            duplicateCopy.totalTimeSpent += selectedComparisonProblemData.timeSpent;

                        }
                        //Else if the list doesn't have a copy of the math problem yet
                        //Make a copy of the math problem
                        else
                        {
                            AnsweredProblemStack newCommonCorrectMathProblem = new AnsweredProblemStack();
                            newCommonCorrectMathProblem.answeredProblemData = selectedComparisonProblemData;
                            newCommonCorrectMathProblem.quantity = 1;
                            newCommonCorrectMathProblem.totalTimeSpent += selectedComparisonProblemData.timeSpent;
                            commonMathProblems.Add(newCommonCorrectMathProblem);
                        }

                    }
                }
            }


        }
        return commonMathProblems;
    }

    public string BuildMathProblemString(AnsweredProblemData p_answeredProblemData)
    {
        string mathProblem = "";
        //Building math problem
        //Adding all operating numbers and math operator
        if (p_answeredProblemData.operatingNumbers.Count > 0)
        {
            for (int i = 0; i < p_answeredProblemData.operatingNumbers.Count;)
            {
                //Adding operating number
                mathProblem += p_answeredProblemData.operatingNumbers[i].ToString() + " ";
                i++;
                //Adding math operator
                if (i < p_answeredProblemData.operatingNumbers.Count && p_answeredProblemData.operatingNumbers.Count != 1)
                {
                    string mathOperator = "";
                    if (p_answeredProblemData.mathOperator == MathProblemOperator.addition)
                    {
                        mathOperator = "+ ";
                    }
                    else if (p_answeredProblemData.mathOperator == MathProblemOperator.subtraction)
                    {
                        mathOperator = "- ";
                    }
                    else if (p_answeredProblemData.mathOperator == MathProblemOperator.multiplication)
                    {
                        mathOperator = "x ";
                    }
                    else if (p_answeredProblemData.mathOperator == MathProblemOperator.division)
                    {
                        mathOperator = "/ ";
                    }
                    mathProblem += mathOperator;
                }
            }
            //Adding equals and math problem's answer
            mathProblem += "= " + p_answeredProblemData.answer.ToString();
        }
        
        

        return mathProblem;
    }
    #endregion

}
