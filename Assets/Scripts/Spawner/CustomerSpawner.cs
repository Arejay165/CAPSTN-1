using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject   customerPrefab;
    public Transform      spawnPoint;
    public TestCalculator displayOrder; 
   
    void Start()
    {
        SpawnCustomer();
    }

 
    void Update()
    {
        
    }

    public void SpawnCustomer()
    {
        GameObject obj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        GameManager.instance.customer = obj.GetComponent<Customer>();
        obj.GetComponent<Customer>().displayOrder = this.displayOrder;
        PerformanceManager.instance.customersEntertained++;
    }
}
