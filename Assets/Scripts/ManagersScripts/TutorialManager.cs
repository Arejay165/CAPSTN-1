using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using TMPro;

public enum TutorialPhrase
{
    ArrowOnGlowingItemTutorial,
    ArrowsOnCashBox,
    ItemCustomerTutorial,
    ArrowsOnOrderSheet,
    PabaryaCustomerTutorial,
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public bool tutorialQuestActive = false;
    public bool canTutorial = true;
    public PlayableDirector currentTutorial;

    public List<PlayableDirector> tutorials;

    public TextMeshProUGUI text;
    public List<string> setOfInstructions;
    public int counter;
    public GameObject interlude;
    public int arrowIndex;
    public bool isFinished;
    public List<GameObject> arrows;
    public TutorialPhrase tutorialPhrase;
 
   // public GameObject controlPanel;

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

    private void Start()
    {
        if (canTutorial)
        {
            NextMessage();
            counter = 1;
        }
    }

    public void DisableInGameUI()
    {
        UIManager.instance.inGameUI.SetActive(false);
    }

    
    public void NextMessage()
    {
        if(counter < setOfInstructions.Count)
        {
            text.text = setOfInstructions[counter];
          
            counter++;
            EnableArrows();

        }
        else
        {
            EndTimeline();
        }
        
    }

    public void EndTimeline()
    {
        // currentTutorial.Stop();
        interlude.gameObject.SetActive(false);
        foreach (GameObject obj in arrows)
        {
            obj.SetActive(false);
        }
        UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);
        SpawnCustomer();
        isFinished = true;
        //PlayerManager.instance.lastItemSpawner.canSpawn = true;
    }

    public void SpawnCustomer()
    {
        GameManager.instance.SetUpRound();
        GameManager.instance.customerSpawner.SpawnCustomer();
    }


    public void ActivateArrows(int index)
    {
       if(counter == index)
        {
            Debug.Log("ACtiavte Arrow");
            foreach (GameObject obj in arrows)
            {
                obj.SetActive(false);
            }
            arrows[arrowIndex].SetActive(true);
            arrowIndex++;
            
        }
     

    }

    public void EnableArrows()
    {
        switch (tutorialPhrase)
        {
            case TutorialPhrase.ArrowOnGlowingItemTutorial:
                ActivateArrows(2);
                break;

            case TutorialPhrase.ArrowsOnCashBox:
                ActivateArrows(4);
                break;

            case TutorialPhrase.ItemCustomerTutorial:
                ActivateArrows(6);
                break;

            case TutorialPhrase.ArrowsOnOrderSheet:
                ActivateArrows(8);
                break;

            case TutorialPhrase.PabaryaCustomerTutorial:
                ActivateArrows(10);
              //  arrows[arrows.Count].SetActive(false);
                break;

            default:
                return;
                
        }
    }
}
