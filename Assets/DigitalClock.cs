using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DigitalClock : MonoBehaviour
{
    
    public TextMeshProUGUI timeValueUI;
    public bool isCountdowning = false;
    Color defaultColor;
    float defaultSize;
    // Start is called before the first frame update
    void Start()
    {
         defaultColor = timeValueUI.color;
        
    }
    private void OnEnable()
    {
        timeValueUI.color = defaultColor;
        timeValueUI.alpha = 255f;
        defaultSize = timeValueUI.fontSize;
        isCountdowning = false;
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        if (DayAndNightCycle.instance)
        {
            float descendingTime = DayAndNightCycle.instance.GetEndTime() - DayAndNightCycle.instance.GetGameTime();

      
            timeValueUI.text = descendingTime.ToString("N0")+ "s";
            if (descendingTime <= 10)
            {
                if (!isCountdowning)
                {
                    isCountdowning = true;
                    StartCoroutine(Countdown());

                }
            }
        }
    }

    IEnumerator Countdown()
    {
        timeValueUI.color = Color.red;
        timeValueUI.alpha = 255f;
        timeValueUI.fontSize += 15f;
        yield return new WaitForSeconds(0.25f);
        timeValueUI.fontSize = defaultSize;
        isCountdowning = false;
        timeValueUI.color = defaultColor;
        timeValueUI.alpha = 255f;

    }
}
