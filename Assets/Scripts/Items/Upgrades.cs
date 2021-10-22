using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Upgrades : MonoBehaviour
{
    public static Upgrades instance;

    public List<Item> newItems;

    public List<Image> itemSprite;
    public List<TextMeshProUGUI> itemName;

    public List<GameObject> newInteractables;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start() //Temp Fix
    {
        //initializeItem();



        for(int i = 0; i < newInteractables.Count;i++)
        {
            newInteractables[i].SetActive(false);
        }

        //for (int i = 0; i< newItems.Count;i++)
        //{
        //    newItems[i].isUnlock = false;
        //}

        
    }

    public void selectItems(int itemIndex)
   {
        MathProblemManager.instance.storeItemList.Add(newItems[itemIndex]);
        //newItems.RemoveAt(itemIndex);

        UIManager.instance.upgradeUI.SetActive(false);
        newInteractables[itemIndex].SetActive(true);
        newInteractables.RemoveAt(itemIndex);
        newItems.RemoveAt(itemIndex);
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
        //newItems[itemIndex].isUnlock = true;
       
    }
   
   public void initializeItem(int index)
   {
        if (newItems.Count == 2)
        {
            itemName[2].text = null;
            itemSprite[2].sprite = null;
        }
        else if (newItems.Count == 1)
        {
            itemName[1].text = null;
            itemSprite[1].sprite = null;
        }
        

        itemName[index].text = newItems[index].name;
        itemSprite[index].sprite = newItems[index].itemSprite;
        
        
   }

    public void getUpgradeItem()
    {
        int counter = 0;


        if (newItems.Count == 0)
        {
            GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
        }
        else
        {
            for (int i = 0; i < newItems.Count; i++)
            {
                initializeItem(i);

                Debug.Log("Can Pick: " + newItems[i].name);

                counter++;

                if (i == 3)
                {
                    break;
                }

            }
        }
       
    }
}
