using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager   instance;
    public Customer             customer;
    public TestCalculator testCalculator;
    public CustomerSpawner customerSpawner;
    public int score = 0;
    public DetectItemInWindow window;
    public bool orderSheetShowing = false;


 
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



    virtual protected void Start()
    {

    }

    private void OnDisable()
    {
        
    }

}
