using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Upgrades : MonoBehaviour
{
    public static Upgrades instance;

    public List<Item> newItems;

    public List<Image> itemSprite;
    public List<TextMeshProUGUI> itemName;

    public List<GameObject> newInteractables;
    [SerializeField] private List<Item> chosenItems = new List<Item>();

    public List<Button> itemButtons;
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

        for (int i = 0; i < 3; i++)
        {
            itemButtons[i].gameObject.SetActive(false);
            itemName[i].gameObject.SetActive(false);
            itemSprite[i].gameObject.SetActive(false);
        }
    }

    public void OnDisable()
    {

        GameManager.OnGameStart -= OnGameStarted;
        GameManager.OnGameEnd -= OnGameEnded;
        //Reset upgrade uis
        //if (newItems.Count == 2)
        //{
        //    itemName[2].text = null;
        //    itemSprite[2].sprite = null;
        //}
        //else if (newItems.Count == 1)
        //{
        //    itemName[1].text = null;
        //    itemSprite[1].sprite = null;
        //}
        //itemName[0].text = null;
        //itemSprite[0].sprite = null;
        //itemName[1].text = null;
        //itemSprite[1].sprite = null;
        //itemName[2].text = null;
        //itemSprite[2].sprite = null;

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
        if(chosenItems.Count > itemIndex)
        {
            MathProblemManager.instance.storeItemList.Add(chosenItems[itemIndex]);
            //newItems.RemoveAt(itemIndex);

            UIManager.instance.upgradeUI.SetActive(false);

            for (int i = 0; i < newItems.Count; i++)
            {
                if (newItems[i].itemName == chosenItems[itemIndex].itemName)
                {
                    newInteractables[i].SetActive(true);
                    newInteractables.RemoveAt(i);
                    newItems.Remove(newItems[i]);
                }
            }
            chosenItems.Clear();
            GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
        }
        
    }
   
    public void getUpgradeItem()
    {

      
        if (newItems.Count == 0)
        {
            //GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
            SceneManager.LoadScene("EndGame");
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
                    foreach (Item selectedItemChecked in chosenItems)
                    {
                        //There is one that exists already
                        if (selectedItemChecked.itemName == newItems[chosenItemIndex].itemName)
                        {
                            isUnique = false;
                            break;
                        }

                    }
                    //name doesnt exist yet
                    if (isUnique)
                    {

                        //sets button to list
                        itemName[chosenItems.Count].text = newItems[chosenItemIndex].itemName;
                        itemSprite[chosenItems.Count].sprite = newItems[chosenItemIndex].itemSprite;
                        //add to list of chosen items
                        chosenItems.Add(newItems[chosenItemIndex]);

                    }


                }
                for (int i = 0; i < chosenItems.Count; i++)
                {
                    Debug.Log("Adding Choosen Items");
                    itemButtons[i].gameObject.SetActive(true);
                    itemName[i].gameObject.SetActive(true);
                    itemSprite[i].gameObject.SetActive(true);
                    
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    itemButtons[i].gameObject.SetActive(false);
                    itemName[i].gameObject.SetActive(false);
                    itemSprite[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < newItems.Count; i++)
                {
                    //sets button to list
                    Debug.Log("Button Debugger");
                    itemName[i].text = newItems[i].itemName;
                    itemSprite[i].sprite = newItems[i].itemSprite;
                    itemButtons[i].gameObject.SetActive(true);
                    itemName[i].gameObject.SetActive(true);
                    itemSprite[i].gameObject.SetActive(true);
                    chosenItems.Add(newItems[i]);
                }
                //for (int i = 0; i < chosenItems.Count; i++)
                //{
                //    itemButtons[i].gameObject.SetActive(true);
                //    itemName[i].gameObject.SetActive(true);
                //    itemSprite[i].gameObject.SetActive(true);
                //}
            }

          

        }
       
    }

}
