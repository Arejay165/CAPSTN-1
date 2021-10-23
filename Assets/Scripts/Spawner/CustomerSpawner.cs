﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject   customerPrefab;
    public Transform      spawnPoint;
    public Transform inShopPoint;
    public Transform outShopPoint;
    public TestCalculator displayOrder;
    public bool canSpawn = false;


    private void Start()
    {
      //  StartCoroutine(SpawnRate());
    }

    public void OnEnable()
    {
        GameManager.OnGameStart += OnGameStarted;
    }
    public void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStarted;
    }

    void OnGameStarted()
    {
        //? means null checker
        //ToggleSpawn();
        canSpawn = true;
        StartCoroutine(SpawnRate());
      

    }
    public void ToggleSpawn()
    {
        canSpawn = canSpawn ? false : true;
        StartCoroutine(SpawnRate());
    }

    public void SpawnCustomer()
    {
        GameObject obj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        GameManager.instance.customer = obj.GetComponent<Customer>();
        
        obj.GetComponent<Customer>().displayOrder = this.displayOrder;

        //animation
        StartCoroutine(MoveAnimation(obj));

        //for tutorial
        if (TutorialManager.instance)
        {
            if (TutorialManager.instance.tutorialQuestActive && TutorialManager.instance.tutorials.IndexOf(TutorialManager.instance.currentTutorial) == 1)
            {
             //   TutorialManager.instance.ToggleTutorialQuest();
           //     TutorialManager.instance.StartTimeline();
            }
        }
    
    }

    IEnumerator MoveAnimation(GameObject obj)
    {
        Tween myTween = obj.transform.DOMove(inShopPoint.position, 1f, false);// animation sequence
        obj.GetComponent<Customer>().panel.gameObject.SetActive(false);
        

        
        yield return myTween.WaitForCompletion(); // Wait to finish
        yield return new WaitForSeconds(0.25f);
        obj.GetComponent<Customer>().panel.gameObject.SetActive(true);
    }


    public IEnumerator SpawnRate()
    {
        if (canSpawn)
        {
            if (GameManager.instance.isPlaying)
            {
                float spawnTime = 0;
                if (!DayAndNightCycle.instance.isRushHour)
                {
                    spawnTime = Random.Range(0.5f, 1.5f);
                }
                yield return new WaitForSeconds(spawnTime);
                SpawnCustomer();
            }
        }

    }
}
