using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnalogueClock : MonoBehaviour
{

    [SerializeField] private float realSecondsInGameDay = 0;
    [SerializeField] private Transform clockHandTransform;
    [SerializeField] private Image timeFill;
    [SerializeField] float timeSpeed = 1f;
 
    [SerializeField] float currentGameTime = 0;
    float timeScale = 0f;
    public Image timeImage;
    public Sprite[] sunAndMoon;
    // Start is called before the first frame update
    void Start()
    {
        realSecondsInGameDay = DayAndNightCycle.instance.GetEndTime();
        timeScale = (360f/ realSecondsInGameDay);
        timeFill.fillAmount = realSecondsInGameDay;
    }

    public void OnEnable()
    {

        GameManager.OnGameStart += OnGameStarted;
        GameManager.OnGameEnd += OnGameEnded;
    }
    public void OnDisable()
    {

        GameManager.OnGameStart -= OnGameStarted;
        GameManager.OnGameEnd -= OnGameEnded;
    }

    void OnGameStarted()
    {
        timeFill.fillAmount = 1f;
    }

    void OnGameEnded()
    {
        

    }

    // Update is called once per frame

    private void Update()
    {
        if(!DayAndNightCycle.instance.GetIsStoreClosed())
        {
            currentGameTime += timeScale * timeSpeed * Time.deltaTime;

            // currentGameTime += DayAndNightCycle.instance.GetGameTime() / DayAndNightCycle.instance.GetEndTime();

            timeFill.fillAmount -= 1.0f / DayAndNightCycle.instance.GetEndTime() * Time.deltaTime;
            // clockHandTransform.eulerAngles = new Vector3(0, 0, -currentGameTime * 1);
            clockHandTransform.eulerAngles = new Vector3(0, 0, -currentGameTime + clockHandTransform.rotation.z + 90f); //86f);
        }
       
    }
}
