using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Customer : MonoBehaviour
{
    public List<Item> items;
    public Item cash;
    public List<Item> itemInCart = new List<Item>();
    public List<Item> itemsWanted = new List<Item>();
    public List<Image> itemSprites = new List<Image>();
    public GameObject itemPrefab;
    public Transform panel;
    public TestCalculator displayOrder;
    public bool willBuy;

    private int maxInventory;
    public float randomExtraMoney;

    private int RNG;

    int placeHolder = 0;
    public Sprite exchangeSprite;

    public int GetMaxInventory()
    {
        return maxInventory;
    }

    void Start()
    {

        //Instatiating Customer Order 

        float identifier = Random.Range(0, 100);

        Debug.Log(identifier);
        if (identifier > 1)
        {
            willBuy = true;
        }
        else
        {
            willBuy = false;
        }


        if (willBuy)
        {
            maxInventory = Random.Range(1, 4);
            //Instatiating Customer Order 
            // For display 
            for (int i = 0; i < maxInventory; i++)
            {
                GameObject obj = Instantiate(itemPrefab);
                itemSprites.Add(obj.GetComponent<Image>());
                obj.transform.SetParent(panel);
                obj.GetComponent<Image>().preserveAspect = true;
            }
            RNG_item();
        }
        else
        {
            //Not buying 

            //Test for whole number answer for Division 
            //dividend / divisor = quotient (answer of player)
            // Reverse the division to multiplication
            // divisor * quotient = dividend
            int divisor = Random.Range(2, 51); // use to multiply to the quotient to always be whole number 
            int quotient = Random.Range(1, 50); // possible answers 
            int dividend = divisor * quotient; // determine the dividend

            cash.numValue = dividend;//Random.Range(60, 500);
            cash.denValue = divisor;//cash.numValue / Random.Range(2, 50);
            cash.price = quotient;//cash.numValue / cash.denValue;
            GameObject obj = Instantiate(itemPrefab);
            itemSprites.Add(obj.GetComponent<Image>());
            obj.transform.SetParent(panel);
            obj.GetComponent<Image>().preserveAspect = true;
            obj.GetComponent<Image>().sprite = exchangeSprite;
            itemsWanted.Add(cash);
           
        }

        // For display 

    }

   

    void RNG_item()
    {

        for (int i = 0; i < maxInventory; i++)
        {

            RNG = Random.Range(0, maxInventory);
            Debug.Log("Customer wants: " + items[RNG].itemName);
            itemSprites[i].sprite = items[RNG].itemSprite;
            itemsWanted.Add(items[RNG]);

        }

    }


}

   