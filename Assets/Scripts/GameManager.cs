using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager   instance;
    public Customer             customer;
    public TestCalculator testCalculator;
    public CustomerSpawner customerSpawner;
    public DetectItemInWindow window;
    public bool orderSheetShowing = false;
    public bool isPlaying = false;


    public static Action OnGameStart;
    public static Action OnGameEnd;
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
        OnGameStart.Invoke();

        if (customer)
        {
            Destroy(customer.gameObject);
        }
        
       
        

        TransitionManager.instances?.changeTransform.gameObject.SetActive(false);
        TransitionManager.instances?.noteBookTransform.gameObject.SetActive(false);
       

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
