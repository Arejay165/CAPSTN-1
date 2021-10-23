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
    public void OnEnable()
    {
        GameManager.OnGameStart += OnGameStarted;
        GameManager.OnGameEnd += OnGameEnded;
    }

    public void OnDisable()
    {

        GameManager.OnGameStart -= OnGameStarted;
        GameManager.OnGameEnd -= OnGameEnded;
        //Reset upgrade uis
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
    }

    void OnGameStarted()
    {

    }

    void OnGameEnded()
    {
   

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
   
    public void getUpgradeItem()
    {

        List<string> chosenItems = new List<string>();
        if (newItems.Count == 0)
        {
            GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
        }
        else
        {
            //If there is more than 3(which is amount of buttons) item upgrades available
            if (newItems.Count > 3)
            {
                while (chosenItems.Count != 3)
                {
                
                    //Randomize which item to get
                    int chosenItemIndex = Random.Range(0, newItems.Count);
                    bool isUnique = true;
                    //Check if chosen item was already chosen before
                    foreach (string selectedItemCheckedName in chosenItems)
                    {
                        //There is one that exists already
                        if(selectedItemCheckedName == newItems[chosenItemIndex].name)
                        {
                            isUnique = false;
                            break;
                        }
                        
                    }
                    //name doesnt exist yet
                    if (isUnique)
                    {
                        
                        //sets button to list
                        itemName[chosenItems.Count].text = newItems[chosenItemIndex].name;
                        itemSprite[chosenItems.Count].sprite = newItems[chosenItemIndex].itemSprite;
                        //add to list of chosen items
                        chosenItems.Add(newItems[chosenItemIndex].name);

                    }


                }
                
            }
            else
            {
                for(int i = 0; i < newItems.Count; i++) 
                {
                    //sets button to list
                    itemName[i].text = newItems[i].name;
                    itemSprite[i].sprite = newItems[i].itemSprite;
                }
            }
            
        }
       
    }
}
