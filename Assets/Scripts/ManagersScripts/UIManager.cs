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
    
    private bool isPause; //for demo purposes

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
   


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void BackToMenu()
    {
        ActivateGameObjects(titleScreenUI.name);
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
    }

    public void Pause()
    {
        ///    ActivateGameObjects(pauseGameUI.name);
        Debug.Log("Ispause");
        isPause = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        //   ActivateGameObjects(inGameUI.name);
        Debug.Log("Resume");
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

    public void Continue() // Open Upgrade
    {

        ActivateGameObjects(upgradeUI.name);
        Upgrades.instance.getUpgradeItem();
        InteractableManager.instances.SpawnController(true);
        //Debug.Log("Continue");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");

    }

    public void ShowTutorial()
    {
        ActivateGameObjects(tutorialUI.name);
    }

}
