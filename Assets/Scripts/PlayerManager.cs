using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public bool isHolding;
    public bool canBroadcastMouseClick = false;
    public GameObject currentSelectedItem;
    public ItemSpawner lastItemSpawner;
    public static event Action OnMouseClick; 
    public bool isStaying = false;

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
    // Start is called before the first frame update
    void Start()
    {
        isHolding = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (canBroadcastMouseClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClick?.Invoke();
                
            }
        }

    }


}
