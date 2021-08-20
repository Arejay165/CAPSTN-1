using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Customer : MonoBehaviour
{
    public Item[] items;
    public List<Item> itemInCart = new List<Item>();
    public List<Item> itemsWanted = new List<Item>();
    public List<Image> itemSprites = new List<Image>();
    public GameObject itemPrefab;
    public Transform panel;
    public TestCalculator displayOrder;

    private int maxInventory;
    public float randomExtraMoney;

    private int RNG;

    int placeHolder = 0;    
    public int GetMaxInventory()
    {
        return maxInventory;
    }

    void Start()
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

    void RNG_item()
    {

        for (int i = 0; i < maxInventory; i++)
        {

            RNG = Random.Range(0, items.Length);
            Debug.Log("Customer wants: " + items[RNG].itemName);
            itemSprites[i].sprite = items[RNG].itemSprite;
            itemsWanted.Add(items[RNG]);
        }
    }
}

   