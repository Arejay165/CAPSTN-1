using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Upgrades : MonoBehaviour
{
    public static Upgrades instance;

    public List<Item> newItems;

    public Image[] itemSprite;
    public TextMeshProUGUI[] itemName;

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

        for (int i = 0; i< newItems.Count;i++)
        {
            newItems[i].isUnlock = false;
        }

        
    }

    public void selectItems(int itemIndex)
   {
        MathProblemManager.instance.storeItemList.Add(newItems[itemIndex]);
        //newItems.RemoveAt(itemIndex);

        UIManager.instance.upgradeUI.SetActive(false);
        newInteractables[itemIndex].SetActive(true);
        newInteractables.RemoveAt(itemIndex);

        //Destroy(itemSprite[itemIndex]);
        //Destroy(itemName[itemIndex]);
        //GameManager.instance.SetUpGame();
        newItems[itemIndex].isUnlock = true;
        // UIManager.instance.Restart(); // restart the level with a new item
    }
   
   public void initializeItem(int index)
   {
       itemName[index].text = newItems[index].name;
       itemSprite[index].sprite = newItems[index].itemSprite;
   }

    public void getUpgradeItem()
    {
        int counter = 0;

        for(int i = 0; i < newItems.Count; i++)
        {
            if (newItems[i].isUnlock == false)
            {
                
                initializeItem(counter);

                Debug.Log(newItems[counter].name);

                counter++;
                if (counter == 3)
                {
                    break;
                }
            }

        }
    }
}
