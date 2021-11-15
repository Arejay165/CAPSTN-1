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
    public TextMeshProUGUI titleInstructText;
    public List<RectTransform> convoTransform;
    public RectTransform dialogueBox;
    public List<GameObject> itemMaskChild;
    public GameObject itemMask;
   


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
            tutorialPhrase = TutorialPhrase.ArrowOnGlowingItemTutorial;
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
        GameManager.instance.isPlaying = true;
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
                 UIManager.instance.inGameUI.SetActive(false);
                 nextButton.SetActive(true);
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
            //Disable Arrows
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
            
           
            if(tutorialCounter == tutorialTexts.Count - 1)
            {
                nextButton.SetActive(false);
                tutorialCounter = tutorialTexts.Count - 1;
                playTutorial = false;
                if (tutorialTexts[tutorialCounter].canSpawnButtons)
                {
                    tutorialButtons[customerCounter].SetActive(true);

                }
                nextButton.SetActive(false);
                // Debug.Log("Stuff");
     
                return;
               
            }
            else
            {
                tutorialCounter++;
            }
         
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
            nextButton.SetActive(false);
            //  tutorial.SetActive(false);
            if (tutorialTexts[customerCounter].canSpawnButtons)
            {
                tutorialButtons[customerCounter].SetActive(true);
                
            }
         
            
        }

        //PlayerManager.instance.lastItemSpawner.canSpawn = true;
    }

    public void SpawnCustomer()
    {
        GameManager.instance.SetUpRound();
        MathProblemManager.instance.TutorialSpawn();
        GameManager.instance.customerSpawner.SpawnCustomer();
    }


    public void ActivateGlow(int index)
    {
       
        if (counter == index)
        {
           
            arrows[arrowIndex].SetActive(true);

            Debug.Log("Counter: " + counter + " " + " Index: " + index + " Tutorial Phase " + tutorialPhrase);
            ActivateGlowItems(arrowIndex);
            arrowIndex++;
            tutorialPhrase++;
            Debug.Log(tutorialPhrase);
        }
        else
        {
            DisableTutorialObjects();
        }


    }

    void DisableTutorialObjects()
    {
        Debug.Log("Disable");
        foreach (GameObject obj in highlightedImage)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in arrows)
        {
            obj.SetActive(false);
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
                 ActivateGlow(3);
                // tutorialPhrase = TutorialPhrase.UpgradeItemArrows;
                //ActivateGlowItems(arrowIndex);
                // arrowIndex++;

                break;

            case TutorialPhrase.UpgradeItemArrows:
                  ActivateGlow(2);
                //  ActivateArrows(4);
                //tutorialPhrase = TutorialPhrase.ArrowsOnCashBox;
                break;

            case TutorialPhrase.ArrowsOnCashBox:
                //   ActivateArrows(7);
                ActivateGlow(2);
            //   DisableTutorialObjects();
                //  tutorialPhrase = TutorialPhrase.ItemCustomerTutorial;
                break;

            case TutorialPhrase.ItemCustomerTutorial:
                ActivateGlow(0);
                // ActivateArrows(8);
                // tutorialPhrase = TutorialPhrase.ArrowsOnOrderSheet;
                break;

            case TutorialPhrase.ArrowsOnOrderSheet:
                //ActivateArrows(8);
             //     tutorialPhrase = TutorialPhrase.ArrowsOnOrderSheet;
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
            dialogueBox.anchoredPosition = convoTransform[1].anchoredPosition;
            tutorial.gameObject.SetActive(false);
            Debug.Log("Tutorial");
            UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);
            SpawnCustomer();
            isFinished = false;
            customerCounter++;
            //  ingameText.gameObject.SetActive(true);
            text.text = "Click on the customer desired items";
            screenText.text = "";
             screenText.transform.parent.gameObject.SetActive(false);

    }


    public void ActivateTutorialUI()
    {

        // playTutorial = false;
        tutorial.SetActive(true);
        screenText.transform.parent.gameObject.SetActive(true);
        TutorialManager.instance.dialogueBox.anchoredPosition = TutorialManager.instance.convoTransform[0].anchoredPosition;
        screenText.text = "Click anywhere to continue";
        NextMessage();
    }

   

    public void RestartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ItemMasksActivator(int index)
    {
        for(int i = 0; i < itemMaskChild.Count; i++)
        {

            itemMaskChild[i].SetActive(false);
        }
        itemMaskChild[index].SetActive(true);

    }
}
