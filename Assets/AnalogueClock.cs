using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnalogueClock : MonoBehaviour
{

    [SerializeField] private float realSecondsInGameDay = 0;
    [SerializeField] private Transform clockHandTransform;
    [SerializeField] private Image timeFill;
 
    [SerializeField] float currentGameTime = 0;
    float timeScale = 0f;
    public Image timeImage;
    public Sprite[] sunAndMoon;
    // Start is called before the first frame update
    void Start()
    {
        realSecondsInGameDay = DayAndNightCycle.instance.GetEndTime();
        timeScale = (360f/ realSecondsInGameDay);
    }

    // Update is called once per frame

    private void Update()
    {
        currentGameTime += timeScale * Time.deltaTime;


        timeFill.fillAmount = DayAndNightCycle.instance.GetGameTime()/DayAndNightCycle.instance.GetEndTime();
        clockHandTransform.eulerAngles = new Vector3(0, 0, currentGameTime * -1);
    }
}
