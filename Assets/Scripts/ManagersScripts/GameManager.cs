using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager   instance;
    public Customer             customer;

    public CustomerSpawner customerSpawner;
    public DetectItemInWindow window;
    
    public bool isPlaying = false;
    public bool isFirstTime =true;

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
        //PlayerPrefs.DeleteAll();
        
    }
    
   

    public void TogglePlaying()
    {
        isPlaying = isPlaying ? false : true;
    }

    public void SetUpRound() //TEMPORARY FIX FOR RESTARTING GAME AFTER UPGRADING, I THINK ITS BEST TO USE OBSERVER PATTERN UNITY EVENTS 
    {                       
        

        if (customer)
        {
            Destroy(customer.gameObject);
        }
        
       
        

        TransitionManager.instances?.changeTransform.gameObject.SetActive(false);
        TransitionManager.instances?.noteBookTransform.gameObject.SetActive(false);
       

    }

    public void StartRound()
    {
        isPlaying = true;
        OnGameStart.Invoke();
      
    }
    public void PlayGame()
    {
        if (isFirstTime)
        {
            UIManager.instance.ActivateGameObjects(UIManager.instance.playerNameUI.name);
        }
        else
        {
            StartCoroutine(DayStart());
            
            

        }
        ////There is no tutorial
        //if (TutorialManager.instance == null)
        //{

        //    StartCoroutine(DayStart());
        //  //  PlayerManager.instance.lastItemSpawner.canSpawn = true;
        //}
        //else if (TutorialManager.instance)
        //{
        //    //There is tutorial but it's not active
        //    if (!TutorialManager.instance.canTutorial)
        //    {
        //        StartCoroutine(DayStart());
        //      //  PlayerManager.instance.lastItemSpawner.canSpawn = true;
        //    }
        //    else
        //    {
        //        //   StartCoroutine(DayStart());
        //        //Do tutorial
        //        UIManager.instance.ActivateGameObjects(UIManager.instance.playerNameUI.name);
        //    }
        //}
    }

    public IEnumerator DayStart()
    {
        CursorManager.instance.PlayCursorAnimation(CursorType.Arrow);
        UIManager.instance.ActivateGameObjects(UIManager.instance.roundBriefingUI.name);
        Scoring.instance.ShowBriefing();
        AudioManager.instance.stopMusic(0);
        AudioManager.instance.stopMusic(2);
        SetUpRound();
        AudioManager.instance.playSound(6);
        yield return new WaitForSeconds(4f);
        UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);
        StartRound();
        AudioManager.instance.playMusic(1);
    }

    public IEnumerator DayEnd()
    {
        CursorManager.instance.PlayCursorAnimation(CursorType.Arrow);
        UIManager.instance.ActivateGameObjects(UIManager.instance.roundDebriefingUI.name);
        InteractableManager.instances.SpawnController(false);
        if (customer != null)
        {
            Destroy(customer.gameObject);
        }

    //    PlayerManager.instance.lastItemSpawner.canSpawn = false;

        Debug.Log("Level Over");
        AudioManager.instance.stopMusic(1);
        AudioManager.instance.playSound(4);
        yield return new WaitForSeconds(4f);
        UIManager.instance.ActivateGameObjects(UIManager.instance.endGameUI.name);
        TransitionManager.instances.MoveTransition(new Vector2(0, 0), 0.5f, UIManager.instance.endGameUI.GetComponent<RectTransform>(), UIManager.instance.endGameUI.gameObject, true);
        Scoring.instance.Results();
        foreach (ClickToSelectItem selectedItem in InteractableManager.instances.clickToSelectItems)
        {
            selectedItem.canSpawn = true;
        }
        //window.invincibility = false;

        InteractableManager.instances.cashBox.clickable = true;

    }
    virtual protected void Start()
    {

        if (TutorialManager.instance.canTutorial)
        {
            //   UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);
            TutorialManager.instance.DisableInGameUI();
        }
        else
        {
            UIManager.instance.ActivateGameObjects(UIManager.instance.titleScreenUI.name);
            AudioManager.instance.playMusic(0);
        }
     
       
    
    }

    private void OnDisable()
    {
        
    }


  

}
