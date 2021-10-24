using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DayAndNightCycle : MonoBehaviour
{
    public static DayAndNightCycle instance;
    [SerializeField]
    int         days;
    // Internal game clock in seconds
    [SerializeField]
    float       gameTime;
    // End time for the game in seconds 
    [SerializeField]
    float       endTime;

    // Used to stop the gameTime and Clock Display 
    [SerializeField]
    bool        isStoreClosed = false;
    [SerializeField]
    bool        isMorning = true;

    // Just like in UNREAL, Event Dispatcher, going to be brodcasted 
    public OnDayChange dayChanged;

    public delegate void OnDayChange();

    public UIManager uIManager;

    public bool isRushHour = false;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    public void OnEnable()
    {
        GameManager.OnGameStart += OnGameStarted;
    }
    public void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStarted;
    }

    void OnGameStarted()
    {
        SetGameTime(0);
        isRushHour = false;
        SetStoreClosed(false);
    }
    public int GetDays()
    {
        return days;
    }
    public bool GetIsMorning()
    {
        return isMorning;
    }
    public float GetGameTime()
    {
        return gameTime;
    }

    public void SetGameTime(float p_newGameTime)
    {
        gameTime = p_newGameTime;
    }

    public void SetStoreClosed(bool p_newIsStoreClosed)
    {
        isStoreClosed = p_newIsStoreClosed;
    }
    public bool GetIsStoreClosed()
    {
        return isStoreClosed;
    }
    public float GetEndTime()
    {
        return endTime;
    }
    private void Update()
    { 
        if (GameManager.instance)
        {
            if (/*GameManager.instance.isPlaying ||*/ uIManager.inGameUI.activeInHierarchy) //If game isn't paused
            {

                // Game time is used for how long the game will last. It will continuously count until the end time is reached  
                if (gameTime < endTime && !isStoreClosed)
                {
                    if(!TutorialManager.instance.canTutorial)
                    gameTime += Time.deltaTime;

                    if (gameTime >= endTime * 2 / 3)
                    {
                        if (isMorning)
                        {
                            isMorning = false;
                        }
                    }
                }
                else if (!isStoreClosed)
                {
                    isStoreClosed = true;

                    GameManager.instance.StartCoroutine(GameManager.instance.DayEnd());
                }
            }
        }
        
        
        
    }

}
