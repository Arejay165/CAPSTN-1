using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DigitalClock : MonoBehaviour
{
    
    public TextMeshProUGUI timeValueUI;
    


    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (DayAndNightCycle.instance)
        {
            float descendingTime = DayAndNightCycle.instance.GetEndTime() - DayAndNightCycle.instance.GetGameTime();
            timeValueUI.text = descendingTime.ToString("N0");
        }


    }
}
