﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject inGameUI;
    public GameObject endGameUI;
    public GameObject pauseGameUI;
    public RectTransform endGamePanel;

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
    // Start is called before the first frame update
    void Start()
    {
        ActivateGameObjects(inGameUI.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseGameUI.activeInHierarchy)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }   
    }

    public void ActivateGameObjects(string nameOfGameObject)
    {
        inGameUI.SetActive(inGameUI.name.Equals(nameOfGameObject));
        endGameUI.SetActive(endGameUI.name.Equals(nameOfGameObject));
        pauseGameUI.SetActive(pauseGameUI.name.Equals(nameOfGameObject));
    }

    public void Pause()
    {
        ActivateGameObjects(pauseGameUI.name);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        ActivateGameObjects(inGameUI.name);
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
}
