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
            //if (!pauseGameUI.activeInHierarchy)
            //{
            //    Pause();
            //}
            //else
            //{
            //    Resume();
            //}

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
        SceneManager.LoadScene("GameScene");
    }

    public void Continue() // Open Upgrade
    {

        ActivateGameObjects(upgradeUI.name);
        Upgrades.instance.getUpgradeItem();
        Debug.Log("Continue");
    }

    public void Testing()
    {
        Debug.Log("Testing");
    }
}
