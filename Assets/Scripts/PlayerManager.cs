using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public bool isHolding;
    public GameObject currentSelectedItem;
    public ClickToSelectItem lastItemSpawner;
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
        if (!TutorialManager.instance.tutorialQuestActive)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                TutorialManager.instance.StartTimeline();
                
            }
        }

    }


}
