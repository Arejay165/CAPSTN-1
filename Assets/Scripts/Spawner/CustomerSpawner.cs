using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject   customerPrefab;
    public Transform      spawnPoint;
    public TestCalculator displayOrder;
    // 
    [SerializeField]
    public DetectItemInWindow orderWindow;
    void Start()
    {
        SpawnCustomer();
        orderWindow = GameManager.instance.window;
    }

 
    void Update()
    {
        
    }

    public void SpawnCustomer()
    {
        GameObject obj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        GameManager.instance.customer = obj.GetComponent<Customer>();
        obj.GetComponent<Customer>().displayOrder = this.displayOrder;
        orderWindow.GetComponent<DetectItemInWindow>().SetCustomer(obj.GetComponent<Customer>());
    }
}
