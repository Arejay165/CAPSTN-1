using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject   customerPrefab;
    public Transform      spawnPoint;
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
       


        //for tutorial
        if (TutorialManager.instance)
        {
            if (TutorialManager.instance.tutorialQuestActive && TutorialManager.instance.tutorials.IndexOf(TutorialManager.instance.currentTutorial) == 1)
            {
                TutorialManager.instance.ToggleTutorialQuest();
                TutorialManager.instance.StartTimeline();
            }
        }
    
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
                    spawnTime = Random.Range(3, 3);
                }
                yield return new WaitForSeconds(spawnTime);
                SpawnCustomer();
            }
        }

    }
}
