using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogueClock : MonoBehaviour
{

    [SerializeField] private float realSecondsInGameDay = 0;
    [SerializeField] private Transform clockHandTransform;
    float gameDay = 0f;
    [SerializeField] float currentGameTime = 0;
    float timeScale = 0f;
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


      
        clockHandTransform.eulerAngles = new Vector3(0, 0, currentGameTime * -1);
    }
}
