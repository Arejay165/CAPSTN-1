using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayGoalScore : MonoBehaviour
{
    Text goalText;
    
    // Start is called before the first frame update
    void Start()
    {
        goalText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        goalText.text = GameManager.instance.scoreGoal.ToString();
    }
}
