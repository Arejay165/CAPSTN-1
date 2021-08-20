using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    
    public DayAndNightCycle     time;
    public TextMeshProUGUI      timeDisplay;
    public Image                timeImage;
    public Sprite[]             sunAndMoon;
    public int                  hour;
    public float                minutes;
    // Used to speed up the time 
    // Might have a better formula for this to properly sync with the gameTime from the Day and Night Cycle
    public float                timeScale;
    public int                  operationalHours;
    // Start is called before the first frame update
    void Start()
    {
        // Total minutes of operational hours/ total open time (end time)
        timeScale = (operationalHours * 60.0f) / time.GetEndTime();
    }

    // Update is called once per frame
    void Update()
    {
        // Run the clock if the Store is not closed
        // Time scale must be adjusted in order to reach the intended clock time in the end of the level 
        if (minutes < 59.0f && !time.GetIsStoreClosed())
        {
            minutes += timeScale * Time.deltaTime;
            timeDisplay.text = Mathf.Ceil(hour).ToString("00") + ":" + minutes.ToString("00");
            
            // Checks if it AM or PM
            if (time.GetIsMorning())
            {
                // Display the Sun Sprite
                timeImage.GetComponent<Image>().sprite = sunAndMoon[0];
            }
            else
            {
                // Display the Moon Sprite 
                timeImage.GetComponent<Image>().sprite = sunAndMoon[1];
            }
        }

        if (minutes > 59.0f)
        {
            if (hour < 12)
            {
                hour += 1;
            }
            else
            {
                hour = 1;
            }

            minutes = 0;
        }
       
    }
}
