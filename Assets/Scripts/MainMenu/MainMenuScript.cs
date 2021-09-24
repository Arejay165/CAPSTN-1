using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject QuitButton;

   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnClickStart(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void OnClickQuit()
    {
        Application.Quit();

        Debug.Log("Quit");
    }
}
