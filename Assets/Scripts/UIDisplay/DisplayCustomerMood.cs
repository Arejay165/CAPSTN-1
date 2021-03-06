using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCustomerMood : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    MoodComponent customerMood;
    [SerializeField]
    public Image moodImage;
    public Image delayedMoodImage;
    void Start()
    {
        InitializeMoodDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeMoodDisplay()
    {
        //This sometimes causes nall ref exception
        //customerMood = this.transform.parent.transform.parent.gameObject.GetComponent<MoodComponent>();
        ////moodSlider = this.GetComponent<Slider>();
        //if (customerMood)
        //{
        //    //moodSlider.maxValue = customerMood.GetMaxMoodAmount();
        //    //moodSlider.minValue = customerMood.GetCurrentMoodAmount();
        //}
    }

    public void DisplayMood()
    {
      
        moodImage.fillAmount = customerMood.GetCurrentMoodAmount() / customerMood.GetMaxMoodAmount();
        //moodSlider.value = customerMood.GetCurrentMoodAmount() / customerMood.GetMaxMoodAmount();
    }
}
