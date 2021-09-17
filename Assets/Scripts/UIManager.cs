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

   
    public List<GameObject> tutorialUIs = new List<GameObject>();
  
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
    public void AddTutorialPanel(GameObject p_additionalPanel)
    {
        tutorialUIs.Add(p_additionalPanel);
        if (TutorialManager.instance)
        {
            
            if (tutorialUIs.Count > 1)
            {
                //Sort 
                for (int x = 0; x < tutorialUIs.Count; x++)
                {
                    int formerIndex = x;
                    int targetIndex = -1;
                    //Look for the UI panel matching the section
                    for (int i = 0; i < System.Enum.GetValues(typeof(TutorialSection)).Length - 2;)
                    {
      
                        if (tutorialUIs[x].GetComponent<TutorialPanel>().tutorialSection == (TutorialSection)i)
                        {

                            Debug.Log("FOUND " + tutorialUIs[x].GetComponent<TutorialPanel>().tutorialSection + " - " + (TutorialSection)i + " - " + x);

                            targetIndex = i;
                            if (targetIndex >= tutorialUIs.Count)
                            {
                                targetIndex = tutorialUIs.Count;
                            }
                            break;
                        }
                        i++;
                        if (i >= System.Enum.GetValues(typeof(TutorialSection)).Length - 2)
                        {
                            Debug.Log("There is no tutorial UI for " + (TutorialSection)i-- + " found");
                        }
                          
                    }
                    //Keep switching places until it is at its desired place
                 

                        
                    GameObject savedTarget = tutorialUIs[targetIndex];

                    tutorialUIs[targetIndex] = tutorialUIs[x];
                    tutorialUIs[x] = savedTarget;
                     
                        
                    
                }
            }
            

        }
        TutorialManager.instance.TutorialSectionStart();
    }

    private void OnEnable()
    {
        ActivateGameObjects(inGameUI.name);
        ActivateGameObjects(upgradeUI.name);

        if (TutorialManager.instance)
        {
            
            //Registers current tutorial action 
            TutorialManager.OnTutorialSectionStarted += ActivateTutorialPanel;
            
            

        }

    }

    private void OnDisable()
    {
        if (TutorialManager.instance)
        {

            //Registers current tutorial action 
            TutorialManager.OnTutorialSectionStarted -= ActivateTutorialPanel;


        }
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
        upgradeUI.SetActive(upgradeUI.name.Equals(nameOfGameObject));
        
    }

    public void ActivateTutorialPanel()
    {
        if (TutorialManager.instance)
        {
            foreach (GameObject selectedTutorialUI in tutorialUIs)
            {
                selectedTutorialUI.SetActive(false);
            }
            
            if (tutorialUIs.Count > 0)
            {
                int currentIndex = (int)TutorialManager.instance.currentTutorialSection;
                if (currentIndex < tutorialUIs.Count)
                {
                    Debug.Log("ACTIVATING: " + (int)TutorialManager.instance.currentTutorialSection + " - " + tutorialUIs[(int)TutorialManager.instance.currentTutorialSection].name);
                    tutorialUIs[currentIndex].SetActive(true);
                }
                

                

            }


        }


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

    public void Continue() // Next Level
    {
        ActivateGameObjects(upgradeUI.name);
    }

    #region Tutorial Functions
    

    
    #endregion
}
