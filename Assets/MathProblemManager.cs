using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MathProblemManager : MonoBehaviour
{
    public GameObject itemIconTemplate;
    public List<Item> storeItemList = new List<Item>();
    public List<Item> itemsWanted = new List<Item>();
    void GenerateStorePrices()
    {
        foreach (Item selectedItem in storeItemList)
        {
            selectedItem.price = Random.Range(0, 125);
        }
    }
    void GenerateMathProblems()
    {
        GenerateStorePrices();
        int mathProblemCount = 0;
       
        while (mathProblemCount < 10)
        {
            if (mathProblemCount < 7) // if it's less than 7, it's order sheet
            {
              
                //Instatiating Customer Order 
                // For display 
                //for (int i = 0; i < storeItemList.Count; i++)
                //{
                //    GameObject newItemIcon = Instantiate(itemIconTemplate);
                //    //itemSprites.Add(newItemIcon.GetComponent<Image>());
                //    //newItemIcon.transform.SetParent(panel);
                //    //newItemIcon.GetComponent<Image>().preserveAspect = true;
                //}
                
                for (int i = 0; i < storeItemList.Count; i++)
                {

                    int selectedItem = Random.Range(0, storeItemList.Count);
                    itemsWanted.Add(storeItemList[selectedItem]);

                }

            }
            else if (mathProblemCount >=7 ) //if it's greater than or equal to 7, it's change sheet
            {
                //Not buying 
                int numerator = Random.Range(60, 500);
                int denominator = numerator / Random.Range(2, 50);
                int answer = numerator / denominator;
          
                
            }
        }
    }
}
