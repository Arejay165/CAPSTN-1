using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MathProblemManager : MonoBehaviour
{
    public static MathProblemManager instance;
    public GameObject itemIconTemplate;
    public List<Item> storeItemList = new List<Item>();
    public Item cash;

    public bool randomizeItemsWanted = false;
    public int currentIndex = 0;

    public int maxGeneratedProblems = 10;
    public float buyItemPercentage = 0.7f;
    [SerializeField] public List<Item> currentItemsWanted = new List<Item>();
    [SerializeField] public List<List<Item>> generatedItemsWanted = new List<List<Item>>();
    [SerializeField] public List<List<Item>> orderedItemsWanted = new List<List<Item>>();
    //public List<Item> itemsWanted = new List<Item>();
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
    private void OnEnable()
    {

        //Register OnGameStart Event in GameManager
        GameManager.OnGameStart += OnGameStarted;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStarted;
    }
    public void OnGameStarted()
    {
        GenerateStorePrices();
    }
    public List<Item> GetGeneratedItemsWanted()
    {
        return orderedItemsWanted[currentIndex];
    }

    public Item GetItemInGeneratedItemsWanted(int p_desiredIndex)
    {

        return orderedItemsWanted[currentIndex][p_desiredIndex];
    }

    public void NextItemWanted()
    {
        if (currentItemsWanted.Count <= 0)
        {
            
            if (!randomizeItemsWanted)
            {
                if (currentIndex < orderedItemsWanted.Count - 1)
                {
                    currentIndex += 1;
                }
                else
                {
                    randomizeItemsWanted = true;
                    currentIndex = Random.Range(0, orderedItemsWanted.Count);
                }
            }
            else
            {
                currentIndex = Random.Range(0, orderedItemsWanted.Count);
            }
            Debug.Log(orderedItemsWanted[currentIndex]);
            foreach (Item selectedItem in orderedItemsWanted[currentIndex])
            {
                //Item item = new Item();
                //item.itemName = selectedItem.itemName;
                //item.price = selectedItem.price;
                //item.numValue = selectedItem.numValue;
                //item.denValue = selectedItem.denValue;
                //item.itemSprite = selectedItem.itemSprite;
                //item.quantity = selectedItem.quantity;
                //item.minRangePrice = selectedItem.minRangePrice;
                //item.maxRangePrice = selectedItem.maxRangePrice;
                //itemsWanted.Add(item);
                
                currentItemsWanted.Add(selectedItem);


            }
            
            

        }
    }
    public List<Item> GetCurrentItemsWanted(bool p_moveNextIndex = false)
    {

        if (p_moveNextIndex)
        {
            NextItemWanted();
        }

        
        return currentItemsWanted;
    }
    public Item GetItemInCurrentItemsWanted(int p_desiredIndex)
    {

        return currentItemsWanted[p_desiredIndex];
    }
    void GenerateStorePrices()
    {
        currentIndex = 0;
        currentItemsWanted.Clear();
        generatedItemsWanted.Clear();
        orderedItemsWanted.Clear();
        randomizeItemsWanted = false;
        foreach (Item selectedItem in storeItemList)
        {
            selectedItem.price = Random.Range((int)selectedItem.minRangePrice, (int)selectedItem.maxRangePrice);
            
        }
        GenerateMathProblems();
    }
    void GenerateMathProblems()
    {
        

        int mathProblemCount = 0;
        
        while (mathProblemCount < maxGeneratedProblems)
        {
            mathProblemCount++;
            if (mathProblemCount < maxGeneratedProblems * buyItemPercentage) // if it's less than 7, it's order sheet
            {
               
                
                List<Item> itemsWanted = new List<Item>();
                int maxInventory = Random.Range(1, 4);
                for (int i = 0; i < maxInventory; i++)
                {

                    int selectedItemIndex = Random.Range(0, storeItemList.Count);
                    Item selectedItem = storeItemList[selectedItemIndex];
                    itemsWanted.Add(selectedItem);
                    

                }
                generatedItemsWanted.Add(itemsWanted);
            }
            else if (mathProblemCount >= maxGeneratedProblems * buyItemPercentage) //if it's greater than or equal to 7, it's change sheet
            {
                //Not buying 
                Debug.Log("NOT BUYING");
                List<Item> itemWanted = new List<Item>();
                

                itemWanted.Add(cash);
                generatedItemsWanted.Add(itemWanted);
            }
           


        }
        //Shuffle
        for (int i = 0; i < generatedItemsWanted.Count;)
        {
            
            int chosenIndex = Random.Range(0, generatedItemsWanted.Count);
            orderedItemsWanted.Add(generatedItemsWanted[chosenIndex]);
            generatedItemsWanted.RemoveAt(chosenIndex);
        }

        foreach (Item selectedItem in orderedItemsWanted[currentIndex])
        {

            currentItemsWanted.Add(selectedItem);
        }
    }
}
