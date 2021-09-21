using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject   customerPrefab;
    public Transform      spawnPoint;
    public TestCalculator displayOrder;
    public bool canSpawn = false;
    void Start()
    {
  
    }

 
    void Update()
    {
        
    }

    public void ToggleSpawn()
    {
        canSpawn = canSpawn ? false : true;
        if (canSpawn)
        {
            if (GameManager.instance.isPlaying)
            {
                StartCoroutine(SpawnRate());
            }
        }
    }

    public void SpawnCustomer()
    {
        GameObject obj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        GameManager.instance.customer = obj.GetComponent<Customer>();
        
        obj.GetComponent<Customer>().displayOrder = this.displayOrder;
        PerformanceManager.instance.customersEntertained++;

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
       
        float spawnTime = Random.Range(3, 3);
        yield return new WaitForSeconds(spawnTime);
        SpawnCustomer();
    }
}
