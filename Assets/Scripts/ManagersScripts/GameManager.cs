using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Customer customer;

    public CustomerSpawner customerSpawner;
    public DetectItemInWindow window;

    public bool isPlaying = false;
    public bool isFirstTime = true;

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

        yield return new WaitForSeconds(3f);
        Scoring.instance.briefingShutter.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, Scoring.instance.briefingShutter.GetComponent<RectTransform>().anchoredPosition3D.z);
        Scoring.instance.briefingInfo.SetActive(false);
        UIManager.instance.ActivateGameObjects(UIManager.instance.inGameUI.name);
        Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.x, 0, Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.z);
        Scoring.instance.gameShutter.SetActive(true);
        Scoring.instance.briefingShutter.SetActive(false);
        Scoring.instance.gameShutter.GetComponent<RectTransform>().DOAnchorPos3D(new Vector3(Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.x, 1500, Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.z), 1f, false);
      
        StartRound();
        AudioManager.instance.playMusic(1);
        //yield return new WaitForSeconds(3f);


    }

    public IEnumerator DayEnd()
    {
        OnGameEnd.Invoke();
        TogglePlaying();
        Scoring.instance.gameShutter.SetActive(true);
        Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 1500, Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.z);

        Scoring.instance.debriefingInfo.SetActive(true);
        CursorManager.instance.PlayCursorAnimation(CursorType.Arrow);
        //  UIManager.instance.ActivateGameObjects(UIManager.instance.roundDebriefingUI.name);
        InteractableManager.instances.SpawnController(false);
        if (customer != null)
        {
            Destroy(customer.gameObject);
        }

        //    PlayerManager.instance.lastItemSpawner.canSpawn = false;

        Debug.Log("Level Over");
        AudioManager.instance.stopMusic(1);
        AudioManager.instance.playSound(4);

        yield return new WaitForSeconds(3f);
        Scoring.instance.gameShutter.GetComponent<RectTransform>().DOAnchorPos3D(new Vector3(Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.x, 0, Scoring.instance.gameShutter.GetComponent<RectTransform>().anchoredPosition3D.z), 1f, false);
        //540
        yield return new WaitForSeconds(3f);

        Scoring.instance.debriefingInfo.SetActive(false);

        UIManager.instance.ActivateGameObjects(UIManager.instance.endGameUI.name);
        Scoring.instance.resultShutter.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, Scoring.instance.resultShutter.GetComponent<RectTransform>().anchoredPosition3D.z);
        Scoring.instance.resultShutter.SetActive(true);
        Scoring.instance.gameShutter.SetActive(false);
        Scoring.instance.resultShutter.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 1500, 0), 1f, false);



        TransitionManager.instances.MoveTransition(new Vector2(0, 0), 0.5f, UIManager.instance.endGameUI.GetComponent<RectTransform>(), UIManager.instance.endGameUI.gameObject, true);
        Scoring.instance.Results();
        foreach (ClickToSelectItem selectedItem in InteractableManager.instances.clickToSelectItems)
        {
            selectedItem.canSpawn = true;
        }
        //window.invincibility = false;
        if (GameManager.instance.customer != null)
        {
            Destroy(GameManager.instance.customer.gameObject);
     
            //Disable customer bubble
            if (GameManager.instance.customer.panel.gameObject != null)
            {
                StartCoroutine(GameManager.instance.customer.ThoughtBubbleDisappear());

            }
            if (GameManager.instance.customer.moodPanel != null)
            {
                //Disable customer mood bar
                GameManager.instance.customer.moodPanel.SetActive(false);
            }

  
            Destroy(GameManager.instance.customer.gameObject);
            GameManager.instance.customer = null;

            
        }
        InteractableManager.instances.cashBox.clickable = true;

    }
    virtual protected void Start()
    {
        isPlaying = false;
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
