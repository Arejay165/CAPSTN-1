using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject inGameUI;
    public GameObject endGameUI;
    public GameObject pauseGameUI;
    public RectTransform endGamePanel;
    public GameObject upgradeUI;
    public GameObject titleScreenUI;
    public GameObject settingsUI;
    public GameObject roundBriefingUI;
    public GameObject roundDebriefingUI;
    public GameObject playerNameUI;
    public GameObject tutorialUI;
    public GameObject creditsUI;
    public GameObject highscoreUI;
    public GameObject quitConfirmation;
    
    public GameObject dayBackGround;
    public GameObject nightBackGround;


    public GameObject quitConfirmTSCopy;

    public bool isIngame = false;
    private bool isPause; //for demo purposes
    public bool quitConfirmationGoTitleScreen = true;
    public bool quitConfirmationOpen = false;
    
    //public List<GameObject> tutorialUIs = new List<GameObject>();

    private void Awake()
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
        //? means null checker
        ActivateGameObjects(inGameUI.name);
    }
   
    public void OpenQuitConfirmation()
    {
        //? means null checker
        ActivateGameObjects("");
        quitConfirmationOpen = true;
       
        if (quitConfirmationGoTitleScreen == true)
        {
            quitConfirmTSCopy.SetActive(true);
        }
        quitConfirmation.SetActive(true);
    }

    public void QuitConfirmationGoBack()
    {
        quitConfirmationOpen = false;
        quitConfirmation.SetActive(false);
        
        if (quitConfirmationGoTitleScreen == true)
        {
            quitConfirmTSCopy.SetActive(false);
            ActivateGameObjects(titleScreenUI.name);
            
        }
        else if (quitConfirmationGoTitleScreen == false)
        {
            ActivateGameObjects(pauseGameUI.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isIngame)
            {
                if (!quitConfirmationOpen)
                {
                    if (!isPause)
                    {
                        Pause();
                    }
                    else
                    {
                        Resume();
                    }
                }
            }
           
        }   
    }

    public void BackToMenu()
    {
        ActivateGameObjects(titleScreenUI.name);
    }

    public void OpenHighscore()
    {
        if (!quitConfirmationOpen)
        {
            ActivateGameObjects(highscoreUI.name);
        }
    }

    public void OpenCredits()
    {
        if (!quitConfirmationOpen)
        {
            ActivateGameObjects(creditsUI.name);
        }
    }

    public void Settings()
    {
        ActivateGameObjects(settingsUI.name);
    }
    public void ActivateGameObjects(string nameOfGameObject)
    {
        inGameUI.SetActive(inGameUI.name.Equals(nameOfGameObject));
        endGameUI.SetActive(endGameUI.name.Equals(nameOfGameObject));
        pauseGameUI.SetActive(pauseGameUI.name.Equals(nameOfGameObject));
        upgradeUI.SetActive(upgradeUI.name.Equals(nameOfGameObject));
        titleScreenUI.SetActive(titleScreenUI.name.Equals(nameOfGameObject));
        settingsUI.SetActive(settingsUI.name.Equals(nameOfGameObject));
        roundBriefingUI.SetActive(roundBriefingUI.name.Equals(nameOfGameObject));
        roundDebriefingUI.SetActive(roundDebriefingUI.name.Equals(nameOfGameObject));
        playerNameUI.SetActive(playerNameUI.name.Equals(nameOfGameObject));
        tutorialUI.SetActive(tutorialUI.name.Equals(nameOfGameObject));
        highscoreUI.SetActive(highscoreUI.name.Equals(nameOfGameObject));
        creditsUI.SetActive(creditsUI.name.Equals(nameOfGameObject));
        if (titleScreenUI.name.Equals(nameOfGameObject))
        {
            if (!AudioManager.instance.BGM[0].musicFile.isPlaying)
            {
                AudioManager.instance.playMusic(0);
            }
            
        }
       // quitConfirmation.SetActive(quitConfirmation.name.Equals(nameOfGameObject));
    }

    public void Pause()
    {
        pauseGameUI.SetActive(true);
        //ActivateGameObjects(pauseGameUI.name);
        Debug.Log("Ispause");
        isPause = true;
        GameManager.instance.isPlaying = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseGameUI.SetActive(false);
        //ActivateGameObjects(inGameUI.name);
        Debug.Log("Resume");
        GameManager.instance.isPlaying = true;
        isPause = false;
        Time.timeScale = 1f;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Restart()
    {
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
       
    }

    public void RestartInPause()
    {
        Resume();
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
          
    }


    public void Continue() // Open Upgrade
    {

        ActivateGameObjects(upgradeUI.name);
      
        Upgrades.instance.getUpgradeItem();
        InteractableManager.instances.SpawnController(true);
        //Debug.Log("Continue");
    }

    public void Tutorial()
    {
        StartCoroutine(Scoring.instance.ShutterEffect(tutorialUI, playerNameUI, LoadTutorial));
      

    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void ShowTutorial()
    {
        if (!quitConfirmationOpen)
        {
            //ActivateGameObjects(tutorialUI.name);
            StartCoroutine(Scoring.instance.ShutterEffect(titleScreenUI, tutorialUI));

        }
        
    }

}
