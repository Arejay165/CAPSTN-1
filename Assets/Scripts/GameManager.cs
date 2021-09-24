using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager   instance;
    public Customer             customer;
    public TestCalculator testCalculator;
    public CustomerSpawner customerSpawner;
    public DetectItemInWindow window;
    public bool orderSheetShowing = false;
    public bool isPlaying = false;
    public int scoreGoal;

 
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

    public void TogglePlaying()
    {
        isPlaying = isPlaying ? false : true;
    }

    public void SetUpGame() //TEMPORARY FIX FOR RESTARTING GAME AFTER UPGRADING, I THINK ITS BEST TO USE OBSERVER PATTERN UNITY EVENTS 
    {                       
        isPlaying = true;
        UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);

        if (customer)
        {
            Destroy(customer.gameObject);
        }
        StartCoroutine(customerSpawner.SpawnRate());
        //? means null checker
        Scoring.instance?.SetScore(0);
        PerformanceManager.instance.customersEntertained = 0;
        DayAndNightCycle.instance?.SetGameTime(0);
        DayAndNightCycle.instance?.SetStoreClosed(false);
        TransitionManager.instances?.changeTransform.gameObject.SetActive(false);
        TransitionManager.instances?.noteBookTransform.gameObject.SetActive(false);
        Scoring.instance?.UpdateGameScoreGoal();
        Scoring.instance?.ResetMultiplier();

    }

    virtual protected void Start()
    {
        //There is no tutorial
        if (TutorialManager.instance == null)
        {
          
            SetUpGame();
        }
        else if (TutorialManager.instance)
        {
            //There is tutorial but it's not active
            if (!TutorialManager.instance.canTutorial)
            {
                SetUpGame();
            }
            else
            {
                //Do tutorial
            }
        }
        
    }

    private void OnDisable()
    {
        
    }

}
