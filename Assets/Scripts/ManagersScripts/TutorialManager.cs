using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;

public enum TutorialPhrase
{
    ArrowOnGlowingItemTutorial,
    UpgradeItemArrows,
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
    public List<TutorialTexts> tutorialTexts;
    public int counter;

    public int arrowIndex;
    public bool isFinished;
    public List<GameObject> arrows;
    public TutorialPhrase tutorialPhrase;
    public int tutorialCounter;
    public GameObject tutorial;
    public bool playTutorial;
    public List<GameObject> tutorialButtons;
    public int customerCounter = -1;
    public TextMeshProUGUI screenText;
    public GameObject nextButton;
    public List<GameObject> highlightedImage;


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
          
            counter = 0;
        }
    }

    private void OnEnable()
    {
        if (canTutorial)
        {
            NextMessage();

            counter = 0;
        }
    }



    public void DisableInGameUI()
    {
        UIManager.instance.inGameUI.SetActive(false);
    }


    public void NextMessage()
    {

            if (counter < tutorialTexts[tutorialCounter].instructions.Count)
            {
                 screenText.text = "Click anywhere to continue";
                 text.text = tutorialTexts[tutorialCounter].instructions[counter].ToString();
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
        if (playTutorial)
        {
            Debug.Log("Endtimeline");
            foreach (GameObject obj in arrows)
            {
                obj.SetActive(false);
            }

            if (tutorialTexts[tutorialCounter].canSpawnCustomers)
            {
                isFinished = true;
            }
            else
            {
                isFinished = false;
            }
       
            tutorialCounter++;
            counter = 0;
            tutorialButtons[0].SetActive(false);

            if (isFinished)
            {
                StartTutorial();
            }
            else
            {
                NextMessage();
            }
       
        }
        else
        {
            playTutorial = true;
            //  tutorial.SetActive(false);
            tutorialButtons[customerCounter].SetActive(true);
            
        }

        //PlayerManager.instance.lastItemSpawner.canSpawn = true;
    }

    public void SpawnCustomer()
    {
        GameManager.instance.SetUpRound();
        MathProblemManager.instance.TutorialSpawn();
        GameManager.instance.customerSpawner.SpawnCustomer();
    }


    public void ActivateArrows(int index)
    {
        if (counter == index)
        {
            Debug.Log("ACtiavte Arrow");
            foreach (GameObject obj in arrows)
            {
                obj.SetActive(false);
            }

            arrows[arrowIndex].SetActive(true);
            ActivateGlowItems(arrowIndex);
            arrowIndex++;
        }
    }

    public void ActivateGlowItems(int index)
    {
        foreach(GameObject obj in highlightedImage)
        {
            obj.SetActive(false);
        }

        highlightedImage[index].SetActive(true);
    }

    public void EnableArrows()
    {


        switch (tutorialPhrase)
        {
            case TutorialPhrase.ArrowOnGlowingItemTutorial:
                ActivateArrows(3);
                
                break;

            case TutorialPhrase.UpgradeItemArrows:
              //  ActivateArrows(4);
                break;

            case TutorialPhrase.ArrowsOnCashBox:
             //   ActivateArrows(7);
                break;

            case TutorialPhrase.ItemCustomerTutorial:
               // ActivateArrows(8);
                break;

            case TutorialPhrase.ArrowsOnOrderSheet:
                //ActivateArrows(8);
                break;

            case TutorialPhrase.PabaryaCustomerTutorial:
                //ActivateArrows(10);
                //  arrows[arrows.Count].SetActive(false);
                break;

            default:
                return;


        }


    }

    public void GoToMainGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void StartTutorial()
    {
        tutorial.gameObject.SetActive(false);
        Debug.Log("Tutorial");
        UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);
        SpawnCustomer();
        isFinished = false;
        customerCounter++;
        screenText.text = "Click on the customer desired items";
    }


    public void ActivateTutorialUI()
    {
       // playTutorial = false;
        tutorial.SetActive(true);
        
        screenText.text = "Click anywhere to continue";
        NextMessage();
    }

   

    public void RestartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
